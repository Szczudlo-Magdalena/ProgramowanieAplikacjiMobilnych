using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AirMonitor.Models
{
    public class DatabaseHelper : IDisposable
    {
        private SQLiteAsyncConnection connection;

        public DatabaseHelper()
        {
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), "airly.db");

            connection = new SQLiteAsyncConnection(path, 
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            InitializeAsync();
        }

        public async Task<List<Installation>> ReadInstallationsAsync()
        {
            List<Installation> installations = new List<Installation>();

            foreach (InstallationEntity entity in 
                await connection.Table<InstallationEntity>().ToListAsync())
            {
                installations.Add(entity.ToInstallation());
            }

            return installations;
        }

        public async Task<List<Measurement>> ReadMeasurementsAsync()
        {
            List<Measurement> measurements = new List<Measurement>();

            foreach (MeasurementEntity measurementEntity in 
                await connection.Table<MeasurementEntity>().ToListAsync())
            {
                Measurement measurement = new Measurement();

                measurement.CurrentDisplayValue = measurementEntity.CurrentDisplayValue;
                MeasurementItemEntity itemEntity = await connection.Table<MeasurementItemEntity>()
                    .Where(item => item.Id == measurementEntity.Current).FirstAsync();
                measurement.Current = itemEntity.ToItem();
                InstallationEntity installationEntity = await connection.Table<InstallationEntity>()
                    .Where(installation => installation.Id == measurementEntity.Installation).FirstAsync();
                measurement.Installation = installationEntity.ToInstallation();

                measurements.Add(measurement);
            }

            return measurements;
        }

        public async Task SaveAsync(IEnumerable<Installation> installations)
        {
            await connection.DeleteAllAsync<InstallationEntity>();

            foreach (Installation installation in installations)
            {
                InstallationEntity entity = new InstallationEntity(installation);

                await connection.InsertAsync(entity);
            }
        }

        public async Task SaveAsync(IEnumerable<Measurement> measurements)
        {
            await connection.RunInTransactionAsync((c) =>
            {
                c.DeleteAll<MeasurementEntity>();
                c.DeleteAll<MeasurementItemEntity>();
                c.DeleteAll<MeasurementValue>();
                c.DeleteAll<AirQualityIndex>();
                c.DeleteAll<AirQualityStandard>();

                foreach (Measurement measurement in measurements)
                {
                    c.InsertAll(measurement.Current.Values, false);
                    c.InsertAll(measurement.Current.Indexes, false);
                    c.InsertAll(measurement.Current.Standards, false);

                    MeasurementItemEntity itemEntity = new MeasurementItemEntity(measurement.Current);
                    c.Insert(itemEntity);

                    MeasurementEntity measurementEntity = new MeasurementEntity()
                    {
                        Current = itemEntity.Id,
                        Installation = measurement.Installation.Id,
                        CurrentDisplayValue = measurement.CurrentDisplayValue
                    };
                    c.Insert(measurementEntity);
                }
            });
        }

        private async Task InitializeAsync()
        {
            await connection.CreateTableAsync<InstallationEntity>();
            await connection.CreateTableAsync<MeasurementEntity>();
            await connection.CreateTableAsync<MeasurementItemEntity>();
            await connection.CreateTableAsync<MeasurementValue>();
            await connection.CreateTableAsync<AirQualityIndex>();
            await connection.CreateTableAsync<AirQualityStandard>();
        }

        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.CloseAsync();
                    connection = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
