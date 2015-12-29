using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRL.Common.Data;
using SmartCOM3Lib;

namespace TRL.Connect.Smartcom.Data
{
    public class SmartComHandlers:SmartComHandlersDatabase
    {
        private static SmartComHandlers theInstance = null;

        private SmartComHandlers()
        {
            base.Add<_IStClient_DisconnectedEventHandler>(OnDisconnected);
        }

        public static SmartComHandlers Instance
        {
            get
            {
                if (theInstance == null)
                    theInstance = new SmartComHandlers();

                return theInstance;
            }
        }

        /// <summary>
        /// Событие об успешном подсоединении к серверу SmartCOM.
        /// </summary>
        public event Action Connected;

        /// <summary>
        /// Событие об успешном отсоединении от сервера SmartCOM или о разрыве соединения.
        /// </summary>
        /// <remarks>
        /// Передаваемые параметры:
        /// <list type="number">
        /// <item>
        /// <description>Причина.</description>
        /// </item>
        /// </list>
        /// </remarks>
        //public event Action<Exception> Disconnected;

        /// <summary>
        /// Событие об успешном отсоединении от сервера SmartCOM или о разрыве соединения.
        /// </summary>
        /// <remarks>
        /// Передаваемые параметры:
        /// <list type="number">
        /// <item>
        /// <description>Причина.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public event Action<string> Disconnected;


        //public static void AddDisconnectedEventHandler(Delegate isDisconnected)
        //{
        //    //throw new NotImplementedException();
        //    Instance.Add<_IStClient_DisconnectedEventHandler>((_IStClient_DisconnectedEventHandler)isDisconnected);
        //}
        internal void OnDisconnected(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                description = description.Trim().ToLowerInvariant();

                switch (description)
                {
                    case "bad user name or password":
                        //description = LocalizedStrings.WrongLoginOrPassword;
                        description = "bad user name or password";
                        break;

                    //case "disconnected by user.":
                    case "disconnected by user":
                        //description = string.Empty;
                        description = "disconnected by user";
                        break;

                    case "timout detected. check your  internet connectivity or event handler code":
                        //description = LocalizedStrings.Str1876;
                        description = "timout detected. check your  internet connectivity or event handler code";
                        break;

                    case "winsock":
                        //description = LocalizedStrings.NetworkError;
                        description = "winsock";
                        break;
                }
            }
            Disconnected.Invoke(description);
        }
    }
}
