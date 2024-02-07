using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DotDocs.Source.One.Supporting;

namespace Test.DotDocs.Source.One.TypeNames
{
    public class TypeNames<T, K>
    {
        // Arrays
        public int TypeName01;
        public int[] TypeName02;
        public int[,] TypeName03;
        public int[][] TypeName04;
        public int[][,][] TypeName05;
        public int[][,][][,,] TypeName06;
        public int[,][][][,,,][,] TypeName07;

        // Simple Generics with Arrays
        public TypeNames<int, int> TypeName08;
        public TypeNames<int, int>[] TypeName09;
        public TypeNames<int, int>[,] TypeName10;
        public TypeNames<int, int>[][] TypeName11;
        public TypeNames<int, int>[][,][] TypeName12;
        public TypeNames<int, int>[][,][][,,] TypeName13;
        public TypeNames<int, int>[,][][][,,,][,] TypeName14;

        // Complex Generics with Complex Arrays
        public MyGenericClassRef<int[], int>[] TypeName15;
        public MyGenericClassRef<int[], MyStructRef>[] TypeName16;
        public MyGenericClassRef<int[], MyGenericClassRef<int[], MyStructRef[][]>>[] TypeName17;
        public MyGenericClassRef<int[,], MyGenericClassRef<int[,], MyStructRef[,][,]>>[] TypeName18;
    }
}
