using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DsConexSql
{
    public class ConexSql
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
