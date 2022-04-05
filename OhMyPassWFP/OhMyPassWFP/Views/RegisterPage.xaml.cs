using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using OhMyPass.models;
using OhMyPassWFP.Models;
using OhMyPassWFP.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        #region Declaracion de variables y Objectos
        FirebaseClient clienteFirebase = new FirebaseClient("https://ohmypass-238d0-default-rtdb.firebaseio.com/");
        private Encriptar _Encrypt;
        public string WebApiKey = "AIzaSyBonMHtWUFs3RXpQXA01UW42rP5AP915AM";
        bool showPassword = true;
        private UserKey userObject;
        int i;
        static string DEFAULTPATH = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        #endregion

        #region Constructor
        public RegisterPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            SetCountry();
            lbVersion.Text = "V-"+ Environment.Version.ToString();
        }
        #endregion

        #region Evento Click
        private async void btnCancelarRegistro(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
        }
        private async void btnRegistrar(object sender, EventArgs e)
        {
            string user = cUsuarioRegister.Text, pass = cPassRegister.Text, rpass = cRPassRegister.Text;
            if (checkCondicionesYServicios.IsChecked)
            {
                if (string.IsNullOrEmpty(user))
                {
                    await DisplayAlert("¡Atención!", "Correo no valido, este campo no puede estar vacío.", "Aceptar");
                }
                else if (string.IsNullOrEmpty(pass))
                {
                    await DisplayAlert("¡Atención!", "No has introducido ninguna contraseña.\nDebe ser un PIN de 4 digitos", "Aceptar");
                }
                else if (!ValidarPass(pass, rpass))
                {
                    await DisplayAlert("¡Atención!", "Las contraseñas no coinciden.", "Aceptar");
                }
                else if (pass.Length < 4)
                {
                    await DisplayAlert("¡Atención!", "Debe ser un pin de 4 caracteres", "Aceptar");
                }
                else
                {
                    try
                    {   
                        //Añado a la contraseña 4 caracteres mas porque el minimo de caracteres de un entry es 6
                        string passFinal = "0X" + pass + "X0";
                        //Autentico al usuario
                        var authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(WebApiKey));
                        //Quito los espacios en blanco
                        user = user.Replace(" ", "");
                        //Creo el usuario en firebase
                        var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(user, passFinal);
                        //Lanzo correo de verificacion
                        await authProvider.SendEmailVerificationAsync(auth.FirebaseToken, "");
                        //Inicializo la clase para encriptar
                        _Encrypt = new Encriptar();
                        //Creo el usuario
                        userObject = new UserKey(_Encrypt.NameEncrypt(user), _Encrypt.PassEncrip(pass), _Encrypt.NameEncrypt("353"), _Encrypt.NameEncrypt("a"));
                        //Guardo localmente el id del usuario
                        new CTask().WriteText("ID", auth.User.LocalId, false, false);
                        //SaveText("ID", auth.User.LocalId);
                        //Guardo el usuario
                        await clienteFirebase.Child(@"Users").Child(auth.User.LocalId).Child(@"Login").PutAsync(userObject);
                        //Muestro mensaje de bienvenida
                        await DisplayAlert("¡Bienvenido!", "Hemos recibido tu solicitud de registro.\nPor favor, revisa tu bandeja de entrada y verifica tu correo.", "Aceptar");
                        //Muestro mensaje de usuario registrado
                        await this.DisplayToastAsync("Usuario registrado correctamente.", 999);
                        //Vuelvo a la pestaña de login
                        await Navigation.PopAsync(false);
                    }
                    catch (FirebaseAuthException)
                    {
                        await DisplayAlert("¡Atención!", "Correo electronico no válido", "Aceptar");
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("¡Atención!", "Ha ocurrido un problema con su conexión a internet, por favor, intentelo de nuevo mas tarde", "Aceptar");
                    }
                }
            }
            else
            {
                await DisplayAlert("¡Atención!", "Debe de acepatar las Condiciones del servicio y la Política de privacidad", "Aceptar");
            }
        }
        private void PsRegisIconClicked(object sender, EventArgs e)
        {
            if (showPassword)
            {
                psRegisIcon.Source = "ojo.png";
                cPassRegister.IsPassword = false;
                showPassword = false;
            }
            else
            {
                psRegisIcon.Source = "ver.png";
                cPassRegister.IsPassword = true;
                showPassword = true;
            }
        }
        private void PsRepitRegisIconClicked(object sender, EventArgs e)
        {
            if (showPassword)
            {
                psRepitRegistIcon.Source = "ojo.png";
                cRPassRegister.IsPassword = false;
                showPassword = false;
            }
            else
            {
                psRepitRegistIcon.Source = "ver.png";
                cRPassRegister.IsPassword = true;
                showPassword = true;
            }
        }
        private void TerminosYCondiciones(object sender, EventArgs e)
        {
            DisplayAlert("Términos y Condiciones de Uso", "INFORMACIÓN RELEVANTE\n" +
               "Es requisito necesario para la adquisición de los productos que se ofrecen en este sitio," +
               " que lea y acepte los siguientes Términos y Condiciones que a continuación se redactan. " +
               "El uso de nuestros servicios así como la compra de nuestros productos implicará que usted ha leído y aceptado los Términos y Condiciones de Uso en el presente documento. " +
               "Todos los productos que son ofrecidos por nuestro sitio web pudieran ser creados, " +
               "cobrados, enviados o presentados por una página web tercera y en tal caso estarían sujetas a sus propios Términos y Condiciones. " +
               "En algunos casos, para adquirir un producto, será necesario el registro por parte del usuario, " +
               "con ingreso de datos personales fidedignos y definición de una contraseña.\n" +
               "El usuario puede elegir y cambiar la clave para su acceso de administración de la cuenta en cualquier momento, " +
               "en caso de que se haya registrado y que sea necesario para la compra de alguno de nuestros productos. " +
               "No asume la responsabilidad en caso de que entregue dicha clave a terceros.\n" +
               "Todas las compras y transacciones que se lleven a cabo por medio de este sitio web, " +
               "están sujetas a un proceso de confirmación y verificación, el cual podría incluir la verificación del stock y disponibilidad de producto, " +
               "validación de la forma de pago, validación de la factura (en caso de existir) " +
               "y el cumplimiento de las condiciones requeridas por el medio de pago seleccionado. " +
               "En algunos casos puede que se requiera una verificación por medio de correo electrónico.\n" +
               "Los precios de los productos ofrecidos en esta Tienda Online es válido solamente en las compras realizadas en este sitio web.\n" +
               "LICENCIA\n" +
               "Oh My Pass a través de su sitio web concede una licencia para que los usuarios utilicen " +
               "los productos que son vendidos en este sitio web de acuerdo a los Términos y Condiciones que se describen en este documento\n" +
               "PROPIEDAD\n" +
               "Usted no puede declarar propiedad intelectual o exclusiva a ninguno de nuestros productos, modificado o sin modificar. " +
               "Todos los productos son propiedad  de los proveedores del contenido. En caso de que no se especifique lo contrario, " +
               "nuestros productos se proporcionan sin ningún tipo de garantía, expresa o implícita. " +
               "En ningún esta compañía será  responsables de ningún daño incluyendo, pero no limitado a, daños directos, indirectos, " +
               "especiales, fortuitos o consecuentes u otras pérdidas resultantes del uso o de la imposibilidad de utilizar nuestros productos.\n" +
               "POLÍTICA DE REEMBOLSO Y GARANTÍA\n" +
               "En el caso de productos que sean  mercancías irrevocables no-tangibles, no realizamos reembolsos después de que se envíe el producto, " +
               "usted tiene la responsabilidad de entender antes de comprarlo.  Le pedimos que lea cuidadosamente antes de comprarlo. " +
               "Hacemos solamente excepciones con esta regla cuando la descripción no se ajusta al producto. " +
               "Hay algunos productos que pudieran tener garantía y posibilidad de reembolso pero este será especificado al comprar el producto. " +
               "En tales casos la garantía solo cubrirá fallas de fábrica y sólo se hará efectiva cuando el producto se haya usado correctamente. " +
               "La garantía no cubre averías o daños ocasionados por uso indebido. " +
               "Los términos de la garantía están asociados a fallas de fabricación y funcionamiento en condiciones normales de los productos y sólo se harán efectivos estos términos" +
               " si el equipo ha sido usado correctamente. Esto incluye:\n" +
               "-De acuerdo a las especificaciones técnicas indicadas para cada producto.\n" +
               "-En condiciones ambientales acorde con las especificaciones indicadas por el fabricante.\n" +
               "-En uso específico para la función con que fue diseñado de fábrica.\n" +
               "-En condiciones de operación eléctricas acorde con las especificaciones y tolerancias indicadas.\n" +
               "COMPROBACIÓN ANTIFRAUDE\n" +
               "La compra del cliente puede ser aplazada para la comprobación antifraude. " +
               "También puede ser suspendida por más tiempo para una investigación más rigurosa, " +
               "para evitar transacciones fraudulentas.\n" +
               "PRIVACIDAD\n" +
               "Oh My Pass garantiza la información personal que usted envía cuenta con la seguridad necesaria. " +
               "Los datos ingresados por usuario o en el caso de requerir una validación de los pedidos no serán entregados a terceros, " +
               "salvo que deba ser revelada en cumplimiento a una orden judicial o requerimientos legales.\n" +
               "La suscripción a boletines de correos electrónicos publicitarios es voluntaria y podría ser seleccionada al momento de crear su cuenta.\n" +
               "Oh My Pass reserva los derechos de cambiar o de modificar estos términos sin previo aviso.", "Aceptar");
        }

        #endregion

        #region ValidarPASS
        private bool ValidarPass(string p, string rp)
        {
            Encriptar _Encrypt = new Encriptar();

            if (_Encrypt.PassEncrip(p).Equals(_Encrypt.PassEncrip(rp)))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region SetCountry
        public void SetCountry()
        {
            var image = new Image();
            image.SetBinding(Image.SourceProperty, "image");

            List<Country> _country = new List<Country>
            {
                new Country {CountryImage = "hogar.png", CountryName = "España", CountryPrefijo = "+34"},
                new Country {CountryImage = "generarpass.png", CountryName = "Alemania", CountryPrefijo = "+34"},
            };

            listCountry.ItemsSource = _country;
        }
        #endregion

        private void CountryItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var selectCountry = e.SelectedItem as Country;

            if (selectCountry != null)
            {
                if (selectCountry.CountryName.Equals("España"))
                {
                    prefijo.Text = selectCountry.CountryPrefijo.ToString();

                }
                else if (selectCountry.CountryName.Equals(""))
                {

                }
            }


        }

        #region SaveFile
        public async static void SaveText(string fileName, string data)
        {
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            //File.Delete(filePath); //Borro fichero para pruebas
            string auxData = "false" + data + "*", aux="";
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, auxData);
            }
            else
            {
                aux = ReadText(fileName);
                File.Delete(filePath);
                auxData = aux + auxData;
                File.WriteAllText(filePath, auxData);
            }
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

       
    }
}