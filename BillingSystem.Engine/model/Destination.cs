using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    /// <summary>
    /// Abstract class for a Solace Topic or Queue destination.
    /// </summary>
    public abstract class Destination 
    {
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SolaceDotNetWrapper.Core.Destination"/> class.
        /// </summary>
        /// <param name="name">Name of the topic or queue destintation.</param>
        /// <param name="isTemporary">Define if the destination is temporary or not.</param>
        public Destination(string name, bool isTemporary = false)
        {
            this.Name = name;
            this.IsTemporary = false;
        }

        /// <summary>
        /// Name of the Solace destination.
        /// </summary>
        /// <value>The Solace destination name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:SolaceDotNetWrapper.Core.Destination"/> 
        /// is a temporary destination.
        /// </summary>
        /// <value><c>true</c> if is temporary; otherwise, <c>false</c>.</value>
        public bool IsTemporary { get; set; }

      
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Destination()
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
