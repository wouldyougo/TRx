using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TRL.Common
{
    public class ConnectionCredentials
    {
        private string host;
        public string Host
        {
            get { return this.host; }
        }
        private short port;
        public short Port
        {
            get { return this.port; }
        }
        private string login;
        public string Login
        {
            get { return this.login; }
        }
        private string password;
        public string Password
        {
            get { return this.password; }
        }

        public ConnectionCredentials()
        {
            this.host = AppSettings.GetStringValue("Host");
            this.port = AppSettings.GetValue<short>("Port");
            this.login = AppSettings.GetStringValue("Login");
            this.password = AppSettings.GetStringValue("Password");
        }
    }
}
