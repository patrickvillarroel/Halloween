using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloweenTown.Entidades
{
    public class Dulce
    {
        public int ID { get; set; }
        public string nombre {  get; set; }

        public override string ToString()
        {
            return nombre;
        }
    }
}
