using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    public interface Consumer

    {
        public void ProcessMessage(string msg);

      
    }
}
