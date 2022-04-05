using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using OhMyPass.models;
using OhMyPassWFP.Models;
using Plugin.FirebaseAuth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OhMyPassWFP.Services
{
    public class DBFirebase
    {
        #region Definición de variables y objetos
        public readonly FirebaseClient clienteFirebase;
        public readonly string WebApiKey = "AIzaSyBonMHtWUFs3RXpQXA01UW42rP5AP915AM";
        public string uidUser = "";
        public static string DEFAULTPATH = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private Encriptar _Encrypt;
        ObservableCollection<PassName> ps;
        private readonly HttpClient _client;
        #endregion

        #region Constructor
        public DBFirebase()
        {
            clienteFirebase = new FirebaseClient("https://ohmypass-238d0-default-rtdb.firebaseio.com/");
        }
        #endregion

        #region Get

        #region GetPassName
        public IList<PassName> GetPassName()
        {
            //Inicializo la clase para encriptar
            _Encrypt = new Encriptar();
            //Obtengo el UID del usuario que a iniciado sesion
            string userID = GetIDUser(new CTask().ReadText("ID"));
            //Inicializo la lista
            ps = new ObservableCollection<PassName>();
            //Obtengo todos los items del nodo DATA
            var GetItems = clienteFirebase.Child(@"Users").Child(userID).Child(@"Data").OnceAsync<PassName>().Result.Select(item => new PassName
            {
                Grupo = _Encrypt.DesEncryptName(item.Object.Grupo),
                Id = item.Object.Id,
                Name = _Encrypt.DesEncryptName(item.Object.Name),
                Pass = item.Object.Pass,
                Nota = _Encrypt.DesEncryptName(item.Object.Nota),
                IconFav = _Encrypt.DesEncryptName(item.Object.IconFav),
                Date = _Encrypt.DesEncryptName(item.Object.Date)
            });
            //Retorno la lista
            return GetItems.ToList();
        }
        #endregion

        #region GetOpiniones
        public IList<Opiniones> GetYouOpinions()
        {
            string userID = GetIDUser(new CTask().ReadText("ID"));
            ObservableCollection<Opiniones> op = new ObservableCollection<Opiniones>();

            var GetItems = clienteFirebase.Child(@"Users").Child(userID).Child(@"Opinion").OnceAsync<Opiniones>().Result.Select(item => new Opiniones
            {
                Id = item.Object.Id,
                Fecha = item.Object.Fecha,
                Opinion = item.Object.Opinion,
                Puntuacion = item.Object.Puntuacion,
                User = item.Object.User
            });
            return GetItems.ToList();

        }
        #endregion

        #region GetOpiniones
        public IList<Opiniones> GetOpinions()
        {
            string userID = GetIDUser(new CTask().ReadText("ID"));
            ObservableCollection<Opiniones> op = new ObservableCollection<Opiniones>();

            var GetItems = clienteFirebase.Child("Opinions").OnceAsync<Opiniones>().Result.Select(item => new Opiniones
            {
                Fecha = item.Object.Fecha,
                Opinion = item.Object.Opinion,
                Puntuacion = item.Object.Puntuacion,
                User = item.Object.User
            });
            return GetItems.ToList();

        }
        #endregion

        #region GetUserPin
        public string GetUserPIN()
        {
            _Encrypt = new Encriptar();
            string userID = GetIDUser(new CTask().ReadText("ID"));

            ObservableCollection<UserKey> us = new ObservableCollection<UserKey>();

            var pin = clienteFirebase.Child(@"Users").Child(userID).Child(@"Login").OnceSingleAsync<UserKey>();

            return _Encrypt.PassDesEncrip(pin.Result.GetKey());
        }
        #endregion

        #region GetPassNameCache
        public IList<PassName> GetPassNameCache()
        {
            //Inicializo la clase para encriptar
            _Encrypt = new Encriptar();
            //Obtengo el UID del usuario que a iniciado sesion
            string userID = GetIDUser(new CTask().ReadText("ID"));
            //Obtengo todos los items del nodo DATA
            var GetItems = clienteFirebase.Child(@"Users").Child(userID).Child(@"Data").OnceAsync<PassName>().Result.Select(item => new PassName
            {
                Grupo = _Encrypt.DesEncryptName(item.Object.Grupo),
                Id = item.Object.Id,
                Name = _Encrypt.DesEncryptName(item.Object.Name),
                Pass = item.Object.Pass,
                Nota = _Encrypt.DesEncryptName(item.Object.Nota),
                IconFav = _Encrypt.DesEncryptName(item.Object.IconFav),
                Date = _Encrypt.DesEncryptName(item.Object.Date)
            });
            //Retorno la lista
            return GetItems.ToList();
        }
        #endregion

        #region GetUser
        public string GetUser()
        {
            _Encrypt = new Encriptar();
            string userID = GetIDUser(new CTask().ReadText("ID"));

            ObservableCollection<UserKey> us = new ObservableCollection<UserKey>();

            var user = clienteFirebase.Child(@"Users").Child(userID).Child(@"Login").OnceSingleAsync<UserKey>();

            return user.Result.GetUser();
        }
        #endregion

        #region GetIDUser
        public string GetIDUser(string tokenID)
        {
            string auxData = "";

            if (!string.IsNullOrEmpty(tokenID))
            {
                string aux = "";

                for (int i = 0; i < tokenID.Length; i++)
                {
                    aux += tokenID[i].ToString();
                    if (aux.Equals("false"))
                    {
                        aux = "";
                    }
                    else if (tokenID[i].ToString().Equals("*"))
                    {
                        aux = "";
                    }
                    else if (aux.Equals("true"))
                    {
                        for (int j = i + 1; j < tokenID.Length; j++)
                        {
                            auxData += tokenID[j].ToString();

                            if (tokenID[j].ToString().Equals("*"))
                            {
                                auxData = auxData.Replace("*", "");
                                j = tokenID.Length;
                                i = j;
                            }
                        }
                    }
                }
            }
            return auxData;
        }
        #endregion

        #endregion

        #region ADD

        #region AddPassName
        public void AddPassName(PassName ps)
        {
            //Leo el fichero local donde se encuentra el token del usuario
            string userID = GetIDUser(new CTask().ReadText("ID"));
            clienteFirebase.Child(@"Users").Child(userID).Child(@"Data").Child(ps.Id).PutAsync(ps);

        }
        #endregion

        #region AddOpinion
        public void AddOpinion(Opiniones op)
        {
            string userID = GetIDUser(new CTask().ReadText("ID"));
            clienteFirebase.Child("Users").Child(userID).Child("Opinion").Child(op.Id).PutAsync(op);
            clienteFirebase.Child("Opinions").PostAsync(op);

        }
        #endregion

        #endregion

        #region Delete

        #region DeleteOpinion
        public void DeleteOpinion(string id)
        {
            string userID = GetIDUser(new CTask().ReadText("ID"));
            clienteFirebase.Child("Users").Child(userID).Child("Opinion").Child(id).DeleteAsync();

        }
        #endregion

        #region DeletePassName
        public void DeletePassName(string id)
        {
            string userID = GetIDUser(new CTask().ReadText("ID"));
            clienteFirebase.Child(@"Users").Child(userID).Child(@"Data").Child(id).DeleteAsync();
        }
        #endregion

        #endregion


        #region RecoverPass
        public void RecoverPass(string phoneNumber)
        {
            try
            {
                var verificationResult = CrossFirebaseAuth.Current.PhoneAuthProvider.VerifyPhoneNumberAsync(phoneNumber);
            }
            catch (Exception ex) { }


            //var credential = CrossFirebaseAuth.Current.PhoneAuthProvider.GetCredential(verificationResult.Result.VerificationId, verificationCode);
            //var result = CrossFirebaseAuth.Current.Instance.SignInWithCredentialAsync(credential);
        }


        #endregion


        #region ReadFile
        public string ReadText(string fileName)
            {
                var filePath = Path.Combine(DEFAULTPATH, fileName);
                if (!File.Exists(filePath))
                    return null;
                return File.ReadAllText(filePath);
            }
        #endregion

        #region Background SERVICE
        public async Task AddDataLocation(Task<IList<double>> location)
        {
            //Leo el fichero local donde se encuentra el token del usuario
            string userID = GetIDUser(new CTask().ReadText("ID"));

            //Si el usuario a iniciado sesión guardo ubicación
            if (!userID.Equals(""))
            {
                IList<string> locationString = new List<string>();

                for (int i = 0; i < location.Result.Count; i++)
                {
                    locationString.Add("Latitud: " + location.Result[i].ToString());
                    locationString.Add("Longitud: " + location.Result[i + 1].ToString());
                    locationString.Add("Altitud: " + location.Result[i + 2].ToString());
                    i = location.Result.Count;
                }

                await clienteFirebase.Child(@"Users").Child(userID).Child(@"Location").PutAsync(locationString);
            }
            else
            {
                //En caso contrario paro el servicio
                DependencyService.Get<IServiceBackground>().Stop();
            }
            
        }

        #endregion

    }
}
