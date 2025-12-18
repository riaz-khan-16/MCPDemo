using System.ComponentModel;
using ModelContextProtocol.Server;
using System;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualBasic;
using System.Runtime.Serialization;
using System.Transactions;


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

        [McpServerTool, Description("Calculate your income tax. Take the income as input and give tax amount as result")]
        public static string TaxTool(int income)
        {
            var tax = income/3;
            return $"Your Tax is  {tax}.";
        }

        
        [McpServerTool, Description("give name of the members of TeamSync")]
        public static async Task<string> MemberOfTeamSync()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://localhost:7162/api/Users");
            var users = JsonSerializer.Deserialize<List<User>>(response,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
            var result=" ";
            foreach (var user in users)
            {
                result+= $"[ Name: {user.name}: Email:  {user.email} ,  Department: {user.department}],";
            
            }
            return result;
        }
        

        [McpServerTool, Description("Add a new User in TeamSync ")]
        public static async Task<string> CreateNewUserinTeamSync(string Name, string Email, string Password, string Role, string Dept)
        {
         await Services.RegisterAsync(Name, Email, Password, Role, Dept);
        
          return "Yoy have successfully registered!! ";

        
        }





       [McpServerTool, Description("Give name of the Projects of TeamSync")]
        public static async Task<string> ProjectsOfTeamSync()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://localhost:7162/api/Projects");
            var projects = JsonSerializer.Deserialize<List<Project>>(response,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
            var result=" ";
           foreach (var project in projects)
                {
                    result += $"[Project Name: {project.Name}, Description: {project.Description}], Project Id: {project.Id}\n";
                    result += "\n";
                }
            return result;
        }
        

 

       [McpServerTool, Description("Add a new Project in TeamSync. if user don't give you the name, use:Name=Test Project, Description=This is a important task, created by: Riaz Khan ")]
        public static async Task<string> CreateNewProjectinTeamSync(string Name, string Description, string CreatedBy)
        {
         await Services.CreateProjectAsync(Name, Description, CreatedBy);
        
        return "Yoy have successfully created the project: ";

        
        }


       


      [McpServerTool, Description("Get your public IP address")]
    public static async Task<string> PublicIpTool()
    {
        using var client = new HttpClient();
        string ip = await client.GetStringAsync("https://api.ipify.org");

        return $"Your public IP is: {ip}";
    }




    }
}