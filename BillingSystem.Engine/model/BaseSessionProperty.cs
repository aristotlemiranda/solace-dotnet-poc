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

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string VpnName { get => vpnName; set => vpnName = value; }
        public string Host { get => host; set => host = value; }

        #pragma warning disable 
        public BaseSessionProperty(string userName, string password, string vpnName, string host)
        {
            UserName = userName;
            Password = password;
            VpnName = vpnName;
            Host = host;
        }


    }
}
