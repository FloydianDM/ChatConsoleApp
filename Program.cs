// See https://aka.ms/new-console-template for more information

namespace ChatConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramInitializer programInitializer = new ProgramInitializer();
            programInitializer.SetSocket();
            programInitializer.StartServer("602", "602");
        }
    }
}



