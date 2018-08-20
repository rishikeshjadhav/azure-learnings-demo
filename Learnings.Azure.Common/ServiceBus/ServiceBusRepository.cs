
namespace Learnings.Azure.Common.ServiceBus
{
    using Logger;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;

    public class ServiceBusRepository
    {
        private static string topicName;
        private static string serviceBusConnectionString;
        private static MessagingFactory messagingFactory;
        private static MessageSender messageSender;

        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceBusRepository()
        {
            topicName = Convert.ToString(ConfigurationManager.AppSettings["ServiceBusTopicName"], CultureInfo.InvariantCulture);
            serviceBusConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsServiceBus"].ConnectionString;
            messagingFactory = MessagingFactory.CreateFromConnectionString(serviceBusConnectionString);
            messageSender = messagingFactory.CreateMessageSender(topicName);
        }

        /// <summary>
        /// Function to send brokered message to Service Bus Topic
        /// </summary>
        /// <param name="message">Current service bus message</param>
        public void Send(ServiceBusMessage message)
        {
            try
            {
                if (null != message)
                {
                    BrokeredMessage brokeredMessage = new BrokeredMessage(message);
                    brokeredMessage.Properties.Add("Department", message.EmpDepartment);
                    messageSender.Send(brokeredMessage);
                }
                else
                {
                    throw new Exception("Null message found");
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }

        /// <summary>
        /// Function to send list of brokered messages to Service Bus Topic
        /// </summary>
        /// <param name="messages">Current service bus message collection</param>
        public void Send(List<ServiceBusMessage> messages)
        {
            if (null != messages)
            {
                messages.ForEach((message) =>
                {
                    Send(message);
                });
            }
            else
            {
                throw new Exception("Null message collection found");
            }
        }
    }
}
