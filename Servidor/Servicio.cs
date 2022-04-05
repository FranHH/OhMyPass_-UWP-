using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;


namespace Servidor
{
    public partial class Servicio : Form
    {
        private string latitud = "", longitud = "", altura = "";
        bool conectado = false;
  
        public Servicio()
        {
            InitializeComponent();
        }

        /*
        private void Servicio_Load(object sender, EventArgs e)
        {
            gmap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.position
            gmap.SetPositionByKeywords("37.88541, -4.79200");

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            while (!conectado)
            {
                try
                {
                    IPAddress ipAd = IPAddress.Any;

                    TcpListener myList = new TcpListener(ipAd, 8001);
                    myList.Start();
                    Console.WriteLine("Servidor corriendo en el puerto 8001");
                    Console.WriteLine("Esperando conexion...");
                    Socket s = myList.AcceptSocket();
                    Console.WriteLine("Conexion recivida desde: " + s.RemoteEndPoint);
                    byte[] b = new byte[100];
                    int k = s.Receive(b);
                    Console.WriteLine("Receive...");
                    string cadena = "";
                    for (int i = 0; i < k; i++)
                    {
                        cadena = cadena + Convert.ToChar(b[i]);
                    }
                    string aux = "";

                    for (int i = 0; i < cadena.Length; i++)
                    {

                        aux += cadena[i].ToString();

                        if (aux.Equals("Latitud:"))
                        {
                            aux = "";
                            for (int j = i + 1; j < cadena.Length; j++)
                            {
                                latitud += cadena[j].ToString();

                                if (cadena[j].ToString().Equals("\n"))
                                {
                                    latitud = latitud.Replace("\n", "");
                                    i = j;
                                    j = cadena.Length;
                                }
                            }
                        }
                        else if (aux.Equals("Longitud:"))
                        {
                            aux = "";
                            for (int j = i + 1; j < cadena.Length; j++)
                            {
                                longitud += cadena[j].ToString();

                                if (cadena[j].ToString().Equals("\n"))
                                {
                                    longitud = longitud.Replace("\n", "");
                                    i = j;
                                    j = cadena.Length;
                                }
                            }
                        }
                        else if (aux.Equals("Altitud:"))
                        {
                            aux = "";
                            for (int j = i + 1; j < cadena.Length; j++)
                            {
                                altura += cadena[j].ToString();

                                if (cadena[j].ToString().Equals("\n"))
                                {
                                    altura = altura.Replace("\n", "");
                                    i = j;
                                    j = cadena.Length;
                                }
                            }
                        }
                    }

                    
                    string conectSQL = @"Server=.\sqlexp;database=Northwnd;" + " Integrated Security=TRUE";
                    SqlConnection cm = new SqlConnection();
                    cm.ConnectionString = conectSQL;
                    cm.Open();

                    SqlCommand cmd = new SqlCommand(cadena, cm);
                    cmd.ExecuteNonQuery();
                    cm.Close();
                    

                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes("Cadena recibida. Comando ejecutado"));
                    Console.WriteLine("\nAcuse enviado");
                    s.Close();
                    myList.Stop();
                    conectado = true;
                    Actualizar(int.Parse(latitud), int.Parse(longitud));

                }
                catch (Exception) { }
            }
        }

        private void Actualizar(double latitud, double longitud)
        {
          
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(latitud, longitud),GMarkerGoogleType.green);
            markersOverlay.Markers.Add(marker);
            gmap.Overlays.Add(markersOverlay);
        }

        */
        private void LoadUbicacion()
        {
            Console.WriteLine("Server application");
            int cont = 0;
            //while (true)
            //{
                try
                {
                    IPAddress ipAd = IPAddress.Any;

                    TcpListener myList = new TcpListener(ipAd, 8001);
                    myList.Start();
                    Console.WriteLine("[Servidor]- Servidor corriendo en el puerto 8001");
                    Console.WriteLine("[Servidor]- Esperando conexion...");
                    Socket s = myList.AcceptSocket();
                    Console.WriteLine("[Servidor]- Conexion recivida desde: " + s.RemoteEndPoint);
                    byte[] b = new byte[100];
                    int k = s.Receive(b);
                    string cadena = "";
                    for (int i = 0; i < k; i++)
                    {
                        cadena = cadena + Convert.ToChar(b[i]);
                    }
                    string aux = "";

                    for (int i=0; i< cadena.Length; i++)
                    {
                        
                        aux += cadena[i].ToString();

                        if (aux.Equals("Latitud:"))
                        {
                            aux = "";
                            for(int j=i+1; j < cadena.Length; j++)
                            {
                                latitud += cadena[j].ToString();
                               
                                if (cadena[j].ToString().Equals("\n"))
                                {
                                    latitud = latitud.Replace("\n", "");
                                    i = j;
                                    j = cadena.Length;
                                }
                            }
                        }
                        else if(aux.Equals("Longitud:"))
                        {
                            aux = "";
                            for (int j = i + 1; j < cadena.Length; j++)
                            {
                                longitud += cadena[j].ToString();
                                
                                if (cadena[j].ToString().Equals("\n"))
                                {
                                    longitud = longitud.Replace("\n", "");
                                    i = j;
                                    j = cadena.Length;
                                }
                            }
                        }
                        else if (aux.Equals("Altitud:"))
                        {
                            aux = "";
                            for (int j = i + 1; j < cadena.Length; j++)
                            {
                                altura += cadena[j].ToString();
                               
                                if (cadena[j].ToString().Equals("\n"))
                                {
                                    altura = altura.Replace("\n", "");
                                    i = j;
                                    j = cadena.Length;
                                }
                            }
                        }
                    }

                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes("[Servidor]- Localización recibida."));
                    s.Close();
                    myList.Stop();
                    conectado = true;

                    MessageBox.Show("Ubicación\nLatitud: " + latitud + "\nLongitud: " + longitud);

                    //Actualizar(double.Parse(latitud), double.Parse(longitud));

                }
                catch (Exception ex) { }
            //}
        }
    }
}
