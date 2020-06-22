using System;
using System.Collections.Generic;
using System.Text;

namespace AirMonitor.Airly
{
    public class MeasurementItem
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public IList<AirQualityValue> Values { get; set; }
        public IList<AirQualityIndex> Indexes { get; set; }
        public IList<AirQualityStandard> Standards { get; set; }

        public double Caqi => (double)Indexes[0].Value;
        public string Description => Indexes[0].Description;
        public string Color => Indexes[0].Color;
        public double Pm25 => GetValue("PM25");
        public double Pm25Percent => GetValue("PM25") * 100;
        public double Pm10 => GetValue("PM10");
        public double Pm10Percent => Pm10 * 100;
        public double Humidity => GetValue("HUMIDITY") / 100.0;
        public double Pressure => GetValue("PRESSURE");

        private double GetValue(string key)
        {
            foreach (AirQualityValue value in Values)
            {
                if (value.Name == key)
                {
                    return value.Value;
                }
            }

            return 0.0;
        }
    }
}
