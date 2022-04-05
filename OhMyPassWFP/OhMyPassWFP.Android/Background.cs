using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OhMyPass.models;
using OhMyPassWFP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(OhMyPassWFP.Droid.Background))]
namespace OhMyPassWFP.Droid
{    
    [Service(ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
    public class Background : Service, IServiceBackground
    {
        public string locali = "";
        public TcpClient tcpclnt = new TcpClient();
        public CancellationTokenSource cts;
        DBFirebase dbFirebase;
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Se ha iniciado el servicio");
                RegisterNotification();
                Task<IList<double>> listaLocation = GetLocation();
                dbFirebase = new DBFirebase();
                
                if (new CTask().ReadText("ID") == null){

                    DependencyService.Get<IServiceBackground>().Stop();
                }
                else
                {
                    //Inicio obtencion de datos
                    dbFirebase.AddDataLocation(listaLocation);






                }
                
            }
            else if (intent.Action == "STOP_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Se ha detenido el servicio");
                StopForeground(true);
                StopSelfResult(startId);
            }
            return StartCommandResult.NotSticky;
        }

        public void Start()
        {
            Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(Background));
            startService.SetAction("START_SERVICE");
            MainActivity.ActivityCurrent.StartService(startService);
        }
        public void Stop()
        {
            Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
            stopIntent.SetAction("STOP_SERVICE");
            MainActivity.ActivityCurrent.StartService(stopIntent);
        }
        private void RegisterNotification()
        {
            NotificationChannel channel = new NotificationChannel("ServicioChannel","OhMyPass Services", NotificationImportance.None);
            
            NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(channel);

            Notification notification = new Notification.Builder(this, "ServicioChannel").SetContentTitle("Servicio iniciado").SetSmallIcon(Resource.Drawable.abc_item_background_holo_light).SetOngoing(true).Build();
            
            StartForeground(50, notification);
            StopForeground(true);
        }

        #region Cliente
        private async void LoadCliente()
        {
            //IpCliente
            IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addr = ip.AddressList;

            Console.WriteLine("Client Application");
       
            try
            {
                //Utilizar ip local ya que cliente y servidor corren en el mismo pc

                tcpclnt.Connect("192.168.100.157", 8001);
                Console.WriteLine("[Cliente]- Conectado con el servidor");

                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                Console.WriteLine("[Cliente]- Obteniendo localización...");
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine("[Cliente]- Localizado");
                    locali = "Latitud:" + location.Latitude + "\nLongitud:" + location.Longitude + "\nAltitud:" + location.Altitude;
                    Stream _stream = tcpclnt.GetStream();
                    ASCIIEncoding _ascii = new ASCIIEncoding();

                    byte[] bLatitude = _ascii.GetBytes(locali);
                    Console.WriteLine("[Cliente]- Transmitiendo localización...");
                    _stream.Write(bLatitude, 0, bLatitude.Length);
                    byte[] bb = new byte[100];
                    int k = _stream.Read(bb, 0, 100);
                    string acuse = "";
                    for (int i = 0; i < k; i++)
                    {
                        acuse = acuse + Convert.ToChar(bb[i]);
                    }
                    Console.WriteLine(acuse);
                    //tcpclnt.Close();
                }
            }
            catch (Exception) { }
  
        }

        #endregion

        #region GetLocation
        private async Task<IList<double>> GetLocation()
        {
            var lastLocation = await Geolocation.GetLastKnownLocationAsync();
            IList<double> listLocation = new List<double>();

            if (lastLocation != null)
            {
                listLocation.Add(lastLocation.Latitude);
                listLocation.Add(lastLocation.Longitude);
                listLocation.Add((double)lastLocation.Altitude);

                return listLocation;
            }
            else
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if(location != null)
                {
                    listLocation.Add(location.Latitude);
                    listLocation.Add(location.Longitude);
                    listLocation.Add((double)location.Altitude);

                    return listLocation;
                }
            }
            return null;
        }
        #endregion

        #region Captura
        private async Task CaptureScreenshot()
        {
            var screenshot = await Screenshot.CaptureAsync();
            var stream = await screenshot.OpenReadAsync();
            ImageSource.FromStream(() => stream);
            Thread.Sleep(999);
        }
        #endregion

        #region CopyData
        private void CopyData()
        {

            var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
                      
            Console.WriteLine(filePath);
        }
        #endregion

    }
}