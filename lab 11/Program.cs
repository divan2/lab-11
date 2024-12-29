using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StockServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StockContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1436;Database=StocksDB;User Id=SA;Password=LEGO1111;TrustServerCertificate=True;");

            var context = new StockContext(optionsBuilder.Options);

            int port = 8888;
            var server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine($"Server started on port {port}");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                _ = Task.Run(async () => await HandleClient(client, context));
            }
        }

        static async Task HandleClient(TcpClient client, StockContext context)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = new byte[256];
                    int bytes = await stream.ReadAsync(data, 0, data.Length);
                    string ticker = Encoding.UTF8.GetString(data, 0, bytes).Trim();

                    var latestStock = context.Stocks
                        .Where(s => s.Ticker == ticker)
                        .OrderByDescending(s => s.Date)
                        .FirstOrDefault();

                    string response;

                    if (latestStock != null)
                    {
                        response = ((decimal)latestStock.Price).ToString(); 
                    }
                    else
                    {
                        response = "Ticker not found";
                    }

                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(responseData, 0, responseData.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
