using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Dto
{
    public class loanDto
    {
        public int id { get; set; }

        public string fecha_inicio { get; set; }

        public string fecha_entrega { get; set; }

        public bool estado { get; set; }

        public int usuario_id { get; set; }

        public int libro_id { get; set; }

        

    }
}