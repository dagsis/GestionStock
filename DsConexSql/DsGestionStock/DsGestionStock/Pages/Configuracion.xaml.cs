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
    public partial class Configuracion : ContentPage
    {
        public Configuracion()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.Properties.ContainsKey("Servidor"))
            {
                txtServidor.Text = Application.Current.Properties["Servidor"] as string;
            }

            if (Application.Current.Properties.ContainsKey("Catalogo"))
            {
                txtCatalogo.Text = Application.Current.Properties["Catalogo"] as string;
            }


            btnGuardar.Clicked += BtnGuardar_Clicked;
        }

        private void BtnGuardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServidor.Text))
            {
                DisplayAlert("Atención", "Ingrese Nombre del Servidor", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(txtCatalogo.Text))
            {
                DisplayAlert("Atención", "Ingrese Nombre del Catalogo Sql", "Aceptar");
                return;
            }

            Application.Current.Properties["Servidor"] = txtServidor.Text;
            Application.Current.Properties["Catalogo"] = txtCatalogo.Text;

            DisplayAlert("Atención", "Datos Guardados Con Exito..", "Aceptar");

        }
    }
}