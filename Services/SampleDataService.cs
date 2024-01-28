namespace ProjektCrossplatform.Services;

public class SampleDataService
{

    private LocalDatabaseService localDatabaseService;

    // Konstruktor inicjuje serwis lokalnej bazy danych
    public SampleDataService(LocalDatabaseService databaseService)
    {
        localDatabaseService = databaseService;
    }

    // Metoda zwraca kolekcję elementów na podstawie przekazanych parametrów
    // Tworzy nowy element SampleItem na podstawie danych o wodzie, kaloriach, itp.
    // Zapisuje ten element do bazy danych, a następnie pobiera zaktualizowaną listę
    public async Task<IEnumerable<SampleItem>> GetItems(string todayDate, string waterEntryText, string kcalEntryText, string waterGoal, string calorieGoal)
    {
        var newItem = new SampleItem
        {
            Title = $"Woda: {waterEntryText}/{waterGoal} \nKcal: {kcalEntryText}/{calorieGoal} \n",
            Date = $"{todayDate}"
        };

        localDatabaseService.SaveSampleItem(newItem);

        var items = await localDatabaseService.GetItemsAsync();

        return items;
    }
}
