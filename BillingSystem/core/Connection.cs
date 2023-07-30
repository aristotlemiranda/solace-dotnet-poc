using BillingSystem.Engine.model;
using Microsoft.Extensions.Hosting;
using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static SolaceSystems.Solclient.Messaging.SessionProperties;

namespace BillingSystem.core
{
    public class Connection<T>
    {
        Consumer Consumer;
        BaseSessionProperty BaseSessionProperty;
      
        private ISession Session;
        private IQueue Queue;
        private IDestination Topic;
        private IFlow Flow;
        const int DefaultReconnectRetries = 3;
        private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
        IContext context;


        #pragma warning disable CS8618
        public Connection(Consumer consumer, BaseSessionProperty baseSessionProp, Queue queue)
        {
            Consumer = consumer!;
            BaseSessionProperty = baseSessionProp!;
            Queue = queue!;


            Console.WriteLine("Connecting to queue '{0}'...", queue.Name);
        }


        public Connection(BaseSessionProperty baseSessionProp, Topic topic)
        {         
            BaseSessionProperty = baseSessionProp!;
            Topic = topic!;
            Console.WriteLine("Connecting to queue '{0}'...", topic.Name);
        }

        #region 
        public void InitiateComponent()
        {            
            // Initialize Solace Systems Messaging API with logging to console at Warning level
            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);

            try
            {
                // Context must be created first
                context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);

                // Validate parameters
                if (context == null)
                {
                    throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
                }

                // Create session properties
                SessionProperties sessionProps = new SessionProperties()
                {
                    Host = BaseSessionProperty.Host,
                    VPNName = BaseSessionProperty.VpnName,
                    UserName = BaseSessionProperty.UserName,
                    Password = BaseSessionProperty.Password,
                    ReconnectRetries = DefaultReconnectRetries
                 
                };

                // Connect to the Solace messaging router
                Console.WriteLine("Connecting as {0}@{1} on {2}...", sessionProps.UserName, sessionProps.VPNName, sessionProps.Host);
                Session = CreateSession(context, sessionProps);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                //ContextFactory.Instance.Cleanup();
            }
            Console.WriteLine("Finished.");

        }
        #endregion

        ISession CreateSession(IContext context, SessionProperties sessionProps) 
        {
            Session = context.CreateSession(sessionProps, null, null);
            ReturnCode returnCode = Session.Connect();

            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                Console.WriteLine("Session successfully connected.");
                
            }
            else
            {
                Console.WriteLine("Error connecting, return code: {0}", returnCode);
                throw new Exception("Error connecting, return code: "+ returnCode + "" );
            }
            return Session;
        }

        public void CreateConsumer() {
            FlowProperties flowProperties = new FlowProperties()
            {
                AckMode = MessageAckMode.ClientAck,
                RequiredOutcomeFailed = false,
                RequiredOutcomeRejected = false
            };
            Console.WriteLine("flowProperties => {0}", flowProperties.RequiredOutcomeFailed);
            Flow = Session.CreateFlow(flowProperties,
            Queue, null, MessageEventHandler, HandleFlowEvent);
            Flow.Start();
            WaitEventWaitHandle.WaitOne();            
        }

        public void PublishMessage(string payload)
        {
            // Create the message
            using (IMessage message = ContextFactory.Instance.CreateMessage())
            {
                message.Destination = ContextFactory.Instance.CreateTopic(Topic.Name);
                message.DMQEligible = true;
                message.DeliveryMode = MessageDeliveryMode.Persistent;
                
                // Create the message content as a binary attachment
                message.BinaryAttachment = Encoding.ASCII.GetBytes(payload);
                // Publish the message to the topic on the Solace messaging router
                Console.WriteLine("Publishing message...");
                ReturnCode returnCode = Session.Send(message);
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    Console.WriteLine("Done.");
                }
                else
                {
                    Console.WriteLine("Publishing failed, return code: {0}", returnCode);
                }
            }
        }

        public void DumpMSGToDMQ(IMessage message)
        {


            // Create the message
            //message.Destination = new Queue("#DEAD_MSG_QUEUE", false, true);
            message.Destination = ContextFactory.Instance.CreateTopic("topic/DMQ");
            message.DMQEligible = false;
            message.DeliveryMode = MessageDeliveryMode.Persistent;

            // Create the message content as a binary attachment
            //message.BinaryAttachment = Encoding.ASCII.GetBytes(messsage);
            // Publish the message to the topic on the Solace messaging router
            Console.WriteLine("Publishing message...");
            ReturnCode returnCode = Session.Send(message);
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                Console.WriteLine("Done.");
            }
            else
            {
                Console.WriteLine("Publishing failed, return code: {0}", returnCode);
            }


        }

        int deliveryCount = 0;

        void MessageEventHandler(object? source, MessageEventArgs args)
        {
            // Received a message
            Console.WriteLine("Received message.");            

            using (IMessage message = args.Message)
            {
                // Expecting the message content as a binary attachment
                Console.WriteLine("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
                try
                {                    
                    try
                    {
                        Consumer.ProcessMessage(Encoding.ASCII.GetString(message.BinaryAttachment));
                        Console.WriteLine("Acknowledge!");
                        Flow.Ack(message.ADMessageId);
                    }
                    catch {
                        Console.WriteLine("NOT Acknowledge! => {0}", source);
                        //Console.WriteLine("Message was redelivered count => {0}", message.DeliveryCount);

                        if (deliveryCount == 0)
                        {
                            deliveryCount = 1;
                        }
                        Console.WriteLine("Message was redelivered count => {0}", deliveryCount);

                        if (deliveryCount >= 3)
                        {
                            DumpMSGToDMQ(message);
                            Flow.Ack(message.ADMessageId);
                            Console.WriteLine("Message was forwarded to DMQ");
                        }
                        else
                        {                         
                            deliveryCount++;
                            Flow.Stop();
                            Thread.Sleep(10000);
                            Flow.Start();
                            MessageEventHandler(source, args);
                        }
                    }                         
                }
                catch 
                {
                    //Handle the exception here
                    //Flow.Dispose(); // Will kick the consumer binding                    
                    //Flow.Stop();
                    //Maybe move to DMQ then ACK
                    //Flow.Ack(message.ADMessageId);                    
                }

                // finish the program
                //WaitEventWaitHandle.Set();
            }
        }

        public void HandleFlowEvent(object? sender, FlowEventArgs args)
        {            
            // Received a flow event
            Console.WriteLine("Received Flow Event '{0}' Type: '{1}' Text: '{2}'",
                args.Event,
                args.ResponseCode.ToString(),
                args.Info);
        }


        public void Dispose()
        {
            Dispose(true);

        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Session != null)
                    {
                        Session.Dispose();
                        Session = null;
                    }
                    if (Queue != null)
                    {
                        Queue.Dispose();
                        Queue = null;
                    }
                    if (Flow != null)
                    {
                        Flow.Dispose();
                        Flow = null;
                    }
                }
                disposedValue = true;
            }
        }
    }
}
