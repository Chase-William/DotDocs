using DotDocs.IO.Routing;
using DotDocs.IO;
using DotDocs.Build.Exceptions;
using DotDocs.Markdown;
using DotDocs;

using System;
using DotDocs.Build;
using DotDocs.Markdown.Extensions;
using System.Linq;

namespace Test.DotDocs.Run
{   
    internal class Program
    {        
        static void Main()
        {
            using var builder = Builder.FromPath(
                "../../../../../../gen/DotDocs/DotDocs.sln",
                "DotDocs.csproj",
                "../../../../../docs");                    
            Run(builder);
        }

        static void Run(Builder builder)
        {
            try
            {
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
