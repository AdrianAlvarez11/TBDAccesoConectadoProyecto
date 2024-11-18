using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampeonesLoL.Models
{
    public class CampeonesContext
    {
        public MySqlConnection Conexion { get; set; } = new MySqlConnection("server=localhost;user=root;password=root;database=CampeonesLoL");
        public void Conectar()
        {
            if (Conexion.State == System.Data.ConnectionState.Closed)
            {
                Conexion.Open();
            }
        }

        public void Desconectar()
        {
            if (Conexion.State == System.Data.ConnectionState.Open)
            {
                Conexion.Close();
            }
        }
    }
}
