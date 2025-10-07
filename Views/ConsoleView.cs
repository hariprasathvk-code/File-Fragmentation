using System;

namespace FileFragmentationMVC.Views
{
    public static class ConsoleView
    {
        public static string GetUserInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
