
namespace Learnings.Azure.Publisher
{
    using Learnings.Azure.Common.ServiceBus;
    using System;

    public static class AddEmployee
    {
        private static ServiceBusRepository serviceBusRepository = new ServiceBusRepository();

        static void Main()
        {
            Console.WriteLine("Welcome to Publisher");
            Console.WriteLine("Sending messages to service bus - Started");

            Console.WriteLine("Press any key to send to HR employee");
            Console.ReadKey();
            ServiceBusMessage employeeHR = new ServiceBusMessage("EMP001", "Rohini Wagh", "HR");
            serviceBusRepository.Send(employeeHR);
            Console.WriteLine("HR employee sent");

            Console.WriteLine("Press any key to send to Admin employee");
            Console.ReadKey();
            ServiceBusMessage employeeAdmin = new ServiceBusMessage("EMP002", "Kiran Joshi", "Admin");
            serviceBusRepository.Send(employeeAdmin);
            Console.WriteLine("Admin employee sent");

            Console.WriteLine("Press any key to send to IT employee");
            Console.ReadKey();
            ServiceBusMessage employeeIT = new ServiceBusMessage("EMP003", "Siddhu Awte", "IT");
            serviceBusRepository.Send(employeeIT);
            Console.WriteLine("IT employee sent");

            Console.WriteLine("Sending messages to service bus - Completed");

            Console.ReadKey();
        }
    }
}
