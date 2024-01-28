using Microsoft.VisualBasic.FileIO;

namespace ProjektCrossplatform.Views;

public partial class Calculator : ContentPage
{
    string genderType;
    double ActivityMultiplier;
    int age;
	public Calculator()
	{
		InitializeComponent();
        SetDefaultValuesFromPreferences();
    }
    private void SetDefaultValuesFromPreferences()
    {
        if (int.TryParse(HeightEntry.Text, out int height))
        {
            Preferences.Set("Height", height);
        }
        // Sprawdzenie, czy w ustawieniach aplikacji istnieje zapisana data urodzenia
        if (DateTime.TryParse(Preferences.Get("BirthDate", string.Empty), out DateTime birthDate))
        {
            // Obliczenie wieku na podstawie aktualnej daty i daty urodzenia
            int age = DateTime.Today.Year - birthDate.Year;

            // Korekta wieku, jeúli urodziny jeszcze nie nadesz≥y w bieøπcym roku
            if (birthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            // Zapisanie obliczonego wieku w ustawieniach aplikacji
            Preferences.Set("Age", age);
        }
        if (int.TryParse(WeightEntry.Text, out int weight))
        {
            Preferences.Set("Weight", weight);
        }
        if (Preferences.ContainsKey("Gender"))
        {
            var gender = Preferences.Get("Gender", string.Empty);
            if (gender == "MÍøczyzna")
                genderType = "M";
            else
                genderType = "F";
        }
        string selectedActivity = ActivityLevelPicker.SelectedItem?.ToString();
        Preferences.Set("SelectedActivityLevel", selectedActivity);
        if (selectedActivity == "Niska aktywnoúÊ fizyczna")
            ActivityMultiplier = 1.37;
        else if (selectedActivity == "Umiarkowana aktywnoúÊ fizyczna")
            ActivityMultiplier = 1.55;
        else if (selectedActivity == "Wysoka aktywnoúÊ fizyczna")
            ActivityMultiplier = 1.72;
        else if (selectedActivity == "Bardzo wysoka aktywnoúÊ fizyczna")
            ActivityMultiplier = 1.9;
    }
    private void CalculateBMRButton_Clicked(object sender, EventArgs e)
    {
        SetDefaultValuesFromPreferences();
        if (int.TryParse(WeightEntry.Text, out int weight) &&
            int.TryParse(HeightEntry.Text, out int height))
        {
            double ppm;
            double cpm;
            int waterNeed;
            double bmi;
            if (genderType=="M")
            {
                ppm = (10 * weight) + (6.25 * height) + (5 * age) + 5;
                cpm = ppm * ActivityMultiplier;
            }
            else if (genderType == "F")
            {
                ppm = (10 * weight) + (6.25 * height) + (5 * age) - 161;
                cpm = ppm * ActivityMultiplier;
            }
            else
            {
                DisplayAlert("B≥πd", "èle uzup≥enione dane", "OK");
                return;
            }
            waterNeed = weight * 30;
            bmi = weight / ((height / 100.0) * (height / 100.0));
            ppmLabel.Text = $"{ppm.ToString("F2")}\n";
            cpmLabel.Text = $"{cpm.ToString("F2")}\n";
            waterLabel.Text = $"{waterNeed}\n";
            bmiLabel.Text = $"{bmi.ToString("F2")}";
        }
    }
}