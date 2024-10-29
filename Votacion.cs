using HalloweenTown.Datos;
using HalloweenTown.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloweenTown
{
    public partial class Votacion : Form
    {
        int key;
        int id;
        Db _db = new Db();
        private Timer screamerTimer;
        private Random random;
        public Votacion(int key, int id)
        {
            InitializeComponent();
            if(key != 0)
            {
                btnAdmin.Visible = true;
            }
            this.key = key;
            this.id = id;
            this.FormBorderStyle = FormBorderStyle.None;
            llenarListView();
            random = new Random();

            // Configura el Timer para activar el screamer en intervalos aleatorios
            screamerTimer = new Timer();
            screamerTimer.Interval = random.Next(2000, 6000); 
            screamerTimer.Tick += MostrarScreamerAleatorio;
            screamerTimer.Start();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin(key);
            admin.Show();
            this.Close();
        }

        private void btmSalir_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Casas casas = new Casas(key, id);
            casas.Show();
            this.Close();
        }

        public void llenarListView()
        {
            lstDisfraces.View = View.Details;

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(70, 70); // Tamaño de las imágenes en el ListView
            lstDisfraces.SmallImageList = imageList;

            List<Usuario> user = _db.ListaDisfraces(id);
            foreach (var participante in user)
            {

                try
                {
                    // Carga la imagen desde el path del participante
                    Image imagenDisfraz = Image.FromFile(participante.imagen);
                    imageList.Images.Add(participante.id.ToString(), imagenDisfraz);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show($"No se encontró la imagen para el participante {participante.nombre}");
                    continue;
                }

                // Crea un nuevo ListViewItem y asigna la imagen, nombre del disfraz y nombre del participante
                ListViewItem item = new ListViewItem
                {
                    Text = participante.id.ToString(),
                    ImageKey = participante.id.ToString() // Vincula la imagen con el item usando la clave
                };
                item.SubItems.Add(participante.nombre_disfraz);
                item.SubItems.Add(participante.nombre);

                // Agrega el item al ListView
                lstDisfraces.Items.Add(item);
            }
        }

        private void ListView_DisfrazSeleccionado(int cantidad)
        {
            if (lstDisfraces.SelectedItems.Count > 0)
            {
                ListViewItem itemSeleccionado = lstDisfraces.SelectedItems[0];

                int idParticipante = int.Parse(itemSeleccionado.Text);

                DialogResult confirmacion = MessageBox.Show($"¿Deseas votar por {itemSeleccionado.SubItems[1].Text}?",
                                                            "Confirmar Votación", MessageBoxButtons.YesNo);
                    
                if (confirmacion == DialogResult.Yes)
                {
                    // Lógica para registrar el voto en la base de datos
                    _db.InsertarPuntos(idParticipante, id, cantidad);

                    // Remueve el participante del ListView para que no pueda ser votado nuevamente
                    lstDisfraces.Items.Remove(itemSeleccionado);

                    NumCantidad.Value = 1;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cantidadPuntos = Convert.ToInt32(NumCantidad.Value);
            ListView_DisfrazSeleccionado(cantidadPuntos);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Podio podio = new Podio();
            podio.Show();
        }

        private void Votacion_Load(object sender, EventArgs e)
        {
            SoundPlayer background = new SoundPlayer("G:\\Descargas\\fondo2.wav");
            background.PlayLooping();
        }

        private void MostrarScreamerAleatorio(object sender, EventArgs e)
        {
            int probabilidad = random.Next(0, 10);

            if (probabilidad == 0)
            {
                screamerTimer.Stop();
                Scream screamer = new Scream();
                screamer.ShowDialog(); // Muestra el screamer

                screamerTimer.Interval = random.Next(20000, 50000);
                screamerTimer.Start();
            }
            SoundPlayer background = new SoundPlayer("G:\\Descargas\\fondo2.wav");
            background.PlayLooping();
        }
    }
}

