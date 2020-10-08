using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DsConexSql
{
    public class ConexSql
    {
        static string cadenaConexion = @"Server=192.168.0.189;Database=GestionElectronicHomo;User ID=sa; Password=hola10; Trusted_Connection=False";

        public static List<Producto> ObtenerProductos(string pServidor, string pBase, string pCodigo)
        {
            List<Producto> listaProductos = new List<Producto>();

            string sSql = "SELECT Productos.Producto, Productos.Descripcion,CONVERT(varchar(10), Productos.FechaCompra, 103) as FechaCompra, Productos.PrecioCompra as PrecioCompra,";
            sSql = sSql + "Productos.CantMinima,Productos.CantMaxima ";
            sSql = sSql + "FROM Productos ";
            sSql = sSql + "WHERE(Productos.Producto = '" + pCodigo + "') ";

            //  string sql = "SELECT * FROM Productos WHERE Producto='" + pCodigo + "'";

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

        public static decimal TraerCantidadVendida(string pServidor, string pBase, string pCodigo, int pDias)
        {
            decimal nCanti = 0;

            using (SqlConnection con = new SqlConnection("Server=" + pServidor + ";Database=" + pBase + ";User ID=sa; Password=hola10; Trusted_Connection=False"))
            {
                try
                {

                    con.Open();
                    string sSql = "SELECT SUM(DetallesComprobantes.Cantidad) AS Cantidad ";
                    sSql = sSql + "FROM DetallesComprobantes INNER JOIN CabComprobantes ON DetallesComprobantes.Movimiento = CabComprobantes.Movimiento RIGHT OUTER JOIN Productos ";
                    sSql = sSql + "ON DetallesComprobantes.Producto = Productos.Producto ";
                    sSql = sSql + "WHERE(CabComprobantes.Tipo = 1) AND(CabComprobantes.Fecha >= CONVERT(DATETIME, GETDATE() - " + pDias + ", 102)) GROUP BY Productos.Producto, Productos.Descripcion, Productos.FechaCompra, Productos.PrecioCompra ";
                    sSql = sSql + "HAVING(Productos.Producto = '" + pCodigo + "') ";

                    using (SqlCommand comando = new SqlCommand(sSql, con))
                    {
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nCanti = (decimal)reader["Cantidad"];
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception)
                {

                    return 0;
                }
            }

            return nCanti;
        }

        public static Double TraerCantidadActual(string pServidor, string pBase, string pCodigo)
        {
            double nCanti = 0;

            using (SqlConnection con = new SqlConnection("Server=" + pServidor + ";Database=" + pBase + ";User ID=sa; Password=hola10; Trusted_Connection=False"))
            {
                try
                {
                    con.Open();

                    string sSql = "SELECT SUM(Cantidades.CantidadDebe) -SUM(Cantidades.CantidadHaber) AS CantidadTotal ";
                    sSql = sSql + "FROM Productos LEFT OUTER JOIN ";
                    sSql = sSql + "Cantidades ON Productos.Producto = Cantidades.Producto ";
                    sSql = sSql + "WHERE (Productos.Producto = '" + pCodigo + "')";


                    using (SqlCommand comando = new SqlCommand(sSql, con))
                    {
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nCanti = reader.GetDouble(0);
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception e)
                {

                    return 0;
                }
            }

            return nCanti;
        }

        public static string TraerUltimaVenta(string pServidor, string pBase, string pCodigo)
        {
            string nUlti = "";

            using (SqlConnection con = new SqlConnection("Server=" + pServidor + ";Database=" + pBase + ";User ID=sa; Password=hola10; Trusted_Connection=False"))
            {
                try
                {
                    con.Open();
                    string sSql = "SELECT TOP 1 CONVERT(varchar(10), CabComprobantes.Fecha, 103) as Fecha ";
                    sSql = sSql + "FROM DetallesComprobantes INNER JOIN ";
                    sSql = sSql + "CabComprobantes ON DetallesComprobantes.Movimiento = CabComprobantes.Movimiento INNER JOIN ";
                    sSql = sSql + "Productos ON DetallesComprobantes.Producto = Productos.Producto ";
                    sSql = sSql + "WHERE(Productos.Producto = '" + pCodigo + "') and CabComprobantes.Tipo = 1 ";
                    sSql = sSql + "ORDER BY CabComprobantes.Fecha DESC ";

                    using (SqlCommand comando = new SqlCommand(sSql, con))
                    {
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nUlti = (string)reader["Fecha"];
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception)
                {

                    return "Sin Ventas";
                }
            }

            return nUlti;
        }
    }
}
