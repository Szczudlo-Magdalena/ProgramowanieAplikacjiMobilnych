using System;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Essentials;

namespace AirMonitor.Models
{
    public class InstallationEntity
    {
        public InstallationEntity() { }

        public InstallationEntity(Installation installation)
        {
            Id = installation.Id;
            Location = JsonConvert.SerializeObject(installation.Location);
            Address = JsonConvert.SerializeObject(installation.Address);
            Elevation = installation.Elevation;
            IsAirlyInstallation = installation.IsAirlyInstallation;
        }

        public Installation ToInstallation()
        {
            return new Installation()
            {
                Id = Id,
                Location = JsonConvert.DeserializeObject<Location>(Location),
                Address = JsonConvert.DeserializeObject<Address>(Address),
                Elevation = Elevation,
                IsAirlyInstallation = IsAirlyInstallation
            };
        }

        [PrimaryKey]
        public string Id { get; set; }

        public string Location { get; set; }
        public string Address { get; set; }
        public double Elevation { get; set; }
        public bool IsAirlyInstallation { get; set; }
    }
}
