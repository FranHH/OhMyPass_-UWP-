using MonkeyCache.FileStore;
using OhMyPass.models;
using OhMyPassWFP.Services;
using OhMyPassWFP.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace OhMyPassWFP
{
    public partial class App : Application, ILoginManager
    {
        #region Declaracion de varibles y objetos
        public static ILoginManager loginManager;
        public static App Current;
        public static int val;
        private DBFirebase dbFirebase;
        private const string BASEURL = "https://ohmypass-238d0-default-rtdb.firebaseio.com/";
        private static string DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #endregion

        #region Inicio APP
        public App()
        {
            InitializeComponent();
            Current = this;
            //Creo un id para la aplicacion
            Barrel.ApplicationId = "OhMyPassID";
            //Inicio una tarea donde obtengo los datos cuando haya internet y los guardo en caché
            Task.Run(() => GetResponse());

            
            //Inicio de sesion para pruebas
            //MainPage = new NavigationPage(new MotorOhMyPass());

            //Compruebo si el usuario tiene una sesion activa
            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") ? (bool)Properties["IsLoggedIn"] : false;
            var filePath = Path.Combine(DEFAULTPATH, "ID");
            
            //Pruebas
            //File.Delete(filePath);
           
            //Si el usuario tiene sesion activa muestro la pagina de contraseñas
            if (isLoggedIn && File.Exists(filePath))
            {

                MainPage = new NavigationPage(new MotorOhMyPass())
                {
                    BarBackgroundColor = Color.DarkGray,
                    BarTextColor = Color.Black,
                };
            }
            else //En caso contrario muestro la pagina de login
            {

                MainPage = new NavigationPage(new LoginPageKey(this));
            }
        }
        #endregion

        #region Metodos Override ON
        protected override void OnStart(){}
        protected override void OnSleep(){}
        protected override void OnResume(){}
        #endregion

        #region ShowMainPage
        public void ShowMainPage()
        {
            MainPage = new NavigationPage(new MotorOhMyPass())
            {
                BarBackgroundColor = Color.DarkGray,
                BarTextColor = Color.Black,
            };
        }
        #endregion

        #region Logout
        public void Logout()
        {
            MainPage = new NavigationPage(new LoginPageKey(this));
        }
        #endregion

        #region GetDataForCache
        public async Task<IEnumerable<PassName>> GetResponse()
        {
            //Inicializo la conexion a firebase
            dbFirebase = new DBFirebase();
            //Obtengo el listado de datos del usuario que ha iniciado sesion
            IList<PassName> ps = dbFirebase.GetPassNameCache();
            if(ps != null)
            {
                //Guardo datos en caché
                Barrel.Current.Add(key: BASEURL, data: ps, expireIn: TimeSpan.FromDays(1));
            }            
            return null;
        }
        #endregion

    }
}
