using BillingSystem.Engine.model;
using Microsoft.Extensions.Hosting;
using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;

namespace BillingSystem.core
{

    public class BillConsumerWorker : MessageListener
    {
        
        AppSettings appSettings = new AppSettings();
        public BillConsumerWorker()
        {       
            SETUP();
        }

        protected void SETUP() {
            Connection<BillConsumerWorker> connection = new Connection<BillConsumerWorker>(this, LoadSessionProperties(), DefineQueue());
            connection.InitiateComponent();
            connection.CreateConsumer();
        }

        public BaseSessionProperty LoadSessionProperties() {
            string currentDir = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine("Current directory {0}", currentDir);
            string fileSettings = currentDir + "\\Properties\\appsettings.json";
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile(fileSettings)
                .AddEnvironmentVariables()
                .Build();

            // Get values from the config given their key and their target type.
            //Settings settings = config.GetRequiredSection("Settings").Get<Settings>();
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            appSettings = config.GetRequiredSection("Settings").Get<AppSettings>();
            ;
            if (appSettings == null)
            {
                // Write the values to the console.
                throw new ApplicationException("Could not load AppSettings");
            }
           

            Console.WriteLine("App settings Username: {0}", appSettings.BrokerUserName);
            BaseSessionProperty sessionProperties = new BaseSessionProperty(appSettings.BrokerUserName, appSettings.BrokerPassword, appSettings.BrokerVPNName, appSettings.Host, appSettings);
           
                       
            return sessionProperties;
        }


        public Queue DefineQueue()
        {
            Queue queue = new Queue(appSettings.QueueNameDemo, false, true);
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
            BillConsumerWorker worker = new BillConsumerWorker();
            worker.SETUP();
        }

    }
}
