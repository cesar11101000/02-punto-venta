using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;

namespace pos_equipo
{
    public partial class vendido : Form
    {
        public vendido()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void vendido_Load(object sender, EventArgs e)
        {
            
            MySqlConnection mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
            mySqlConnection.Open();
            String query = "SELECT ventas_detalle.id_producto, nombre_producto, COUNT(ventas_detalle.id_producto) as cantidad FROM productos INNER JOIN ventas_detalle USING (id_producto) GROUP BY ventas_detalle.id_producto HAVING cantidad = ( SELECT COUNT(ventas_detalle.id_producto) FROM ventas_detalle INNER JOIN productos USING (id_producto) GROUP BY ventas_detalle.id_producto ORDER BY COUNT(ventas_detalle.id_producto) DESC LIMIT 1) ORDER BY cantidad";
            MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            label1.Text = "";

            while (reader.Read())
            {
                
                label1.Text += (reader["id_producto"].ToString() + "-" + reader["nombre_producto"].ToString() + "-" + reader["cantidad"].ToString());
              
            }

            
            mySqlConnection = new MySqlConnection("server=127.0.0.1; user=root; database=punto_de_venta; SSL mode=none");
            mySqlConnection.Open();
            query = "SELECT ventas_detalle.id_producto, nombre_producto, COUNT(ventas_detalle.id_producto) as cantidad FROM productos INNER JOIN ventas_detalle USING (id_producto) GROUP BY ventas_detalle.id_producto HAVING cantidad = ( SELECT COUNT(ventas_detalle.id_producto) FROM ventas_detalle INNER JOIN productos USING (id_producto) GROUP BY ventas_detalle.id_producto ORDER BY COUNT(ventas_detalle.id_producto) LIMIT 1) ORDER BY cantidad";
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            reader = mySqlCommand.ExecuteReader();
            label2.Text = "";

            while (reader.Read())
            {

                label2.Text += (reader["id_producto"].ToString() + "-" + reader["nombre_producto"].ToString() + "-" + reader["cantidad"].ToString());

            }
        }
    }
}
