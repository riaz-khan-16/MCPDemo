using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MediumWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {

        private static readonly List<object> Messages = new()
        {
            new {User="Hi", Bot="What?"}

        };


        [HttpGet]
        public IEnumerable<object>  Get()
        {
            return Messages;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string message)
        {





            //---------------------------------------Make a MCP CLient------------------------------------//

            // You're telling the MCP server who you are.
            var clientOptions = new McpClientOptions
            {
                ClientInfo = new() { Name = "demo-client", Version = "1.0.0" }
            };




            /* Define MCP Server Configuration
             MCP Server Configuration: “To talk to an MCP Server, run this EXE file using STDIO.” */


            var serverConfig = new McpServerConfig
            {
                Id = "demo-server",   //internal unique identifier
                Name = "Demo Server",  // human readable name (shown in UI/logs)
                TransportType = TransportTypes.StdIo, // use stdio transport
                TransportOptions = new Dictionary<string, string>  // When the client needs the MCP server, it will run this EXE
                {
                    ["command"] = @"C:\Users\riaz.khan\Desktop\MCPChatBot\MCPServer\MCPServer\bin\Debug\net10.0\MCPServer.exe"
                }
            };


            // Logger Creates a logger. Logs will appear in console. Minimum level = Information.
            using var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddConsole().SetMinimumLevel(LogLevel.Information));


            // Create MCP Client using ||||  McpClientFactory:  Creates an ModelContextProtocol.Client.IMcpClient, connecting it to the specified server.
            await using var mcpClient =
                await McpClientFactory.CreateAsync(serverConfig, clientOptions, loggerFactory: loggerFactory);














            //-----------------------------Build a Chat Client --------------------------------------------//


            // Configure Ollama LLM Client.. we need to define uri here
            var ollamaChatClient = new OllamaChatClient(
                new Uri("http://localhost:11434/"),  // Connects to your local Ollama server.
                "llama3.2:3b"  // Uses model llama3.2:3b.
                //"qwen2.5-coder:7b"
            );

            // Build .NET AI Chat Client
            var chatClient = new ChatClientBuilder(ollamaChatClient)  // Wraps Ollama into a full “AI chat” pipeline.
                .UseLogging(loggerFactory)  // Adds logging.
                .UseFunctionInvocation()  //  Enables function-calling (so it can call MCP tools).
                .Build();
            // Get available tools from MCP Server
            var mcpTools = await mcpClient.ListToolsAsync();



            bool isGeneric =
                Regex.IsMatch(
                    message.Trim(),
                    @"^(hi|hello|hey|assalamu alaikum|how are you|who are you)\b",
                    RegexOptions.IgnoreCase
                );


            var messages = new List<ChatMessage>
            {
                new(ChatRole.System, @"

                      You are a helpful AI assistant.

Rules:
- For greetings, small talk, or generic questions, ALWAYS reply with normal text.
- Do NOT call any tool for generic questions or explanations.
- Use a tool ONLY when the question explicitly requires external, real-time, or authoritative data.
- If no tool is required, answer directly in plain text.
- Never expose tool calls, JSON, or internal reasoning.
- If a tool is used, respond only with the final human-readable answer.



                 "

                ),
                new(ChatRole.User, message)
            };

           
            var response = await chatClient.GetResponseAsync(
                messages,
                new ChatOptions
                {
                    Tools = [..mcpTools],   // IList<AITool>
                    Temperature = 0.2f,          // float literal
                    ToolMode = isGeneric ? ChatToolMode.None : ChatToolMode.Auto,
                    MaxOutputTokens = 512,


                }
            );





            var assistantMessage = response.Messages
                .LastOrDefault(m => m.Role == ChatRole.Assistant);








            string res = assistantMessage?.ToString() ?? "(no reply)";
            Messages.Add(new { User = message, Bot = res });






            return Ok(new
            {
                reply = assistantMessage?.ToString() ?? "(no reply)"
            });

        }
    }
}