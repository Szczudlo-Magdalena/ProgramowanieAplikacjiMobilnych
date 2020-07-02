using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AirMonitor.Models;
using AirMonitor.Views;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace AirMonitor
{
    public partial class App : Application
    {
        public static string AirlyApiKey { get; private set; }
        public static string AirlyApiUrl { get; private set; }
        public static string AirlyApiMeasurementUrl { get; private set; }
        public static string AirlyApiInstallationUrl { get; private set; }
        public static DatabaseHelper Db;

        public App()
        {
            InitializeComponent();

            InitializeApp();
        }

        private async Task InitializeApp()
        {
            Db = new DatabaseHelper();
            await LoadConfig();

            MainPage = new RootTabbedPage();
        }

        private static async Task LoadConfig()
        {
            var assembly = Assembly.GetAssembly(typeof(App));
            var resourceNames = assembly.GetManifestResourceNames();
            var configName = resourceNames.FirstOrDefault(s => s.Contains("config.json"));
            
            using (var stream = assembly.GetManifestResourceStream(configName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var dynamicJson = JObject.Parse(json);

                    AirlyApiKey = dynamicJson["AirlyApiKey"].Value<string>();
                    AirlyApiUrl = dynamicJson["AirlyApiUrl"].Value<string>();
                    AirlyApiMeasurementUrl = dynamicJson["AirlyApiMeasurementUrl"].Value<string>();
                    AirlyApiInstallationUrl = dynamicJson["AirlyApiInstallationUrl"].Value<string>();
                }
            }
        }

        protected override void OnStart()
        {
            if (Db != null)
            {
                Db = new DatabaseHelper();
            }
        }

        protected override void OnSleep()
        {
            Db.Dispose();
            Db = null;
        }

        protected override void OnResume()
        {
            if (Db != null)
            {
                Db = new DatabaseHelper();
            }
        }
    }
}
