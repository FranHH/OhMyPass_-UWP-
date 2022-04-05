using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcercaDe : ContentPage
    {
        #region Constructor
        public AcercaDe()
        {
            InitializeComponent();
            lblVersion.Text = Environment.Version.ToString();
        }
        #endregion

        #region Evento Click
        private async void btnFollowIg(object sender, EventArgs e)
        {
            if (await Launcher.CanOpenAsync("https://www.instagram.com")) 
            {
                await Launcher.OpenAsync("https://www.instagram.com/fran_.jpg/?hl=es");
            }

        }
        private async void btnLinkedin(object sender, EventArgs e)
        {
            if (await Launcher.CanOpenAsync("https://www.linkedin.com"))
            {
                await Launcher.OpenAsync("https://www.linkedin.com/in/francisco-josé-pérez-garcía-0a83151b7/");
            }
        
        }
        private async void btnYoutube(object sender, EventArgs e)
        {
            if (await Launcher.CanOpenAsync("https://www.youtube.com"))
            {
                await Launcher.OpenAsync("https://www.youtube.com/channel/UC6C4uebD0aGavg8sSUxr9SQ");
            }
        }
        private void btnInfoDispositivo(object sender, EventArgs e)
        {
            string infoDispositivo = "Nombre : " + Environment.MachineName.ToString() + "\n" +
                                     "Usuario : " + Environment.UserName.ToString() + "\n" +
                                     "VersionOS : " + Environment.OSVersion.ToString() + "\n" +
                                     "Modelo : " + DeviceInfo.Model.ToString() + "\n" +
                                     "Dispositivo : " + DeviceInfo.Manufacturer.ToString()+"\n" +
                                     "Nombre del dispositivo : " + DeviceInfo.Name.ToString()+"\n" +
                                     "Android : "+ DeviceInfo.VersionString.ToString()+"\n" +
                                     "Plataforma : "+ DeviceInfo.Platform.ToString()+"\n" +
                                     "Idioma : "+ DeviceInfo.Idiom.ToString()+"\n" +
                                     "Tipo de dispositivo : "+ DeviceInfo.DeviceType.ToString()+"\n"+
                                     "ID de hardware : " + CrossDeviceInfo.Current.Id;

            DisplayAlert("Informacion del dispositivo",infoDispositivo,"Aceptar");

        }
        private void btnInfoVersionApp(object sender, EventArgs e)
        {
            DisplayAlert("Version",Environment.Version.ToString(),"Aceptar");
        }
        private void btnSobreNosotros(object sender, EventArgs e)
        {

        }
        #endregion
    }
}