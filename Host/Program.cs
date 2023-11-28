using System;
using System.ServiceModel; 

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            using (ServiceHost serviceHost = new ServiceHost(typeof(Services.Implementations.AccountService)))
            {

                serviceHost.Open();

                Console.WriteLine("Servidor WCF en ejecución. Presiona Enter para detenerlo.");
                Console.ReadLine();

            }

        }
    }
}
