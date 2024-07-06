using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Dto
{
    public class usersDto
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public string clave { get; set; }

        public string permisos { get; set; }
    }
}