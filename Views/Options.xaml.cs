namespace ProjektCrossplatform.Views;

public partial class Options : ContentPage
{
	public Options()
	{
		InitializeComponent();
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
	{
        if (MaleRadioButton.IsChecked)
        {
            Preferences.Set("Gender", "Mê¿czyzna");
        }
        else if (FemaleRadioButton.IsChecked)
        {
            Preferences.Set("Gender", "Kobieta");
        }
        if (int.TryParse(StepLimitEntry.Text, out int stepLimit))
        {
            Preferences.Set("StepLimit", stepLimit);
        }
        if (int.TryParse(CalorieGoalEntry.Text, out int calorieGoal))
        {
            Preferences.Set("CalorieGoal", calorieGoal);
        }
        if (int.TryParse(WaterGoalEntry.Text, out int waterGoal))
        {
            Preferences.Set("WaterGoal", waterGoal);
        }
        Preferences.Set("BirthDate", DatePicker.Date.ToString("yyyy-MM-dd"));
    }
}