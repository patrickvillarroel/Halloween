using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloweenTown.Entidades
{
    public class Casa
    {
        public int id {  get; set; }
        public string nombre { get; set; }

        public Casa() { }
        public Casa(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }

        // Sobrescribe ToString para mostrar solo el nombre en el ListBox
        public override string ToString()
        {
            return nombre;
        }
    }


}
