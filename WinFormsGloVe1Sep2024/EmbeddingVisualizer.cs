using LibraryDimensionalityReducer1Sep2024;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WinFormsGloVe1Sep2024
{    
    /// <summary>
    /// ChatGPT
    /// </summary>
    public class EmbeddingVisualizer
    {
        private readonly Dictionary<string, double[]> wordEmbeddings;

        public EmbeddingVisualizer(Dictionary<string, double[]> wordEmbeddings)
        {
            this.wordEmbeddings = wordEmbeddings;
        }

        public PlotModel CreateEmbeddingPlot(string[] tokens, int embeddingDim)
        {
            // Get the embeddings for the tokens
            List<double[]> embeddings = tokens.Select(word => wordEmbeddings.ContainsKey(word) ? wordEmbeddings[word] : null)
                                              .Where(embedding => embedding != null)
                                              .ToList();

            // Perform dimensionality reduction (e.g., PCA or t-SNE) here. Assuming `ReduceDimensionsTo2D` is a function that does this.
            List<double[]> reducedEmbeddings = ReduceDimensionsTo2D(embeddings);

            // Create the OxyPlot model
            var plotModel = new PlotModel { Title = "Word Embeddings in 2D" };

            // Set up axes
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Principal Component 1" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Principal Component 2" });

            // Add points to the plot
            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };

            for (int i = 0; i < reducedEmbeddings.Count; i++)
            {
                // Map 2D coordinates
                double[] coords = reducedEmbeddings[i];
                scatterSeries.Points.Add(new ScatterPoint(coords[0], coords[1]));

                // Add text annotations for each word
                var textAnnotation = new TextAnnotation
                {
                    Text = tokens[i],
                    TextPosition = new DataPoint(coords[0], coords[1]),
                    FontSize = 10,
                    TextColor = OxyColors.DarkBlue,
                    Stroke = OxyColors.Transparent
                };
                plotModel.Annotations.Add(textAnnotation);
            }

            plotModel.Series.Add(scatterSeries);
            return plotModel;
        }

        // Placeholder method for dimensionality reduction (e.g., using PCA or t-SNE)
        private List<double[]> ReduceDimensionsTo2D(List<double[]> embeddings)
        {
            return DimensionalityReducer.ReduceTo2D(embeddings);
        }
    }
}