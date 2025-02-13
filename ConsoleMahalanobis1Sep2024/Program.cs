﻿using LibraryGlobalVectors1Sep2024;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConsoleMahalanobis1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    class Program
    {
        static void Main()
        {
            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";
            int embeddingDim = 50;

            // Load GloVe embeddings
            var glove = new GloveLoader(filePath, embeddingDim);

            var physicsSentences = new List<string[]>
            {
                new[] { "energy", "is", "conserved", "in", "system" },
                new[] { "force", "equals", "mass", "times", "acceleration" },
                new[] { "light", "travels", "in", "vacuum", "at", "speed" },
                new[] { "electron", "has", "negative", "charge" },
                new[] { "gravity", "pulls", "objects", "towards", "earth" },
                new[] { "momentum", "is", "mass", "times", "velocity" },
                new[] { "work", "is", "force", "applied", "over", "distance" },
                new[] { "atoms", "are", "made", "of", "protons", "neutrons", "and", "electrons" },
                new[] { "temperature", "is", "measure", "of", "average", "kinetic", "energy" },
                new[] { "current", "flows", "through", "conductor", "in", "circuit" },
                new[] { "magnetic", "fields", "affect", "moving", "charges" },
                new[] { "power", "is", "rate", "of", "doing", "work" },
                new[] { "sound", "travels", "as", "longitudinal", "wave" },
                new[] { "pressure", "equals", "force", "divided", "by", "area" },
                new[] { "acceleration", "is", "rate", "of", "change", "of", "velocity" },
                new[] { "kinetic", "energy", "is", "half", "mass", "times", "velocity", "squared" },
                new[] { "potential", "energy", "is", "stored", "in", "objects", "due", "to", "position" },
                new[] { "resistance", "opposes", "current", "in", "electric", "circuit" },
                new[] { "heat", "flows", "from", "hotter", "to", "colder", "objects" },
                new[] { "wave", "frequency", "is", "cycles", "per", "second" }
            };

            // Step 1: Get embeddings for each word
            var wordEmbeddings = new List<Vector<double>>();
            var wordList = new List<string>();
            foreach (var sentence in physicsSentences)
            {
                foreach (var word in sentence)
                {
                    var embedding = glove.GetEmbedding(word);
                    if (embedding == null)
                    {
                        Console.WriteLine($"Warning: No embedding found for '{word}'");
                        continue;
                    }
                    wordEmbeddings.Add(DenseVector.OfArray(embedding));
                    wordList.Add(word);
                }
            }

            // Step 2: Create the data matrix
            var dataMatrix = DenseMatrix.OfRowVectors(wordEmbeddings);

            // Step 3: Calculate the covariance matrix
            var covarianceMatrix = CalculateCovarianceMatrix(dataMatrix);

            // Step 4: Find the highest and lowest covariance pairs
            var (maxPairs, minPairs) = FindCovarianceExtremes(covarianceMatrix);

            // Step 5: Display results
            Console.WriteLine("Words with Highest Covariances:");
            foreach (var (index1, index2, value) in maxPairs)
            {
                if (IsValidPair((index1, index2), wordList))
                    Console.WriteLine($"{wordList[index1]} and {wordList[index2]}: {value:F6}");
            }

            Console.WriteLine("\nWords with Lowest Covariances:");
            foreach (var (index1, index2, value) in minPairs)
            {
                if (IsValidPair((index1, index2), wordList))
                    Console.WriteLine($"{wordList[index1]} and {wordList[index2]}: {value:F6}");
            }
                      
            /*
            Words with Highest Covariances:
            momentum and measure: 0,003319
            equals and momentum: 0,003218

            Words with Lowest Covariances:
            momentum and work: -0,005528
            equals and work: -0,004588
            */
        }

        static Matrix<double> CalculateCovarianceMatrix(Matrix<double> dataMatrix)
        {
            var meanVector = dataMatrix.ColumnSums() / dataMatrix.RowCount;
            var centeredMatrix = dataMatrix - DenseMatrix.OfRowVectors(Enumerable.Repeat(meanVector, dataMatrix.RowCount));
            return centeredMatrix.TransposeThisAndMultiply(centeredMatrix) / (dataMatrix.RowCount - 1);
        }

        static (List<(int, int, double)> maxPairs, List<(int, int, double)> minPairs) FindCovarianceExtremes(Matrix<double> covarianceMatrix)
        {
            var maxPairs = new List<(int, int, double)>();
            var minPairs = new List<(int, int, double)>();
            double max1 = double.MinValue, max2 = double.MinValue;
            double min1 = double.MaxValue, min2 = double.MaxValue;
            (int, int) maxPair1 = (-1, -1), maxPair2 = (-1, -1);
            (int, int) minPair1 = (-1, -1), minPair2 = (-1, -1);

            for (int i = 0; i < covarianceMatrix.RowCount; i++)
            {
                for (int j = i + 1; j < covarianceMatrix.ColumnCount; j++) // Avoid diagonal and duplicates
                {
                    double value = covarianceMatrix[i, j];

                    // Update highest values
                    if (value > max1)
                    {
                        max2 = max1; maxPair2 = maxPair1;
                        max1 = value; maxPair1 = (i, j);
                    }
                    else if (value > max2)
                    {
                        max2 = value; maxPair2 = (i, j);
                    }

                    // Update lowest values
                    if (value < min1)
                    {
                        min2 = min1; minPair2 = minPair1;
                        min1 = value; minPair1 = (i, j);
                    }
                    else if (value < min2)
                    {
                        min2 = value; minPair2 = (i, j);
                    }
                }
            }

            maxPairs.Add((maxPair1.Item1, maxPair1.Item2, max1));
            maxPairs.Add((maxPair2.Item1, maxPair2.Item2, max2));
            minPairs.Add((minPair1.Item1, minPair1.Item2, min1));
            minPairs.Add((minPair2.Item1, minPair2.Item2, min2));

            return (maxPairs, minPairs);
        }

        static bool IsValidPair((int, int) pair, List<string> wordList)
        {
            return pair.Item1 >= 0 && pair.Item1 < wordList.Count && pair.Item2 >= 0 && pair.Item2 < wordList.Count;
        }
    }

}
