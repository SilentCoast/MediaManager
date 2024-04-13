using System.Runtime.CompilerServices;

namespace PhotoSorter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string target = "";
            string destination = "";

            FileProcesser.Process(target,destination);

            Console.WriteLine("\nApp fineshed working");
            Console.ReadKey();
        }
        
    }
}
