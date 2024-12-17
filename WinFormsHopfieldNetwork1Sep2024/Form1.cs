using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace WinFormsHopfieldNetwork1Sep2024
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
                Location = new System.Drawing.Point(0, 0)
            };
            Controls.Add(plotView);
        }

        private void RunSimulationAndPlot()
        {
            int size = 100; // Number of neurons in the Hopfield network
            int maxPatterns = 15; // Maximum number of patterns to test
            double[] temperatures = { 0.0, 0.1, 0.2, 0.3 }; // Different noise levels

            // Run the simulation
            double[,] accuracyResults = PhaseDiagramSimulation.RunSimulation(size, maxPatterns, temperatures);

            // Create the plot model and configure it
            var model = new PlotModel { Title = "Phase Diagram of Hopfield Network" };

            // Configure the heatmap series and add it to the model
            var heatmapSeries = new HeatMapSeries
            {
                X0 = 1,
                X1 = maxPatterns,
                Y0 = temperatures[0],
                Y1 = temperatures[temperatures.Length - 1],
                Interpolate = false,
                Data = accuracyResults
            };
            model.Series.Add(heatmapSeries);

            // Add a separate color axis for the heatmap
            var colorAxis = new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.Rainbow(100),
                Minimum = 0,
                Maximum = 1,
                Title = "Accuracy"
            };
            model.Axes.Add(colorAxis);

            // Add X and Y axes
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Number of Patterns" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Temperature" });

            // Set the model to the plot view
            plotView.Model = model;
        }

    }
}
