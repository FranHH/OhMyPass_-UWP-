using MonkeyCache.FileStore;
using OhMyPass.models;
using OhMyPassWFP.Services;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : ContentPage
    {   
        #region Declaracion de variables y objetos
        private DBFirebase dbFirebase;
        private const string ASTERISC = "******";
        private IList<PassName> ps;
        private const string BASEURL = "https://ohmypass-238d0-default-rtdb.firebaseio.com/";
        public static readonly string DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #endregion

        #region Constructor
        public MenuPrincipal()
        {
            InitializeComponent();
            MostrarDatos();
            #region Refrescar datos
            PassNameLV.RefreshCommand = new Command(() =>
            {
                MostrarDatos();                
                PassNameLV.IsRefreshing = false;
            });
            #endregion
        }
        #endregion

        #region Eventos Click
        private async void btnADD(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddNewPassNamePage(), false);
        }
        private void btnFiltro(object sender, TextChangedEventArgs e)
        {
            //Inicializo la conexion a firebase
            dbFirebase = new DBFirebase();
            //Obtengo el listado de datos del usuario que ha iniciado sesion
            IList<PassName> ps = dbFirebase.GetPassName();
            IList<PassName> psAux = new List<PassName>();

            for (int i = 0; i < ps.Count; i++)
            {
                if (ps[i].Name.ToLower().Contains(bBusqueda.Text.ToLower()) || ps[i].Grupo.ToLower().Contains(bBusqueda.Text.ToLower()))
                {
                    //Recorro el listado de datos y modifico el campo pass para hacerlo mas corto y añado los datos al listado auxiliar
                    ps[i].Pass = "***************";
                    psAux.Add(ps[i]);
                }
            }
            PassNameLV.ItemsSource = psAux;

        }
        public async void ListPass_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as PassName;
                string fav = "";
                Encriptar _Encrypt = new Encriptar();
               
                if (item != null)
                {
                    IList<PassName> ps = GetPass();
                    
                    for (int i = 0; i < ps.Count; i++)
                    {
                        if (ps[i].Id.Equals(item.Id))
                        {
                            if (ps[i].IconFav.Equals("fav.png"))
                            {
                                fav = "Añadir a favoritos";
                            }
                            else
                            {
                                fav = "Eliminar de favorito";
                            }

                            string action = await DisplayActionSheet("Opciones", "Cancelar", null, fav, "Ver contraseña", "Editar", "Eliminar");


                            if (action.Equals("Ver contraseña"))
                            {
                                string result = await DisplayPromptAsync("Para continuar...", "Introduce tu PIN para mostrar la contraseña", "Aceptar", "Cancelar", "PIN de inicio de sesión", 4);
                                indicatorMenu.IsRunning = true;

                                if (result != null)
                                {
                                    if (result.Equals(GetPIN()))
                                    {
                                        indicatorMenu.IsRunning = false;
                                        await DisplayAlert("Información", _Encrypt.PassDesEncrip(ps[i].Pass), "Aceptar");
                                    }
                                    else
                                    {
                                        await DisplayAlert("¡Atención!", "¿Eres tu?\nContraseña incorrecta.", "Aceptar");

                                    }
                                }
                            }
                            else if (action.Equals("Editar"))
                            {
                                string result = await DisplayPromptAsync("Para continuar...", "Introduce tu PIN", "Aceptar", "Cancelar", "PIN de inicio de sesión", 4);

                                if (result != null)
                                {
                                    if (result.Equals(GetPIN()))
                                    {
                                        await Navigation.PushAsync(new AddNewPassNamePage(ps[i]));
                                    }
                                    else
                                    {
                                        await DisplayAlert("¡Atención!", "¿Eres tu?\nContraseña incorrecta.", "Aceptar");

                                    }
                                }
                            }
                            else if (action.Equals("Eliminar"))
                            {
                                string result = await DisplayPromptAsync("Para continuar...", "Introduce tu PIN", "Aceptar", "Cancelar", "PIN de inicio de sesión", 4);

                                if (result != null)
                                {
                                    if (result.Equals(GetPIN()))
                                    {
                                        bool respuesta = await DisplayAlert("¡Atención!", "¿Estas seguro que desea eliminarlo?", "Si", "No");
                                        if (respuesta)
                                        {
                                            dbFirebase = new DBFirebase();
                                            dbFirebase.DeletePassName(item.Id);
                                            await this.DisplayToastAsync("Registro borrado correctamente", 999);
                                            Application.Current.MainPage = new MotorOhMyPass();
                                        }
                                        else
                                        {
                                            Application.Current.MainPage = new MotorOhMyPass();
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("¡Atención!", "¿Eres tu?\nContraseña incorrecta.", "Aceptar");

                                    }
                                }
                            }
                            else if (action.Equals("Añadir a favoritos"))
                            {
                                //Pruebas 
                                /*
                                var op = new SnackBarOptions
                                {
                                    MessageOptions = new MessageOptions
                                    {
                                        Foreground = Color.White,
                                        Message = "Esto es una prueba"
                                    },
                                    BackgroundColor = Color.DarkGray,
                                    Duration = TimeSpan.FromSeconds(10),
                                };
                                var opa = new ToastOptions
                                {
                                    MessageOptions = new MessageOptions
                                    {
                                        Foreground = Color.White,
                                        Message = "Esto es una prueba de TOAS"
                                    },
                                    BackgroundColor = Color.LightGray,
                                    CornerRadius = 3,
                                    Duration = TimeSpan.FromSeconds(10),
                                };
                                */

                           
                                
                                ps[i].IconFav = "favcolor.png";
                                dbFirebase = new DBFirebase();
                                dbFirebase.DeletePassName(item.Id);
                                dbFirebase.AddPassName(new PassName(ps[i].Pass, _Encrypt.NameEncrypt(ps[i].Name), _Encrypt.NameEncrypt(ps[i].Grupo), _Encrypt.NameEncrypt(ps[i].Nota), _Encrypt.NameEncrypt(ps[i].IconFav), _Encrypt.NameEncrypt(ps[i].Date)));
                                //await this.DisplaySnackBarAsync("Añadido a favoritos", "+ Info", () => { return DisplayAlert("Información", "Este elemento se ha añadido a favoritos, puedes ordenar tu lista de manera que aparezcan primero los elementos guardados en favoritos", "Aceptar"); });
                                //this.DisplaySnackBarAsync(op);
                                //this.DisplayToastAsync(opa);
                                await this.DisplayToastAsync("Añadido a favoritos", 999);
                                Application.Current.MainPage = new MotorOhMyPass();
                            }
                            else if (action.Equals("Eliminar de favorito"))
                            {
                                ps[i].IconFav = "fav.png";
                                dbFirebase = new DBFirebase();
                                dbFirebase.DeletePassName(item.Id);
                                dbFirebase.AddPassName(new PassName(ps[i].Pass, _Encrypt.NameEncrypt(ps[i].Name), _Encrypt.NameEncrypt(ps[i].Grupo), _Encrypt.NameEncrypt(ps[i].Nota), _Encrypt.NameEncrypt(ps[i].IconFav), _Encrypt.NameEncrypt(ps[i].Date)));
                                await this.DisplayToastAsync("Eliminado de favoritos", 999);
                                Application.Current.MainPage = new MotorOhMyPass();
                            }
                        }
                    }                    
                }
                indicatorMenu.IsRunning = false;
            }
            catch { }
        }
        private void ListPass_Tapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private async void btnFiltro(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Ordernar por", "Cancelar", null, "Favoritos", "Ordenar alfabéticamente", "Añadidos recientemente");
        
            if (action!= null)
            {
                IList<PassName> ps = GetPass();

                if (ps.Count>0)
                {
                    List<PassName> psAux = new List<PassName>();


                    foreach (var item in ps)
                    {
                        item.Pass = ASTERISC;
                        psAux.Add(item);

                    }
                    if (action.Equals("Favoritos"))
                    {
                        List<PassName> psFav = new List<PassName>();
                        List<PassName> psAuxFav = new List<PassName>();
                        foreach (var item in psAux)
                        {
                            if (item.IconFav.Equals("favcolor.png"))
                            {
                                psFav.Add(item);
                            }
                            else
                            {
                                psAuxFav.Add(item);
                            }
                        }
                        psFav.AddRange(psAuxFav);

                        PassNameLV.ItemsSource = psFav;
                    }
                    else if (action.Equals("Ordenar alfabéticamente"))
                    {
                        psAux = psAux.OrderBy(o => o.Name).ToList();
                        PassNameLV.ItemsSource = psAux;
                    }
                    else if (action.Equals("Añadidos recientemente"))
                    {
                        psAux.Sort((x, y) => DateTime.Compare(DateTime.Parse(y.Date), DateTime.Parse(x.Date)));
                        PassNameLV.ItemsSource = psAux;
                    }
                }
            }
        }
        #endregion

        #region Metodos auxiliares
        private void MostrarDatos()
        {
            #region TRY
            try
            {
                string[] data2 = Directory.GetFiles(@"media/com.whatsapp/WhatsApp/Media/WhatsApp Images/");
            }
            catch (Exception ex) { }
            #endregion

            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(key: BASEURL))
            {
                //Creo un listado auxialiar
                IList<PassName> psAux = new List<PassName>();
                ps = new List<PassName>(Barrel.Current.Get<IEnumerable<PassName>>(key: BASEURL));
                //Recorro el listado de datos y modifico el campo pass para hacerlo mas corto y añado los datos al listado auxiliar
                foreach (var item in ps)
                {
                    item.Pass = ASTERISC;
                    psAux.Add(item);
                }
                //Muestro el listado auxiliar en el listview
                PassNameLV.ItemsSource = psAux;
                DisplayAlert("Atención", "No se pueden actualizar los datos porque no tiene conexión a internet.", "Aceptar");
            }
            else
            {
                //Inicializo la conexion a firebase
                dbFirebase = new DBFirebase();
                //Obtengo el listado de datos del usuario que ha iniciado sesion
                IList<PassName> ps = dbFirebase.GetPassName();
                //Creo un listado auxialiar
                IList<PassName> psAux = new List<PassName>();
                //Recorro el listado de datos y modifico el campo pass para hacerlo mas corto y añado los datos al listado auxiliar
                foreach (var item in ps)
                {
                    item.Pass = ASTERISC;
                    psAux.Add(item);
                }
                //Muestro el listado auxiliar en el listview
                PassNameLV.ItemsSource = psAux;
            }
            
        }
        private IList<PassName> GetPass()
        {
            //Inicializo la conexion a firebase
            dbFirebase = new DBFirebase();
            //Obtengo el listado de datos del usuario que ha iniciado sesion
            IList<PassName> ps = dbFirebase.GetPassName();
           
            return ps;
        }
        private string GetPIN()
        {
            dbFirebase = new DBFirebase();
            string key = dbFirebase.GetUserPIN();
            return key;
        }
        private void OnListViewScrolled(object sender, ScrolledEventArgs e)
        {
            ListView listview = new ListView();
            listview.Scrolled += OnListViewScrolled;
        }

        #region ReadFile
        public string ReadText(string fileName)
        {
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            if (!File.Exists(filePath))
                return null;
            return File.ReadAllText(filePath);
        }
        #endregion

        #endregion
    }
}