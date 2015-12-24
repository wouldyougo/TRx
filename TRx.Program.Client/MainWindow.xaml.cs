using System;
using System.Net.Http;
using System.Windows;
using Microsoft.AspNet.SignalR.Client;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
//using OxyPlot.Wpf;

using TRL.Common.Models;
using TRx.Helpers;
using TRL.Charting;
using System.Collections.Generic;

//Chartings
using MWC = System.Windows.Forms.DataVisualization.Charting;

namespace TRx.Program.Client
{   
    /// <summary>
    /// SignalR client hosted in a WPF application. The client
    /// lets the user pick a user name, connect to the server asynchronously
    /// to not block the UI thread, and send chat messages to all connected 
    /// clients whether they are hosted in WinForms, WPF, or a web application.
    /// For simplicity, MVVM will not be used for this sample.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This name is simply added to sent messages to identify the user; this 
        /// sample does not include authentication.
        /// </summary>
        public String UserName { get; set; }
        public IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://localhost:8080/signalr";
        //const string ServerURI = "http://localhost:30005/signalr";
        public HubConnection Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            HubProxy.Invoke("Send", UserName, TextBoxMessage.Text);
            TextBoxMessage.Text = String.Empty;
            TextBoxMessage.Focus();
        }

        /// <summary>
        /// Creates and connects the hub connection and hub proxy. This method
        /// is called asynchronously from SignInButton_Click.
        /// </summary>
        private async void ConnectAsync()
        {
            Connection = new HubConnection(ServerURI);
            Connection.Closed += Connection_Closed;
            HubProxy = Connection.CreateHubProxy("TradeHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            HubProxy.On<string, string>("AddMessage", (name, message) =>
                this.Dispatcher.Invoke(() =>
                    RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", name, message))
                )
            );
            HubProxy.On<string, string, string, string, string, string, string>("AddStringBar", (Symbol, DateTime, Open, High, Low, Close, Volume) =>
                this.Dispatcher.Invoke(() =>
                    rtbBarList.AppendText(String.Format("{0}: {1} O: {2} H: {3} L: {4} C: {5} V: {6}\r", Symbol, DateTime, Open, High, Low, Close, Volume))
                )
            );

            HubProxy.On<Bar>("AddBar", (Bar) =>
                this.Dispatcher.Invoke(() =>
                    NewBarHandler(Bar)
                )
            );

            HubProxy.On<Trade>("AddTrade", (trade) =>
                this.Dispatcher.Invoke(() =>
                    NewTradeHandler(trade)
                )
            );

            HubProxy.On<Order>("AddOrder", (order) =>
                this.Dispatcher.Invoke(() =>
                    NewOrderHandler(order)
                )
            );

            HubProxy.On<Signal>("AddSignal", (signal) =>
                this.Dispatcher.Invoke(() =>
                    NewSignalHandler(signal)
                )
            );

            HubProxy.On<double>("addDouble1", (signal) =>
                this.Dispatcher.Invoke(() =>
                    NewDouble1Handler(signal)
                )
            );

            HubProxy.On<double>("addDouble2", (signal) =>
                this.Dispatcher.Invoke(() =>
                    NewDouble2Handler(signal)
                )
            );

            HubProxy.On<ValueDouble>("addValueDouble1", (signal) =>
                this.Dispatcher.Invoke(() =>
                    NewValueDouble1Handler(signal)
                )
            );

            HubProxy.On<ValueDouble>("addValueDouble2", (signal) =>
                this.Dispatcher.Invoke(() =>
                    NewValueDouble2Handler(signal)
                )
            );

            HubProxy.On<ValueBool>("addValueBool", (signal) =>
                this.Dispatcher.Invoke(() =>
                    NewValueBoolHandler(signal)
                )
            );

            HubProxy.On<string>("AddCommand", (message) =>
                this.Dispatcher.Invoke(() => { 
                        RichTextBoxConsole.AppendText(String.Format("{0}\r", message));
                        NewCommandHandler(message);
                    }                    
                )
            );

            try
            {
                await Connection.Start();
            }
            catch (HttpRequestException)
            {
                StatusText.Content = "Unable to connect to server: Start server before connecting clients.";
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            //Show chat UI; hide login UI
            SignInPanel.Visibility = Visibility.Collapsed;
            ChatPanel.Visibility = Visibility.Visible;
            ButtonSend.IsEnabled = true;
            TextBoxMessage.Focus();
            RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + "\r");
        }

        private void NewValueBoolHandler(ValueBool item)
        {
            Console.WriteLine();
            RichTextBoxConsole.AppendText(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал {2}", item.DateTime, item.Value, item.Name));
            rtbIndicatorList.AppendText(  String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал {2}", item.DateTime, item.Value, item.Name));
            ChartLoadNewValueBool(item);
            //throw new NotImplementedException();
        }

        private void NewValueDouble2Handler(ValueDouble item)
        {
            //Выводим индикатор на текстбокс
            RichTextBoxConsole.AppendText(String.Format("{0} Ma2 {1}\r", item.DateTime, item.Value));
            rtbIndicatorList.AppendText(String.Format("{0} Ma2 {1}\r", item.DateTime, item.Value));
            ChartLoadNewValueDouble2(item);
        }

        private void NewValueDouble1Handler(ValueDouble item)
        {
            //Выводим индикатор на текстбокс
            RichTextBoxConsole.AppendText(String.Format("{0} Ma1 {1}\r", item.DateTime, item.Value));
            rtbIndicatorList.AppendText(String.Format("{0} Ma1 {1}\r", item.DateTime, item.Value));
            ChartLoadNewValueDouble1(item);
        }
        private void NewDouble2Handler(double item)
        {
            //Выводим индикатор на текстбокс
            RichTextBoxConsole.AppendText(String.Format("{0} Ma2 {1}\r", System.DateTime.Now, item));
            rtbIndicatorList.AppendText(String.Format("{0} Ma2 {1}\r", System.DateTime.Now, item));
            ChartLoadNewDouble2(item);
        }

        private void NewDouble1Handler(double item)
        {
            //Выводим индикатор на текстбокс
            RichTextBoxConsole.AppendText(String.Format("{0} Ma1 {1}\r", System.DateTime.Now, item));
            rtbIndicatorList.AppendText(String.Format("{0} Ma1 {1}\r", System.DateTime.Now, item));
            ChartLoadNewDouble1(item);
        }

        private void NewCommandHandler(string message)
        {
            if (message == "clearChart") {
                (this.OxyPlot.Model.Series[0] as LineSeries).Points.Clear();
                (this.OxyPlot.Model.Series[1] as LineSeries).Points.Clear();
                (this.OxyPlot.Model.Series[2] as LineSeries).Points.Clear();
                (this.OxyPlot.Model.Series[3] as LineSeries).Points.Clear();
                (this.OxyPlot.Model.Series[4] as LineSeries).Points.Clear();
                this.OxyPlot.InvalidatePlot(true);
                RichTextBoxConsole.AppendText(String.Format("{0}: {1}\r", "Command", message));
                
                // очищаем все свечки на графике msChart
                ClearChartSeriesPointsMS();
            }
        }

        /// <summary>
        /// If the server is stopped, the connection will time out after 30 seconds (default), and the 
        /// Closed event will fire.
        /// </summary>
        void Connection_Closed()
        {
            //Hide chat UI; show login UI
            var dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() => ChatPanel.Visibility = Visibility.Collapsed);
            dispatcher.Invoke(() => ButtonSend.IsEnabled = false);
            dispatcher.Invoke(() => StatusText.Content = "You have been disconnected.");
            dispatcher.Invoke(() => SignInPanel.Visibility = Visibility.Visible);
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            UserName = UserNameTextBox.Text;
            //Connect to server (use async method to avoid blocking UI thread)
            if (!String.IsNullOrEmpty(UserName))
            {     
                StatusText.Visibility = Visibility.Visible;
                StatusText.Content = "Connecting to server...";
                ConnectAsync();
            }
        }

        private void WPFClient_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
            }
        }

        private void WPFClient_Loaded(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.Text = "TradeClient.WPF";
            SignInButton_Click(this, null);
            
            this.OxyPlot.Model = (new OxyPlotViewModel()).plotModel;
            this.Plot0.Model = (new MainViewModel0()).MyModel;
            this.Plot1.Model = (new MainViewModel1()).MyModel;
            this.Plot3.Model = (new MainViewModel3()).MyModel;
            this.msChart.CreateChart();
            _chartForCandle = this.msChart.getChart;
        }

        private Random rand = new Random(0);
        private void btnAddPoint_Click(object sender, RoutedEventArgs e)
        {
            //(this.Plot1.Model.Series[0] as LineSeries).Points.Add(new DataPoint(10, 1));

            int Count = (this.Plot0.Model.Series[0] as LineSeries).Points.Count;
            var point = new DataPoint(Count, rand.Next(10));
            (this.Plot0.Model.Series[0] as LineSeries).Points.Add(point);
            
            this.Plot0.InvalidatePlot(true);
        }
        /// <summary>
        /// Обработчик появления нового бара
        /// </summary>
        /// <param name="Bar"></param>
        private void NewBarHandler(TRL.Common.Models.Bar Bar)
        {
            //Выводим бар на текстбокс
            RichTextBoxConsole.AppendText(String.Format("{0}\r", Bar.ToString()));
            rtbBarList.AppendText(String.Format("{0}\r", Bar.ToString()));

            //----------------------------------------
            //Добавляем поинт на график
            int Count = (this.OxyPlot.Model.Series[0] as LineSeries).Points.Count;
            var point = new DataPoint(Count, Bar.Open);
            (this.OxyPlot.Model.Series[0] as LineSeries).Points.Add(point);

            point = new DataPoint(Count, Bar.High);
            (this.OxyPlot.Model.Series[1] as LineSeries).Points.Add(point);

            point = new DataPoint(Count, Bar.Low);
            (this.OxyPlot.Model.Series[2] as LineSeries).Points.Add(point);

            point = new DataPoint(Count, Bar.Close);
            (this.OxyPlot.Model.Series[3] as LineSeries).Points.Add(point);            
            
            this.OxyPlot.InvalidatePlot(true);
            //----------------------------------------            
            _barList.Add(Bar);
            ChartLoadNewCandle(Bar);

        }

        /// <summary>
        /// Обработчик появления нового трейда
        /// </summary>
        /// <param name="Bar"></param>
        private void NewTradeHandler(TRL.Common.Models.Trade item)
        {
            RichTextBoxConsole.AppendText(String.Format("{0}\r", item.ToString()));
            rtbTradeList.AppendText(String.Format("{0}\r", item.ToString()));
            ChartLoadNewTrade(item);
            //----------------------------------------
            //Добавляем поинт на график
            //int Count = (this.OxyPlot.Model.Series[0] as LineSeries).Points.Count;
            //var point = new DataPoint(Count, Bar.Open);
            //(this.OxyPlot.Model.Series[0] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.High);
            //(this.OxyPlot.Model.Series[1] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.Low);
            //(this.OxyPlot.Model.Series[2] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.Close);
            //(this.OxyPlot.Model.Series[3] as LineSeries).Points.Add(point);

            //this.OxyPlot.InvalidatePlot(true);
            ////----------------------------------------            
            //_barList.Add(Bar);
            //LoadNewCandle(Bar);
        }

        /// <summary>
        /// Обработчик появления нового сигнала
        /// </summary>
        /// <param name="Bar"></param>
        private void NewSignalHandler(TRL.Common.Models.Signal item)
        {
            //Выводим бар на текстбокс
            rtbBarList.AppendText(String.Format("{0}\r", item.ToString()));

            //----------------------------------------
            //Добавляем поинт на график
            //int Count = (this.OxyPlot.Model.Series[0] as LineSeries).Points.Count;
            //var point = new DataPoint(Count, Bar.Open);
            //(this.OxyPlot.Model.Series[0] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.High);
            //(this.OxyPlot.Model.Series[1] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.Low);
            //(this.OxyPlot.Model.Series[2] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.Close);
            //(this.OxyPlot.Model.Series[3] as LineSeries).Points.Add(point);

            //this.OxyPlot.InvalidatePlot(true);
            ////----------------------------------------            
            //_barList.Add(Bar);
            //LoadNewCandle(Bar);
        }

        /// <summary>
        /// Обработчик появления нового сигнала
        /// </summary>
        /// <param name="Bar"></param>
        private void NewOrderHandler(TRL.Common.Models.Order item)
        {
            //Выводим бар на текстбокс
            rtbBarList.AppendText(String.Format("{0}\r", item.ToString()));

            //----------------------------------------
            //Добавляем поинт на график
            //int Count = (this.OxyPlot.Model.Series[0] as LineSeries).Points.Count;
            //var point = new DataPoint(Count, Bar.Open);
            //(this.OxyPlot.Model.Series[0] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.High);
            //(this.OxyPlot.Model.Series[1] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.Low);
            //(this.OxyPlot.Model.Series[2] as LineSeries).Points.Add(point);

            //point = new DataPoint(Count, Bar.Close);
            //(this.OxyPlot.Model.Series[3] as LineSeries).Points.Add(point);

            //this.OxyPlot.InvalidatePlot(true);
            ////----------------------------------------            
            //_barList.Add(Bar);
            //LoadNewCandle(Bar);
        }

        private void btnClearPoints_Click(object sender, RoutedEventArgs e)
        {
            ClearChartSeriesPointsMS();
            rtbBarList.Document.Blocks.Clear();
        }


        //-----------------------------------------------------------
        #region Charting
        //-----------------------------------------------------------
        //private System.Windows.Forms.DataVisualization.Charting.Chart _chartForCandle; // это для ms чарта
        private System.Windows.Forms.DataVisualization.Charting.Chart _chartForCandle { get; set; } // это для ms чарта
        private string _pathToHistory; // это адрес для строки с путём к  данным
        //private Bar[] _candleArray; // это массив свечек
        private IList<Bar> _barList = new List<Bar>(); // это массив свечек

        /// <summary>
        /// кнопка указать историю 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetFileHist_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog myDialog = new System.Windows.Forms.OpenFileDialog
            {
                // создаём стандартное спецОкошко для указания файла с историей
                CheckFileExists = true,
                Multiselect = false
            };

            myDialog.ShowDialog();

            if (myDialog.FileName != "") // если хоть что-то выбрано и это свечи
            {
                // здесь происходит сохранение адреса выбранного фала.
                // по хорошему надо бы здесь поставить проверку, что в нём лежит
                _pathToHistory = myDialog.FileName;
            }
        }

        /// <summary>
        /// кнопка медленная прорисовка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChartHistSlow_Click(object sender, RoutedEventArgs e)
        {
            btnChartHistFast.IsEnabled = false;
            btnChartHistSlow.IsEnabled = false;
            //ButtonRew.IsEnabled = true;

            // медленная прорисовка графика, по одной свечке

            if (_pathToHistory == null)
            {
                // если история ещё не подключена
                MessageBox.Show("Прежде чем прогрузить график, надо подгрузить историю");
                return;
            }
            // очищаем все свечки на графике
            ClearChartSeriesPointsMS();

            MWC.ChartArea candleArea = _chartForCandle.ChartAreas.FindByName("ChartAreaCandle");
            while (candleArea != null && candleArea.AxisX.ScaleView.IsZoomed)
            {
                // сбрасываем увеличение на чарте
                candleArea.AxisX.ScaleView.ZoomReset();
            }


            // вызываем метод прорисовки графика, но делаем это отдельным потоком, чтобы форма оставалась живой
            System.Threading.Thread worker = new System.Threading.Thread(SlowStart) { IsBackground = true };
            worker.Start();
        }

        private void SlowStart() // метод вызывающийся в новом потоке, для прорисовки графика
        {
            LoadCandleFromFile(_pathToHistory);
            LoadCandleOnChartSlow();
        }
        /// <summary>
        /// вызывает быструю прорисовку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChartHistFast_Click(object sender, RoutedEventArgs e)
        {
            btnChartHistFast.IsEnabled = false;
            btnChartHistSlow.IsEnabled = false;

            // быстрая прорисовка графика, по одной свечке

            if (_pathToHistory == null)
            {
                // если история ещё не подключена
                MessageBox.Show("Прежде чем прогрузить график, надо подгрузить историю");
                return;
            }
            // очищаем все свечки на графике
            ClearChartSeriesPointsMS();

            MWC.ChartArea candleArea = _chartForCandle.ChartAreas.FindByName("ChartAreaCandle");
            while (candleArea != null && candleArea.AxisX.ScaleView.IsZoomed)
            {
                // сбрасываем увеличение на чарте
                candleArea.AxisX.ScaleView.ZoomReset();
            }

            // вызываем метод прорисовки графика, но делаем это отдельным потоком, чтобы форма оставалась живой
            System.Threading.Thread worker = new System.Threading.Thread(FastStart) { IsBackground = true };
            worker.Start();
        }

        /// <summary>
        /// очищаем все свечки на графике
        /// </summary>
        private void ClearChartSeriesPointsMS()
        {
            _barList = new List<Bar>();
            foreach (MWC.Series series in _chartForCandle.Series)
            {                
                series.Points.Clear();
            }
        }
        private void FastStart()
        {
            LoadCandleFromFile(_pathToHistory);
            //LoadCandleOnChartFast();
            //var _candleArray = (TRL.Common.Models.Bar[])(this._barList);
            var _candleArray = (this._barList as List<Bar>).ToArray();
            this.msChart.LoadCandleOnChartFast(_candleArray);
            StopLoadHistory();
        }
        //-----------------------------------------------------------
        #region FastStart
        //-----------------------------------------------------------
        /// <summary>
        /// загрузить свечки из файла
        /// </summary>
        private void LoadCandleFromFile(string _pathToHistory)
        {
            try
            {
                // используем перехватчик исключений, т.к. файл может быть занят или содержать каку.

                int lenghtArray = 0;

                using (System.IO.StreamReader reader = new System.IO.StreamReader(_pathToHistory))
                {
                    // подсоединяемся к файлу
                    while (!reader.EndOfStream)
                    {
                        //считаем кол-во строк
                        lenghtArray++;
                        reader.ReadLine();
                    }
                }

                Bar[] newCandleArray = new Bar[lenghtArray];


                using (System.IO.StreamReader reader = new System.IO.StreamReader(_pathToHistory))
                {
                    // подсоединяемся к файлу

                    for (int iteratorArray = 0; iteratorArray < newCandleArray.Length; iteratorArray++)
                    {
                        // закачиваем свечки из файла в массив
                        //newCandleArray[iteratorArray] = new Bar();
                        //newCandleArray[iteratorArray].SetCandleFromString(reader.ReadLine());
                        //newCandleArray[iteratorArray] = TradeBar.Parse(reader.ReadLine());
                        newCandleArray[iteratorArray] = Bar.Parse(reader.ReadLine());
                    }
                }

                //_barList = newCandleArray; // сохраняем изменения
                _barList = new List<Bar>(newCandleArray);
            }
            catch (Exception error)
            {
                MessageBox.Show("Произошла ошибка при скачивании данных из файла. Ошибка: " + error);
            }
        }

        /// <summary>
        /// формируем серии данных
        /// </summary>
        private void LoadCandleOnChartFast()
        {
            // суть быстрой прогрузки в том, чтобы создать уже готовые серии данных и только потом
            // подгружать их на график. 
            MWC.Series candleSeries = new MWC.Series("Candle")
            {
                ChartType = MWC.SeriesChartType.Candlestick,// назначаем этой коллекции тип "Свечи"
                YAxisType = MWC.AxisType.Secondary,// назначаем ей правую линейку по шкале Y (просто для красоты)
                ChartArea = "ChartAreaCandle",// помещаем нашу коллекцию на ранее созданную область
                ShadowOffset = 2,  // наводим тень
                YValuesPerPoint = 4 // насильно устанавливаем число У точек для серии
            };
            var _candleArray = (this._barList as List<Bar>).ToArray();
            //var _candleArray = (TRL.Common.Models.Bar[])(this._barList);
            for (int i = 0; i < _candleArray.Length; i++)
            {
                // забиваем новую свечку
                candleSeries.Points.AddXY(i, _candleArray[i].Low, _candleArray[i].High, _candleArray[i].Open,
                    _candleArray[i].Close);

                // подписываем время
                candleSeries.Points[candleSeries.Points.Count - 1].AxisLabel =
                    _candleArray[i].DateTime.ToString(System.Globalization.CultureInfo.InvariantCulture);

                // разукрышиваем в привычные цвета
                if (_candleArray[i].Close > _candleArray[i].Open)
                {
                    candleSeries.Points[candleSeries.Points.Count - 1].Color = System.Drawing.Color.Green;
                }
                else
                {
                    candleSeries.Points[candleSeries.Points.Count - 1].Color = System.Drawing.Color.Red;
                }
            }

            MWC.Series volumeSeries = new MWC.Series("Volume")
            {
                ChartType = MWC.SeriesChartType.Column, // назначаем этой коллекции тип "Свечи"
                YAxisType = MWC.AxisType.Secondary,// назначаем ей правую линейку по шкале Y (просто для красоты) Везде ж так
                ChartArea = "ChartAreaVolume", // помещаем нашу коллекцию на ранее созданную область
                ShadowOffset = 2 // наводим тень
            };

            for (int i = 0; i < _candleArray.Length; i++)
            {
                volumeSeries.Points.AddXY(i, _candleArray[i].Volume);
                // разукрышиваем в привычные цвета
                if (volumeSeries.Points.Count > 1)
                {
                    if (volumeSeries.Points[volumeSeries.Points.Count - 2].YValues[0] < (double)_candleArray[i].Volume)
                    {
                        volumeSeries.Points[volumeSeries.Points.Count - 1].Color = System.Drawing.Color.Green;
                    }
                    else
                    {
                        volumeSeries.Points[volumeSeries.Points.Count - 1].Color = System.Drawing.Color.Red;
                    }
                }
            }

            SetSeries(candleSeries, volumeSeries);
            //chartMs.SetSeries(series1, series2);
            StopLoadHistory();
        }
        /// <summary>
        /// подгружает серии данных на график
        /// </summary>
        /// <param name="series1"></param>
        /// <param name="series2"></param>
        private void SetSeries(MWC.Series candleSeries, MWC.Series volumeSeries)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<MWC.Series, MWC.Series>(SetSeries), candleSeries, volumeSeries);
                return;
            }

            _chartForCandle.Series.Clear(); // убираем с нашего графика все до этого созданные серии с данными

            _chartForCandle.Series.Add(candleSeries);
            _chartForCandle.Series.Add(volumeSeries);

            MWC.ChartArea candleArea = _chartForCandle.ChartAreas.FindByName("ChartAreaCandle");
            if (candleArea != null && candleArea.AxisX.ScrollBar.IsVisible)
            // если уже выбран какой-то диапазон
            {
                // сдвигаем представление вправо
                candleArea.AxisX.ScaleView.Scroll(_chartForCandle.ChartAreas[0].AxisX.Maximum);
            }
            ChartResize();
            _chartForCandle.Refresh();
        }
        /// <summary>
        /// вызываем при завершении прорисовки
        /// </summary>
        private void StopLoadHistory()
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action(StopLoadHistory));
                return;
            }

            if (_chartForCandle.Visible == false)
            {
                _chartForCandle.Visible = true;
                _chartForCandle.Refresh();
            }

            btnChartHistFast.IsEnabled = true;
            btnChartHistSlow.IsEnabled = true;
            //ButtonRew.IsEnabled = false;
        }
        /// <summary>
        /// устанавливает границы представления по оси У
        /// </summary>
        private void ChartResize()
        {
            // вообще-то можно это автоматике доверить, но там вечно косяки какие-то, поэтому лучше самому следить за всеми осями
            try
            {
                //var _candleArray = (this._candleArray as List<Bar>).ToArray();
                //var _candleArray = (TRL.Common.Models.Bar[])(this._barList);
                TRL.Common.Models.Bar[] _candleArray = (this._barList as List<Bar>).ToArray();                                
                
                if (_candleArray == null)
                {
                    return;
                }
                if (_candleArray.Length == 0)
                {
                    return;
                }
                // свечи
                MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
                MWC.ChartArea candleArea = _chartForCandle.ChartAreas.FindByName("ChartAreaCandle");

                if (candleArea == null ||
                    candleSeries == null)
                {
                    return;
                }

                int startPozition = 0; // первая отображаемая свеча
                int endPozition = candleSeries.Points.Count; // последняя отображаемая свеча

                if (_chartForCandle.ChartAreas[0].AxisX.ScrollBar.IsVisible)
                {
                    // если уже выбран какой-то диапазон, назначаем первую и последнюю исходя из этого диапазона

                    startPozition = Convert.ToInt32(candleArea.AxisX.ScaleView.Position);
                    endPozition = Convert.ToInt32(candleArea.AxisX.ScaleView.Position) +
                                  Convert.ToInt32(candleArea.AxisX.ScaleView.Size);
                }

                //candleArea.AxisY2.Maximum = (double)GetMaxValueOnChart(_candleArray, startPozition, endPozition);
                //candleArea.AxisY2.Minimum = (double)GetMinValueOnChart(_candleArray, startPozition, endPozition);


                candleArea.AxisY.Maximum = (double)GetMaxValueOnChart(_candleArray, startPozition, endPozition);
                candleArea.AxisY.Minimum = (double)GetMinValueOnChart(_candleArray, startPozition, endPozition);

                // объёмы
                MWC.Series volumeSeries = _chartForCandle.Series.FindByName("Volume");
                MWC.ChartArea volumeArea = _chartForCandle.ChartAreas.FindByName("ChartAreaVolume");

                if (volumeSeries != null &&
                    volumeArea != null)
                {
                    //volumeArea.AxisY2.Maximum = GetMaxVolume(_candleArray, startPozition, endPozition);
                    //volumeArea.AxisY2.Minimum = 0;
                    volumeArea.AxisY.Maximum = GetMaxVolume(_candleArray, startPozition, endPozition);
                    volumeArea.AxisY.Minimum = 0;
                }

                _chartForCandle.Refresh();
            }
            catch (Exception error)
            {
                MessageBox.Show("Обибка при изменении ширины представления. Ошибка: " + error);
            }
        }
        /// <summary>
        /// берёт минимальное значение из массива свечек
        /// </summary>
        /// <param name="book"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private double GetMinValueOnChart(Bar[] book, int start, int end)
        {
            double result = double.MaxValue;

            for (int i = start; i < end && i < book.Length; i++)
            {
                if (book[i].Low < result)
                {
                    result = book[i].Low;
                }
            }

            return result;
        }

        /// <summary>
        /// берёт максимальное значение из массива свечек
        /// </summary>
        /// <param name="book"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private double GetMaxValueOnChart(Bar[] book, int start, int end)
        {
            double result = 0;

            for (int i = start; i < end && i < book.Length; i++)
            {
                if ((double)book[i].High > result)
                {
                    result = (double)book[i].High;
                }
            }

            return result;
        }
        /// <summary>
        /// берёт максимальное значение объёма за период
        /// </summary>
        /// <param name="book"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private double GetMaxVolume(Bar[] book, int start, int end)
        {
            double result = double.MinValue;

            for (int i = start; i < end && i < book.Length; i++)
            {
                if ((double)book[i].Volume > result)
                {
                    result = (double)book[i].Volume;
                }
            }

            return result;
        }
        //-----------------------------------------------------------
        #endregion
        //-----------------------------------------------------------

        //-----------------------------------------------------------
        #region Slow
        //-----------------------------------------------------------
        /// <summary>
        /// прогрузить свечки на график по одной
        /// </summary>
        private void LoadCandleOnChartSlow()
        {
            var _candleArray = (this._barList as List<Bar>).ToArray();
            //var _candleArray = (TRL.Common.Models.Bar[])(this._barList);

            if (_candleArray == null)
            {
                //если наш массив пуст по каким-то причинам
                return;
            }

            for (int i = 0; i < _candleArray.Length; i++)
            {
                // отправляем наш массив по свечкам на прорисовку
                System.Threading.Thread.Sleep(1);
                // спим 5ть миллисекунд между свечками, чтобы форма не висела и могла отвечать на запросы пользователя 
                //LoadNewCandle(_candleArray[i], i);
                ChartLoadNewCandle(_candleArray[i]);
            }

            StopLoadHistory();

        }
        /// <summary>
        /// добавить одну свечу на график
        /// </summary>
        /// <param name="newCandle"></param>
        /// <param name="index"></param>
        private void ChartLoadNewCandle(Bar newCandle, int numberInArray)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<Bar, int>(ChartLoadNewCandle), newCandle, numberInArray);
                return;
            }
            // свечи
            MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            //System.Windows.Forms.DataVisualization.Charting

            if (candleSeries != null)
            {
                // забиваем новую свечку
                candleSeries.Points.AddXY(numberInArray, newCandle.Low, newCandle.High, newCandle.Open, newCandle.Close);

                // подписываем время
                candleSeries.Points[candleSeries.Points.Count - 1].AxisLabel =
                    newCandle.DateTime.ToString(System.Globalization.CultureInfo.InvariantCulture);

                // разукрышиваем в привычные цвета
                if (newCandle.Close > newCandle.Open)
                {
                    candleSeries.Points[candleSeries.Points.Count - 1].Color = System.Drawing.Color.Green;
                }
                else
                {
                    candleSeries.Points[candleSeries.Points.Count - 1].Color = System.Drawing.Color.Red;
                }

                MWC.ChartArea candleArea = _chartForCandle.ChartAreas.FindByName("ChartAreaCandle");
                if (candleArea != null && candleArea.AxisX.ScrollBar.IsVisible) // если уже выбран какой-то диапазон
                {
                    // сдвигаем представление вправо
                    candleArea.AxisX.ScaleView.Scroll(_chartForCandle.ChartAreas[0].AxisX.Maximum);
                }
            }
            // объём
            MWC.Series volumeSeries = _chartForCandle.Series.FindByName("Volume");

            if (volumeSeries != null)
            {
                volumeSeries.Points.AddXY(numberInArray, newCandle.Volume);
                // разукрышиваем в привычные цвета
                if (volumeSeries.Points.Count > 1)
                {
                    if (volumeSeries.Points[volumeSeries.Points.Count - 2].YValues[0] < (double)newCandle.Volume)
                    {
                        volumeSeries.Points[volumeSeries.Points.Count - 1].Color = System.Drawing.Color.Green;
                    }
                    else
                    {
                        volumeSeries.Points[volumeSeries.Points.Count - 1].Color = System.Drawing.Color.Red;
                    }
                }
            }

            ChartResize(); // Выводим нормальные рамки
        }

        /// <summary>
        /// добавить одну свечу на график
        /// </summary>
        /// <param name="newCandle"></param>
        /// <param name="index"></param>
        private void ChartLoadNewCandle(Bar newCandle)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<Bar>(ChartLoadNewCandle), newCandle);
                return;
            }
            // свечи
            MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            //System.Windows.Forms.DataVisualization.Charting

            if (candleSeries != null)
            {
                // забиваем новую свечку
                candleSeries.Points.AddXY(candleSeries.Points.Count, newCandle.Low, newCandle.High, newCandle.Open, newCandle.Close);

                // подписываем время
                candleSeries.Points[candleSeries.Points.Count - 1].AxisLabel =
                    newCandle.DateTime.ToString(System.Globalization.CultureInfo.InvariantCulture);

                // разукрышиваем в привычные цвета
                if (newCandle.Close > newCandle.Open)
                {
                    candleSeries.Points[candleSeries.Points.Count - 1].Color = System.Drawing.Color.Green;
                }
                else
                {
                    candleSeries.Points[candleSeries.Points.Count - 1].Color = System.Drawing.Color.Red;
                }

                MWC.ChartArea candleArea = _chartForCandle.ChartAreas.FindByName("ChartAreaCandle");
                if (candleArea != null && candleArea.AxisX.ScrollBar.IsVisible) // если уже выбран какой-то диапазон
                {
                    // сдвигаем представление вправо
                    candleArea.AxisX.ScaleView.Scroll(_chartForCandle.ChartAreas[0].AxisX.Maximum);
                }
            }
            // объём
            MWC.Series volumeSeries = _chartForCandle.Series.FindByName("Volume");

            if (volumeSeries != null)
            {
                volumeSeries.Points.AddXY(volumeSeries.Points.Count, newCandle.Volume);
                // разукрышиваем в привычные цвета
                if (volumeSeries.Points.Count > 1)
                {
                    if (volumeSeries.Points[volumeSeries.Points.Count - 2].YValues[0] < (double)newCandle.Volume)
                    {
                        volumeSeries.Points[volumeSeries.Points.Count - 1].Color = System.Drawing.Color.Green;
                    }
                    else
                    {
                        volumeSeries.Points[volumeSeries.Points.Count - 1].Color = System.Drawing.Color.Red;
                    }
                }
            }

            ChartResize(); // Выводим нормальные рамки
        }

        private void ChartLoadNewValueDouble2(ValueDouble item)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<ValueDouble>(ChartLoadNewValueDouble2), item);
                return;
            }
            // свечи
            //MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            MWC.Series series1 = _chartForCandle.Series.FindByName("Ma2");

            //if ((series1 != null)&&(candleSeries != null))
            //{
            //    while (candleSeries.Points.Count - series1.Points.Count > 1)
            //    {
            //        //series1.Points.AddXY(series1.Points.Count, item);
            //        series1.Points.AddXY(series1.Points.Count, double.NaN);
            //    }
            //}

            if (series1 != null)
            {
                // забиваем новую точку
                //series1.Points.AddXY(series1.Points.Count, item);
                series1.Points.AddXY(item.Id, item.Value);
            }
            ChartResize(); // Выводим нормальные рамки
        }

        private void ChartLoadNewValueDouble1(ValueDouble item)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<ValueDouble>(ChartLoadNewValueDouble1), item);
                return;
            }
            // свечи
            //MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            MWC.Series series1 = _chartForCandle.Series.FindByName("Ma1");

            //if ((series1 != null)&&(candleSeries != null))
            //{
            //    while (candleSeries.Points.Count - series1.Points.Count > 1)
            //    {
            //        //series1.Points.AddXY(series1.Points.Count, item);
            //        series1.Points.AddXY(series1.Points.Count, double.NaN);
            //    }
            //}

            if (series1 != null)
            {
                // забиваем новую точку
                //series1.Points.AddXY(series1.Points.Count, item);
                series1.Points.AddXY(item.Id, item.Value);
            }
            ChartResize(); // Выводим нормальные рамки
        }
        private void ChartLoadNewValueBool(ValueBool item)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<ValueBool>(ChartLoadNewValueBool), item);
                return;
            }
            // свечи
            MWC.Series series1 = _chartForCandle.Series.FindByName("Signal1");

            if ((series1 != null))
            {
                while (item.Id - series1.Points.Count  > 0)
                {
                    series1.Points.AddXY(series1.Points.Count, double.NaN);
                }
            }
            if (series1 != null)
            {
                // забиваем новую точку
                //series1.Points.AddXY(series1.Points.Count, item);
                int val = item.Value == true ? 1 : 0;
                series1.Points.AddXY(item.Id, val);
            }
            ChartResize(); // Выводим нормальные рамки
        }

        private void ChartLoadNewDouble1(double item)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<double>(ChartLoadNewDouble1), item);
                return;
            }
            
            // свечи
            MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            MWC.Series series1 = _chartForCandle.Series.FindByName("Ma1");

            //if ((series1 != null)&&(candleSeries != null))
            //{
            //    while (candleSeries.Points.Count - series1.Points.Count > 1)
            //    {
            //        //series1.Points.AddXY(series1.Points.Count, item);
            //        series1.Points.AddXY(series1.Points.Count, double.NaN);
            //    }
            //}

            if (series1 != null)
            {
                // забиваем новую точку
                series1.Points.AddXY(series1.Points.Count, item);
            }
            ChartResize(); // Выводим нормальные рамки
        }

        private void ChartLoadNewDouble2(double item)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<double>(ChartLoadNewDouble2), item);
                return;
            }

            // свечи
            MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            MWC.Series series1 = _chartForCandle.Series.FindByName("Ma2");

            //if ((series1 != null) && (candleSeries != null))
            //{
            //    while (candleSeries.Points.Count - series1.Points.Count > 1)
            //    {
            //        //series1.Points.AddXY(series1.Points.Count, item);
            //        series1.Points.AddXY(series1.Points.Count, double.NaN);
            //    }
            //}

            if (series1 != null)
            {
                // забиваем новую точку
                series1.Points.AddXY(series1.Points.Count, item);
            }
            ChartResize(); // Выводим нормальные рамки
        }

        private void ChartLoadNewTrade(Trade item)
        {
            if (!CheckAccess())
            {
                // перезаходим в метод потоком формы, чтобы не было исключения
                Dispatcher.Invoke(new Action<Trade>(ChartLoadNewTrade), item);
                return;
            }

            // свечи
            MWC.Series candleSeries = _chartForCandle.Series.FindByName("Candle");
            MWC.Series series1;
            if (item.Buy)
            {
                series1 = _chartForCandle.Series.FindByName("Trade");
            }
            else 
            {
                series1 = _chartForCandle.Series.FindByName("TradeX");
            }
            

            if ((series1 != null) && (candleSeries != null))
            {
                while (candleSeries.Points.Count - series1.Points.Count > 1)
                {
                    //series1.Points.AddXY(series1.Points.Count, item);
                    series1.Points.AddXY(series1.Points.Count, double.NaN);
                }
            }

            if (series1 != null)
            {
                // забиваем новую точку
                series1.Points.AddXY(series1.Points.Count, item.Price);
            }
            ChartResize(); // Выводим нормальные рамки
        }

        //-----------------------------------------------------------
        #endregion
        //-----------------------------------------------------------

        //-----------------------------------------------------------
        #endregion
        //-----------------------------------------------------------
    }
}

