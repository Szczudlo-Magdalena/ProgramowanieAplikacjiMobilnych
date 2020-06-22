using System;
using System.IO;
using System.Reflection;
using AirMonitor.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirMonitor
{
    public partial class App : Application
    {
        internal static string API_URL;
        internal static string API_KEY;

        public App()
        {
            InitializeComponent();
            LoadConfiguration();
            MainPage = new RootTabbedPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void LoadConfiguration()
        {
            Stream stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("AirMonitor.config.json");
            
            using (StreamReader reader = new StreamReader(stream))
            {
                JObject config = JObject.Parse(reader.ReadToEnd());

                API_URL = config.Value<string>("apiurl");
                API_KEY = config.Value<string>("apikey");
            }  
        }
    }
}
