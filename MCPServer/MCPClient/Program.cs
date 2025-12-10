using Microsoft.Extensions.AI;

using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using System.Text.Json;

Console.WriteLine("MCP Client Started!");

// You're telling the MCP server who you are.
var clientOptions = new McpClientOptions   
{
    ClientInfo = new() { Name = "demo-client", Version = "1.0.0" }
};




// Define MCP Server Configuration

// MCP Server Configuration: “To talk to an MCP Server, run this EXE file using STDIO.”
var serverConfig = new McpServerConfig
{
    Id = "demo-server",   //internal unique identifier
    Name = "Demo Server",  // human readable name (shown in UI/logs)
    TransportType = TransportTypes.StdIo, // use stdio transport
    TransportOptions = new Dictionary<string, string>  // When the client needs the MCP server, it will run this EXE
    {
        ["command"] = @"C:\Users\riaz.khan\Desktop\Medium\MCPServer\bin\Debug\net10.0\MCPServer.exe"
    }
};





// Logger Creates a logger. Logs will appear in console. Minimum level = Information.
using var loggerFactory = LoggerFactory.Create(builder =>
    builder.AddConsole().SetMinimumLevel(LogLevel.Information));





// Create MCP Client using ||||  McpClientFactory:  Creates an ModelContextProtocol.Client.IMcpClient, connecting it to the specified server.
await using var mcpClient =
    await McpClientFactory.CreateAsync(serverConfig, clientOptions, loggerFactory: loggerFactory);

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

// Get available tools from MCP Server
var mcpTools = await mcpClient.ListToolsAsync();

var toolsJson = JsonSerializer.Serialize(mcpTools, new JsonSerializerOptions { WriteIndented = true }); // Converts tools into beautiful JSON text.
Console.WriteLine("\nAvailable Tools:\n" + toolsJson); // Shows on console.

await Task.Delay(100);  // Small delay to ensure logs flush.

// Prompt loop
Console.WriteLine("Type your message below (type 'exit' to quit):");

while (true)
{
    Console.Write("\n You: ");
    var userInput = Console.ReadLine(); //     Read User Input

    if (string.IsNullOrWhiteSpace(userInput)) // Skip empty input
        continue;

    if (userInput.Trim().ToLower() == "exit") // Exit condition
    {
        Console.WriteLine("Exiting chat...");
        break;
    }

    // Build Chat Messages for LLM
    var messages = new List<ChatMessage>   
    {
        new(ChatRole.System, "You are a helpful assistant."),
        new(ChatRole.User, userInput)
    };

    try
    {

        // Send Chat + Tools to Ollama
        var response = await chatClient.GetResponseAsync(
            messages, // Sends prompt to Ollama
            new ChatOptions { Tools = [.. mcpTools] }); // Also sends all MCP tools




        var assistantMessage = response.Messages.LastOrDefault(m => m.Role == ChatRole.Assistant);  // Print AI Response


        
        if (assistantMessage != null)
        {
            var textOutput = string.Join(" ", assistantMessage.Contents.Select(c => c.ToString()));
            Console.WriteLine("\n AI: " + textOutput);
        }
        else
        {
            Console.WriteLine("\n AI: (no assistant message received)");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n Error: {ex.Message}");
    }

}


