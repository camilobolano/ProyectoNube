using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bibliotecaModels.Dto
{
    public class responseDto
    {
        public bool IsExitoso  {get; set;}  = true;
        public object Resultado  {get; set;} 
        public string Mensaje   {get; set;} 
    }
}