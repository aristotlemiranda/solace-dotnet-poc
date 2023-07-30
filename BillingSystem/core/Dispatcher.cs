using BillingSystem.Engine.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.core
{
    internal class Dispatcher
    {

        Connection<Dispatcher> Publisher;

        public Dispatcher() 
        {
            SETUP();
        }

        protected void SETUP()
        {
            Publisher = new Connection<Dispatcher>(LoadSessionProperties(), DefineTopic());
            Publisher.InitiateComponent();            
        }

        public BaseSessionProperty LoadSessionProperties()
        {
            BaseSessionProperty sessionProperties = new BaseSessionProperty("admin", "admin", "default", "localhost");

            return sessionProperties;
        }


        public Topic DefineTopic()
        {
            Topic topic = new Topic("tutorial/requests", false, true);
            return topic;
        }

     /*   static public void Main(string[] args)
        {
            string msg = args[0];
            Dispatcher dispatcher = new Dispatcher();

            Console.WriteLine("Publishing message = {0}", msg);
            dispatcher.Publisher.PublishMessage(msg);
            Console.WriteLine("Message dispatched successfully!");
        }*/
    }

    
}
