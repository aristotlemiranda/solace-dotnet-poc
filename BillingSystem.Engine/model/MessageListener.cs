using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Engine.model
{
    public abstract class MessageListener : BackgroundService, Consumer
    {
        private readonly ILogger<MessageListener> _logger;       
        public abstract void ProcessMessage(string msg);        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
               //Start Service Here


                await Task.Delay(1_000, stoppingToken);
            }
        }
    }
}
