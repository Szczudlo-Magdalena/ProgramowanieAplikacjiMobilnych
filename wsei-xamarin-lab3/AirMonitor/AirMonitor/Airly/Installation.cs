namespace AirMonitor.Airly
{
    public class Installation
    {
        public int Id { get; set; }
        public double Elevation { get; set; }
        public bool Airly { get; set; }
        public Location Location { get; set; }
        public Address Address { get; set; }
    }
}
