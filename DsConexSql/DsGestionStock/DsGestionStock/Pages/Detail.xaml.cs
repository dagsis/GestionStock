using DsConexSql;
using DsGestionStock.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DsGestionStock.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detail : ContentPage
    {
        public Detail()
        {
            InitializeComponent();

            btnConectar.Clicked += BtnConectar_Clicked;
            btnConsultar.Clicked += BtnConsultar_Clicked;
            btnLimpiar.Clicked += BtnLimpiar_Clicked;
            btnAplicar.Clicked += BtnAplicar_Clicked;
        }

        private void BtnAplicar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCantidad.Text))
            {
                string sServidor = Application.Current.Properties["Servidor"] as string;
                string sBase = Application.Current.Properties["Catalogo"] as string;
                string nVendedor = Application.Current.Properties["Vendedor"] as string;

                List<Producto> producto = DsConexSql.ConexSql.GrabarProducto(sServidor, sBase, txtBarCode.Text,txtDescripcion.Text,nVendedor, Convert.ToDecimal(txtCantidad.Text));

                if (producto.Count != 0)
                {
                    DisplayAlert("Atención", "Producto Grabado Con Exito", "Aceptar");
                    Limpiar();
                    txtBarCode.Text = "";
                    txtBarCode.Focus();
                }
                else
                {

                }
              
            } else
            {
                DisplayAlert("Atención", "Cantidad no puede estar Vacio", "Aceptar");
                txtCantidad.Focus();
            }
        }

        private void BtnLimpiar_Clicked(object sender, EventArgs e)
        {
            txtBarCode.Text = "";
            Limpiar();
            //  txtBarCode.Focus();
        }

        private void Limpiar()
        {
            txtDescripcion.Text = "";
            txtCantidad.Text = "";
        }

        private void Consultar(string pCodigo)
        {
            Limpiar();


            if (pCodigo != "Cancelado")
            {
                string sServidor = Application.Current.Properties["Servidor"] as string;
                string sBase = Application.Current.Properties["Catalogo"] as string;

                txtBarCode.Text = pCodigo;
                List<Producto> producto = DsConexSql.ConexSql.ObtenerProductos(sServidor, sBase, pCodigo);

                if (producto.Count != 0)
                {
                    foreach (var item in producto)
                    {
                        txtBarCode.Text = item.Codigo;
                        txtDescripcion.Text = item.Descripcion;
                    }
                    txtCantidad.Focus();
                }
                else
                {
                    DisplayAlert("Atención", "Producto No Encontrado", "Aceptar");
                    txtBarCode.Text = "";
                }
            }
        }

        private void BtnConsultar_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarCode.Text))
            {
                Limpiar();
                Consultar(txtBarCode.Text);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!Application.Current.Properties.ContainsKey("Servidor"))
            {
                Application.Current.Properties["Servidor"] = "192.168.0.28";
            }

            if (!Application.Current.Properties.ContainsKey("Catalogo"))
            {
                Application.Current.Properties["Catalogo"] = "GestionElectronicHomo";
            }

            if (!Application.Current.Properties.ContainsKey("Vendedor"))
            {
                Application.Current.Properties["Vendedor"] = "1";
            }

        }

        private async void BtnConectar_Clicked(object sender, EventArgs e)
        {


            //DisplayAlert("Atención", DsConexSql.ConexSql.Conectar(sServidor,sBase), "Aceptar");


            //var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
            //options.PossibleFormats = new List<ZXing.BarcodeFormat>() {
            // ZXing.BarcodeFormat.EAN_8, ZXing.BarcodeFormat.EAN_13
            // };



            var scanner = DependencyService.Get<IQrCodeScanningService>();
            var result = await scanner.ScanAsync();

            if (result != null)
            {
                Consultar(result);
            }
        }
    }
}