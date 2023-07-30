using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    public class Topic : IDestination
    {
        
        private bool isReceiveAllDeliverToOne;

        private string name;

        private bool temporary;


        public Topic(string name, bool temporary, bool isReceiveAllDeliverToOne)
        {
            this.name = name;
            this.temporary = temporary;
            this.isReceiveAllDeliverToOne = isReceiveAllDeliverToOne;
        }

        public bool Temporary { get => temporary; set => temporary = value; }
        public bool IsReceiveAllDeliverToOne { get => isReceiveAllDeliverToOne; set => isReceiveAllDeliverToOne = value; }
        public string Name { get => name; set => name = value; }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
