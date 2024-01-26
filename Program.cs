// See https://aka.ms/new-console-template for more information

namespace ChatConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramInitializer programInitializer = new ProgramInitializer();
            programInitializer.SetSocket();

            Console.WriteLine("Local Port: ");
            string localPortNumber = Console.ReadLine();

            Console.WriteLine("Other Local Port:");
            string localPortNumber2 = Console.ReadLine();

            programInitializer.StartServer(localPortNumber: localPortNumber, remotePortNumber: localPortNumber2);

            Console.WriteLine("+++Chat Program+++");
            
            while (true)
            {
                Console.WriteLine("Send: ");

                string message = Console.ReadLine();
                programInitializer.SendMessage(message);
            }
        }
    }
}



