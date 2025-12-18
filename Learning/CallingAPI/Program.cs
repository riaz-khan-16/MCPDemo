using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Tools.CreateProjectAsync();
            Console.ReadLine();
        }

       
    }

    // ================= MODELS =================

    
}
