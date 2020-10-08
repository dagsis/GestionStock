using System;
using System.Collections.Generic;
using System.Text;

namespace DsConexSql
{
    public class Producto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
    }
}
