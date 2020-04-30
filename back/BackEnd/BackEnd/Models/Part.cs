using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class Part
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ModelUrl { get; private set; }   
        public float Price { get; private set; }
        public IEnumerable<Material> PossibleMaterials { get; private set; }
    }
}