 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
public class Services{
                public static async Task CreateProjectAsync(string Name, string Description, string CreatedBy)
                        {
                            using var client = new HttpClient();  // Creates an HTTP client to send web requests

                            client.BaseAddress = new Uri("https://localhost:7162");  //Sets the base URL of your API.. Later requests can use relative paths like

                            var projectId = Guid.NewGuid(); // Creates a unique ID for the project

                            var project = new Project  
                            {
                            
                                Name = Name,
                                Description = Description,
                                CreatedBy = CreatedBy,

                            
                            };



                            // Converts the C# object into JSON text
                            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                WriteIndented = true
                            });



                            Console.WriteLine(" Sending JSON:");
                            Console.WriteLine(json);

                            var content = new StringContent(json, Encoding.UTF8, "application/json");   // Wraps JSON into HTTP body

                            try
                            {
                                //Send POST Request
                                var response = await client.PostAsync("/api/Projects", content); // Send Post Request

                                Console.WriteLine($"\n Status Code: {response.StatusCode}");
                        
                                var responseBody = await response.Content.ReadAsStringAsync();
                                Console.WriteLine(" Response:");
                                Console.WriteLine(responseBody);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(" Error:");
                                Console.WriteLine(ex.Message);
                            }
                        }




// add new user

public static async Task RegisterAsync(string Name, string Email, string Password, string Role, string Dept)
                        {
                            using var client = new HttpClient();  // Creates an HTTP client to send web requests

                            client.BaseAddress = new Uri("https://localhost:7162");  //Sets the base URL of your API.. Later requests can use relative paths like

                 

                            var User = new User
                            {
                            
                                name = Name,
                                email = Email,
                                password = Password,
                                role= Role,
                                department=Dept

                            
                            };



                            // Converts the C# object into JSON text
                            var json = JsonSerializer.Serialize(User, new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                WriteIndented = true
                            });



                            Console.WriteLine(" Sending JSON:");
                            Console.WriteLine(json);

                            var content = new StringContent(json, Encoding.UTF8, "application/json");   // Wraps JSON into HTTP body

                            try
                            {
                                //Send POST Request
                                var response = await client.PostAsync("/api/Auth/register", content); // Send Post Request

                                
                            }
                            catch (Exception ex)
                            {
                              
                                Console.WriteLine(ex.Message);
                            }
                        }







}