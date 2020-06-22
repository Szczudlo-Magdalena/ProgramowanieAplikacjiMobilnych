using AirMonitor.ViewModels;
using Xamarin.Forms;

namespace AirMonitor.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            BindingContext = new HomeViewModel(Navigation);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((HomeViewModel)BindingContext).GoToDetailsCommand.Execute(e.Item);
        }
    }
}
