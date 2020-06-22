using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirMonitor.Airly
{
    public class AirlyApi
    {
        private string apiUrl;
        private string apiKey;

        public AirlyApi(string apiUrl, string apiKey)
        {
            this.apiUrl = apiUrl;
            this.apiKey = apiKey;
        }

        public async Task<IList<Installation>> GetInstallationsAsync(Location location)
        {
            HttpClient client = CreateHttpClient();
            HttpResponseMessage response = await client.GetAsync(
                $"{apiUrl}installations/nearest?lat={location.Latitude}&lng={location.Longitude}");
            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IList<Installation>>(json);
        }

        public async Task<Measurement> GetMeasurementAsync(Installation installation)
        {
            HttpClient client = CreateHttpClient();
            HttpResponseMessage response = await client.GetAsync(
                $"{apiUrl}measurements/installation?indexType=AIRLY_CAQI&installationId={installation.Id}");
            string json = await response.Content.ReadAsStringAsync();
            Measurement measurement = JsonConvert.DeserializeObject<Measurement>(json);

            measurement.Installation = installation;

            return measurement;
        }

        public async Task<Measurement> GetMeasurementAsync(Location location)
        {
            foreach (Installation installation in await GetInstallationsAsync(location))
            {
                return await GetMeasurementAsync(installation);
            }

            throw new ArgumentException("Can't find installation.");
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Language", "pl");
            client.DefaultRequestHeaders.Add("apikey", apiKey);

            return client;
        }
    }
}
