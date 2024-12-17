using LibraryGlobalVectors1Sep2024;
using OxyPlot.WindowsForms;

namespace WinFormsGloVe1Sep2024
{
    /// <summary>
    /// ChatGPT
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string filePath = "../../../../../../../GloVe/glove.6B.50d.txt";

            int embeddingDim = 50;

            var embeddingLoader = new EmbeddingLoader(filePath, embeddingDim);
            var wordEmbeddings = embeddingLoader.Embeddings;

            string[] tokens1 = { "comparative", "city", "strong", "paris", "man", "Honolulu", "toad", "queen", "IBM",
                        "litoria", "french", "hello", "world", "clear", "this", "soft", "dark", "is",
                        "a", "test", "france", "belgium", "clearer", "softer", "darker", "frog",
                        "frogs", "lizard", "Nashville", "uncle", "woman", "sir", "United", "stronger",
                        "superlative", "company", "brother", "sister", "observation", "elevator",
                        "blood", "pressure", "town", "phone", "hamburger", "solid", "gas", "water",
                        "fashion", "ice", "steam" };

            string[] tokens2 = { "happy", "joyful", "cheerful", "content", "sad", "unhappy", "downcast", "miserable" };

            // ChatGPT-4o
            string[] tokens3 = { "happy", "joyful", "cheerful", "content", "sad", "unhappy", "downcast", "miserable",
                    "city", "town", "village", "country", "state", "province", "capital", "municipality" };


            var plotView = new PlotView
            {
                Dock = DockStyle.Fill
            };

            // Create the embedding visualizer
            var visualizer = new EmbeddingVisualizer(wordEmbeddings);
            plotView.Model = visualizer.CreateEmbeddingPlot(tokens3, embeddingDim: embeddingDim);

            Controls.Add(plotView);
            Text = "GloVe Embeddings Visualization";
            Width = 800;
            Height = 600;
        }
    }
}
