namespace ProjektCrossplatform.Views;

public partial class ListDetailPage : ContentPage
{
    // Serwis do obsługi danych
    readonly SampleDataService dataService;

    // Kolekcja elementów wyświetlanych na liście
    private ObservableCollection<SampleItem> Items { get; set; }

    public string TodayDate { get; set; }

    public ListDetailPage(SampleDataService service)
    {
        InitializeComponent();

        // Inicjalizacja serwisu danych
        //Działa to na zasadzie ustawienia referencji do obiektu service wewnątrz klasy ListDetailPage. Dzięki temu, klasa ta 
        //może korzystać z funkcji i danych dostarczanych przez ten serwis.
        dataService = service;

        // Ustawienie bieżącego kontekstu danych jako instancji tej strony
        // Pozwala na powiązanie danych z interfejsem użytkownika
        BindingContext = this;
    }

    // Metoda asynchroniczna do wczytania listy
    private async void LoadList()
    {
        // Pobranie bieżącego dnia tygodnia i konwersja na polską nazwę
        var dayOfWeek = DateTime.Now.DayOfWeek.ToString();
        var dayOfWeekPl = ConvertDayOfWeekToPolish(dayOfWeek);

        // Utworzenie tekstowej reprezentacji aktualnej daty
        TodayDate = $"{dayOfWeekPl}, {DateTime.Now.ToString("dd.MM.yyyy")}";

        string waterGoal = "0";
        string calorieGoal = "0";

        // Sprawdzenie, czy istnieją zapisane cele dotyczące wody i kalorii w ustawieniach
        if (Preferences.ContainsKey("WaterGoal"))
        {
            waterGoal = Preferences.Get("WaterGoal", 0).ToString();
        }
        if (Preferences.ContainsKey("CalorieGoal"))
        {
            calorieGoal = Preferences.Get("CalorieGoal", 0).ToString();
        }

        string waterEntryText = WaterEntry.Text;
        string kcalEntryText = KcalEntry.Text;

        // Wczytanie listy elementów z serwisu danych, uwzględniając cele i wprowadzone dane
        Items = new ObservableCollection<SampleItem>(await dataService.GetItems(TodayDate, waterEntryText, kcalEntryText, waterGoal, calorieGoal));
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Wczytanie listy po kliknięciu przycisku
        LoadList();
    }

    // Wywoływane przy pojawieniu się strony
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Wczytanie listy i przypisanie jej do widoku CollectionView
        LoadList();
        collectionview.ItemsSource = Items;
    }

    // Metoda konwertująca nazwę dnia tygodnia na polską
    private string ConvertDayOfWeekToPolish(string dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case "Monday":
                return "Poniedziałek";
            case "Tuesday":
                return "Wtorek";
            case "Wednesday":
                return "Środa";
            case "Thursday":
                return "Czwartek";
            case "Friday":
                return "Piątek";
            case "Saturday":
                return "Sobota";
            case "Sunday":
                return "Niedziela";
            default:
                return dayOfWeek;
        }
    }
}
