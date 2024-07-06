using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Entity
{
    public class books
    {
        [Key]
        public int id { get; set; }

        public string nombre { get; set; }

        public string autor { get; set; }

        public string salida { get; set; }

    }
}