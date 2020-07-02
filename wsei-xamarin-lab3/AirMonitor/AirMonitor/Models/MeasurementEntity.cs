using SQLite;

namespace AirMonitor.Models
{
    public class MeasurementEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int CurrentDisplayValue { get; set; }
        public int Current { get; set; }
        public string Installation { get; set; }
    }
}
