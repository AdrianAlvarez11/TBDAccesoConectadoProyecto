using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampeonesLoL.Models
{
    public class Campeon
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Apodo { get; set; } = "";
        public string Imagen { get; set; } = "";
        public string Carril { get; set; } = "";
        public string Rol { get; set; } = "";
        public string Dificultad { get; set; } = "";

    }
}
