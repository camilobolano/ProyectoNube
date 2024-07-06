using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Entity
{
    public class users
    {
        [Key]

        public int id { get; set; }

        public string nombre { get; set; }

        public string clave { get; set; }

        public string permisos { get; set; }

        
    }
}