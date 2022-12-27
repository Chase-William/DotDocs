﻿using DotDocs.Core.Loader;
using LoxSmoke.DocXml;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace DotDocs.Core
{
    /// <summary>
    /// The entry-point for .Docs.Core.
    /// </summary>
    public class DotDocs
    {
        ///// <summary>
        ///// The main hub for controlling preparing, loading, rendering.
        ///// </summary>
        //public Builder Builder { get; set; }

        ///// <summary>
        ///// Initializwes a new instance of <see cref="global::DotDocs"/> loaded with data.
        ///// </summary>
        ///// <param name="csProjFile">csProject used for locating dependencies and dll/xml if needed.</param>
        ///// <param name="outputPath">Location for JSON output.</param>
        //public DotDocs(string csProjFile, string outputPath)
        //    => Builder = new Builder(csProjFile, outputPath);                           

        static IMongoDatabase commentsDatabase;

        public class Test
        {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
        }

        public static void Init()
        {
            // Create connection to database
            var client = new MongoClient("mongodb://localhost:27017");
            commentsDatabase = client.GetDatabase("test");
            //var col = commentsDatabase.GetCollection<Test>("example");

            //col.InsertOne(new Test
            //{
            //    Name = "Mark Reynolds"
            //});

            //var test = client.ListDatabaseNames();
            //var test2 = test.ToList();
            //Console.WriteLine();
        }

        public static Builder New(string projectFile)
            => new(projectFile, commentsDatabase);

        ///// <summary>
        ///// Cleanup unmanaged resources linked with <see cref="Builder"/>.
        ///// </summary>
        //public void Dispose()
        //{
        //    Builder?.Dispose();
        //    Builder = null;
        //}

        ///// <inheritdoc cref="Builder.Prepare"/>
        //public void Prepare()
        //    => Builder.Prepare();
        ///// <inheritdoc cref="Builder.Load"/>
        //public void Load()
        //    => Builder.Load();
        ///// <inheritdoc cref="Builder.Document"/>
       
        //public MemoryStream Document()
        //{
        //    // Utility.CleanDirectory(output);
        //    var baseOutStream = new MemoryStream();
        //    var zip = new ZipArchive(baseOutStream, ZipArchiveMode.Create, true);
        //    Builder.Document(zip);
        //    return baseOutStream;
        //}
    }
}
