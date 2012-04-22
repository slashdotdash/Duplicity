using System.IO;
using Microsoft.VisualBasic.Devices;

namespace Duplicity.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceDirectory = TempPath.GetTempDirectoryName();
            var targetDirectory = TempPath.GetTempDirectoryName();
            
            new Computer().FileSystem.CopyDirectory(sourceDirectory, targetDirectory);

            using (new Duplicator(sourceDirectory, targetDirectory))
            {
                System.Console.WriteLine(string.Format(@"Duplicating changes: ""{0}"" => ""{1}""", sourceDirectory, targetDirectory));
                System.Console.WriteLine("Press any key to exit...");
                System.Console.ReadLine();
            }

            Directory.Delete(targetDirectory, true);
        }
    }
}