using FlorianMezzo.Pages;
using System.ComponentModel;
using FlorianMezzo.Constants;
using System.Diagnostics;

namespace FlorianMezzo
{
    public partial class MainPage : ContentPage
    {

        private partial void LocatioinPermissionPrompt()
        {
            Debug.WriteLine("This is where Location permissions are requested");
        }

    }

}
