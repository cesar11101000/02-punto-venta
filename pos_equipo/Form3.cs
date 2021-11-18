using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace pos_equipo
{
    public partial class Form3 : Form
    {
        bool pagocompleto = false;
        public Form3()
        {
            InitializeComponent();
            textBox2.Focus();
            textBox1.Text = venta.total.ToString();
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            

            if (e.KeyCode == Keys.Enter)
            {
                double Total = Double.Parse(textBox1.Text);
                double pagado = Double.Parse(textBox2.Text);
                if (pagado == Total) 
                {
                    pagocompleto = true;
                    textBox3.Text = "0.00";
                }
                else
                if (pagado > Total)
                {
                    double cambio = pagado - Total;
                    textBox3.Text = cambio.ToString();
                    pagocompleto = true;
                } 
                else
                {
                    MessageBox.Show("Pago incompleto.");
                    pagocompleto = false;
                }

            } else 
            
            if (e.KeyCode == Keys.F2)
            {
                this.Hide();
            } else 
            
            if (e.KeyCode == Keys.F5)
            {
                if (pagocompleto == true)
                {
                    MessageBox.Show("Insertamos venta");
                    //insertamos venta en la tabla venta
                    try
                    {
                        MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
                        mySqlConnection.Open();
                        String query = "INSERT INTO ventas values (NULL, CURRENT_DATE(), CURRENT_TIME(), "+datos_usuarios.id+")";
                        MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                        mySqlCommand.ExecuteNonQuery();
                        //obtenemos ultimo id de la venta
                        query = "SELECT LAST_INSERT_ID()";
                        mySqlCommand = new MySqlCommand(query, mySqlConnection);
                        MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                        int idVenta = 0;
                        if (mySqlDataReader.HasRows)
                        {
                            mySqlDataReader.Read();
                            idVenta = mySqlDataReader.GetInt32(0);
                            //MessageBox.Show(mySqlDataReader.GetInt32(0).ToString());
                            //mySqlDataReader.GetDouble(0);
                        }
                        //insertamos los detalles en venta detalles

                        mySqlConnection.Close();
                        mySqlConnection.Open();

                        for (int i = 0; i < Form2.dataGridView1.Rows.Count; i++)
                        {
                            query = "INSERT INTO ventas_detalle (id_venta, id_producto, cantidad, precio_producto) VALUES "+
                                "("+idVenta+","+ Form2.dataGridView1[2,i].Value.ToString() + ","+ Form2.dataGridView1[0,i].Value.ToString() +
                                ","+ Form2.dataGridView1[3,i].Value.ToString() + " ) ";
                            mySqlCommand = new MySqlCommand(query, mySqlConnection);
                            mySqlCommand.ExecuteNonQuery();
                        }

                        venta.venta_realizada = true;
                        MessageBox.Show("Gracias por su compra!.");
                        this.Hide();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Error añadiendo venta a la tabla " + error.ToString());
                    }
                    
                }
                else
                {
                    MessageBox.Show("Pago incompleto.");
                }
            }
        }
    }
}
