using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing.Mobile;
using ZXing.Net.Mobile;
using DsGestionStock.Services;
using DependencyAttribute = Xamarin.Forms.DependencyAttribute;


[assembly: Dependency(typeof(DsGestionStock.Droid.Service.QrCodeScanningService))]
namespace DsGestionStock.Droid.Service
{
    public class QrCodeScanningService : IQrCodeScanningService
    {   
            public async Task<string> ScanAsync()
            {
                var optionsDefault = new MobileBarcodeScanningOptions();
                var optionsCustom = new MobileBarcodeScanningOptions();


                var scanner = new MobileBarcodeScanner()
                {
                    TopText = "Acerca  la Camara al elemento",
                    BottomText = "Toca la Pantalla para Enfocar",
                };

                string msg;

                var result = await scanner.Scan(optionsCustom);

                if (result != null && !string.IsNullOrEmpty(result.Text))
                    msg = result.Text;
                else
                    msg = "Cancelado";

                return msg;
            } 
    }

}