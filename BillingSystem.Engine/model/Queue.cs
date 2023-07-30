using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    public class Queue : IQueue
    {
        private bool disposedValue;

        private string name;

        private bool temporary;

        private bool durable;

        public string Name { get => name; set => name = value; }

       public bool Temporary { get => temporary; set => temporary = value; }

        public bool Durable { get => durable; set => durable = value; }        
        
        public bool DisposedValue { get => disposedValue; set => disposedValue = value; }



        public Queue(String name, bool temporary, bool durable) 
        {
            Name = name;
            Temporary = temporary;
            Durable = durable;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                DisposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Queue()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
