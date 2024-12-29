using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StockClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter stock ticker (or 'exit' to quit): ");
                string ticker = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(ticker))
                {
                    continue;
                }
                if (ticker.ToLower() == "exit")
                {
                    break;
                }

                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        await client.ConnectAsync("localhost", 8888);
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] data = Encoding.UTF8.GetBytes(ticker);
                            await stream.WriteAsync(data, 0, data.Length);

                            byte[] responseData = new byte[256];
                            int bytes = await stream.ReadAsync(responseData, 0, responseData.Length);
                            string response = Encoding.UTF8.GetString(responseData, 0, bytes).Trim();

                            Console.WriteLine($"Price for {ticker}: {response}");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }
}
