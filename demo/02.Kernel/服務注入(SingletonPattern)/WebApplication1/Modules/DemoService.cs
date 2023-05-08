using MDP.Registration;

namespace WebApplication1
{
    [Service<DemoService>(singleton:true)]
    public class DemoService
    {
        // Fields
        private int _count = 0;


        // Methods
        public string GetMessage()
        {
            // Count
            _count++;

            // Return
            return "Count=" + _count.ToString();
        }
    }
}
