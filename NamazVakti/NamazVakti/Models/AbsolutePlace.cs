using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NamazVakti.Models
{
    public class AbsolutePlace
    {
        public Ulke Country { get; set; }
        public Sehir City { get; set; }
        public Ilce Town { get; set; }
    }
}
