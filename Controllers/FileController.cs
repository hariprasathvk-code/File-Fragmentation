using System;
using System.Collections.Generic;
using System.IO;
using FileFragmentationMVC.Models;
using FileFragmentationMVC.Views;

namespace FileFragmentationMVC.Controllers
{
    public class FileController
    {
        private FileManager _fileManager = new FileManager();
        private List<Fragment> _fragments = new List<Fragment>();
        private string _folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files");
        private string _inputFile => Path.Combine(_folderPath, "input.txt");
        private string _outputFile => Path.Combine(_folderPath, "output.txt");


        public void Run()
        {
            try
            {
                // Ensure folder exists
                if (!Directory.Exists(_folderPath))
                    Directory.CreateDirectory(_folderPath);

                // Step 1: Create input file
                string paragraph = ConsoleView.GetUserInput("Enter paragraph: ");
                if (string.IsNullOrWhiteSpace(paragraph))
                    throw new ArgumentException("Paragraph cannot be empty!");
                _fileManager.CreateFile(_inputFile, paragraph);

                // Step 2: Fragmentation
                int fragSize = int.Parse(ConsoleView.GetUserInput("Enter fragment size (number of characters per file): "));
                if (fragSize <= 0)
                    throw new ArgumentException("Fragment size must be greater than zero.");

                _fragments = _fileManager.FragmentFile(_inputFile, _folderPath, fragSize);

                ConsoleView.DisplayMessage("\nFragments created:");
                foreach (var frag in _fragments)
                    ConsoleView.DisplayMessage(Path.GetFileName(frag.FileName));

                // Step 3: Verify fragment
                string fragName = ConsoleView.GetUserInput("\nEnter fragment to check: ");
                string fragPath = Path.Combine(_folderPath, fragName);
                string fragContent = _fileManager.CheckFragment(fragPath);
                ConsoleView.DisplayMessage($"\nContent of {fragName}: {fragContent}");

                // Step 4: Defragmentation
                _fileManager.DefragmentFiles(_fragments, _outputFile);
                ConsoleView.DisplayMessage($"\nDefragmentation done. Output file created: {_outputFile}");

                // Step 5: Compare input and output files
                bool isEqual = _fileManager.CompareFiles(_inputFile, _outputFile);
                ConsoleView.DisplayMessage(isEqual ? "\nSuccess! Input and Output files are identical."
                                                    : "\nSomething went wrong! Files do not match.");

                // Step 6: Skip cleanup
                ConsoleView.DisplayMessage("\nAll files have been kept. You can delete them manually if needed.");
            }
            catch (Exception ex)
            {
                ConsoleView.DisplayMessage($"\nError: {ex.Message}");
            }
        }
    }
}
