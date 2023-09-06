namespace BudgetSystem.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void btnClearExpenseFileOnClicked(object sender, EventArgs e)
    {
		File.Delete(FileAccessHelper.GetLocalFilePath("Expenses.db3"));
    }
}