using Accord.Statistics.Analysis;

namespace LibraryDimensionalityReducer1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public static class DimensionalityReducer
    {
        public static List<double[]> ReduceTo2D(List<double[]> embeddings)
        {
            // Convert List<double[]> to a 2D array
            double[,] matrix = new double[embeddings.Count, embeddings[0].Length];
            for (int i = 0; i < embeddings.Count; i++)
            {
                for (int j = 0; j < embeddings[0].Length; j++)
                {
                    matrix[i, j] = embeddings[i][j];
                }
            }

            // Convert to jagged array
            double[][] jaggedMatrix = Converter.ToJaggedArray(matrix);

            // Perform PCA to reduce to 2 dimensions
            var pca = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center
            };

            pca.Learn(jaggedMatrix);
            double[][] reducedMatrix = pca.Transform(jaggedMatrix, dimensions: 2);

            // Convert the reduced matrix back to List<double[]>
            var reducedList = new List<double[]>();
            foreach (var row in reducedMatrix)
            {
                reducedList.Add(new double[] { row[0], row[1] });
            }

            return reducedList;
        }
    }
}