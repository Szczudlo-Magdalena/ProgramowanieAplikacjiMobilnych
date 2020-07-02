using Newtonsoft.Json;
using SQLite;
using System;
namespace AirMonitor.Models
{
    public class MeasurementItemEntity
    {
        public MeasurementItemEntity() { }

        public MeasurementItemEntity(MeasurementItem item)
        {
            FromDateTime = item.FromDateTime;
            TillDateTime = item.TillDateTime;
            Values = JsonConvert.SerializeObject(item.Values);
            Indexes = JsonConvert.SerializeObject(item.Indexes);
            Standards = JsonConvert.SerializeObject(item.Standards);
        }

        public MeasurementItem ToItem()
        {
            return new MeasurementItem()
            {
                FromDateTime = FromDateTime,
                TillDateTime = TillDateTime,
                Values = JsonConvert.DeserializeObject<MeasurementValue[]>(Values),
                Indexes = JsonConvert.DeserializeObject<AirQualityIndex[]>(Indexes),
                Standards = JsonConvert.DeserializeObject<AirQualityStandard[]>(Standards)
            };
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public string Values { get; set; }
        public string Indexes { get; set; }
        public string Standards { get; set; }
    }
}
