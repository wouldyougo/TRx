using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using TRL.Common.Models;
using TRx.Helpers;

namespace TRx.Trader
{
    /// <summary>
    /// Echoes messages sent using the Send message by calling the
    /// addMessage method on the client. Also reports to the console
    /// when clients connect and disconnect.
    /// </summary>
    
    public class TradeHub : Microsoft.AspNet.SignalR.Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
            Console.WriteLine(String.Format("{0}: {1}" + Environment.NewLine, name, message));
        }
        public override Task OnConnected()
        {
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Client disconnected: " + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
    /// <summary>
    /// Used by OWIN's startup process. 
    /// </summary>
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class TradeHubStarter
    {
        public IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:8080";

        /// <summary>
        /// Starts the server and checks for error thrown when another server is already 
        /// running. This method is called asynchronously from Button_Start.
        /// </summary>
        public void StartServer()
        {
            try
            {
                SignalR = Microsoft.Owin.Hosting.WebApp.Start(ServerURI);
                timerStart();
            }
            catch (TargetInvocationException)
            {
                Console.WriteLine("Server failed to start. A server is already running on " + ServerURI);
                return;
            }
            Console.WriteLine("Server started at " + ServerURI);
        }

        /// <summary>
        /// Таймер
        /// </summary>
        //private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private static System.Timers.Timer timer;
        //public static bool ConsoleWriteTime = true;
        public static bool ConsoleWriteTime = false;

        /// <summary>
        /// Старт таймера
        /// </summary>
        private void timerStart()
        {
            //timer.Interval = new TimeSpan(0, 0, 5);
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
            timer = new System.Timers.Timer(10000);
            // Hook up the Elapsed event for the timer.
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            // Set the Interval to 2 seconds (2000 milliseconds).
            timer.Interval = 5000;
            timer.Enabled = true;
        }

        /// <summary>
        /// Тик таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addMessage("Server", DateTime.Now.ToLongTimeString());
            //context.Clients.All.addMessage1("Server");
            if (ConsoleWriteTime)
                //Console.WriteLine(DateTime.Now.ToLongTimeString());
                Console.WriteLine(DateTime.Now.ToString());
        }

        /// <summary>
        /// Отправить бар стрками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void sendBarString(Bar bar)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addStringBar(  bar.Symbol, 
                                                bar.DateTime, 
                                                bar.Open,
                                                bar.High,
                                                bar.Low,
                                                bar.Close,
                                                bar.Volume
                                                );
        }

        /// <summary>
        /// Отправить бар
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void sendBar(Bar item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addBar(item);
        }
        /// <summary>
        /// Отправить ордер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void sendOrder(Order item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addOrder(item);
            //Console.WriteLine(item.ToString());
        }

        /// <summary>
        /// Отправить сигнал
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void sendSignal(Signal item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addSignal(item);
            //Console.WriteLine(item.ToString());
        }

        /// <summary>
        /// Отправить трейд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void sendTrade(Trade item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addTrade(item);
            //Console.WriteLine(item.ToString());
        }

        public static void clearChart()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addCommand("clearChart");
        }
        //public static void sendDouble1(double item)
        //{
        //    var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
        //    context.Clients.All.addDouble1(item);
        //    //Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал Indicator1", DateTime.Now, item));
        //}
        //public static void sendDouble2(double item)
        //{
        //    var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
        //    context.Clients.All.addDouble2(item);
        //    //Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал Indicator2", DateTime.Now, item));
        //}
        //public static void sendIndicator1(double item)
        //{
        //    var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
        //    context.Clients.All.addIndicator1(item);
        //    //Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал Indicator1", DateTime.Now, item));
        //}
        public static void sendValueDouble1(ValueDouble item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addValueDouble1(item);
            //Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал Indicator1", DateTime.Now, item));
        }
        public static void sendValueDouble2(ValueDouble item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addValueDouble2(item);
            //Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал Indicator1", DateTime.Now, item));
        }
        public static void sendValueBool(ValueBool item)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TradeHub>();
            context.Clients.All.addValueBool(item);
            //Console.WriteLine(String.Format("{0:dd/MM/yyyy H:mm:ss.fff}, {1}, сигнал {2}", item.DateTime, item.Value, item.Name));
        }

    }
}
