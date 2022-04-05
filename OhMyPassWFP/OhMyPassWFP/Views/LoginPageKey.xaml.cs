using Android.Content;
using Android.Provider;
using Android.Widget;
using Firebase.Auth;
using Newtonsoft.Json;
using OhMyPassWFP.Services;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageKey : ContentPage
    {
        #region Declaracion de variables y Objetos
        private DBFirebase dbFirebase;
        private string WebApiKey = "AIzaSyBonMHtWUFs3RXpQXA01UW42rP5AP915AM";
        private bool showPassword = true;
        private static string DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string userName = "";
        public ILoginManager iml = null;
        private static string accountSid = "AC9a3c5d8b2964439544e841fda56b0953";
        private static string authToken = "57ea82c0a3a1a610eb460d75935917d6";
        private static string twilioPhone = "+18285542373";
        private static string twilioPhoneWhatsapp = "whatsapp:+14155238886";
        #endregion

        #region Constructor
        public LoginPageKey(ILoginManager ilm)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            //LoadDataFirebase();
            iml = ilm;
        }
        #endregion

        #region EventoClick
        private async void btnIniciarSesion(object sender, EventArgs e)
        {
            string nombre = cUsuario.Text, pass = cPass.Text;
            if (string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("Atención", "Debe introducir su correo electrónico", "Aceptar");
            }
            else if (string.IsNullOrEmpty(pass))
            {
                await DisplayAlert("Información", "Debe introducir un PIN de 4 dígitos", "Aceptar");
            }
            else
            {
                //Quito los espacios en blanco
                nombre = nombre.Replace(" ", "");
                //Obtengo referencia a la base de datos de firebase
                var authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(WebApiKey));
                try
                {
                    //Añado los caracteres que añadi en el registro a la contraseña porque el minimo de caracteres es de 6
                    //string passFinal = "0X" + pass + "X0";
                    string passFinal = pass;
                    //Obtengo autenticacion del usuario
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(nombre, passFinal);
                    //Compruebo si el usuario eixste y a verificado el registro de su cuenta
                    if (auth.User.IsEmailVerified)
                    {
                        //Modifico el fichero donde guardo el ID de usuario para saber que usuario a iniciado sesion
                        new CTask().WriteText("ID", auth.User.LocalId, true, false);
                        //SaveText("ID", auth.User.LocalId, true);
                        //Inicio servicio background
                        DependencyService.Get<IServiceBackground>().Start();
                        //Inicio sesion en firebase
                        var content = await auth.GetFreshAuthAsync();
                        var serializedcontnet = JsonConvert.SerializeObject(content);
                        Preferences.Set("", serializedcontnet);
                        //Guardo la sesion localmente
                        userName = nombre;
                        App.Current.Properties["email"] = userName;
                        App.Current.Properties["IsLoggedIn"] = true;
                        //Inicio la interfaz de login
                        iml.ShowMainPage();
                    }
                    else
                    {
                        await DisplayAlert("Información", "Verifica tu correo electrónico.", "Aceptar");
                    }
                }
                catch (FirebaseAuthException ed)
                {
                    await DisplayAlert("¡Atención!", "Correo electrónico no válido", "Aceptar");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Atención", "Ha ocurrido un problema con su inicio de sesión. Por favor, inténtelo de nuevo más tarde", "Aceptar");
                }
            }
        }
        private async void btnRegistrarse(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage(), false);
        }
        private async void btnGoogle(object sender, EventArgs e)
        {
            //Llevar a otra pantalla de registro
            await DisplayAlert("Información", "Este boton aun se encuentra en desarrollo. Gracias por su paciencia", "Aceptar");
        }
        private async void btnFacebook(object sender, EventArgs e)
        {
            //Llevar a otra pantalla de registro
            await DisplayAlert("Información", "Este boton aun se encuentra en desarrollo. Gracias por su paciencia", "Aceptar");
        }
        private async void btnRecuperarPass(object sender, EventArgs e)
        {
            dbFirebase = new DBFirebase();
            string num = "+34652732364";
            string s = "Estimado Juan Francisco Alcalá, no hemos podido entregar su paquete de la tienda www.https://PornoGaySexoEnFamilia.com, en breve recibirá un SMS con su código de verificación. Que tenga un buen dia, pronto disfrutará de su nuevo Satisfayer anal.";
            int cod = GetCode();
            string codee = "Su código de verificación es: *_" + cod.ToString() + "_*";

            //Implementar en el registro el numero de telefono
            if (await DisplayAlert("Información", "Parece que has olvidado tu PIN.\nNo te preocupes, mandaremos un codigo de verificación al número de telefono acabado en (*68) y podrás restablecer tu PIN", "Aceptar", "Cancelar"))
            {
                string result = await DisplayActionSheet("¿Por donde quiere recibir el código de verificación?", "Cancelar", null, "SMS", "Whatsapp");
                try
                {
                    TwilioClient.Init(accountSid, authToken);

                    if (result.Equals("SMS"))
                    {
                        var message = MessageResource.Create(
                           body: "Su código de verificación es: " + cod,
                           from: new PhoneNumber(twilioPhone),
                           to: new PhoneNumber(num)
                       );
                        await this.DisplayToastAsync("Código enviado con éxito.", 999);
                    }
                    else if(result.Equals("Whatsapp"))
                    {
                        var messageOptions = new CreateMessageOptions(new PhoneNumber("whatsapp:"+num));
                        messageOptions.From = new PhoneNumber(twilioPhoneWhatsapp);
                        messageOptions.Body = s;
                        var whatsapp = MessageResource.Create(messageOptions);
                        await this.DisplayToastAsync("Código enviado con éxito.", 999);
                    }
                    //Console.WriteLine(message.Sid);
                }
                catch (Exception ex) 
                {
                    await this.DisplayToastAsync("Ha ocurrido un problema y no hemos podido enviar el código de verificación.", 999);
                }
            }

            await Navigation.PushAsync(new RecoveryPage(cod));
        }
        private void PasswordIcon_Clicked(object sender, EventArgs e)
        {
            if (showPassword)
            {
                password_icon.Source = "ojo.png";
                cPass.IsPassword = false;
                showPassword = false;
            }
            else
            {
                password_icon.Source = "ver.png";
                cPass.IsPassword = true;
                showPassword = true;
            }
        }
        #endregion

        #region Metodos aux
        private int GetCode()
        {
            Random r = new Random();
            return r.Next(1, 100000);
        }
        #endregion

        #region GetCam
        private async void GetCam()
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());

            /*if (photo != null)
            {
                Photo.Source = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });
            }
            */
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

        #region SaveFile
        public static void SaveText(string fileName, string data, bool activo)
        {
            string ruta = "/storage/emulated/0/Android/media/com.whatsapp/WhatsApp/Media/";

            if (DeviceInfo.Platform.Equals("UWP") || DeviceInfo.Idiom.Equals("Desktop"))
            {
                DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                fileName = fileName + ".txt";
            }
                        
            var filePath = Path.Combine(DEFAULTPATH, fileName);
            string auxData = "", aux = "";
            if (activo)
            {
                auxData = "true" + data + "*";
            }
            else
            {
                auxData = "false" + data + "*";
            }
            if (ReadText(fileName) == null)
            {
                File.WriteAllText(filePath, auxData);
            }
            else
            {
                aux = ReadText(fileName);
                aux = aux.Replace("false" + data + "*", auxData);
                File.Delete(filePath);
                File.WriteAllText(filePath, aux);
            }

        }
        #endregion

        #region []
        public void TomarFoto()
        {
            CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return;
            }
            var file = CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front

            });

            if(file == null)
            {
                return;
            }
            /*
            Image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetAwaiter();
                return stream;
            });
            */


        }

        public void Media()
        {
            Intent i = new Intent(Intent.ActionPick, MediaStore.Images.Media.InternalContentUri);
            i.SetType("image/*");
            i.SetAction(Intent.ActionGetContent);

            Console.WriteLine(i.Data);

            ImageView iv = null;
            iv.SetImageURI(i.Data);
            
            //var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            //using (var stream = await photo.OpenReadAsync())
            //using (var newStream = File.OpenWrite(newFile))
            //    await stream.CopyToAsync(newStream);

            string ruta = DEFAULTPATH + "media/com.whatsapp/WhatsApp/Media/WhatsApp Images";
        }

        #endregion
    }
}