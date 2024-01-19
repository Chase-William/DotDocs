using DotDocs.Build.Exceptions;
using DotDocs.Markdown;
using System;
using System.Threading.Tasks;

namespace DotDocs.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            // Test test library
            // Run(Builder.FromUrl("https://github.com/Chase-William/Custom2DArray"));
            Run(Builder.FromPath(
                @"C:\Dev\ex\DotDocs", 
                "docs",
                new MarkdownRenderer()));
        }

        static void Run(Builder builder)
        {
            try
            {                              
                builder.Prepare(); // Performs downloads if nessessary/checks directory validity
                builder.Build(); // Compiles project(s) and creates models using results
                builder.Document(); // Creates documentation from models
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
