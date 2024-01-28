using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektCrossplatform.Services
{
    public class LocalDatabaseService
    {
        private SQLiteAsyncConnection _database;

        // Konstruktor inicjuje połączenie z bazą danych i tworzy tabelę SampleItem, jeśli nie istnieje
        public LocalDatabaseService()
        {
            _database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DietList.db"));
            _database.CreateTableAsync<SampleItem>().Wait();
        }
        // Metoda zwraca listę elementów z bazy danych, posortowaną malejąco według Id
        public async Task<List<SampleItem>> GetItemsAsync()
        {
            return await _database.Table<SampleItem>().OrderByDescending(item => item.Id).ToListAsync();
        }
        // Metoda zapisuje pojedynczy element do bazy danych
        public async void SaveSampleItem(SampleItem item)
        {
            await _database.InsertAsync(item);
        }
        // Metoda zapisuje kolekcję elementów do bazy danych
        public async void SaveItems(ObservableCollection<SampleItem> items)
        {
            foreach (var item in items)
            {
                await _database.InsertAsync(item);
            }
        }
    }
}
