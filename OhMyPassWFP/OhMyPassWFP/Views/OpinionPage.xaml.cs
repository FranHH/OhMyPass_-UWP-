using OhMyPassWFP.Models;
using OhMyPassWFP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OhMyPassWFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpinionPage : TabbedPage
    {
        #region Constructor
        public OpinionPage()
        {
            InitializeComponent();
            BarBackgroundColor = Color.DarkGray;
            BarTextColor = Color.Black;
            MostrarOpiniones();

            #region Refrescar datos
            opinionLV.RefreshCommand = new Command(() =>
            {
                MostrarOpiniones();
                opinionLV.IsRefreshing = false;
            });
            #endregion
        }
        #endregion

        #region MostrarOpiniones
        private void MostrarOpiniones()
        {
            //Conexion a firebase
            DBFirebase dbFirebase = new DBFirebase();
            //Obtengo el listado de datos del usuario que ha iniciado sesion
            CalcularMedia(dbFirebase.GetOpinions());   
        }
        #endregion

        #region CalcularMedia
        private void CalcularMedia(IList<Opiniones> op)
        {
            float valor = 0;
            for (int i =0; i<op.Count; i++)
            {
                valor += int.Parse(op[i].Puntuacion);
            }
            float media = valor / op.Count;
            Valoracion.Text = media.ToString()+"/5";

            if (media == 5)//Si la media es maxima, 5 estrellas
            {
                estrella1.Source = "estrella64.png";
                estrella2.Source = "estrella64.png";
                estrella3.Source = "estrella64.png";
                estrella4.Source = "estrella64.png";
                estrella5.Source = "estrella64.png";
            }
            else if (media > 4)//En caso contrario si la media es mayor a 4 e inferior a 5, 4 estrellas y media
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrella64o.png";
                estrella3.Source = "estrella64o.png";
                estrella4.Source = "estrella64o.png";
                estrella5.Source = "estrellamedia64.png";
            }
            else if(media == 4)//En caso contario si la media es 4, 4 estrellas
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrella64o.png";
                estrella3.Source = "estrella64o.png";
                estrella4.Source = "estrella64o.png";
            }
            else if(media > 3)//En caso contrario si la media es mayor a 3 e inferior a 4, 3 estrellas y media
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrella64o.png";
                estrella3.Source = "estrella64o.png";
                estrella4.Source = "estrellamedia64.png";
            }
            else if(media == 3)//En caso contrario si la media es 3, 3 estrellas
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrella64o.png";
                estrella3.Source = "estrella64o.png";
       
            }
            else if(media > 2)//En caso contario si la media es mayor a 2 e inferior a 3, 2 estrellas y media
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrella64o.png";
                estrella3.Source = "estrellamedia64.png";
            }
            else if(media == 2)//En caso contarrio si la media es 2, 2 estrellas
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrella64o.png";
            }
            else if(media >1)//En caso contrario si la media es mayor a 1 e inferior a 2, 1 estrella y media
            {
                estrella1.Source = "estrella64o.png";
                estrella2.Source = "estrellamedia64.png";
            }
            else if(media == 1)//En caso contrario si la media es 1, 1 estrella
            {
                estrella1.Source = "estrella64o.png";
            }
            opinionLV.ItemsSource = op;
        }
        #endregion

        #region Scrolled & Tapped
        private void OnListViewScrolled(object sender, ScrolledEventArgs e)
        {
            ListView listview = new ListView();
            listview.Scrolled += OnListViewScrolled;
        }
        private void MenuTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        #endregion
    }
}