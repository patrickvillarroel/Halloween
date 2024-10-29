using HalloweenTown.Datos;
using HalloweenTown.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloweenTown
{
    public partial class Podio : Form
    {
        Db _db = new Db();
        public Podio()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            CargarPodio();
        }

        public void CargarPodio()
        {
            List<Usuario> topDisfraces = _db.ObtenerTop3Disfraces();
            int cont = 0;

            foreach (Usuario top in topDisfraces)
            {
                if (cont == 0)
                {
                    lblPrimero.Text = topDisfraces[cont].nombre_disfraz;
                    pbPrimero.Image = Image.FromFile(topDisfraces[cont].imagen);
                }
                else if (cont == 1)
                {
                    lblSegundo.Text = topDisfraces[cont].nombre_disfraz;
                    pbSegundo.Image = Image.FromFile(topDisfraces[cont].imagen);
                }
                else
                {
                    lblTercero.Text = topDisfraces[cont].nombre_disfraz;
                    pbTercero.Image = Image.FromFile(topDisfraces[cont].imagen);
                }
                cont++;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
