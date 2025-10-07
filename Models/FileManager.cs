using System;
using System.IO;
using System.Collections.Generic;

namespace FileFragmentationMVC.Models
{
	public class FileManager
	{
		public void CreateFile(string path, string content)
		{
			File.WriteAllText(path, content);
		}

		public List<Fragment> FragmentFile(string inputPath, string folderPath, int fragmentSize)
		{
			var fragments = new List<Fragment>();
			if (!File.Exists(inputPath))
				throw new FileNotFoundException("Input file not found!");

			string content = File.ReadAllText(inputPath);
			int count = 1;

			for (int i = 0; i < content.Length; i += fragmentSize)
			{
				string fragmentContent = content.Substring(i, Math.Min(fragmentSize, content.Length - i));
				string fragmentName = Path.Combine(folderPath, $"{count:D3}.txt");
				File.WriteAllText(fragmentName, fragmentContent);
				fragments.Add(new Fragment(fragmentName, fragmentContent));
				count++;
			}

			return fragments;
		}

		public string CheckFragment(string fragmentName)
		{
			if (!File.Exists(fragmentName))
				throw new FileNotFoundException("Fragment file not found!");
			return File.ReadAllText(fragmentName);
		}

		public void DefragmentFiles(List<Fragment> fragments, string outputFile)
		{
			string combined = "";
			fragments.Sort((a, b) => string.Compare(a.FileName, b.FileName));

			foreach (var frag in fragments)
			{
				combined += File.ReadAllText(frag.FileName);
			}

			File.WriteAllText(outputFile, combined);
		}

		public bool CompareFiles(string file1, string file2)
		{
			if (!File.Exists(file1) || !File.Exists(file2)) return false;
			return File.ReadAllText(file1) == File.ReadAllText(file2);
		}

		public void CleanupFiles(List<Fragment> fragments, string outputFile)
		{
			foreach (var frag in fragments)
			{
				if (File.Exists(frag.FileName)) File.Delete(frag.FileName);
			}
			if (File.Exists(outputFile)) File.Delete(outputFile);
		}
	}
}
