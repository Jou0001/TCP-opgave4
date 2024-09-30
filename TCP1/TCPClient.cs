using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TCP1

{


    public class TcpClientProgram
    {
        public void Start()
        {
            Console.WriteLine("TCP Client started");

            try
            {
                using TcpClient client = new TcpClient("127.0.0.1", 7);
                NetworkStream ns = client.GetStream();
                StreamReader reader = new StreamReader(ns);
                StreamWriter writer = new StreamWriter(ns);

                // Spørger brugeren om kommandoen (Random, Add, Subtract)
                Console.WriteLine("Enter command (Random, Add, Subtract): ");
                string command = Console.ReadLine();
                writer.WriteLine(command);
                writer.Flush();

                // Venter på serverens svar ("Input numbers")
                string serverResponse = reader.ReadLine();
                Console.WriteLine("Server says: " + serverResponse);

                // Spørger brugeren om tal1 og tal2
                Console.WriteLine("Enter two numbers separated by a space: ");
                string numbers = Console.ReadLine();
                writer.WriteLine(numbers);
                writer.Flush();

                // Modtager resultatet fra serveren
                string result = reader.ReadLine();
                Console.WriteLine("Server result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}