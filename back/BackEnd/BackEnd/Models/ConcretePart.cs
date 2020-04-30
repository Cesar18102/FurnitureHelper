using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEnd.Models
{
    public class ConcretePart : Part
    {
        public Material SelectedMaterial { get; private set; }
        public PartColor SelectedColor { get; private set; }
    }
}