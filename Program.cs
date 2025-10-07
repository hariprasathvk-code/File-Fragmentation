using FileFragmentationMVC.Controllers;

namespace FileFragmentationMVC
{
    class Program
    {
        static void Main(string[] args)
        {
            FileController controller = new FileController();
            controller.Run();
        }
    }
}
