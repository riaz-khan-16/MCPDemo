using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);
builder.Services
    .AddMcpServer()                       // Registers the MCP server core.
    .WithToolsFromAssembly();            // Find all my tool methods and expose them to the LLM

await builder.Build().RunAsync();         // Start the MCP server

