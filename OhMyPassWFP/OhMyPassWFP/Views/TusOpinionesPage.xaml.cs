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
    public partial class TusOpinionesPage : ContentPage
    {
        #region Declaracion de variables y objetos
        private DBFirebase dbFirebase;
        #endregion

        #region Constructor
        public TusOpinionesPage()
        {
            InitializeComponent();
            MostrarOpiniones();

            #region Refrescar datos
            tuOpinionLV.RefreshCommand = new Command(() =>
            {
                MostrarOpiniones();
                tuOpinionLV.IsRefreshing = false;
            });
            #endregion
        }
        #endregion

        #region MostrarOpiniones
        private void MostrarOpiniones()
        {
            //Inicializo la conexion a firebase
            dbFirebase = new DBFirebase();
            //Obtengo el listado de datos del usuario que ha iniciado sesion
            IList<Opiniones> ps = dbFirebase.GetYouOpinions();
            tuOpinionLV.ItemsSource = ps;
        }
        #endregion

        #region Selected & Scrolled & Tapped
        private async void OpSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Opiniones;
            try
            {
                IList<Opiniones> op = dbFirebase.GetYouOpinions();
                for (int i = 0; i < op.Count; i++)
                {
                    if (op[i].Id.Equals(item.Id))
                    {
                        string action = await DisplayActionSheet("Opciones", "Cancelar", null, "Editar", "Eliminar");

                        if (action.Equals("Editar"))
                        {
                            await Navigation.PushAsync(new AddOpinionPage(op[i]));
                        }
                        else if (action.Equals("Eliminar"))
                        {
                            dbFirebase.DeleteOpinion(op[i].Id);
                        }

                        i = op.Count;
                    }
                }
            }
            catch { }            
        }
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