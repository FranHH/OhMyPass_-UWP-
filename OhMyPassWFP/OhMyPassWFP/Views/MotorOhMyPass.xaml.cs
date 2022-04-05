using OhMyPass.models;
using OhMyPassWFP.Models;
using OhMyPassWFP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MotorOhMyPass : MasterDetailPage
    {
        #region Declaracion de variables y objetos
        private string userName;
        public static string UserName;
        UserKey objectUser = new UserKey(UserName);
        private static string DEFAULTPATH = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #endregion

        #region Constructor
        public MotorOhMyPass()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            userName = objectUser.GetUser();
            InitializeComponent();
            Init();
        }
        #endregion

        #region Init Menu
        private void Init()
        {
            var image = new Image();
            image.SetBinding(Image.SourceProperty, "image");

            List<Menu> menu = new List<Menu>
            {
                new Menu {MenuImage = "hogar.png", MenuTitle = "Inicio"},
                new Menu {MenuImage = "generarpass.png", MenuTitle = "Generar contraseña"},
                new Menu {MenuImage = "ajuste.png", MenuTitle = "Ajustes"},
                new Menu {MenuImage = "acercade.png", MenuTitle = "Acerca de"},
                new Menu {MenuImage = "faq.png", MenuTitle = "FAQ"},
                new Menu {MenuImage = "feedback.png", MenuTitle = "Opinión"},
                new Menu {MenuImage = "op.png", MenuTitle= "Opiniones de otros usuarios"},
                new Menu {MenuImage = "privacidad.png", MenuTitle = "Politica de Privacidad"},
                new Menu {MenuImage = "calificacion.png", MenuTitle= "Califica la aplicación en Play Store"},
                new Menu {MenuImage = "logout.png" , MenuTitle = "Cerrar sesión"}
            };

            ListMenu.ItemsSource = menu;
            
            
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MenuPrincipal))) { BarBackgroundColor = Color.DarkGray, BarTextColor = Color.Black};
        }
        #endregion

        #region Clicked - Seleccted - Tapped
        private async void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu;

            if (menu != null)
            {
                if (menu.MenuTitle.Equals("Inicio"))
                {
                    IsPresented = false;
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MenuPrincipal))) { BarBackgroundColor = Color.DarkGray, BarTextColor = Color.Black };

                } else if (menu.MenuTitle.Equals("Generar contraseña"))
                {
                    IsPresented = false;
                    string pass = PassGenerator();

                    if (await DisplayAlert("Contraseña", pass, "Copiar","Cancelar"))
                    {
                        IsPresented = false;
                        await Clipboard.SetTextAsync(pass);
                        await this.DisplayToastAsync("Copiado al portapeles", 1000);
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MenuPrincipal))) { BarBackgroundColor = Color.DarkGray, BarTextColor = Color.Black };
                    }
                }
                else if (menu.MenuTitle.Equals("Ajustes"))
                {
                    IsPresented = false;
                    //Crear pagina de Ajustes
                    //Detail = new NavigationPage(new AjustesPage());
                } else if (menu.MenuTitle.Equals("Acerca de"))
                {
                    await Navigation.PushAsync(new AcercaDe());

                }else if (menu.MenuTitle.Equals("FAQ"))
                {

                }else if (menu.MenuTitle.Equals("Opinión"))
                {
                    await Navigation.PushAsync(new AddOpinionPage());
                   
                }else if (menu.MenuTitle.Equals("Opiniones de otros usuarios"))
                {
                    await Navigation.PushAsync(new OpinionPage());
                }
                else if (menu.MenuTitle.Equals("Politica de Privacidad"))
                {

                }else if(menu.MenuTitle.Equals("Califica la aplicación en Play Store"))
                {
                    if (await Launcher.CanOpenAsync("https://play.google.com"))
                    {
                        await Launcher.OpenAsync("https://play.google.com/store?hl=es&gl=US");
                    }
                }
                else if (menu.MenuTitle.Equals("Cerrar sesión"))
                {
                    new CTask().WriteText("ID", null, false, true);
                    //EditFile();
                    IsPresented = false;
                    App.Current.Properties["IsLoggedIn"] = false;
                    App.Current.Logout();
                }
            }
        }
        private void menuTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        #endregion

        #region EditFile
        public void EditFile()
        {

            var filePath = Path.Combine(DEFAULTPATH, "ID");

            string auxData = "", aux = "";

            aux = ReadText("ID");

            for (int i = 0; i < aux.Length; i++)
            {
                auxData += aux[i].ToString();

                if (auxData.Equals("false"))
                {
                    auxData = "";
                }
                else if (aux[i].ToString().Equals("*"))
                {
                    auxData = "";
                }
                else if (auxData.Equals("true"))
                {
                    auxData = "";

                    for (int j = i + 1; j < aux.Length; j++)
                    {
                        auxData += aux[j].ToString();
                        if (aux[j].ToString().Equals("*"))
                        {
                            j = aux.Length;
                            i = j;
                            aux = aux.Replace("true" + auxData, "false" + auxData);
                        }
                    }
                }
            }
            File.Delete(filePath);
            File.WriteAllText(filePath, aux);

        }
        #endregion

        #region ReadFile
        public static string ReadText(string fileName)
        {
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            if (!File.Exists(filePath))
                return null;
            return File.ReadAllText(filePath);
        }
        #endregion

        #region Generator
        private string PassGenerator()
        {
            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            int longitud = caracteres.Length;
            char letra;
            int longitudContrasena = 10;
            string contraseniaAleatoria = string.Empty;
            for (int i = 0; i < longitudContrasena; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                contraseniaAleatoria += letra.ToString();
            }
            return contraseniaAleatoria;
        }

        #endregion

    }
    #region Class Menu
    public class Menu
    {
        public string MenuImage
        {
            get;
            set;
        }
        public string MenuTitle
        {
            get;
            set;
        }
      
    }
    #endregion
}