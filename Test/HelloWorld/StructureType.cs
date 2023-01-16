using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    internal struct StructureType
    {
        internal readonly int Id { get; init; }
        public readonly int age;
        internal string Name { get; set; }

        public StructureType()
        {
            Id = 10;
            Name = "abc";
            age = 10;
        }
    }

    internal readonly struct StructureType1
    {
        internal int Id { get; }


    }
}
