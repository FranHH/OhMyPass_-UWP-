using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using OhMyPassWFP.Views;

namespace OhMyPassWFP.Services
{
    public class Login : CarouselPage
    {
        public ContentPage login;

        public Login(ILoginManager ilm)
        {
            login = new LoginPageKey(ilm);

            this.Children.Add(login);
            MessagingCenter.Subscribe<ContentPage>(this, "Login", (sender) =>
              {
                  this.SelectedItem = login;
              });
        }

    }
}