/*
                    <Button x:Name="btnCreate" Content="Create" HorizontalAlignment="Left" Margin="10,10,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnConnect" Content="Connect" HorizontalAlignment="Left" Margin="10,60,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnDisconnect" Content="Disconnect" HorizontalAlignment="Left" Margin="10,231,0,0" VerticalAlignment="Top" Width="75"/>
                    <TextBox x:Name="tbServer" HorizontalAlignment="Left" Height="20" Margin="114,10,0,0" TextWrapping="Wrap" Text="mxdemo.ittrade.ru" VerticalAlignment="Top" Width="120"/>
                    <TextBox x:Name="tbLogin" HorizontalAlignment="Left" Height="20" Margin="114,60,0,0" TextWrapping="Wrap" Text="H8J28UGS" VerticalAlignment="Top" Width="120"/>
                    <PasswordBox x:Name="tbPassword" HorizontalAlignment="Left" Margin="114,85,0,0" VerticalAlignment="Top" Width="120" Height="20" Password="PZ542H"/>
                    <TextBox x:Name="tbServerPort" HorizontalAlignment="Left" Height="20" Margin="114,35,0,0" TextWrapping="Wrap" Text="8443" VerticalAlignment="Top" Width="120"/>
                    <TextBox x:Name="tbPortfolio" HorizontalAlignment="Left" Margin="281,35,0,0" TextWrapping="Wrap" Text="Портфель" VerticalAlignment="Top" Width="120" Height="20"/>
                    <TextBox x:Name="tbTicker" HorizontalAlignment="Left" Height="20" Margin="281,60,0,0" TextWrapping="Wrap" Text="SBRF-6.15_FT" VerticalAlignment="Top" Width="120"/>
                    <Button x:Name="btnStart" Content="Start" HorizontalAlignment="Left" Margin="426,85,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnStop" Content="Stop" HorizontalAlignment="Left" Margin="426,110,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnClearLog" Content="ClearLog" HorizontalAlignment="Left" Margin="10,206,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnGetPortfolio" Content="Portfolio" HorizontalAlignment="Left" Margin="426,35,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnGetBarsHist" Content="Bars" HorizontalAlignment="Left" Margin="426,60,0,0" VerticalAlignment="Top" Width="75"/>
                    <TextBox x:Name="tbCountHist" HorizontalAlignment="Left" Height="20" Margin="281,110,0,0" TextWrapping="Wrap" Text="60" VerticalAlignment="Top" Width="120"/>
                    <TextBox x:Name="tbInterval" HorizontalAlignment="Left" Height="20" Margin="281,85,0,0" TextWrapping="Wrap" Text="1" VerticalAlignment="Top" Width="120"/>
                    <DatePicker HorizontalAlignment="Left" Margin="281,135,0,0" VerticalAlignment="Top" Width="120"/>
                    <Button x:Name="btnGetTickHist" Content="TickHist" HorizontalAlignment="Left" Margin="426,135,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnGetTick" Content="ListenTicks" HorizontalAlignment="Left" Margin="426,160,0,0" VerticalAlignment="Top" Width="75"/>

                    <Button x:Name="btnStartTickGen" Content="StartTickGen" HorizontalAlignment="Left" Margin="426,186,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnStopTickGen" Content="StopTickGen" HorizontalAlignment="Left" Margin="426,211,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnGetFileHist" Content="История" HorizontalAlignment="Left" Margin="536,10,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnChartHistSlow" Content="ShowSlow" HorizontalAlignment="Left" Margin="536,35,0,0" VerticalAlignment="Top" Width="75"/>
                    <Button x:Name="btnChartHistFast" Content="ShowFast" HorizontalAlignment="Left" Margin="536,60,0,0" VerticalAlignment="Top" Width="75"/>

                     <TextBox x:Name="tbLog"  
                            TextWrapping="Wrap"
                            AcceptsReturn="True"
                            VerticalScrollBarVisibility="Visible" 
                            SpellCheck.IsEnabled="True"/> 
 */
