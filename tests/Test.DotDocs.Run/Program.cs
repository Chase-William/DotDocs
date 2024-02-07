using DotDocs.IO.Routing;
using DotDocs.IO;
using DotDocs.Build.Exceptions;
using DotDocs.Markdown;
using DotDocs;

using System;

namespace Test.DotDocs.Run
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            using var builder = Builder.FromPath("../../../../../tests/Data", "docs");                    
            Run(builder);
        }

        static void Run(Builder builder)
        {
            try
            {                              
                builder.Prepare(); // Performs downloads if nessessary/checks directory validity
                builder.Build(); // Compiles project(s) and creates models using results
                builder.Render(); // Creates documentation from models
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
