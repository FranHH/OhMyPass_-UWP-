using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerOhMyPass
{
    public partial class Service1 : ServiceBase
    {
        private string latitud = "", longitud = "", altura = "";
        public Service1()
        {
            InitializeComponent();
        }
        public void OnStart()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
        
            Console.WriteLine("*****************************************************");
            Console.WriteLine("**  Inicio del servicio  **");
            Console.WriteLine("*****************************************************");
            LoadUbicacion();

        }

        protected override void OnStop()
        {
        }


        private void LoadUbicacion()
        {
            Console.WriteLine("Server application");
            int cont = 0;
            while (true)
            {
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

                ASCIIEncoding asen = new ASCIIEncoding();
                s.Send(asen.GetBytes("[Servidor]- Localización recibida."));
                s.Close();
                myList.Stop();
                MessageBox.Show("Ubicación\nLatitud: " + latitud + "\nLongitud: " + longitud);

                //Actualizar(double.Parse(latitud), double.Parse(longitud));

            }
            catch (Exception ex) { }
            }
        }

    }
}
