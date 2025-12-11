using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using System.Text.Json;

namespace MediumWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatlamaController : ControllerBase
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





            

            // Logger Creates a logger. Logs will appear in console. Minimum level = Information.
            using var loggerFactory = LoggerFactory.Create(builder =>
                builder.AddConsole().SetMinimumLevel(LogLevel.Information));














            //-----------------------------Build a Chat Client --------------------------------------------//


            // Configure Ollama LLM Client.. we need to define uri here
            var ollamaChatClient = new OllamaChatClient(
                new Uri("http://localhost:11434/"),  // Connects to your local Ollama server.
                "llama3.2:3b"  // Uses model llama3.2:3b.
            );

            // Build .NET AI Chat Client
            var chatClient = new ChatClientBuilder(ollamaChatClient)  // Wraps Ollama into a full “AI chat” pipeline.
                .UseLogging(loggerFactory)  // Adds logging.
                .UseFunctionInvocation()  //  Enables function-calling (so it can call MCP tools).
                .Build();





            var messages = new List<ChatMessage>
            {
                new(ChatRole.System, "You are a helpful assistant.At first give generic answer.. then use MCP tools"),
                new(ChatRole.User, message)
            };

            // Send Chat + Tools to Ollama
            var response = await chatClient.GetResponseAsync(
                messages); // Also sends all MCP tools);




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