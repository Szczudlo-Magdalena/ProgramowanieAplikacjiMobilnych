using System;
using System.Windows.Input;
using AirMonitor.Airly;
using AirMonitor.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AirMonitor.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;

        public HomeViewModel(INavigation navigation)
        {
            _navigation = navigation;
            InitializeMeasurement();
        }

        private ICommand _goToDetailsCommand;
        public ICommand GoToDetailsCommand => _goToDetailsCommand ?? (_goToDetailsCommand = new Command(OnGoToDetails));

        private Measurement measurement;
        public Measurement Measurement 
        { 
            get => measurement;
            set => SetProperty(ref measurement, value); 
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        // 'parameter' -> element kliknięty na liście pomiarów
        private void OnGoToDetails(object parameter)
        {
            _navigation.PushAsync(new DetailsPage((MeasurementItem)parameter));
        }

        private async void InitializeMeasurement()
        {
            IsLoading = true;
            AirlyApi api = new AirlyApi(App.API_URL, App.API_KEY);

            Xamarin.Essentials.Location last = await Geolocation.GetLastKnownLocationAsync();
            Airly.Location location = new Airly.Location()
            {
                //Latitude = last.Latitude,
                //Longitude = last.Longitude
                Latitude = 50.062006,
                Longitude = 19.940984
            };

            Measurement = await api.GetMeasurementAsync(location);
            IsLoading = false;
        }
    }
}
