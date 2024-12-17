using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using OxyPlot;

namespace WinFormsHopfieldNetwork3Sep2024
{
    public partial class Form1 : Form
    {
        private PlotView plotView;

        public Form1()
        {
            InitializeComponent();

            Text = "Phase Diagram of Hopfield Network";
            InitializePlotView();
            RunSimulationAndPlot();
        }

        private void InitializePlotView()
        {
            plotView = new PlotView
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 0)
            };
            Controls.Add(plotView);
        }

        private void RunSimulationAndPlot()
        {
            int size = 100; // Number of neurons in the Hopfield network
            int maxPatterns = 50; // Increased number of patterns for finer resolution
            double[] temperatures = Enumerable.Range(0, 31).Select(i => i * 0.01).ToArray(); // Finer temperature steps

            // Run the simulation with increased resolution
            double[,] accuracyResults = PhaseDiagramSimulation.RunSimulation(size, maxPatterns, temperatures);

            // Create the plot model and configure it
            var model = new PlotModel { Title = "Phase Diagram of Hopfield Network", TitleFontSize = 18 };

            // Configure the heatmap series with interpolation enabled
            var heatmapSeries = new HeatMapSeries
            {
                X0 = 1,
                X1 = maxPatterns,
                Y0 = temperatures[0],
                Y1 = temperatures[temperatures.Length - 1],
                Interpolate = true, // Enable interpolation for smoother transitions
                Data = accuracyResults
            };
            model.Series.Add(heatmapSeries);

            // Add a color axis with a contrasting color palette
            var colorAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Jet(100), // Jet palette for clearer phase transitions
                Minimum = 0,
                Maximum = 1,
                Title = "Accuracy",
                TitleFontSize = 14
            };
            model.Axes.Add(colorAxis);

            // Add X and Y axes with labels and larger font
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Number of Patterns",
                TitleFontSize = 14,
                FontSize = 12,
                MajorStep = 5, // Adjusted step for clarity
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperature",
                TitleFontSize = 14,
                FontSize = 12,
                MajorStep = 0.05 // Adjusted step for finer temperature increments
            });

            // Add annotations to indicate specific regions, such as "Palimpsest"
            var palimpsestAnnotation = new TextAnnotation
            {
                Text = "Palimpsest",
                TextColor = OxyColors.Black,
                FontSize = 14,
                Stroke = OxyColors.Transparent,
                TextPosition = new DataPoint(15, 0.15), // Adjust coordinates as necessary
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
                TextVerticalAlignment = OxyPlot.VerticalAlignment.Middle
            };
            model.Annotations.Add(palimpsestAnnotation);

            // Add other annotations if needed
            var stableMemoryAnnotation = new TextAnnotation
            {
                Text = "Stable Memory",
                TextColor = OxyColors.Black,
                FontSize = 14,
                Stroke = OxyColors.Transparent,
                TextPosition = new DataPoint(5, 0.05), // Example coordinates for stable region, adjust as necessary
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
                TextVerticalAlignment = VerticalAlignment.Middle
            };
            model.Annotations.Add(stableMemoryAnnotation);

            var chaoticRegionAnnotation = new TextAnnotation
            {
                Text = "Chaotic Region",
                TextColor = OxyColors.Black,
                FontSize = 14,
                Stroke = OxyColors.Transparent,
                TextPosition = new DataPoint(25, 0.25), // Example coordinates for chaotic region, adjust as necessary
                TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center,
                TextVerticalAlignment = VerticalAlignment.Middle
            };
            model.Annotations.Add(chaoticRegionAnnotation);

            // Set the model to the plot view
            plotView.Model = model;
            plotView.Dock = DockStyle.Fill;
            this.ClientSize = new Size(1200, 800); // Increase window size for higher resolution
        }


    }
}
