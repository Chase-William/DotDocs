using DotDocs.IO;
using DotDocs.Models;
using DotDocs.Models.Language;
using DotDocs.Models.Language.Members;
using DotDocs.Render;
using DotDocs.Render.Args;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;

namespace DotDocs.Markdown
{    
    public class MarkdownRenderer : IRenderable
    {
        const int DEFAULT_STR_BUILDER_CAPACITY = 256;
        
        public RepositoryModel Model { get; set; }

        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }

        public IOutputable Output { get; set; }
         

        public void RenderType(TypeModel model)
        {
            try
            {
                var builder = new StringBuilder(DEFAULT_STR_BUILDER_CAPACITY);
                using var fstream = File.CreateText(Path.Combine(Output.GetValue(), model.Name + ".md"));

                model.Name.ToHeader(builder, HeaderVariant.H1);
                "Exported Fields".ToHeader(builder, HeaderVariant.H2);
                model.Fields.ToFields(builder);

                fstream.Write(builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }

    public enum HeaderVariant : byte
    {
        H1 = 1,
        H2,
        H3,
        H4,
        H5
    }

    /// <summary>
    /// 
    /// 
    /// If output will be just a output file stream, then results should be written directly there
    /// instead of new string instances.
    /// 
    /// 
    /// </summary>

    public static class Extensions
    {
        public static void ToFields(this List<FieldModel> fields, StringBuilder builder)
        {
            foreach (var field in fields)
            {
                builder.Append(field.Name);
                builder.AppendLine();
            }
        }

        public static string ToItalic(this string str)
            => $"*{str}*";

        public static string ToBold(this string str)
            => $"**{str}**";

        public static string ToBoldItanlic(this string str)
            => $"***{str}***";

        public static void ToHeader(
            this string str,
            StringBuilder builder,
            HeaderVariant variant,
            bool newLine = true
            ) {            
            // + 1 for space character
            var temp = new char[(int)variant + 1];
            byte i = 0;
            for (; i < (byte)variant; i++)
                temp[i] = '#';
            temp[i++] = ' ';

            builder.Append(temp);
            builder.Append(str);
            if (newLine)
                builder.AppendLine();
        }
    }

    //public static class Extensions
    //{
    //    public static void Render(this TypeModel model, IOutputable output)
    //    {
    //        string fullPath = model.FullName.Replace('.', '/');
    //        try
    //        {
    //            // File.Create(Path.Combine(output. fullPath + ".md");
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }
    //}
}