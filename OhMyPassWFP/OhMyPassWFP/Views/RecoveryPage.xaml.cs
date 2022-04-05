using OhMyPassWFP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoveryPage : ContentPage
    {
        private DBFirebase dbFirebase;
        private HttpClient client;
        private int code;
        public RecoveryPage(int code)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            this.code = code;
        }

        private async void btnCancelar(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnEnviar(object sender, EventArgs e)
        {
            dbFirebase = new DBFirebase();

            string correo = correoRecu.Text;
            string codRecu = tlfnRecu.Text;

            if (code.Equals(codRecu))
            {
                
            }


        }
    }
}