using ProjektCrossplatform.Views;

namespace ProjektCrossplatform.Views
{
    // Mechanizm powiadamiania o zmianie właściwości
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private int stepCount;
        private int stepLimit;

        public MainPage()
        {
            // Inicjalizacja komponentów interfejsu użytkownika
            InitializeComponent();

            // Ustawienie kontekstu wiązania na obiekt klasy MainPage
            // Pozwala na powiązanie danych z interfejsem użytkownika
            BindingContext = this;

            // Rejestracja zdarzenia zmiany odczytu akcelerometru
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;

            // Rozpoczęcie monitorowania akcelerometru
            Accelerometer.Start(SensorSpeed.UI);

            LoadPreferences();
        }

        private void LoadPreferences()
        {
            if (Preferences.ContainsKey("StepCount"))
                StepCount = Preferences.Get("StepCount", 0);

            if (Preferences.ContainsKey("StepLimit"))
                StepLimit = Preferences.Get("StepLimit", 0);
        }

        // Właściwość StepCount z mechanizmem powiadamiania o zmianie
        public int StepCount
        {
            get => stepCount;
            set
            {
                if (stepCount != value)
                {
                    // Aktualizacja wartości krokomierza
                    stepCount = value;

                    // Powiadomienie o zmianie wartości StepCount
                    OnPropertyChanged(nameof(StepCount));

                    // Zapisanie aktualnej wartości do preferencji
                    Preferences.Set("StepCount", stepCount);

                    // Powiadomienie o zmianie wartości procentowej postępu
                    OnPropertyChanged(nameof(ProgressPercentage));
                }
            }
        }

        // Właściwość StepLimit z mechanizmem powiadamiania o zmianie
        public int StepLimit
        {
            get => stepLimit;
            set
            {
                if (stepLimit != value)
                {
                    // Aktualizacja wartości limitu kroków
                    stepLimit = value;

                    // Powiadomienie o zmianie wartości StepLimit
                    OnPropertyChanged(nameof(StepLimit));

                    // Zapisanie aktualnej wartości do preferencji
                    Preferences.Set("StepLimit", stepLimit);

                    // Powiadomienie o zmianie wartości procentowej postępu
                    OnPropertyChanged(nameof(ProgressPercentage));
                }
            }
        }

        // Właściwość obliczająca procentowy postęp na podstawie ilości kroków i limitu kroków
        public double ProgressPercentage => Convert.ToDouble(StepCount) / Convert.ToDouble(StepLimit);

        // Obsługa zdarzenia zmiany odczytu akcelerometru
        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            // "sender" wskazuje na obiekt, który zgłosił zdarzenie (czyli akcelerometr w tym przypadku)
            // "e" zawiera informacje o zmianie odczytu akcelerometru, takie jak wartości przyspieszenia

            // Obliczenie długości wektora przyspieszenia
            double length = Math.Sqrt(e.Reading.Acceleration.X * e.Reading.Acceleration.X + e.Reading.Acceleration.Y * e.Reading.Acceleration.Y + e.Reading.Acceleration.Z * e.Reading.Acceleration.Z);
            //Powyższe równanie to trójwymiarowa odległość euklidesowa, czyli długość odcinka łączącego punkt o współrzędnych (X, Y, Z) z początkiem układu współrzędnych (0, 0, 0).
            //W kontekście akcelerometru, wartości X Y Z reprezentują przyspieszenie wzdłuż trzech osi.

            // Jeżeli długość przekracza próg, zwiększ liczbę kroków
            if (length >= 2)
                StepCount += 1;
        }

        // Metoda wywoływana przy zanikaniu strony
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Wyrejestrowanie zdarzenia zmiany odczytu akcelerometru (opcjonalne)
            //Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
        }

        // Metoda wywoływana przy pojawianiu się strony
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Wczytanie preferencji
            LoadPreferences();

            // Ustawienie tekstu celu kroków na interfejsie użytkownika
            goalStep.Text = StepLimit.ToString();

            // Ustawienie tekstu liczby kroków na interfejsie użytkownika
            stepCountLabel.Text = StepCount.ToString();
        }
    }
}
