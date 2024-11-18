using CampeonesLoL.Models;
using CampeonesLoL.Viewmodels;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampeonesLoL.Repositories
{
    public class CampeonesRepository
    {
        CampeonesContext context = new();
        MySqlCommand sql = new MySqlCommand();
        MySqlDataReader lector;
        List<Campeon> ListaCampeones = new();

        public IEnumerable<Campeon> GetAllCampeones()
        {
            ListaCampeones.Clear();
            context.Conectar();
            sql.CommandText = "select * from campeones order by Nombre";
            sql.Connection = context.Conexion;
            lector = sql.ExecuteReader();

            while (lector.Read())
            {
                Campeon c = new()
                {
                    Id = lector.GetInt32("Id"),
                    Nombre = lector.GetString("Nombre").ToUpper(),
                    Apodo = lector.GetString("Apodo").ToUpper(),
                    Imagen = lector.GetString("Urlimagen"),
                    Carril = lector.GetString("Carril"),
                    Dificultad = lector.GetString("Dificultad"),
                    Rol = lector.GetString("Rol")

                };
                ListaCampeones.Add(c);

            }
            lector.Close();
            context.Desconectar();
            return ListaCampeones;
        }

        public void Agregar(Campeon c)
        {
            context.Conectar();

            sql.CommandText = $"insert into Campeones (Nombre, Apodo, Urlimagen, Carril, Rol, Dificultad) " +
                $"values ('{c.Nombre}', '{c.Apodo}', '{c.Imagen}', '{c.Carril}', '{c.Rol}', '{c.Dificultad}');";
            sql.Connection = context.Conexion;
            sql.ExecuteNonQuery();

            context.Desconectar();

        }

        public bool Validar(Campeon c, out string errores)
        {
            List<string> listaErrores = new List<string>();

            if (string.IsNullOrWhiteSpace(c.Nombre))
                listaErrores.Add("Introduzca el nombre del campeón");

            if (string.IsNullOrWhiteSpace(c.Apodo))
                listaErrores.Add("Introduzca el apodo del campeón");

            if (GetAllCampeones().Any(x => x.Nombre == c.Nombre))
                listaErrores.Add("El nombre del campeón está repetido");

            if (string.IsNullOrWhiteSpace(c.Carril))
                listaErrores.Add("Introduzca el carril principal del campeón");

            if (string.IsNullOrWhiteSpace(c.Rol))
                listaErrores.Add("Introduzca el rol principal del campeón");

            if (string.IsNullOrWhiteSpace(c.Dificultad))
                listaErrores.Add("Introduzca la dificultad del campeón");

            if (string.IsNullOrWhiteSpace(c.Imagen) || (!c.Imagen.EndsWith(".jpg")))
                listaErrores.Add("Introduzca una URL de imagen en formato .jpg");

            errores = string.Join(Environment.NewLine, listaErrores);

            if (listaErrores.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Eliminar(Campeon c)
        {
            context.Conectar();

            sql.CommandText = $"delete from campeones where Id={c.Id}";
            sql.Connection = context.Conexion;
            sql.ExecuteNonQuery();

            context.Desconectar();
        }

        public long GetConteoCarriles(Carriles carril)
        {
            context.Conectar();
            sql.CommandText = ($"select Count(*) from campeones where Carril='{carril}'");
            sql.Connection = context.Conexion;

            long n = (long)sql.ExecuteScalar();
            return n;
        }

        public long GetConteoTotal()
        {
            context.Conectar();
            sql.CommandText = ($"select Count(*) from campeones");
            sql.Connection = context.Conexion;

            long n = (long)sql.ExecuteScalar();
            return n;
        }
        public long GetConteoRoles(Roles roles)
        {
            context.Conectar();
            sql.CommandText = ($"select Count(*) from campeones where Rol='{roles}'");
            sql.Connection = context.Conexion;

            long n = (long)sql.ExecuteScalar();
            return n;
        }

        public long GetConteoDificultad(Dificultad dificultad)
        {
            context.Conectar();
            sql.CommandText = ($"select Count(*) from campeones where Dificultad='{dificultad}'");
            sql.Connection = context.Conexion;

            long n = (long)sql.ExecuteScalar();
            return n;
        }
    }
}
