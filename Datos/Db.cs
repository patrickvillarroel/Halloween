using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using HalloweenTown.Entidades;
using System.Media;

namespace HalloweenTown.Datos
{
    public class Db
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        public Db()
        {
            string cadenaConexion = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            con = new SqlConnection();
            con.ConnectionString = cadenaConexion;
            cmd = new SqlCommand();
            cmd.Connection = con;
        }

        public int Login(string nombre, string password, string cod)
        {

            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT rol, codigo_invitacion FROM Usuarios WHERE nombre = @nombre AND user_password = @password";
                cmd.Connection.Open();

                // Uso de parámetros para evitar SQL Injection
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataReader reader = cmd.ExecuteReader();
                    
                if (reader.Read()) // Verifica si hay resultados
                {
                    int rol = Convert.ToInt32(reader["rol"]);
                    string codigoInvitacion = reader["codigo_invitacion"].ToString();

                    if (rol == 1) // Usuario es administrador
                    {
                        MessageBox.Show("Bienvenido Admin");
                        return 1; // Retorna valor para admin
                    }
                    else // Usuario es regular
                    {
                        

                        if (cod == codigoInvitacion)
                        {
                            MessageBox.Show("Bienvenido Usuario");
                            return 0; // Retorna valor para usuario normal con código correcto
                        }
                        else
                        {
                            MessageBox.Show("Código de invitación incorrecto");
                            return 3; // Retorna un valor indicando código incorrecto
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Usuario no encontrado");
                    return 2; // Retorna valor cuando no se encuentra el usuario
                    
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 2;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public void CreacionUser(string nombre, string pass, string cod)
        {
            try
            {   cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_InsertarUsuario";
                // Agregar parámetros
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@user_password", pass);
                cmd.Parameters.AddWithValue("@codigo_invitacion", cod);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario insertado correctamente.");
            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al insertar el usuario: " + s.Message);

            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

        }

        public void InsertarDisfraz(int id ,string nombre, string path)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_InsertarDisfrazYFoto";
                // Agregar parámetros
                cmd.Parameters.AddWithValue("@id_usuario", id);
                cmd.Parameters.AddWithValue("@nombreDisfraz", nombre);
                cmd.Parameters.AddWithValue("@imagenDisfraz", path);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                SoundPlayer background = new SoundPlayer("G:\\Descargas\\risa.wav");
                background.Play();
                MessageBox.Show("Bienvenido Participante");
            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al registrar el disfraz: " + s.Message);

            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public int VerficarDisfraz(string nombre, string pass)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_VerificarLogin";
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@user_password", pass);
                cmd.Connection.Open();
                int resultado;

                // Ejecutar y obtener el resultado
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    resultado = Convert.ToInt32(result);

                    if (resultado == 2)
                    {
                        SoundPlayer background = new SoundPlayer("G:\\Descargas\\risa.wav");
                        background.Play();
                        return 1;
                    }
                    else if (resultado == 1)
                    {
                        return 2;
                    }
                    else
                    {
                        MessageBox.Show("Usuario no encontrado.");
                        return 0;
                    }
                }
            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al buscar el disfraz: " + s.Message);
                return 0;

            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
                return 0;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return 0;
        }

        public int Id_Usuario(string nombre, string pass)
        {
            int usuarioId = 0;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT id_usuarios FROM Usuarios WHERE nombre = @nombre AND user_password = @password";
                cmd.Connection.Open();
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@password", pass);

                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    usuarioId = Convert.ToInt32(result);
                }
            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al obtener el ID del usuario: " + s.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally { cmd.Connection.Close(); }
            return usuarioId;
        }

        public List<Usuario> ObtenerUsers()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Usuarios WHERE NOT rol = 1";

                cmd.Connection.Open();
                ds = new DataSet();

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var user = new Usuario()
                        {
                            id = Convert.ToInt32(row["id_usuarios"].ToString()),
                            nombre = row["nombre"].ToString(),
                            password = row["user_password"].ToString(),
                            imagen = row["imagenDisfraz"].ToString(),
                            nombre_disfraz = row["nombreDisfraz"].ToString(),
                            
                        };
                        usuarios.Add(user);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return usuarios;
        }

        public void InsertarCasa(string nombre)
        {
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.CommandText = "sp_InsertarCasa";

            try
            {
                cmd.Connection.Open();
                int afectado = cmd.ExecuteNonQuery();

            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al crear la casa: " + s.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally { cmd.Connection.Close(); }
        }

        public void InsertarDulce(string nombre)
        {
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.CommandText = "sp_InsertarDulce";

            try
            {
                cmd.Connection.Open();
                int afectado = cmd.ExecuteNonQuery();

            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al crear el dulce: " + s.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally { cmd.Connection.Close(); }
        }

        public List<Casa> ObtenerCasa()
        {
            List<Casa> casas = new List<Casa>();
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM Casas";

                cmd.Connection.Open();
                ds = new DataSet();

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var casa = new Casa()
                        {
                            id = Convert.ToInt32(row["id_casas"].ToString()),
                            nombre = row["nombre"].ToString(),
                           
                        };
                        casas.Add(casa);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return casas;
        }

        public List<(int id, string NombreDulce, int Cantidad)> ObtenerDulcesPorCasa(int idCasa)
        {
            List<(int id, string NombreDulce, int Cantidad)> dulces = new List<(int, string, int)>();
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdCasa", idCasa);
            cmd.CommandText = "sp_ObtenerDulcesPorCasa";
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {   
                        int id = Convert.ToInt32(reader["Id"].ToString());
                        string nombreDulce = reader["NombreDulce"].ToString();
                        int cantidad = Convert.ToInt32(reader["Cantidad"]);

                        dulces.Add((id, nombreDulce, cantidad));
                    }
                }
            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al obtener dulces: " + s.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return dulces;
        }

        public void RelacionarCasaDulces(int idCasa, int dulcesId, int cantidad) 
        {
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@casas_id", idCasa);
            cmd.Parameters.AddWithValue("@dulces_id", dulcesId);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.CommandText = "sp_RelacionarCasaDulce";
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Relación de casa y dulce creada correctamente.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al relacionar la casa con el dulce: " + ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public List<Dulce> ObtenerDulces()
        {
            List<Dulce> dulces = new List<Dulce>();

            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_ObtenerDulces";
            try
            {
                cmd.Connection.Open();
                ds = new DataSet();

                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var dulce = new Dulce()
                        {
                            ID = Convert.ToInt32(row["id_dulces"].ToString()),
                            nombre = row["nombre"].ToString(),

                        };
                        dulces.Add(dulce);
                    }
                }
            }
            catch (SqlException s)
            {
                MessageBox.Show("Error al obtener dulces: " + s.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dulces;

        }

        public void ActualizarCantidadDulces(int dulceId, int nuevaCantidad)
        {
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Parada_Dulces SET Cantidad = @Cantidad WHERE dulces_id = @DulceId";
            cmd.Parameters.AddWithValue("@Cantidad", nuevaCantidad);
            cmd.Parameters.AddWithValue("@DulceId", dulceId);
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Actualizado");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

        }

        public List<Usuario> ListaDisfraces(int ID)
        {
            List<Usuario> usuarios = new List<Usuario>();
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUsuarioLista";
            cmd.Parameters.AddWithValue("@id_usuario", ID);
            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            nombre = reader["nombre"].ToString(),
                            imagen = reader["imagenDisfraz"].ToString(),
                            nombre_disfraz = reader["nombreDisfraz"].ToString(),
                            id = Convert.ToInt32(reader["id_usuarios"])
                        };

                        usuarios.Add(usuario);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return usuarios;
        }

        public void InsertarPuntos(int idParticipante, int idVotante, int puntuacion)
        {
            // Limpiar parámetros previos
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_Puntuaciones";

            // Agregar parámetros al comando
            cmd.Parameters.AddWithValue("@IdParticipante", idParticipante);
            cmd.Parameters.AddWithValue("@Id_Votante", idVotante);
            cmd.Parameters.AddWithValue("@Puntuacion", puntuacion);

            try
            {
                // Ejecutar el procedimiento almacenado
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al votar: " + ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public List<Usuario> ObtenerTop3Disfraces()
        {
            List<Usuario> topDisfraces = new List<Usuario>();

            cmd.CommandText = "SELECT id_usuarios, nombreDisfraz, imagenDisfraz FROM v_PodioTop3";
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.Connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topDisfraces.Add(new Usuario
                        {
                            id = reader.GetInt32(0),
                            nombre_disfraz = reader.GetString(1),
                            imagen = reader.GetString(2)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el podio: " + ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return topDisfraces;
        }

    }
}
