using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace NamedPipeSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Named Pipe Sender");

            using (var pipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.Out))
            {
                Console.WriteLine("Attempting to connect to pipe...");
                await pipeClient.ConnectAsync();
                Console.WriteLine("Connected to pipe.");

                using (var writer = new StreamWriter(pipeClient))
                {
                    while (true)
                    {
                        Console.WriteLine("Enter 'message' to send a message or 'file' to send a file:");
                        var input = Console.ReadLine();

                        if (input == "message")
                        {
                            Console.Write("Enter message (or 'exit' to quit): ");
                            var message = Console.ReadLine();
                            if (message == "exit")
                                break;

                            await writer.WriteLineAsync(message);
                            await writer.FlushAsync();
                        }
                        else if (input == "file")
                        {
                            Console.Write("Enter file path: ");
                            string? filePath = Console.ReadLine();

                            if (filePath != null)
                            {
                                using (var fileStream = File.OpenRead(filePath))
                                {
                                    await fileStream.CopyToAsync(pipeClient);
                                }
                                Console.WriteLine("File sent successfully.");
                            }
                            else
                            {
                                Console.WriteLine("File path cannot be null. Please enter a valid file path.");
                            }
                        }
                    }
                }
            }
        }
    }
}