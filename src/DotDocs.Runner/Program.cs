using DotDocs.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace DotDocs.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            // Test test library
            Run(url: "https://github.com/Chase-William/DotDocs",
                outputPath: @"C:\Users\cxr69\Desktop");
        }

        static async Task Run(string url, string outputPath)
        {
            try
            {
                var builder = DotDocs.New(url);
                builder.Document();
            }
            catch (BuildException ex)
            {
                Console.WriteLine("There was a problem with building the user's project; they may have requested us to build a faulty project. " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
