namespace TRx.Program.Client
{
    using System;

    using OxyPlot;
    using OxyPlot.Series;

    public class MainViewModel1
    {
        public MainViewModel1()
        {
            this.MyModel = new PlotModel { Title = "Example 1" };
            this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        }

        public PlotModel MyModel { get; private set; }
    }
}