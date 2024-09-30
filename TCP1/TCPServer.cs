using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP1
{
    public class TcpServer
    {
        public static void Main()
        {
            Console.WriteLine("TCP Server started");
            TcpListener listener = new TcpListener(IPAddress.Any, 7);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                IPEndPoint clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client connected: " + clientEndPoint.Address);

                // Håndter hver klient i en ny tråd
                Task.Run(() => HandleClient(socket));
            }
        }

        static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            Random random = new Random();

            try
            {
                while (socket.Connected)
                {
                    // Læs kommando fra klienten (Random, Add, Subtract)
                    string? command = reader.ReadLine();
                    Console.WriteLine("Received command: " + command);

                    if (command == null) break;

                    // Svar tilbage til klienten
                    writer.WriteLine("Input numbers");
                    writer.Flush();

                    // Læs tal1 og tal2 fra klienten
                    string? numberInput = reader.ReadLine();
                    if (numberInput == null) break;

                    string[] numbers = numberInput.Split(' ');
                    if (numbers.Length != 2 ||
                        !int.TryParse(numbers[0], out int num1) ||
                        !int.TryParse(numbers[1], out int num2))
                    {
                        writer.WriteLine("Invalid input");
                        writer.Flush();
                        continue;
                    }

                    // Håndtering af kommandoer
                    if (command == "Random")
                    {
                        int randomNumber = random.Next(num1, num2 + 1);
                        writer.WriteLine(randomNumber);
                    }
                    else if (command == "Add")
                    {
                        int sum = num1 + num2;
                        writer.WriteLine(sum);
                    }
                    else if (command == "Subtract")
                    {
                        int result = num1 - num2;
                        writer.WriteLine(result);
                    }
                    else
                    {
                        writer.WriteLine("Unknown command");
                    }

                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                socket.Close();
            }
        }
    }
}

