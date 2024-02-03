using DotDocs.Build.Exceptions;
using DotDocs.IO;
using DotDocs.IO.Routing;
using DotDocs.Markdown;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DotDocs.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            // Test test library
            // Run(Builder.FromUrl("https://github.com/Chase-William/Custom2DArray"));            
            using var builder = Builder.FromPath(
                @"C:\Dev\ex\DotDocs.TestData",
                new MarkdownRenderer(
                    new TextFileOutput(
                        "docs",
                        new FlatRouter(),
                        ".md")));
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
