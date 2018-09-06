
namespace Learnings.Azure.FunctionApp
{
    using Learnings.Azure.Common.Logger;
    using Learnings.Azure.Common.ServiceBus;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.ServiceBus.Messaging;
    using System;

    public static class ReadEmployee
    {
        private static void ProcessEmployee(BrokeredMessage message, TraceWriter log, string filter)
        {
            try
            {
                log.Info("----------------------------------------------------------------------------------------------------------------------------");
                log.Info("Processing new message in " + filter + " subscription - Started");
                log.Info("Message Id: " + message.MessageId);
                ServiceBusMessage messageBody = message.GetBody<ServiceBusMessage>();
                log.Info("Received message for following employee");
                log.Info("Employee Code: " + messageBody.EmpCode);
                log.Info("Employee Name: " + messageBody.EmpName);
                log.Info("Employee Department: " + messageBody.EmpDepartment);
                log.Info("Processing new message in " + filter + " subscription - Completed");
                log.Info("----------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                message.Abandon();
            }
        }

        //[Disable]
        [FunctionName("ReadHREmployee")]
        public static void ReadHREmployee([ServiceBusTrigger("%Topic%", "%HRSubscription%", AccessRights.Listen, Connection = "AzureWebJobsServiceBus")]BrokeredMessage message, TraceWriter log)
        {
            ProcessEmployee(message, log, "HR");
        }

        //[Disable]
        [FunctionName("ReadAdminEmployee")]
        public static void ReadAdminEmployee([ServiceBusTrigger("%Topic%", "%AdminSubscription%", AccessRights.Listen, Connection = "AzureWebJobsServiceBus")]BrokeredMessage message, TraceWriter log)
        {
            ProcessEmployee(message, log, "Admin");
        }

        //[Disable]
        [FunctionName("ReadITEmployee")]
        public static void ReadITEmployee([ServiceBusTrigger("%Topic%", "%ITSubscription%", AccessRights.Listen, Connection = "AzureWebJobsServiceBus")]BrokeredMessage message, TraceWriter log)
        {
            ProcessEmployee(message, log, "IT");
        }

        [Disable]
        [FunctionName("ReadAllEmployee")]
        public static void ReadAllEmployee([ServiceBusTrigger("%Topic%", "%AllSubscription%", AccessRights.Listen, Connection = "AzureWebJobsServiceBus")]BrokeredMessage message, TraceWriter log)
        {
            ProcessEmployee(message, log, "All");
        }
    }
}
