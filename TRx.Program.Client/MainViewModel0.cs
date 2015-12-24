namespace TRx.Program.Client
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    public class MainViewModel0
    {
        public MainViewModel0()
        {
            //this.plotModel = new PlotModel { Title = "Example 1" };
            //this.plotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            this.MyModel = CreatePlotModel();
        }

        private PlotModel CreatePlotModel()
        {

            var plotModel = new PlotModel { Title = "OxyPlot Demo" };

            //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 10, Minimum = 0 });

            var series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            //IList<DataPoint> Points = new List<DataPoint>
            //                  {
            //                      new DataPoint(0, 4),
            //                      new DataPoint(10, 13),
            //                      new DataPoint(20, 15),
            //                      new DataPoint(30, 16),
            //                      new DataPoint(40, 12),
            //                      new DataPoint(50, 12)
            //                  };

            series1.Points.Add(new DataPoint(0.0, 6.0));
            series1.Points.Add(new DataPoint(1.4, 2.1));
            series1.Points.Add(new DataPoint(2.0, 4.2));
            series1.Points.Add(new DataPoint(3.3, 2.3));
            series1.Points.Add(new DataPoint(4.7, 7.4));
            series1.Points.Add(new DataPoint(6.0, 6.2));
            series1.Points.Add(new DataPoint(8.9, 8.9));

            plotModel.Series.Add(series1);

            return plotModel;
        }

        public PlotModel MyModel { get; private set; }
    }
}