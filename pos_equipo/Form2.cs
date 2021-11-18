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

    public partial class Form2 : Form
    {
        public double total = 0.0;
        public int cantidad_prod = 1;
        public Form2()
        {
            InitializeComponent();
            ventaRealizada();
            autocompletar();
            dataGridView1.Columns[0].Width = 25;
            labelErrorCheck.Text = "";
            label11.Text = "";
            label12.Text = "";
            dataGridView1.Columns[0].Width = 25;
            Form2.ActiveForm.Width = 843;
            Form2.ActiveForm.Height = 610;
            Acomodar();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
            label3.Text = datos_usuarios.nombre;
            label5.Text = datos_usuarios.sucursal;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString();
        }

        private void CalcularTotal()
        {
            total = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                total += Double.Parse(dataGridView1[3, i].Value.ToString());
            }
            label11.Text = String.Format("{0:0.00}", total);
            venta.total = Double.Parse(label11.Text);
        }

        public void autocompletar()
        {
            AutoCompleteStringCollection lista = new AutoCompleteStringCollection();
            MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
            mySqlConnection.Open();
            String query = "SELECT * from productos";
            MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            while (reader.Read())
            {
                labelErrorCheck.Text = "";
                lista.Add(reader["id_producto"].ToString() + "-" + reader["nombre_producto"].ToString());
                textBox_buscarProducto.AutoCompleteCustomSource = lista;
            }
        }

        private void textBox_buscarProducto_TextChanged(object sender, EventArgs e)
        {
            textBox_idProducto.Text = textBox_buscarProducto.Text;
            labelErrorCheck.Text = "";
            String itemBuscar = textBox_idProducto.Text;
            if (itemBuscar.Contains("-"))
            {
                labelErrorCheck.Text = "";
                char delimitador = '-';
                string[] idItem = itemBuscar.Split(delimitador);
                label_idProducto.Text = idItem[0].ToString();
                textBox_buscarProducto.AutoCompleteMode.Equals(0); //0 none, 3 suggestAppend

            }
            else if (textBox_idProducto.Text == "")
            {
                labelErrorCheck.Text = "";
                label_idProducto.Text = "";
                textBox_buscarProducto.AutoCompleteMode.Equals(3);
            }
            else
            {
                labelErrorCheck.Text = "No items found";
                label_idProducto.Text = "";
                textBox_buscarProducto.AutoCompleteMode.Equals(3);
            }
        }

        private void textBox_buscarProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
                    mySqlConnection.Open();
                    String query = "select * from productos where id_producto =" + label_idProducto.Text;
                    MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
                    MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
                    if (mySqlDataReader.HasRows)
                    {
                        bool rep = false;
                        mySqlDataReader.Read();
                        int id_producto = mySqlDataReader.GetInt32(0);
                        String nom_prod = mySqlDataReader.GetString(1);
                        Double pre_prod = mySqlDataReader.GetDouble(2);

                        if (dataGridView1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                String nombreBuscar = dataGridView1.Rows[i].Cells["Nombre_producto"].Value.ToString();
                                if (nombreBuscar == nom_prod)
                                {
                                    String can_prod = dataGridView1.Rows[i].Cells["cantidad"].Value.ToString();
                                    int cant_prod = Int32.Parse(can_prod);
                                    cant_prod++;

                                    //calcular suma precios
                                    String precio_tabla = dataGridView1.Rows[i].Cells["precio"].Value.ToString();
                                    Double prec_tabla = Double.Parse(precio_tabla);
                                    prec_tabla = prec_tabla + pre_prod;

                                    dataGridView1.Rows[i].Cells["cantidad"].Value = cant_prod;
                                    dataGridView1.Rows[i].Cells["precio"].Value = prec_tabla;
                                    rep = true;
                                }
                            }
                        }
                        if (!rep)
                        {
                            dataGridView1.Rows.Add(cantidad_prod, nom_prod, id_producto, String.Format("{0:0.00}", pre_prod));
                        }

                        CalcularTotal();
                        label12.Text = dataGridView1.Rows.Count.ToString();
                        textBox_buscarProducto.Clear();
                        textBox_buscarProducto.Focus();
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron productos");
                        textBox_buscarProducto.Clear();
                        textBox_buscarProducto.Focus();
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("No se encontraron productos");
                }
            }
            else

            if (e.KeyCode == Keys.F2)
            {
                borrarTodo();
                textBox_buscarProducto.Focus();
            }
            else

            if (e.KeyCode == Keys.F3)
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No hay productos en la tabla.");
                    textBox_buscarProducto.Focus();
                }
                else
                {
                    dataGridView1.Focus();
                }

            }
            else

            if (e.KeyCode == Keys.F4)
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No hay productos en la tabla.");
                }
                else
                {
                    Form3 ventana_pago = new Form3();
                    ventana_pago.Show();
                }

            }
            else

            if (e.KeyCode == Keys.F5)
            {
                Form1 ventana_inicio = new Form1();
                ventana_inicio.Show();
                this.Hide();

            }


        }

        private void borrarUno()
        {
            try
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                CalcularTotal();
                label12.Text = dataGridView1.Rows.Count.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("No hay productos en la tabla.");
            }
        }

        public void ventaRealizada()
        {

            if (venta.venta_realizada == true)
            {
                borrarTodo();
                venta.venta_realizada = false;
            }
        }

        public void borrarTodo()
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                CalcularTotal();
                label12.Text = dataGridView1.Rows.Count.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("No hay productos en la tabla.");
            }
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            ventaRealizada();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            ventaRealizada();
        }

        private void Acomodar()
        {
            int width =this.Width;
            int heigth = this.Height;

            //Rigth side Fixing
            pictureBox1.Location = new Point(width - pictureBox1.Width - 26, 16);
            label9.Location = new Point(pictureBox1.Location.X, 153);
            label10.Location = new Point(pictureBox1.Location.X, 179);
            label11.Location = new Point(pictureBox1.Location.X + pictureBox1.Width - label11.Width, 149);
            label12.Location = new Point(pictureBox1.Location.X + pictureBox1.Width - label11.Width, 175);
            textBox_idProducto.Location = new Point(pictureBox1.Location.X + 27, 243);
            label_idProducto.Location = new Point(pictureBox1.Location.X + 113, 243);

            //Left Side Fixing
            int resizedWidth = pictureBox1.Location.X - 39;

            label1.Location = new Point(23 + (resizedWidth / 2) - (label1.Width / 2), 18);

            label2.Location = new Point(23, 43);
            label3.Location = new Point(23 + label2.Width + 7, 43);
            label4.Location = new Point(23 + resizedWidth - 128, 43);
            label5.Location = new Point(label4.Location.X + label4.Width + 2, 43);

            textBox_buscarProducto.Width = resizedWidth;
            label7.Location = new Point(23 + (resizedWidth / 2) - (label7.Width / 2), 98);

            dataGridView1.Size = new Size(resizedWidth, heigth - 255);

            label7.Location = new Point(23, heigth - 98);
            label13.Location = new Point(label13.Location.X, heigth - 98);
            label14.Location = new Point(label14.Location.X, heigth - 98);
            label15.Location = new Point(label15.Location.X, heigth - 98);

            label8.Location = new Point(23, heigth - 75);
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            Acomodar();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
        }


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
            {
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
                int cant = Int32.Parse(selectedRow.Cells[0].Value.ToString());
                Double pre_tabla = Double.Parse(selectedRow.Cells[3].Value.ToString());
                int id_pd = Int32.Parse(selectedRow.Cells[2].Value.ToString());
                if (cant == 1)
                {
                    dataGridView1.Rows.RemoveAt(selectedRowIndex);
                    CalcularTotal();
                    textBox_buscarProducto.Focus();
                    label12.Text = dataGridView1.Rows.Count.ToString();
                }
                else if (cant > 1)
                {
                    cant = cant-1;
                    pre_tabla = pre_tabla - obtener_precio_unitario(id_pd);

                    dataGridView1.Rows[selectedRowIndex].Cells["cantidad"].Value = cant;
                    dataGridView1.Rows[selectedRowIndex].Cells["precio"].Value = pre_tabla;

                    CalcularTotal();
                    label12.Text = dataGridView1.Rows.Count.ToString();
                }

            }
            else if (e.KeyCode == Keys.Escape)
            {
                textBox_buscarProducto.Focus();

            }
        }

        private Double obtener_precio_unitario(int Index)
        {
            Double pre_prod = 0;
            MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
            mySqlConnection.Open();
            String query = "select precio from productos where id_producto =" + Index;
            MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            if (mySqlDataReader.HasRows)
            {
                mySqlDataReader.Read();
                pre_prod = mySqlDataReader.GetDouble(0);
            }
            return pre_prod;
        }
    }
}
