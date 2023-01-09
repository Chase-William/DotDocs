using System;
using System.IO;
using System.IO.Compression;
using DotDocs.Core;
using DotDocs.Core.Models.Exceptions;

namespace DotDocs.Runner
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            //Run(csProjFile: args[0],
            //    outputPath: args[1]);

            //var docs = Charper.From(
            //    csProjPath: @"C:\Dev\Charp.Core\src\Charp.Core\Charp.Core.csproj",
            //    dllPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.dll",
            //    xmlPath: @"C:\Dev\Charp.Core\src\Charp.Core\bin\Debug\net6.0\Charp.Core.xml",
            //    outputPath: @"C:\Users\Chase Roth\Desktop"
            //);

            // Test project in entire diff dir
            //Run(csProjFile: @"C:\Dev\ProjDepResolver\src\ProjDepResolver\ProjDepResolver.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop");

            // Test test library
            //Run(csProjFile: @"C:\Dev\Docshark.Core\test\Docshark.Test.Data\Docshark.Test.Data.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop");

            // Test for project tree serialization (has complex local project dependency setup)
            //Run(@"C:\Dev\Docshark.Core\test\Docshark.Test.Mapper.Project.Data\ProjectA\ProjectA.csproj",
            //    @"C:\Users\Chase Roth\Desktop");

            // Test test library
            Run(url: "https://github.com/Chase-William/.Docs.Core",
                outputPath: @"C:\Users\Chase Roth\Desktop");

            //Run(csProjFile: @"C:\Dev\.Docs.Core - Copy\src\DotDocs.Core\DotDocs.Core.csproj",
            //    outputPath: @"C:\Users\Chase Roth\Desktop");
        }

        static void Run(string url, string outputPath)
        {
            try
            {
                // using var outStream = new MemoryStream();
                // using var zipArchive = new ZipArchive(outStream, ZipArchiveMode.Create, true);
                // var fileInArchive = zipArchive.CreateEntry("test.csv", CompressionLevel.Optimal);
                // using var fileInArchiveStream = fileInArchive.Open();
                // fileInArchiveStream.Write(System.Text.Encoding.UTF8.GetBytes("Hello, World"));
                // File.WriteAllBytes(Path.Combine(outputPath, "test.zip"), outStream.GetBuffer());

                //using var outStream = new MemoryStream();
                //using var zipArchive = new ZipArchive(outStream, ZipArchiveMode.Create, true);
                //var fileInArchive = zipArchive.CreateEntry("test.csv", CompressionLevel.Optimal);
                //using var fileInArchiveStream = fileInArchive.Open();
                //fileInArchiveStream.Write(System.Text.Encoding.UTF8.GetBytes("Hello, World"));
                //return File(outStream.GetBuffer(), "application/zip");

                string config = "{\r\n  \"perspective\": \"internal\",\r\n  \"type\": {\r\n    \"class\": {\r\n      \"showIfInternalProtected\": true,\r\n      \"denoteIfStatic\": false\r\n    }\r\n  },\r\n  \"member\": {\r\n    \"property\": {\r\n      \"showIfPublic\": false,\r\n      \"showIfInternalProtected\": true,\r\n      \"denoteIfStatic\": false,\r\n      \"denoteIfSetonly\": false\r\n    }\r\n  }\r\n}";

                Core.DotDocs.Init();
                var builder = Core.DotDocs.New(url, config);
                builder.Document();
                //builder.Prepare();
                //builder.Load();
                //using var docs = new DotDocs(
                //    csProjFile: csProjFile,
                //    outputPath: outputPath
                //);

                //docs.Prepare();
                //docs.Load();
                //string temp = Path.Combine(outputPath, "test.zip");
                //Console.WriteLine(temp);
                //using var test = docs.Document();
                //File.WriteAllBytes(temp, test.ToArray());
            }
            catch (BuildException ex)
            {
                Console.WriteLine(((int)ErrorCodes.UserError) + " " + ex.Message);
            }
            catch (MissingProjectFileException ex)
            {
                Console.WriteLine(((int)ErrorCodes.UserError) + " " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine((int)ErrorCodes.InternalError + " " + ex.Message);
            }
        }
    }
}
