using SQLite;

namespace ProjektCrossplatform.Models;

public class SampleItem
{
    // SampleItem to model reprezentujący pojedynczą pozycję w bazie danych
    [PrimaryKey,AutoIncrement]
	public int Id { get; set; }
	public string Title { get; set; }

	public string Date { get; set; }
}
