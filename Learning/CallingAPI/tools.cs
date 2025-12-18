 using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
public class Tools{
public static async Task CreateProjectAsync()
        {
            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://localhost:7162");

            var projectId = Guid.NewGuid();

            var project = new Project
            {
              
                Name = "Project-TeamSync",
                Description = "Develop",
                CreatedBy = "Riaz",

               
            };

            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            Console.WriteLine("📤 Sending JSON:");
            Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("/api/Projects", content);

                Console.WriteLine($"\n📥 Status Code: {response.StatusCode}");

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("📥 Response:");
                Console.WriteLine(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error:");
                Console.WriteLine(ex.Message);
            }
        }
}