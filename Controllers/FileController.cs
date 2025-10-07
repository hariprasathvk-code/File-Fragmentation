using System;
using System.Collections.Generic;
using System.IO;
using FileFragmentationMVC.Models;
using FileFragmentationMVC.Views;

namespace FileFragmentationMVC.Controllers
{
    public class FileController
    {
        private FileManager file_Manager = new FileManager();
        private List<Fragment> frag_ments = new List<Fragment>();
        private string folder_Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
        private string input_File => Path.Combine(folder_Path, "input.txt");
        private string output_File => Path.Combine(folder_Path, "output.txt");


        public void Run()
        {
            try
            {
                //file exist check
                if (!Directory.Exists(folder_Path))
                    Directory.CreateDirectory(folder_Path);

                //input
                string paragraph = ConsoleView.GetUserInput("Enter paragraph: ");
                if (string.IsNullOrWhiteSpace(paragraph))
                    throw new ArgumentException("Paragraph cannot be empty!");
                file_Manager.CreateFile(input_File, paragraph);

                //Fragmentation
                int fragSize = int.Parse(ConsoleView.GetUserInput("Enter the number of characters per file: "));
                if (fragSize <= 0)
                    throw new ArgumentException("Fragment size must be greater than zero.");

                frag_ments = file_Manager.FragmentFile(input_File, folder_Path, fragSize);

                ConsoleView.DisplayMessage("\nFragments created:");
                foreach (var frag in frag_ments)
                    ConsoleView.DisplayMessage(Path.GetFileName(frag.FileName));

                //Verify fragment
                string fragName = ConsoleView.GetUserInput("\nEnter fragment to check: ");
                string fragPath = Path.Combine(folder_Path, fragName);
                string fragContent = file_Manager.CheckFragment(fragPath);
                ConsoleView.DisplayMessage($"\nContent of {fragName}: {fragContent}");

                //Defragmentation
                file_Manager.DefragmentFiles(frag_ments, output_File);
                ConsoleView.DisplayMessage($"\nDefragmentation Completed - Output file created: {output_File}");

                //Compare input and output files
                bool isEqual = file_Manager.CompareFiles(input_File, output_File);
                ConsoleView.DisplayMessage(isEqual ? "\nSuccess! Input and Output files are same."
                                                    : "\nSomething went wrong! Files do not match.");
            }
            catch (Exception ex)
            {
                ConsoleView.DisplayMessage($"\nError: {ex.Message}");
            }
        }
    }
}
