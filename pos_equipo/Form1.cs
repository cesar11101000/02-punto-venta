using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos_equipo
{

    //Operadores/Cajero tipo 1
    //Administrador tipo 2

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "")
            {
                label_errores.Text = "Ingrese credenciales";
                textBox1.Focus();
            }
            else if (textBox1.Text == "")
            {
                label_errores.Text = "Ingrese el usuario";
                textBox1.Text = "";
                
                textBox1.Focus();
            }
            else if (textBox2.Text == "")
            {
                label_errores.Text = "Ingrese la contraseña";
               
                textBox2.Text = "";
                textBox2.Focus();
            } else
            {
                try
                {
                    MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
                    mySqlConnection.Open();
                    String query = "select * from usuarios where id_usuario = " + textBox1.Text + " and password = " + textBox2.Text;
                    MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            datos_usuarios.id = Int32.Parse(reader["id_usuario"].ToString());
                            datos_usuarios.sucursal = "Centro";
                            datos_usuarios.nombre = datos_usuarios.nombre = reader["nombre_usuario"].ToString() + " " + reader["apellido_usuario"].ToString();

                            if (reader["permisosUsuario"].ToString().Equals("1"))
                            {
                                Form2 ventana_pos = new Form2();
                                this.Hide();
                                ventana_pos.ShowDialog();
                                this.Show();
                            }
                            else
                            {
                                Form4 ventana_pos = new Form4();
                                this.Hide();
                                ventana_pos.ShowDialog();
                                this.Show();
                                
                            }
                            
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Usuario/Contraseña incorrecto.");
                    }
                    mySqlConnection.Close();
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Usuario/Contraseña incorrecto.");
                    textBox1.Text = "";
                    textBox2.Text = "";
                } 
                
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
