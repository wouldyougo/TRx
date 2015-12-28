using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
//using SmartCOM3Lib;
using SmartCOM3Lib;
using TRL.Common;

namespace TRL.Connect.Smartcom
{
    public class SmartCom
    {
        //StServerClass SmartCOM = new SmartCOM3Lib.StServerClass(); 
        private static SmartCOM3Lib.StServerClass stServer = null;        

        public static StServer Instance
        {            
            get
            {
                if (stServer == null)
                {
                    stServer = new StServerClass();
                    stServer.ConfigureClient(GetClientConfigurationString());
                    stServer.ConfigureServer(GetServerConfigurationString());
                }                    
                return stServer;
            }
        }

        public static void Destroy()
        {
            if (stServer is StServerClass)
                Marshal.ReleaseComObject(stServer);

            stServer = null;
        }

        public static bool IsNull
        {
            get
            {
                return stServer == null;
            }
        }

        private SmartCom(){}

        public static string GetServerLogLevelConfigurationString()
        {
            try
            {
                return String.Format("logLevel={0}", AppSettings.GetValue<int>("ServerLogLevel"));
            }
            catch
            {
                return "logLevel=2";
            }
        }

        public static string GetServerLogFilePathConfigurationString()
        {
            try
            {
                return String.Format("logFilePath={0}", AppSettings.GetStringValue("ServerLogFilePath"));
            }
            catch
            {
                return "logFilePath=.";
            }
        }

        public static string GetClientLogLevelConfigurationString()
        {
            try
            {
                return String.Format("logLevel={0}", AppSettings.GetValue<int>("ClientLogLevel"));
            }
            catch
            {
                return "logLevel=2";
            }
        }

        public static string GetClientLogFilePathConfigurationString()
        {
            try
            {
                return String.Format("logFilePath={0}", AppSettings.GetStringValue("ClientLogFilePath"));
            }
            catch
            {
                return "logFilePath=.";
            }
        }

        public static string GetClientConfigurationString()
        {
            return String.Concat(GetClientLogFilePathConfigurationString(),
                ";",
                GetClientLogLevelConfigurationString());
        }

        public static string GetServerConfigurationString()
        {
            return String.Concat(GetServerLogFilePathConfigurationString(),
                ";",
                GetServerLogLevelConfigurationString());
        }
    }
}
