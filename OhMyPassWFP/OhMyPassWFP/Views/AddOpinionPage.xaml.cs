using OhMyPassWFP.Models;
using OhMyPassWFP.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOpinionPage : ContentPage
    {

        #region Declaracion de variables y objetos
        private DBFirebase dbFirebase;
        private Encriptar _Encrypt;
        private bool es1 = false, es2 = false, es3 = false, es4=false, es5=false, edit=false;
        string idOp = "";
        #endregion

        #region Constructores
        public AddOpinionPage()
        {
            InitializeComponent();
        }
        public AddOpinionPage(Opiniones op)
        {
            InitializeComponent();
            edit = true;
            idOp = op.Id;
            eOpinion.Text = op.Opinion;
            checkPrivacidadDeDatos.IsChecked = true;

            if (op.Puntuacion.Equals("5")) 
            {
                estrella5.Source = "estrellacolor64.png";
                estrella4.Source = "estrellacolor64.png";
                estrella3.Source = "estrellacolor64.png";
                estrella2.Source = "estrellacolor64.png";
                estrella1.Source = "estrellacolor64.png";
                es1 = true;
                es2 = true;
                es3 = true;
                es4 = true;
                es5 = true;

            }else if (op.Puntuacion.Equals("4"))
            {
                estrella4.Source = "estrellacolor64.png";
                estrella3.Source = "estrellacolor64.png";
                estrella2.Source = "estrellacolor64.png";
                estrella1.Source = "estrellacolor64.png";
                es1 = true;
                es2 = true;
                es3 = true;
                es4 = true;

            }else if (op.Puntuacion.Equals("3"))
            {
                estrella3.Source = "estrellacolor64.png";
                estrella2.Source = "estrellacolor64.png";
                estrella1.Source = "estrellacolor64.png";
                es1 = true;
                es2 = true;
                es3 = true;
            }
            else if (op.Puntuacion.Equals("2"))
            {
                estrella2.Source = "estrellacolor64.png";
                estrella1.Source = "estrellacolor64.png";
                es1 = true;
                es2 = true;

            }
            else if(op.Puntuacion.Equals("1"))
            {
                estrella1.Source = "estrellacolor64.png";
                es1 = true;
            }

            

        }
        #endregion

        #region SendData
        private async void SendData(object sender, EventArgs e)
        {
            dbFirebase = new DBFirebase();
            _Encrypt = new Encriptar();
            string punt = "";
            if (es5)
            {
                punt = "5";

            }else if (es4)
            {
                punt = "4";

            }else if (es3)
            {
                punt = "3";

            }else if (es2)
            {
                punt = "2";

            }else if (es1)
            {
                punt = "1";
            }
            else
            {
                punt = "0";
            }
            if (checkPrivacidadDeDatos.IsChecked)
            {
                if (edit)
                {
                    dbFirebase.DeleteOpinion(idOp);
                    //Inicializo el objeto con los datos encriptados
                    Opiniones op = new Opiniones(eOpinion.Text, _Encrypt.DesEncryptName(dbFirebase.GetUser()), DateTime.Now.ToString(new CultureInfo("es-ES")), punt);
                    //Añado a firebase el objeto
                    dbFirebase.AddOpinion(op);
                    await this.DisplayToastAsync("Editado correctamente", 999);
                }
                else
                {
                    Opiniones op = new Opiniones(eOpinion.Text, _Encrypt.DesEncryptName(dbFirebase.GetUser()), DateTime.Now.ToString(new CultureInfo("es-ES")), punt);
                    dbFirebase.AddOpinion(op);
                    await this.DisplayToastAsync("Editado correctamente", 999);
                    await Navigation.PopAsync();
                }
                
            }
            else
            {
                if(await DisplayAlert("¡Atención!", "Tienes que aceptar los terminos de privacidad de datos", "Acptar","Cancelar"))
                {
                    checkPrivacidadDeDatos.IsChecked = true;
                }
            }
            
        }
        #endregion

        #region Cancelar & Termino y condiciones
        private async void BtnCancelar(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void TerminosYCondiciones(object sender, EventArgs e)
        {
            DisplayAlert("Términos y Condiciones de privacidad", "INFORMACIÓN RELEVANTE\n" +
                "Los datos personales del usuario son recogidos e introducidos por EDP en un archivo automatizado para llevar a cabo actividades incluidas en el ámbito de la iniciativa de Oh My Pass\n" +
                "Los datos recogidos podrán transmitirse a otras sociedades del Grupo EDP, con sede en Portugal o en el extranjero, " +
                "para llevar a cabo cualquier actividad incluida en la iniciativa del sitio web. La recogida y el uso de datos personales " +
                "se llevarán a cabo de acuerdo con la legislación portuguesa, aplicable y vigente, y todas las autorizaciones que, a tal fin, " +
                "resulten necesarias, se obtendrán previamente ante la Comisión Nacional de Protección de Datos. Según la ley," +
                " EDP adoptó las medidas necesarias para garantizar que los datos proporcionados estén protegidos contra el acceso o uso no " +
                "autorizado por parte de terceros no autorizados. Sin embargo, EDP alerta ante el riesgo residual de acceso no autorizado a través de Internet." +
                "Oh My Pass reserva los derechos de cambiar o de modificar estos términos sin previo aviso.", "Aceptar");
        }
        #endregion

        #region Estrellas
        private void BtnEst1(object sender, EventArgs e)
        {
            if(!es5 && !es4 && !es3 && !es2)
            {
                if (es1)
                {
                    estrella1.Source = "estrella64.png";
                    es1 = false;
                }
                else
                {
                    estrella1.Source = "estrellacolor64.png";
                    es1 = true;
                }
            }
            else
            {
                estrella2.Source = "estrella64.png";
                estrella3.Source = "estrella64.png";
                estrella4.Source = "estrella64.png";
                estrella5.Source = "estrella64.png";
                es5 = false;
                es4 = false;
                es3 = false;
                es2 = false;
            }

        }
        private void BtnEst2(object sender, EventArgs e)
        {
            if(!es5 && !es4 && !es3)
            {
                if (es2)
                {
                    estrella2.Source = "estrella64.png";
                    estrella1.Source = "estrella64.png";
                    es1 = false;
                    es2 = false;
                }
                else
                {
                    estrella2.Source = "estrellacolor64.png";
                    estrella1.Source = "estrellacolor64.png";
                    es1 = true;
                    es2 = true;
                }
            }
            else
            {
                estrella3.Source = "estrella64.png";
                estrella4.Source = "estrella64.png";
                estrella5.Source = "estrella64.png";
                es5 = false;
                es4 = false;
                es3 = false;
            }
        }
        private void BtnEst3(object sender, EventArgs e)
        {
            if (!es5 && !es4)
            {
                if (es3)
                {
                    estrella3.Source = "estrella64.png";
                    estrella2.Source = "estrella64.png";
                    estrella1.Source = "estrella64.png";
                    es1 = false;
                    es2 = false;
                    es3 = false;
                }
                else
                {
                    estrella3.Source = "estrellacolor64.png";
                    estrella2.Source = "estrellacolor64.png";
                    estrella1.Source = "estrellacolor64.png";
                    es1 = true;
                    es2 = true;
                    es3 = true;
                }
            }
            else
            {
                estrella4.Source = "estrella64.png";
                estrella5.Source = "estrella64.png";
                es5 = false;
                es4 = false;

            }
        }
        private void BtnEst4(object sender, EventArgs e)
        {
            if (!es5)
            {
                if (es4)
                {
                    estrella4.Source = "estrella64.png";
                    estrella3.Source = "estrella64.png";
                    estrella2.Source = "estrella64.png";
                    estrella1.Source = "estrella64.png";
                    es1 = false;
                    es2 = false;
                    es3 = false;
                    es4 = false;
                }
                else
                {
                    estrella4.Source = "estrellacolor64.png";
                    estrella3.Source = "estrellacolor64.png";
                    estrella2.Source = "estrellacolor64.png";
                    estrella1.Source = "estrellacolor64.png";
                    es1 = true;
                    es2 = true;
                    es3 = true;
                    es4 = true;
                }

            }
            else
            {
                estrella5.Source = "estrella64.png";
                es5 = false;
            }
        }
        private void BtnEst5(object sender, EventArgs e)
        {
            if (es5)
            {
                estrella5.Source = "estrella64.png";
                estrella4.Source = "estrella64.png";
                estrella3.Source = "estrella64.png";
                estrella2.Source = "estrella64.png";
                estrella1.Source = "estrella64.png";
                es1 = false;
                es2 = false;
                es3 = false;
                es4 = false;
                es5 = false;
            }
            else
            {
                estrella5.Source = "estrellacolor64.png";
                estrella4.Source = "estrellacolor64.png";
                estrella3.Source = "estrellacolor64.png";
                estrella2.Source = "estrellacolor64.png";
                estrella1.Source = "estrellacolor64.png";
                es1 = true;
                es2 = true;
                es3 = true;
                es4 = true;
                es5 = true;
            }
        }
        #endregion
    }
}