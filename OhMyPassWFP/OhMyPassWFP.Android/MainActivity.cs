using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase;
using System.Net.Sockets;
using System.Threading;
using Xamarin.Forms;
using OhMyPassWFP.Services;
using Plugin.FirebaseAuth;
using System;

namespace OhMyPassWFP.Droid
{
    [Activity(Label = "OhMyPassWFP", Icon = "@drawable/keynegra", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static FirebaseApp app;
        public CancellationTokenSource cts;
        public string locali = "";
        public TcpClient tcpclnt = new TcpClient();
    
        public static Activity ActivityCurrent { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
           
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //Huella
            //CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);

            ActivityCurrent = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0));

            LoadApplication(new App());
            InitFirebaseAuth();
            Background();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void InitFirebaseAuth()
        {
            
            var options = new FirebaseOptions.Builder()
            .SetApplicationId("ohmypass-238d0")
            .SetApiKey("AIzaSyBonMHtWUFs3RXpQXA01UW42rP5AP915AM")
            .SetDatabaseUrl("https://ohmypass-238d0-default-rtdb.firebaseio.com/")
            .SetGcmSenderId("964772550633")
            .SetStorageBucket("ohmypass - 238d0.appspot.com")
            .Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options, "OhMyPassWFP");

            

        }

       

        #region Background
        private void Background()
        {
            DependencyService.Get<IServiceBackground>().Start();
        }
        #endregion







    }
}