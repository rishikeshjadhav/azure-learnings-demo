
namespace Learnings.ProvisionServiceBus
{
    using Learnings.Azure.Common.Logger;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Configuration;
    using System.Globalization;

    public class Provision
    {
        private static string serviceBusConnectionString;
        private static NamespaceManager nameSpaceManager;

        internal static void CreateSubscription(string topicName, string subscriptionName, string department = null)
        {
            if (!nameSpaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Subscription with name {0} under Topic {1} in service bus namespace", subscriptionName, topicName));
                RuleDescription ruleDescription = null;
                if (null != department)
                {
                    ruleDescription = new RuleDescription()
                    {
                        Name = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_Rule", topicName, subscriptionName),
                        Filter = new SqlFilter(string.Format(CultureInfo.InvariantCulture, "Department = '{0}'", department))
                    };
                }
                SubscriptionDescription subscriptionDescription = new SubscriptionDescription(topicName, subscriptionName);
                subscriptionDescription.EnableDeadLetteringOnMessageExpiration = true;
                subscriptionDescription.EnableDeadLetteringOnFilterEvaluationExceptions = true;
                subscriptionDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(ConfigurationManager.AppSettings["MessageTimeToLiveInSeconds"], CultureInfo.InvariantCulture));
                subscriptionDescription.LockDuration = new TimeSpan(0, 0, Convert.ToInt32(ConfigurationManager.AppSettings["LockDurationInSeconds"], CultureInfo.InvariantCulture));
                subscriptionDescription.MaxDeliveryCount = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDeliveryCount"], CultureInfo.InvariantCulture);
                if (null == ruleDescription)
                {
                    nameSpaceManager.CreateSubscription(subscriptionDescription);
                }
                else
                {
                    nameSpaceManager.CreateSubscription(subscriptionDescription, ruleDescription);
                }
                Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} under Topic {1} created in service bus namespace", subscriptionName, topicName));
            }
            else
            {
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Subscription with name {0} already exists under Topic with name {1} under in service bus namespace", subscriptionName, topicName));
            }
        }

        internal static void CreateSubscriptions()
        {
            string topicName = Convert.ToString(ConfigurationManager.AppSettings["ServiceBusTopicName"], CultureInfo.InvariantCulture);
            CreateSubscription(topicName, Convert.ToString(ConfigurationManager.AppSettings["HRSubscriptionName"], CultureInfo.InvariantCulture), Convert.ToString(ConfigurationManager.AppSettings["HRSubscriptionName"], CultureInfo.InvariantCulture));
            CreateSubscription(topicName, Convert.ToString(ConfigurationManager.AppSettings["AdminSubscriptionName"], CultureInfo.InvariantCulture), Convert.ToString(ConfigurationManager.AppSettings["AdminSubscriptionName"], CultureInfo.InvariantCulture));
            CreateSubscription(topicName, Convert.ToString(ConfigurationManager.AppSettings["ITSubscriptionName"], CultureInfo.InvariantCulture), Convert.ToString(ConfigurationManager.AppSettings["ITSubscriptionName"], CultureInfo.InvariantCulture));
            CreateSubscription(topicName, "All");
        }

        internal static void CreateTopic()
        {
            string topicName = Convert.ToString(ConfigurationManager.AppSettings["ServiceBusTopicName"], CultureInfo.InvariantCulture);
            if (!nameSpaceManager.TopicExists(topicName))
            {
                Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Creating Topic with name {0} in service bus namespace", topicName));
                TopicDescription topicDescription = new TopicDescription(topicName);
                topicDescription.DefaultMessageTimeToLive = new TimeSpan(0, 0, 0, Convert.ToInt32(ConfigurationManager.AppSettings["MessageTimeToLiveInSeconds"], CultureInfo.InvariantCulture));
                topicDescription.MaxSizeInMegabytes = Convert.ToInt32(ConfigurationManager.AppSettings["TopicMaxSizeInMegabytes"], CultureInfo.InvariantCulture);
                topicDescription.RequiresDuplicateDetection = true;
                nameSpaceManager.CreateTopic(topicDescription);
                Logger.LogMessage(string.Format(CultureInfo.InvariantCulture, "Topic with name {0} created in service bus namespace", topicName));
            }
            else
            {
                Logger.LogError(string.Format(CultureInfo.InvariantCulture, "Topic with name {0} already exists in service bus namespace", topicName));
            }
        }

        static void Main(string[] args)
        {
            try
            {
                serviceBusConnectionString = Convert.ToString(ConfigurationManager.AppSettings["AzureWebJobsServiceBus"], CultureInfo.InvariantCulture);
                nameSpaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["CreateTopic"], CultureInfo.InvariantCulture))
                {
                    CreateTopic();
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["CreateSubscription"], CultureInfo.InvariantCulture))
                {
                    CreateSubscriptions();
                }

            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}
