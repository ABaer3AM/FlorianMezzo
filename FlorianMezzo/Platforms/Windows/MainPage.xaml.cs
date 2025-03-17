using FlorianMezzo.Pages;
using System.ComponentModel;
using FlorianMezzo.Constants;
using System.Diagnostics;
using Windows.Devices.Geolocation;

namespace FlorianMezzo
{
    public partial class MainPage : ContentPage
    {

        private async partial void LocatioinPermissionPrompt()
        {
            // attempt to access the location hardware on the device
            var accessStatus = await Geolocator.RequestAccessAsync();
            Debug.WriteLine("Requested Location Access");
        }

    }

}
