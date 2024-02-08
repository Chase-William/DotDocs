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

                //var prop = typeof(TypeNames<,>).GetField("TypeName15");
                //prop.FieldType.PutTypeName(typeof(TypeNames<,>));
                //var name = RenderState.Builder.ToString();
                // var outs = builder.Renderer.Assemblies[0].binary.ExportedTypes;
                // var t = builder.Renderer.Assemblies[0].binary.ExportedTypes.Single(t => t.Name == (typeof(TypeNames<,>).Name));
                // var fromDeps = typeof(TypeNames<,>);
                // var fromCtx = builder.Renderer.Assemblies[0].binary.ExportedTypes.Single(t => t.Name == (typeof(TypeNames<,>).Name));
                //var prop = fromCtx.GetField("TypeName15");
                //prop.FieldType.PutTypeName(fromCtx);
                //var name = RenderState.Builder.ToString();
                //Console.WriteLine();
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
