using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    public class BaseSessionProperty
    {
        private string userName;
        private string password;
        private string vpnName;
        private string host;
        private AppSettings appSettings;

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string VpnName { get => vpnName; set => vpnName = value; }
        public string Host { get => host; set => host = value; }
        public AppSettings AppSettings { get => appSettings; set => appSettings = value; }


#pragma warning disable
        public BaseSessionProperty(string userName, string password, string vpnName, string host, AppSettings appSettings)
        {
            UserName = userName;
            Password = password;
            VpnName = vpnName;
            Host = host;
            AppSettings = appSettings;
        }


    }
}
