using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DsConexSql
{
    public static class ConexSql
    {

        public static List<Producto> ObtenerProductos(string pServidor, string pBase, string pCodigo)
        {
            List<Producto> listaProductos = new List<Producto>();

            string sSql = "SELECT Id,Productos.Producto, Productos.Descripcion ";
            sSql = sSql + "FROM Productos ";
            sSql = sSql + "WHERE(Productos.Producto = '" + pCodigo + "') ";

            using (SqlConnection con = new SqlConnection("Server=" + pServidor + ";Database=" + pBase + ";User ID=sa; Password=hola10; Trusted_Connection=False"))
            {
                try
                {
                    con.Open();

                    using (SqlCommand comando = new SqlCommand(sSql, con))
                    {
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Producto producto = new Producto()
                                {
                                    Codigo = (string)reader["Producto"],
                                    Descripcion = (string)reader["Descripcion"],
                                    Cantidad = 0
                                };

                                listaProductos.Add(producto);
                            }
                        }
                    }

                    con.Close();
                }

                catch (Exception e)
                {
                    Producto producto = new Producto()
                    {
                        Codigo = "0000",
                        Descripcion = e.Message,
                    };

                    listaProductos.Add(producto);
                }

                return listaProductos;
            }
        }

        public static List<Producto> GrabarProducto(string pServidor, string pBase, string pCodigo,string pDescripcion,string pVendedor, decimal pCantidad)
        {
            List<Producto> listaProductos = new List<Producto>();

            try
            {
                using (SqlConnection con = new SqlConnection("Server=" + pServidor + ";Database=" + pBase + ";User ID=sa; Password=hola10; Trusted_Connection=False"))
                {
                    int rowsAffected;
                    decimal compra = 0;
                    decimal venta = 0;
                    decimal porcentaje = 0;
                    decimal iva = 0;

                    con.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("Stock_A", con);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@Vendedor", pVendedor));
                    cmd.Parameters.Add(new SqlParameter("@Producto", pCodigo));
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", pDescripcion));
                    cmd.Parameters.Add(new SqlParameter("@Cantidad", pCantidad));
                    cmd.Parameters.Add(new SqlParameter("@Compra", compra));
                    cmd.Parameters.Add(new SqlParameter("@Iva", iva));
                    cmd.Parameters.Add(new SqlParameter("@Porcentaje", porcentaje));
                    cmd.Parameters.Add(new SqlParameter("@Venta", venta));

                    rowsAffected = cmd.ExecuteNonQuery();

                    // execute the command

                    Producto producto = new Producto()
                    {
                        Codigo = "0000",
                        Descripcion = "OK",
                    };

                    listaProductos.Add(producto);
                }

            }
            catch (Exception e)
            {

                Producto producto = new Producto()
                {
                    Codigo = "0000",
                    Descripcion = e.Message,
                };

                listaProductos.Add(producto);
            }

            return listaProductos;
        }

        public static String Conectar(string pServidor, string pBase)
        {
            using (SqlConnection con = new SqlConnection("Server=" + pServidor + ";Database=" + pBase + ";User ID=sa; Password=hola10; Trusted_Connection=False"))
            {
                try
                {
                    con.Open();

                }
                catch (Exception e)
                {

                    return e.Message;
                }


            }
            return "Ok";
        }

      
    }
}
