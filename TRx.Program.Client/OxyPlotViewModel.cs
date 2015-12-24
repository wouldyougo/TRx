namespace TRx.Program.Client
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.Axes;

//  <package id="OxyPlot.Core" version="2014.1.546" targetFramework="net451" />
//  <package id="OxyPlot.Wpf" version="2014.1.546" targetFramework="net451" />

    public class OxyPlotViewModel
    {
        public OxyPlotViewModel()
        {
            //this.plotModel = new PlotModel { Title = "Example 1" };
            //this.plotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            this.plotModel = CreatePlotModel();
        }

        private PlotModel CreatePlotModel()
        {
            var plotModel = new PlotModel { Title = "OxyPlot данные из консоли" };

            //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            //plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 10, Minimum = 0 });

            var seriesO = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };
            var seriesH = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };
            var seriesL = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };
            var seriesC = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };
            var seriesV = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            //series1.Points.Add(new DataPoint(0.0, 6.0));
            //series1.Points.Add(new DataPoint(1.4, 2.1));
            //series1.Points.Add(new DataPoint(2.0, 4.2));
            //series1.Points.Add(new DataPoint(3.3, 2.3));
            //series1.Points.Add(new DataPoint(4.7, 7.4));
            //series1.Points.Add(new DataPoint(6.0, 6.2));
            //series1.Points.Add(new DataPoint(8.9, 8.9));

            plotModel.Series.Add(seriesO);
            plotModel.Series.Add(seriesH);
            plotModel.Series.Add(seriesL);
            plotModel.Series.Add(seriesC);
            plotModel.Series.Add(seriesV);

            return plotModel;
        }

        public static PlotModel CandlesVolumecombinedvolume()
        {
            var plotModel1 = new PlotModel();
            plotModel1.Title = "Candles + Volume (combined volume)";
            var dateTimeAxis1 = new DateTimeAxis();
            dateTimeAxis1.Maximum = 42192.5003736849;
            dateTimeAxis1.Minimum = 42192.499447759;
            plotModel1.Axes.Add(dateTimeAxis1);
            var linearAxis1 = new LinearAxis();
            linearAxis1.Maximum = 107.6522857004;
            linearAxis1.Minimum = 100.945594915212;
            linearAxis1.StartPosition = 0.25;
            plotModel1.Axes.Add(linearAxis1);
            var linearAxis2 = new LinearAxis();
            linearAxis2.EndPosition = 0.22;
            linearAxis2.Key = "Volume";
            linearAxis2.Maximum = 5000;
            linearAxis2.Minimum = 0;
            plotModel1.Axes.Add(linearAxis2);
            var candleStickAndVolumeSeries1 = new CandleStickAndVolumeSeries();
            candleStickAndVolumeSeries1.SeparatorColor = OxyColors.Gray;
            candleStickAndVolumeSeries1.PositiveHollow = false;
            plotModel1.Series.Add(candleStickAndVolumeSeries1);
            return plotModel1;
        }

        public PlotModel plotModel { get; private set; }
    }
}