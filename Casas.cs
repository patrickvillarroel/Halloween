using HalloweenTown.Datos;
using HalloweenTown.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloweenTown
{
    public partial class Casas : Form
    {
        private Button[] casas;
        private ContextMenuStrip[] menusDulces;
        private Db _db = new Db();  // Instancia de acceso a base de datos
        private List<Casa> listaCasas;  // Lista de casas desde la base de datos
        private int key;
        private int id;
        public Casas(int key, int id)
        {
            this.Text = "Paradas de Dulces";
            this.Size = new Size(800, 600);
            this.key = key;
            InicializarCasas();
            RevisarTodasLasCasas();  // Verificación inicial de estado de dulces en cada casa
            if(key == 1)
            {
                CrearBotonAdmin();
            }
            this.id = id;
            CrearBotonVotacion();
            this.WindowState = FormWindowState.Maximized;
            this.BackgroundImage = Image.FromFile("C:\\Users\\Patrick\\source\\repos\\HalloweenTown\\Resources\\wallpaperbetter.jpg");
            this.FormBorderStyle = FormBorderStyle.None;
            SoundPlayer background = new SoundPlayer("G:\\Descargas\\fondo4.wav");
            background.PlayLooping();
        }

        private void CrearBotonAdmin()
        {
            Button btnActualizar = new Button
            {
                Text = "Admin",
                Location = new Point(1020, 11),
                Size = new Size(120, 30),
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold)
            };

            btnActualizar.Click += (sender, e) =>
            {
                // Crear instancia de la ventana Admin
                Admin admin = new Admin(key);

                // Mostrar la ventana Admin
                admin.Show();

                // Cerrar la ventana actual (Casas)
                this.Close();
            };

            this.Controls.Add(btnActualizar);
        }

        private void CrearBotonVotacion()
        {
            Button btnActualizar = new Button
            {
                Text = "Votacion",
                Location = new Point(1150, 11),
                Size = new Size(120, 30),
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold)
            };

            btnActualizar.Click += (sender, e) =>
            {
                // Crear instancia de la ventana Admin
                Votacion votacion = new Votacion(key, id);

                // Mostrar la ventana Admin
                votacion.Show();

                // Cerrar la ventana actual (Casas)
                this.Close();
            };

            this.Controls.Add(btnActualizar);
        }


        private void InicializarCasas()
        {
            listaCasas = _db.ObtenerCasa();
            int numeroDeCasas = listaCasas.Count;
            casas = new Button[numeroDeCasas];
            menusDulces = new ContextMenuStrip[numeroDeCasas];

            Random random = new Random();
            int separacionMinima = 150;

            for (int i = 0; i < numeroDeCasas; i++)
            {
                casas[i] = new Button
                {
                    Size = new Size(100, 100),
                    BackgroundImage = Image.FromFile("C:\\Users\\Patrick\\source\\repos\\Prueba casas\\Casas.png"),
                    BackgroundImageLayout = ImageLayout.Stretch,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.Transparent,
                    Location = GenerarUbicacionAleatoria(random, separacionMinima, i)
                };

                InicializarMenuDulces(i, listaCasas[i].id);

                int indice = i;
                casas[indice].Click += (sender, e) =>
                {
                    // Verifica si la casa tiene dulces antes de mostrar el menú
                    var dulces = _db.ObtenerDulcesPorCasa(listaCasas[indice].id);
                    bool tieneDulces = dulces.Any(d => d.Cantidad > 0);

                    if (tieneDulces)
                    {
                        Button casaSeleccionada = sender as Button;
                        menusDulces[indice].Show(casaSeleccionada, new Point(casaSeleccionada.Width, 0));
                    }
                    else
                    {
                        MessageBox.Show("Esta casa ya no tiene dulces disponibles.");
                    }
                };

                this.Controls.Add(casas[i]);
            }
        }

        private Point GenerarUbicacionAleatoria(Random random, int separacionMinima, int indiceActual)
        {
            int x, y;
            bool ubicadoCorrectamente;

            do
            {
                x = random.Next(50, 700);
                y = random.Next(50, 500);
                ubicadoCorrectamente = true;

                for (int j = 0; j < indiceActual; j++)
                {
                    if (Math.Abs(casas[j].Location.X - x) < separacionMinima && Math.Abs(casas[j].Location.Y - y) < separacionMinima)
                    {
                        ubicadoCorrectamente = false;
                        break;
                    }
                }
            } while (!ubicadoCorrectamente);

            return new Point(x, y);
        }

        private void InicializarMenuDulces(int index, int casaId)
        {
            menusDulces[index] = new ContextMenuStrip();
            var dulces = _db.ObtenerDulcesPorCasa(casaId);

            foreach (var dulce in dulces)
            {
                string Nombredulces = dulce.NombreDulce.ToString() + "(" + dulce.Cantidad.ToString() + ")";
                ToolStripMenuItem itemDulce = new ToolStripMenuItem(Nombredulces);
                ToolStripControlHost numericControl = new ToolStripControlHost(new NumericUpDown()
                {
                    Maximum = dulce.Cantidad  // Limita la cantidad máxima según la base de datos
                });
                ToolStripMenuItem confirmarSeleccion = new ToolStripMenuItem("Tomar dulces");

                confirmarSeleccion.Click += (sender, e) =>
                {
                    NumericUpDown cantidadDulces = (NumericUpDown)numericControl.Control;
                    int cantidadTomada = (int)cantidadDulces.Value;
                    int cantidadRestante = dulce.Cantidad - cantidadTomada;

                    _db.ActualizarCantidadDulces(dulce.id, cantidadRestante);
                    MessageBox.Show($"Has tomado {cantidadTomada} {dulce.NombreDulce} en la casa {index + 1}");

                    if (cantidadRestante <= 0)
                    {
                        itemDulce.Enabled = false;
                        RevisarEstadoCasa(casaId, index);
                    }
                };

                itemDulce.DropDownItems.Add(numericControl);
                itemDulce.DropDownItems.Add(confirmarSeleccion);
                menusDulces[index].Items.Add(itemDulce);
            }
        }

        private void RevisarTodasLasCasas()
        {
            for (int i = 0; i < listaCasas.Count; i++)
            {
                RevisarEstadoCasa(listaCasas[i].id, i);
            }
        }

        private void RevisarEstadoCasa(int casaId, int indiceBoton)
        {
            var dulces = _db.ObtenerDulcesPorCasa(casaId);
            bool tieneDulces = dulces.Any(d => d.Cantidad > 0);

            if (!tieneDulces)
            {
                casas[indiceBoton].BackgroundImage = Image.FromFile("C:\\Users\\Patrick\\source\\repos\\HalloweenTown\\Resources\\CasaVacía.png");
            }
            else
            {
                casas[indiceBoton].BackgroundImage = Image.FromFile("C:\\Users\\Patrick\\source\\repos\\Prueba casas\\Casas.png");
            }
        }
    }

}


