using DotDocs.Models.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Render.Args
{
    public class RenderTypeEventArgs
    {
        public TypeModel Model { get; init; }

        public RenderTypeEventArgs(TypeModel model)
            => Model = model;
    }
}
