using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Dto
{
    public class booksDto
    {
        public int id { get; set; }

        public string nombre { get; set; }

        public string autor { get; set; }

        public string salida { get; set; }
    }
}