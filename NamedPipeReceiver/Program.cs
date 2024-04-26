using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace NamedPipeReceiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Named Pipe Receiver");

            using (var pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.In))
            {
                Console.WriteLine("Waiting for connection...");
                await pipeServer.WaitForConnectionAsync();
                Console.WriteLine("Client connected.");

                using (var reader = new StreamReader(pipeServer))
                {
                    while (true)
                    {
                        char[] buffer = new char[256];
                        int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                            break;

                        string message = new string(buffer, 0, bytesRead);
                        Console.WriteLine($"Received: {message}");
                    }
                }
            }
        }
    }
}