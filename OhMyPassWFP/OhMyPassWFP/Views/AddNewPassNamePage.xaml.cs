using OhMyPass.models;
using OhMyPassWFP.Services;
using System;
using System.Globalization;
using System.Threading;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewPassNamePage : ContentPage
    {
        #region Declaracion de variables y objetos
        bool showPassword = true;
        Encriptar _Encrypt = new Encriptar();
        DBFirebase dbFirebase;
        PassName ps;
        public readonly string WebApiKey = "AIzaSyBonMHtWUFs3RXpQXA01UW42rP5AP915AM";
        bool edit = false;
        string idEdit = "", source="fav.png";
        #endregion

        #region Constructor
        public AddNewPassNamePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
       
        }
        public AddNewPassNamePage(PassName ps)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            
            InitializeComponent();
            cNamePass.Text = ps.Name;
            cPassEncrypt.Text = _Encrypt.PassDesEncrip(ps.Pass);
            cGrupo.Text = ps.Grupo;
            idEdit = ps.Id;
            edit = true;
        }
        #endregion

        #region Evento Click
        private void PassIcon_Clicked(object sender, EventArgs e)
        {
            if (showPassword)
            {
                pass_icon.Source = "ojo.png";
                cPassEncrypt.IsPassword = false;
                showPassword = false;
            }
            else
            {
                pass_icon.Source = "ver.png";
                cPassEncrypt.IsPassword = true;
                showPassword = true;
            }
        }
        private async void btnSavePass(object sender, EventArgs e)
        {
            string nombre = cNamePass.Text, pass = cPassEncrypt.Text, grupo = cGrupo.Text, notas = cNotas.Text;

            if (string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("¡Atención!", "El nombre no puede estar vacío", "Aceptar");
            }
            else if (string.IsNullOrEmpty(pass))
            {
                await DisplayAlert("¡Atención!", "No has introducido ninguna contraseña", "Aceptar");
            }
            else if (string.IsNullOrEmpty(grupo))
            {
                await DisplayAlert("¡Atención!", "El grupo no puede estar vacío", "Aceptar");
            }
            else
            {
                //Inicializo la instancia y conexion a firebase
                dbFirebase = new DBFirebase();

                //El campo notas es opcional por eso lo añado como vacio si está vacio
                if (string.IsNullOrEmpty(notas))
                {
                    notas = "";
                }

                if (edit)
                {
                    bool respuesta = await DisplayAlert("¡Atención!", "¿Estas seguro que desea reemplazarlo?", "Si", "No");

                    if (respuesta)
                    {
                        dbFirebase.DeletePassName(idEdit);
                        //Inicializo el objeto con los datos encriptados
                        ps = new PassName(_Encrypt.PassEncrip(pass), _Encrypt.NameEncrypt(nombre), _Encrypt.NameEncrypt(grupo), _Encrypt.NameEncrypt(notas), _Encrypt.NameEncrypt(source), _Encrypt.NameEncrypt(DateTime.Now.ToString(new CultureInfo("es-ES"))));
                        //Añado a firebase el objeto
                        dbFirebase.AddPassName(ps);
                        await this.DisplayToastAsync("Editado correctamente", 999);
                        //Vuelvo a la pantalla principal
                        await Navigation.PopAsync(false);
                        await Navigation.PushAsync(new MotorOhMyPass(), false);
                    }
                }
                else
                {
                    //Inicializo el objeto con los datos encriptados
                    ps = new PassName(_Encrypt.PassEncrip(pass), _Encrypt.NameEncrypt(nombre), _Encrypt.NameEncrypt(grupo), _Encrypt.NameEncrypt(notas), _Encrypt.NameEncrypt(source), _Encrypt.NameEncrypt(DateTime.Now.ToString(new CultureInfo("es-ES"))));
                    //Añado a firebase el objeto
                    dbFirebase.AddPassName(ps);
                    #region Borrado de campos
                    cNamePass.Text = "";
                    cPassEncrypt.Text = "";
                    cGrupo.Text = "";
                    cNotas.Text = "";
                    #endregion
                    await this.DisplayToastAsync("Guardado correctamente", 999);

                }
            }
        }
        private void AboutIcon_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Información", "Ingresa el nombre del grupo al que puede pertenecer la contraseña.\n" +
                "P.Ej. Tenemos una contraseña de Correo.\n" +
                "Nombre para la contraseña: Correo de compras.\n" +
                "Contraseña: *.0H_My_Pa$$.\n" +
                "Grupo: Gmail", "Aceptar");
        }
        private void AboutNotasIcon_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Información", "Opcional*\nAgrega una nota a esta contraseña.", "Aceptar");
        }
        private async void btnAtras(object sender, EventArgs e)
        {
            //Vuelvo a la pantalla principal
            Application.Current.MainPage = new NavigationPage(new MotorOhMyPass())
            {
                BarBackgroundColor = Color.DarkGray,
                BarTextColor = Color.Black,
            };

        }

        #endregion


    }
}