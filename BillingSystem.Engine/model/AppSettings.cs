using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    public sealed class AppSettings
    {
        private string brokerUserName = "";
        private string brokerVPNName = "";
        private string queueNameDemo = "";
        private string host = "";
        private string queueDMQ = "";
        private string topicNameDemo = "";
        private string topicNameDMQ = "";


        public string BrokerUserName { get; set; } = "";
        public string BrokerPassword { get; set; } = "";
        public string BrokerVPNName { get; set; } = "";
        public string QueueNameDemo { get; set; } = "";
        public string QueueDMQ { get; set; } = "";
        public string TopicNameDemo { get; set; } = "";
        public string TopicNameDMQ { get; set; } = "";
        public string Host { get; set; } = "";
    }
}
