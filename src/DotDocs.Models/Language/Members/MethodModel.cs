﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Models.Language.Members
{
    public class MethodModel : MemberModel
    {
        public bool ContainsGenericParameters { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsConstructedGenericMethod { get; set; }
        public bool IsConstructor { get; set; }
        public bool IsFinal { get; set; }
        public bool IsGenericMethod { get; set; }
        public bool IsGenericMethodDefinition { get; set; }
        public bool IsHideBySig { get; set; }
        public bool IsVirtual { get; set; }
    }
}