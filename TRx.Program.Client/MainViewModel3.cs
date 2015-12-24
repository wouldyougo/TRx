namespace TRx.Program.Client
{
    using System.Collections.Generic;

    using System;
    using System.Linq;

    //using OxyPlot;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class MainViewModel3
    {
        public PlotModel MyModel { get; private set; }
        public Example example { get; private set; }

        public MainViewModel3()
        {
            //this.plotModel = new PlotModel { Title = "Example 1" };
            //this.plotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            this.MyModel = CreatePlotModel();
        }

        private PlotModel CreatePlotModel()
        {
            example = CandleStickAndVolumeSeriesExamples.CombinedVolume_Adjusting();
            return example.Model;
        }
    }


    //[Examples("CandleStickAndVolumeSeries")]
    //[Tags("Series")]
    public static class CandleStickAndVolumeSeriesExamples
    {
        [Example("Candles + Volume (combined volume), adjusting Y-axis")]
        public static Example CombinedVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (combined volume)",
                VolumeStyle.Combined,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (combined volume), natural Y-axis")]
        public static Example CombinedVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (combined volume)",
                VolumeStyle.Combined,
                naturalY: true,
                naturalV: true);
        }

        [Example("Candles + Volume (stacked volume), adjusting Y-axis")]
        public static Example StackedVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (stacked volume)",
                VolumeStyle.Stacked,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (stacked volume), natural Y-axis")]
        public static Example StackedVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (stacked volume)",
                VolumeStyle.Stacked,
                naturalY: true,
                naturalV: true);
        }

        [Example("Candles + Volume (+/- volume), adjusting Y-axis")]
        public static Example PosNegVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (+/- volume)",
                VolumeStyle.PositiveNegative,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (+/- volume), natural Y-axis")]
        public static Example PosNegVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (+/- volume)",
                VolumeStyle.PositiveNegative,
                naturalY: true,
                naturalV: true);
        }

        [Example("Candles + Volume (volume not shown), adjusting Y-axis")]
        public static Example NoVolume_Adjusting()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (volume not shown)",
                VolumeStyle.None,
                naturalY: false,
                naturalV: false);
        }

        [Example("Candles + Volume (volume not shown), natural Y-axis")]
        public static Example NoVolume_Natural()
        {
            return CreateCandleStickAndVolumeSeriesExample(
                "Candles + Volume (volume not shown)",
                VolumeStyle.None,
                naturalY: true,
                naturalV: true);
        }

        /// <summary>
        /// Creates the candle stick and volume series example.
        /// </summary>
        /// <returns>The candle stick and volume series example.</returns>
        /// <param name="title">Title.</param>
        /// <param name="style">Style.</param>
        /// <param name="n">N.</param>
        /// <param name="naturalY">If set to <c>true</c> natural y.</param>
        /// <param name="naturalV">If set to <c>true</c> natural v.</param>
        private static Example CreateCandleStickAndVolumeSeriesExample(
            string title,
            VolumeStyle style,
            int n = 10000,
            bool naturalY = false,
            bool naturalV = false)
        {
            var pm = new PlotModel { Title = title };

            var series = new CandleStickAndVolumeSeries
            {
                PositiveColor = OxyColors.DarkGreen,
                NegativeColor = OxyColors.Red,
                PositiveHollow = false,
                NegativeHollow = false,
                SeparatorColor = OxyColors.Gray,
                SeparatorLineStyle = LineStyle.Dash,
                VolumeStyle = style
            };

            // create bars
            foreach (var bar in OhlcvItemGenerator.MRProcess(n))
            {
                series.Append(bar);
            }

            // create visible window
            var Istart = n - 200;
            var Iend = n - 120;
            var Ymin = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.Low).Min();
            var Ymax = series.Items.Skip(Istart).Take(Iend - Istart + 1).Select(x => x.High).Max();
            var Xmin = series.Items[Istart].X;
            var Xmax = series.Items[Iend].X;

            // setup axes
            var timeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = Xmin,
                Maximum = Xmax
            };
            var barAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = series.BarAxisKey,
                StartPosition = 0.25,
                EndPosition = 1.0,
                Minimum = naturalY ? double.NaN : Ymin,
                Maximum = naturalY ? double.NaN : Ymax
            };
            var volAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = series.VolumeAxisKey,
                StartPosition = 0.0,
                EndPosition = 0.22,
                Minimum = naturalV ? double.NaN : 0,
                Maximum = naturalV ? double.NaN : 5000
            };

            switch (style)
            {
                case VolumeStyle.None:
                    barAxis.Key = null;
                    barAxis.StartPosition = 0.0;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    break;

                case VolumeStyle.Combined:
                case VolumeStyle.Stacked:
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;

                case VolumeStyle.PositiveNegative:
                    volAxis.Minimum = naturalV ? double.NaN : -5000;
                    pm.Axes.Add(timeAxis);
                    pm.Axes.Add(barAxis);
                    pm.Axes.Add(volAxis);
                    break;
            }

            pm.Series.Add(series);

            if (naturalY == false)
            {
                timeAxis.AxisChanged += (sender, e) => AdjustYExtent(series, timeAxis, barAxis);
            }

            var controller = new PlotController();
            controller.UnbindAll();
            controller.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            return new Example(pm, controller);
        }

        /// <summary>
        /// Adjusts the Y extent.
        /// </summary>
        /// <param name="series">Series.</param>
        /// <param name="xaxis">Xaxis.</param>
        /// <param name="yaxis">Yaxis.</param>
        private static void AdjustYExtent(CandleStickAndVolumeSeries series, DateTimeAxis xaxis, LinearAxis yaxis)
        {
            var xmin = xaxis.ActualMinimum;
            var xmax = xaxis.ActualMaximum;

            var istart = series.FindByX(xmin);
            var iend = series.FindByX(xmax, istart);

            var ymin = double.MaxValue;
            var ymax = double.MinValue;
            for (int i = istart; i <= iend; i++)
            {
                var bar = series.Items[i];
                ymin = Math.Min(ymin, bar.Low);
                ymax = Math.Max(ymax, bar.High);
            }

            var extent = ymax - ymin;
            var margin = extent * 0.10;

            yaxis.Zoom(ymin - margin, ymax + margin);
        }
    }

    /// <summary>
    /// Represents an example.
    /// </summary>
    public class Example
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Example"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="controller">The controller.</param>
        public Example(PlotModel model, IPlotController controller = null)
        {
            this.Model = model;
            this.Controller = controller;
        }

        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        public IPlotController Controller { get; private set; }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public PlotModel Model { get; private set; }
    }

    /// <summary>
    /// Specifies the title for an example.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExampleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleAttribute"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public ExampleAttribute(string title = null)
        {
            this.Title = title;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }
    }

    /// <summary>
    /// Creates realistic OHLCV items.
    /// </summary>
    public static class OhlcvItemGenerator
    {
        /// <summary>
        /// The random number generator.
        /// </summary>
        private static readonly Random Rand = new Random();

        /// <summary>
        /// Creates bars governed by a MR process.
        /// </summary>
        /// <param name="n">N.</param>
        /// <param name="x0">X0.</param>
        /// <param name="v0">V0.</param>
        /// <param name="csigma">Csigma.</param>
        /// <param name="esigma">Esigma.</param>
        /// <param name="kappa">Kappa.</param>
        /// <returns>
        /// The process.
        /// </returns>
        public static IEnumerable<OhlcvItem> MRProcess(
            int n,
            double x0 = 100.0,
            double v0 = 500,
            double csigma = 0.50,
            double esigma = 0.75,
            double kappa = 0.01)
        {
            double x = x0;
            var baseT = DateTime.UtcNow;
            for (int ti = 0; ti < n; ti++)
            {
                var dx_c = -kappa * (x - x0) + RandomNormal(0, csigma);
                var dx_1 = -kappa * (x - x0) + RandomNormal(0, esigma);
                var dx_2 = -kappa * (x - x0) + RandomNormal(0, esigma);

                var open = x;
                var close = x = x + dx_c;
                var low = Min(open, close, open + dx_1, open + dx_2);
                var high = Max(open, close, open + dx_1, open + dx_2);

                var dp = close - open;
                var v = v0 * Math.Exp(Math.Abs(dp) / csigma);
                var dir = (dp < 0) ?
                    -Math.Min(-dp / esigma, 1.0) :
                    Math.Min(dp / esigma, 1.0);

                var skew = (dir + 1) / 2.0;
                var buyvol = skew * v;
                var sellvol = (1 - skew) * v;

                var nowT = baseT.AddSeconds(ti);
                var t = DateTimeAxis.ToDouble(nowT);
                yield return new OhlcvItem(t, open, high, low, close, buyvol, sellvol);
            }
        }

        /// <summary>
        /// Finds the minimum of the specified a, b, c and d.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        /// <param name="d">D.</param>
        /// <returns>The minimum.</returns>
        private static double Min(double a, double b, double c, double d)
        {
            return Math.Min(a, Math.Min(b, Math.Min(c, d)));
        }

        /// <summary>
        /// Finds the maximum of the specified a, b, c and d.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">B.</param>
        /// <param name="c">C.</param>
        /// <param name="d">D.</param>
        /// <returns>The maximum.</returns>
        private static double Max(double a, double b, double c, double d)
        {
            return Math.Max(a, Math.Max(b, Math.Max(c, d)));
        }

        /// <summary>
        /// Gets random normal
        /// </summary>
        /// <param name="mu">Mu.</param>
        /// <param name="sigma">Sigma.</param>
        /// <returns></returns>
        private static double RandomNormal(double mu, double sigma)
        {
            return InverseCumNormal(Rand.NextDouble(), mu, sigma);
        }

        /// <summary>
        /// Fast approximation for inverse cum normal
        /// </summary>
        /// <param name="p">probability</param>
        /// <param name="mu">Mean</param>
        /// <param name="sigma">std dev</param>
        private static double InverseCumNormal(double p, double mu, double sigma)
        {
            const double A1 = -3.969683028665376e+01;
            const double A2 = 2.209460984245205e+02;
            const double A3 = -2.759285104469687e+02;
            const double A4 = 1.383577518672690e+02;
            const double A5 = -3.066479806614716e+01;
            const double A6 = 2.506628277459239e+00;

            const double B1 = -5.447609879822406e+01;
            const double B2 = 1.615858368580409e+02;
            const double B3 = -1.556989798598866e+02;
            const double B4 = 6.680131188771972e+01;
            const double B5 = -1.328068155288572e+01;

            const double C1 = -7.784894002430293e-03;
            const double C2 = -3.223964580411365e-01;
            const double C3 = -2.400758277161838e+00;
            const double C4 = -2.549732539343734e+00;
            const double C5 = 4.374664141464968e+00;
            const double C6 = 2.938163982698783e+00;

            const double D1 = 7.784695709041462e-03;
            const double D2 = 3.224671290700398e-01;
            const double D3 = 2.445134137142996e+00;
            const double D4 = 3.754408661907416e+00;

            const double Xlow = 0.02425;
            const double Xhigh = 1.0 - Xlow;

            double z, r;

            if (p < Xlow)
            {
                // Rational approximation for the lower region 0<x<u_low
                z = Math.Sqrt(-2.0 * Math.Log(p));
                z = (((((C1 * z + C2) * z + C3) * z + C4) * z + C5) * z + C6) /
                    ((((D1 * z + D2) * z + D3) * z + D4) * z + 1.0);
            }
            else if (p <= Xhigh)
            {
                // Rational approximation for the central region u_low<=x<=u_high
                z = p - 0.5;
                r = z * z;
                z = (((((A1 * r + A2) * r + A3) * r + A4) * r + A5) * r + A6) * z /
                    (((((B1 * r + B2) * r + B3) * r + B4) * r + B5) * r + 1.0);
            }
            else
            {
                // Rational approximation for the upper region u_high<x<1
                z = Math.Sqrt(-2.0 * Math.Log(1.0 - p));
                z = -(((((C1 * z + C2) * z + C3) * z + C4) * z + C5) * z + C6) /
                    ((((D1 * z + D2) * z + D3) * z + D4) * z + 1.0);
            }

            // error (f_(z) - x) divided by the cumulative's derivative
            r = (CumN0(z) - p) * Math.Sqrt(2.0) * Math.Sqrt(Math.PI) * Math.Exp(0.5 * z * z);

            // Halley's method
            z -= r / (1 + (0.5 * z * r));

            return mu + (z * sigma);
        }

        /// <summary>
        /// Cumulative for a N(0,1) distribution
        /// </summary>
        /// <returns>The n0.</returns>
        /// <param name="x">The x coordinate.</param>
        private static double CumN0(double x)
        {
            const double B1 = 0.319381530;
            const double B2 = -0.356563782;
            const double B3 = 1.781477937;
            const double B4 = -1.821255978;
            const double B5 = 1.330274429;
            const double P = 0.2316419;
            const double C = 0.39894228;

            if (x >= 0.0)
            {
                double t = 1.0 / (1.0 + (P * x));
                return (1.0 - C * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * B5 + B4) + B3) + B2) + B1));
            }
            else
            {
                double t = 1.0 / (1.0 - P * x);
                return (C * Math.Exp(-x * x / 2.0) * t * (t * (t * (t * (t * B5 + B4) + B3) + B2) + B1));
            }
        }
    }
}