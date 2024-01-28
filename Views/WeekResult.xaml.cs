using Microcharts;
using SkiaSharp;
using SQLite;
using System;

namespace ProjektCrossplatform.Views
{

    public partial class WeekResult : ContentPage
    {
        // Po³¹czenie z baz¹ danych SQLite
        private SQLiteAsyncConnection database;

        // Tablica obiektów ChartEntry reprezentuj¹cych dane do wyœwietlenia na wykresie
        private ChartEntry[] chartEntriesArray;

        public WeekResult()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeChartData();
            AddStepsToDatabaseFromPreferences();
        }

        // Inicjalizacja bazy danych SQLite
        private async void InitializeDatabase()
        {
            // Utworzenie po³¹czenia z baz¹ danych w folderze aplikacji lokalnej
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Steps.db"));

            // Utworzenie tabeli dla kroków, jeœli nie istnieje
            await database.CreateTableAsync<StepEntryDb>();

            // Iteracja przez dni tygodnia i dodanie wpisów, jeœli nie istniej¹
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek));
            foreach (DayOfWeek day in daysOfWeek)
            {
                string dayString = day.ToString();
                var existingEntry = await database.Table<StepEntryDb>().Where(x => x.DayOfWeek == dayString).FirstOrDefaultAsync();
                if (existingEntry == null)
                {
                    await database.InsertAsync(new StepEntryDb { DayOfWeek = dayString, StepCountDb = 0 });
                }
            }
        }

        // Inicjalizacja danych dla wykresu
        private async void InitializeChartData()
        {
            // Pobranie wszystkich wpisów z bazy danych
            var entries = await database.Table<StepEntryDb>().ToListAsync();

            // Przygotowanie danych dla wykresu
            chartEntriesArray = entries.Select(entry => new ChartEntry(entry.StepCountDb)
            {
                Label = ConvertDayOfWeekToPolish(entry.DayOfWeek),
                ValueLabel = entry.StepCountDb.ToString(),
                Color = SKColor.Parse("#bbbcbd"),
            }).ToArray();

            // Ustawienie danych dla wykresu
            chartViewXaml.Chart = new BarChart { Entries = chartEntriesArray, BackgroundColor = SKColors.Transparent, LabelTextSize = 40f, LabelColor = SKColors.White };
        }

        // Wywo³ywane, gdy strona staje siê widoczna
        protected override async void OnAppearing()
        {
            // Wywo³anie metody bazowej
            // Jest standardow¹ metod¹ cyklu ¿ycia strony (ContentPage), która jest wywo³ywana, gdy strona staje siê widoczna dla u¿ytkownika.
            base.OnAppearing();

            await LoadDataAsync();
        }

        // Za³aduj dane asynchronicznie
        private async Task LoadDataAsync()
        {
            // Pobranie wszystkich wpisów z bazy danych
            var entries = await database.Table<StepEntryDb>().ToListAsync();

            // Konwersja nazw dni tygodnia na polski
            foreach (var entry in entries)
            {
                entry.DayOfWeek = ConvertDayOfWeekToPolish(entry.DayOfWeek);
            }

            // Ustawienie Ÿród³a danych dla widoku kolekcji
            CollectionView.ItemsSource = entries;
        }

        // Dodaj kroki do bazy danych z preferencji u¿ytkownika
        private async void AddStepsToDatabaseFromPreferences()
        {
            // Pobranie liczby kroków z preferencji u¿ytkownika
            int stepsToAdd = Preferences.Get("StepCount", 0);

            // Pobranie dzisiejszej nazwy dnia tygodnia po polsku
            string todayEng = DateTime.Now.DayOfWeek.ToString();
            var today = ConvertDayOfWeekToPolish(todayEng);

            // Pobranie wpisu z bazy danych odpowiadaj¹cego dzisiejszemu dniu
            var todayEntry = await database.Table<StepEntryDb>().Where(x => x.DayOfWeek == today).FirstOrDefaultAsync();

            // Jeœli wpis istnieje, dodaj kroki, zaktualizuj datê i zapisz do bazy danych
            if (todayEntry != null)
            {
                todayEntry.StepCountDb += stepsToAdd;
                todayEntry.LastUpdatedDate = DateTime.Now;
                await database.UpdateAsync(todayEntry);
            }
        }

        // Konwersja nazwy dnia tygodnia na polski
        private string ConvertDayOfWeekToPolish(string todayEng)
        {
            switch (todayEng)
            {
                case "Monday":
                    return "Poniedzia³ek";
                case "Tuesday":
                    return "Wtorek";
                case "Wednesday":
                    return "Œroda";
                case "Thursday":
                    return "Czwartek";
                case "Friday":
                    return "Pi¹tek";
                case "Saturday":
                    return "Sobota";
                case "Sunday":
                    return "Niedziela";
                default:
                    return todayEng;
            }
        }
    }

    // Klasa reprezentuj¹ca wpis kroków w bazie danych
    public class StepEntryDb
    {
        [PrimaryKey]
        public string DayOfWeek { get; set; }
        public int StepCountDb { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
