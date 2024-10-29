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
    public partial class Admin : Form
    {
        Db _db = new Db();
        int casa_id = 0;
        int dulce_id = 0;
        string casa_Nombre;
        int key;
        public Admin(int key)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            llenarGrid();
            llenarListaCasas();
            llenarListaDulces();
            this.key = key;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Casas casas1 = new Casas(key, 0);
            casas1.Show();
            this.Hide();
        }

        private void btnCrearU_Click(object sender, EventArgs e)
        {
            gbParticipante.Visible = true;
            gbParticipante.BringToFront();
        }

        private void btnCrearUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text) ||
                string.IsNullOrWhiteSpace(txtCod.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de continuar.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nombre = txtNombre.Text;
            string pass = txtPass.Text;
            string cod = txtCod.Text;
            txtNombre.Clear();
            txtPass.Clear();
            txtCod.Clear();
            _db.CreacionUser(nombre, pass, cod);
            gbParticipante.Visible = false;
            llenarGrid();
        }

        private void llenarGrid()
        {
            List<Usuario> users = _db.ObtenerUsers();
            dataGridView1.Rows.Clear();
            foreach (Usuario user in users)
            {
                int rowIndex = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                row.Cells[0].Value = user.id;
                row.Cells[1].Value = user.nombre;
                row.Cells[2].Value = user.nombre_disfraz;
            }
        }

        private void llenarListaCasas()
        {
            List<Casa> casas = _db.ObtenerCasa();
            lstCasas.Items.Clear();
            foreach (Casa casa in casas)
            {
                lstCasas.Items.Add(casa);
            }

        }

        private void btnCasa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCasa.Text))
            {
                MessageBox.Show("Por favor, complete el campo para crear", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nombreCasa = txtNombreCasa.Text;
            _db.InsertarCasa(nombreCasa);
            llenarListaCasas();
        }

        private void lstCasas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCasas.SelectedItem is Casa casaSeleccionada)
            {
                int idCasaSeleccionada = casaSeleccionada.id;
                if (idCasaSeleccionada > 0)
                {
                    casa_id = idCasaSeleccionada;
                    casa_Nombre = casaSeleccionada.nombre;
                }
            }
            else
            {
                MessageBox.Show("Error: El elemento seleccionado no es una instancia de Casa.");
            }

            if (lstCasas.SelectedItem is Casa casaSeleccion)
            {
                // Limpiar el ListBox de dulces antes de mostrar nuevos datos
                lstDulces.Items.Clear();

                // Obtener dulces de la casa seleccionada
                var dulces = _db.ObtenerDulcesPorCasa(casaSeleccion.id);

                // Agregar los dulces y sus cantidades al ListBox
                foreach (var dulce in dulces)
                {
                    lstDulces.Items.Add($"{dulce.NombreDulce}: {dulce.Cantidad}");
                }
            }
        }



        private void btnDulce_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDulce.Text))
            {
                MessageBox.Show("Por favor, complete el campo para crear", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string nombreDulce = txtDulce.Text;
            _db.InsertarDulce(nombreDulce);
            llenarListaDulces();

        }

        private void llenarListaDulces()
        {
            List<Dulce> dulces = _db.ObtenerDulces();
            lstDulce.Items.Clear();

            foreach (Dulce dulce in dulces)
            {
                lstDulce.Items.Add(dulce); // Agrega el objeto completo
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregarEnCasa_Click(object sender, EventArgs e)
        {

            if (lstDulce.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un dulce de la lista primero");
            }
            else
            {
                if (Convert.ToInt32(CantidadAgregar.Value) > 0)
                {
                    int cantidad = Convert.ToInt32(CantidadAgregar.Value);
                    _db.RelacionarCasaDulces(casa_id, dulce_id, cantidad);
                    CantidadAgregar = null;
                    
                }
                else
                {
                    MessageBox.Show("Verifique la cantidad a agregar");
                }
            }
            
        }

        private void lstDulce_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDulce.SelectedItem is Dulce selectedDulce)
            {
                int dulceId = selectedDulce.ID;
                if (dulceId > 0) 
                {
                    dulce_id = dulceId;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lstCasas.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una casa de la lista primero");
            }
            else
            {
                gbCasaDulces.Visible = true;
                if (gbCasaDulces.Visible)
                {
                    lblNombreCasa.Text = casa_Nombre.ToString();
                }
            }
                
                
        }

        private void btmSalir_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();

            form1.Show();

            this.Close();
        }

        private void btnVotacion_Click(object sender, EventArgs e)
        {
            Votacion votacion = new Votacion(key, 0);
            votacion.Show();
            this.Close();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            SoundPlayer background = new SoundPlayer("G:\\Descargas\\fondo3.wav");
            background.PlayLooping();
        }
    }
}
