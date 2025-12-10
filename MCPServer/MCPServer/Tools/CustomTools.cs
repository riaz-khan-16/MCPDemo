using System.ComponentModel;
using ModelContextProtocol.Server;

namespace MCPServer.MCPTools
{
    [McpServerToolType]
    public static class CustomTool
    {
        [McpServerTool, Description("Get the current day of the week")]
        public static string GetDayOfWeek()
        {
            var today = DateTime.Now.ToString("dddd");
            return $"Today is {today}.";
        }

        [McpServerTool, Description("Calculate your income tax")]
        public static string TaxTool(int income)
        {
            var tax = income/3;
            return $"Your Tax is  {tax}.";
        }



        
    }
}