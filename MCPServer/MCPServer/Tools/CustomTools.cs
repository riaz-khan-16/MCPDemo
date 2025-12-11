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

       [McpServerTool, Description("List of emplyee in Orbit")]
        public static List<string>  EmplyeeTool()
        {

            string[]  employee={"Riaz", "Khan"};
           
            return employee.ToList();
        }


      [McpServerTool, Description("Get your public IP address")]
    public static async Task<string> PublicIpTool()
    {
        using var client = new HttpClient();
        string ip = await client.GetStringAsync("https://api.ipify.org");

        return $"Your public IP is: {ip}";
    }


    [McpServerTool, Description("Get current temperature in Dhaka")]
public static async Task<string> DhakaTemperatureTool()
{
    using var client = new HttpClient();
    string url = "https://api.open-meteo.com/v1/forecast?latitude=23.8103&longitude=90.4125&current_weather=true";
    
    var result = await client.GetStringAsync(url);
    return $"Dhaka Weather Data: {result}";
}

[McpServerTool, Description("Get geographical info about your public IP")]
public static async Task<string> IpLocationTool()
{
    using var client = new HttpClient();
    string data = await client.GetStringAsync("https://ipinfo.io/json");
    
    return $"IP Location Info: {data}";
}


   






        


    }
}