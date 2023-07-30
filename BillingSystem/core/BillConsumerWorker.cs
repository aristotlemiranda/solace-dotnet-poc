using BillingSystem.Engine.model;
using Microsoft.Extensions.Hosting;
using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BillingSystem.core
{

    public class BillConsumerWorker : MessageListener
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<BillConsumerWorker> _logger;

        public BillConsumerWorker(IHostApplicationLifetime hostApplicationLifetime, ILogger<BillConsumerWorker> logger)
        {
            // this._hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            // _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            SETUP();
        }

        protected void SETUP() {
            Connection<BillConsumerWorker> connection = new Connection<BillConsumerWorker>(this, LoadSessionProperties(), DefineQueue());
            connection.InitiateComponent();
            connection.CreateConsumer();
        }

        public BaseSessionProperty LoadSessionProperties() {
            BaseSessionProperty sessionProperties = new BaseSessionProperty("app-demo", "appdemo", "default", "localhost");
            
            return sessionProperties;
        }


        public Queue DefineQueue()
        {
            Queue queue = new Queue("Q/tutorial", false, true);
            return queue;
        }

        public override void ProcessMessage(string message)
        {
            Console.WriteLine("BillConsumerWorker => Message was received successfully => {0}", message); 
            if (message.Equals("STOP"))
            {
                throw new Exception("Oops cannot process!!!");
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Main line");
            BillConsumerWorker worker = new BillConsumerWorker(null, null);
            worker.SETUP();
        }

    }
}
