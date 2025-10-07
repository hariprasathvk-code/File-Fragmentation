namespace FileFragmentationMVC.Models
{
    public class Fragment
    {
        public string FileName { get; set; }
        public string Content { get; set; }

        public Fragment(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
