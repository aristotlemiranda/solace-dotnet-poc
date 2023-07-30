/*using BillingSystem.Engine.model;
using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.core
{
    internal class StartService : IDisposable
    {
        static private User User = new User();
        private ISession Session;
        private IQueue Queue;
        private IFlow Flow;
        const int DefaultReconnectRetries = 3;
        private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
  
        #region Main
        public static void Main(string[] args) {
            User.Name = "Aristotle Miranda";
            Console.WriteLine("Service is running.... {0}", User.Name);
        
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
                IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);
                StartService startService= new StartService();
                startService.Run(context, "localhost");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }
            Console.WriteLine("Finished.");

        }
        #endregion

        void Run(IContext context, string host)
        {
            // Validate parameters
            if (context == null)
            {
                throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
            }

            // Create session properties
            SessionProperties sessionProps = new SessionProperties()
            {
                Host = host,
                VPNName = "default",
                UserName = "admin",
                Password = "admin",
                ReconnectRetries = DefaultReconnectRetries
            };

            // Connect to the Solace messaging router
            Console.WriteLine("Connecting as {0}@{1} on {2}...",sessionProps.UserName, sessionProps.VPNName, host);
            Session = context.CreateSession(sessionProps, null, null);
            ReturnCode returnCode = Session.Connect();
           
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                Console.WriteLine("Session successfully connected.");

                // Provision the queue
                string queueName = "Q/tutorial";
                
                Queue  = new Queue(queueName, false, true);
                
                Flow = Session.CreateFlow(new FlowProperties()
                {
                    AckMode = MessageAckMode.ClientAck
                },
                Queue, null, MessageEventHandler, HandleFlowEvent);
                Flow.Start();
                Console.WriteLine("Waiting for a message in the queue '{0}'...", queueName);

                WaitEventWaitHandle.WaitOne();
            }
            else
            {
                Console.WriteLine("Error connecting, return code: {0}", returnCode);
            }
        }

        void MessageEventHandler(object? source, MessageEventArgs args)
        {
            // Received a message
            Console.WriteLine("Received message.");
            using (IMessage message = args.Message)
            {
                // Expecting the message content as a binary attachment
                Console.WriteLine("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
                // ACK the message
                Flow.Ack(message.ADMessageId);
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
*/