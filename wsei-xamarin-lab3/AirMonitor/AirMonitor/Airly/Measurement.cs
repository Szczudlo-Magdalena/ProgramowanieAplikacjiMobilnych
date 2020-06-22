using System.Collections.Generic;

namespace AirMonitor.Airly
{
    public class Measurement
    {
        public MeasurementItem Current { get; set; }
        public IList<MeasurementItem> History { get; set; }
        public IList<MeasurementItem> Forecast { get; set; }
        public Installation Installation { get; set; }
    }
}
