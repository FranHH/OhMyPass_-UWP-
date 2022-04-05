using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Com.Airbnb.Lottie;
using System;
using System.Threading.Tasks;

namespace OhMyPassWFP.Droid
{
    [Activity(  Label = "OhMyPass", 
                Theme = "@style/SplashTheme",
                MainLauncher =true, 
                NoHistory =true, 
                ConfigurationChanges =Android.Content.PM.ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {
        LottieAnimationView animationView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SplashScreen);

        }


        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() => { SimulateStartupAsync(); });
            startupWork.Start();
        }

        private async void SimulateStartupAsync()
        { 
            await Task.Delay(3000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
          
    }
}