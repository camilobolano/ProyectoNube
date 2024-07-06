using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Entity
{
    public class loan
    {
        [Key]

        public int id { get; set; }

        public string fecha_inicio { get; set; }

        public string fecha_entrega { get; set; }

        public bool estado { get; set; }

       [ForeignKey("usuario_id")]

        public int usuario_id { get; set; }

        public users users { get; set; }

        [ForeignKey("libro_id")]

        public int libro_id { get; set; }

        public books books { get; set; }
    }
}