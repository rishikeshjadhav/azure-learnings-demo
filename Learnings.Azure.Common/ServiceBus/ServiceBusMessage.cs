
namespace Learnings.Azure.Common.ServiceBus
{
    public class ServiceBusMessage
    {
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpDepartment { get; set; }

        public ServiceBusMessage()
        {
        }

        public ServiceBusMessage(string empCode, string empName, string empDepartment)
        {
            EmpCode = empCode;
            EmpName = empName;
            EmpDepartment = empDepartment;
        }
    }
}
