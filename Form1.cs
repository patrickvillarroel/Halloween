using HalloweenTown.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HalloweenTown
{
    public partial class Form1 : Form
    {
        int key = 0; 
        Db obj = new Db();
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            SoundPlayer background = new SoundPlayer("G:\\Descargas\\fondo.wav");
            background.PlayLooping();
            Disfraz.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {   
            if(!string.IsNullOrWhiteSpace(txtPassword.Text) & !string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                string nombre = txtNombre.Text;
                string contrasena = txtPassword.Text;
                string codigo = txtCodigo.Text;


                int rol = obj.Login(nombre, contrasena, codigo);
                if (rol == 1)
                {
                    key = 1;
                    Admin admin = new Admin(key);
                    admin.Show();
                    this.Hide();
                }
                else
                {
                    if (rol == 0)
                    {
                        int verificado = obj.VerficarDisfraz(nombre, contrasena);
                        key = 0;

                        if (verificado == 2)
                        {
                            loginPanel.Hide();
                            Disfraz.Visible = true;
                            Disfraz.BringToFront();
                        }
                        else if (verificado == 1)
                        {
                            int id_Usuario = obj.Id_Usuario(nombre, contrasena);
                            Votacion votacion = new Votacion(key, id_Usuario);
                            SoundPlayer background = new SoundPlayer("G:\\Descargas\\risa.wav");
                            background.PlaySync();
                            votacion.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Intente de nuevo");
                        }

                    }
                }

            }
            else
            {
                MessageBox.Show("Debe llenar los campos nombre y password");
            }
            
        }

        private void foto_disfraz_Click(object sender, EventArgs e)
        {
            var ok = disfrazFoto.ShowDialog();
            if (ok == DialogResult.OK)
            {
                foto_disfraz.Image = Image.FromFile(disfrazFoto.FileName);
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (foto_disfraz.Image != null & !string.IsNullOrWhiteSpace(txtDisfraz_name.Text))
            {
                string nombre_disfraz = txtDisfraz_name.Text;
                string imagen_disfraz = disfrazFoto.FileName;
                string nombre = txtNombre.Text;
                string contrasena = txtPassword.Text;
                int id_Usuario = obj.Id_Usuario(nombre, contrasena);
                obj.InsertarDisfraz(id_Usuario, nombre_disfraz, imagen_disfraz);
                Votacion votacion = new Votacion(key, id_Usuario);
                votacion.Show();
                this.Hide();
            }
            else
            {
                // El PictureBox está vacío
                MessageBox.Show("Debe elegir una imagen y un nombre primero");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
    