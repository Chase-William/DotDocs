using DotDocs.IO;
using DotDocs.Models;
using DotDocs.Models.Language;
using DotDocs.Models.Language.Members;
using DotDocs.Render;
using System.Collections.Immutable;
using System.Linq;
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

                // Class Name
                builder.AppendMarkdownHeader(model.Name, HeaderVariant.H1, false);
                if (model.BaseType is not null)
                    builder.Append($" : {model.BaseType.AsMaybeLink()}");
                builder.LinePadding();

                builder.AppendMarkdownHeader("Exported Fields", HeaderVariant.H2);
                model.Fields.ToMarkdown(builder, (m) =>
                {
                    builder.AppendMarkdownHeader($"{m.FieldType.AsMaybeLink()} {m.Name}", HeaderVariant.H3);                                          

                    return Padding.NoPadding;
                });

                builder.AppendMarkdownHeader("Exported Methods", HeaderVariant.H2);
                model.Methods.ToMarkdown(builder, (m) =>
                {
                    builder.AppendMarkdownHeader($"{m.ReturnType.AsMaybeLink()} {m.Name}", HeaderVariant.H3, false);
                   
                    // Create parameter listing
                    builder.Append(m.Parameters.AsMarkdownParams());
                    

                    return Padding.Default;
                });

                builder.AppendMarkdownHeader("Exported Propertes", HeaderVariant.H2);
                model.Properties.ToMarkdown(builder, (m) =>
                {
                    builder.AppendMarkdownHeader($"{m.PropertyType.AsMaybeLink()} {m.Name}", HeaderVariant.H3);
                    return Padding.NoPadding;
                });

                builder.AppendMarkdownHeader("Exported Events", HeaderVariant.H2);
                model.Events.ToMarkdown(builder, (m) =>
                {
                    builder.AppendMarkdownHeader($"{m.EventHandlerType.AsMaybeLink()} {m.Name}", HeaderVariant.H3);
                    return Padding.NoPadding;
                });

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

    public enum Padding
    {
        Default,
        NoPadding
    }

    public static class Extensions
    {
        const string DEFAULT_SPACING = "\n\n";

        /// <summary>
        /// Iterates over a collection of models calling the given render function on each one.
        /// After each callback, default line padding is added to the provided <see cref="StringBuilder"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="models"></param>
        /// <param name="builder"></param>
        /// <param name="render"></param>
        public static void ToMarkdown<T>(
            this IEnumerable<T> models,
            StringBuilder builder,
            Func<T, Padding> render)
            where T : MemberModel
        {
            foreach (var model in models)
            {
                if (render(model) == Padding.Default)
                    builder.LinePadding();
            }
        }

        public static string AsMarkdownParams(this ParamInfoModel[] _params)        
            => $"({string.Join(", ", _params.Select(p => $"{p.ParamType.Name.AsCode()} {p.Name.AsItalic()}"))})";                                

        public static string AsItalic(this string str)
            => $"*{str}*";

        public static string AsBold(this string str)
            => $"**{str}**";

        public static string AsBoldItalic(this string str)
            => $"***{str}***";

        public static string AsCode(this string str)
            => $"`{str}`";        

        public static string AsMaybeLink(this ITypeable model)
        {
            if (model is TypeModel)
                return model.Name
                .AsCode()
                .AsLink("./" + model.Name);
            return model.Name.AsCode();
        }

        //public static string AsLink(this TypeModel model)
        //    => model.Name
        //        .AsCode()
        //        .AsLink("./" + model.Name);

        public static string AsLink(this string str, string href)
            => $"[{str}]({href})";

        public static void AppendMarkdownHeader(
            this StringBuilder builder,
            string str,
            HeaderVariant variant,
            bool padded = true
            ) {            
            // + 1 for space character
            var temp = new char[(int)variant + 1];
            byte i = 0;
            for (; i < (byte)variant; i++)
                temp[i] = '#';
            temp[i++] = ' ';

            builder.Append(temp);
            builder.Append(str);
            if (padded)
                builder.LinePadding();
        }

        /// <summary>
        /// Adds a line height padding using <see cref="DEFAULT_SPACING"/>.
        /// </summary>
        /// <param name="builder"></param>
        public static void LinePadding(this StringBuilder builder)
            => builder.Append(DEFAULT_SPACING);
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