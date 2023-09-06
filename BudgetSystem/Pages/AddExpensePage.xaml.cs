using BudgetSystem.Models;

namespace BudgetSystem.Pages;

public partial class AddExpensePage : ContentPage
{
	private List<Category> catList;
	private List<string> strCatList;
	private Expense connectedExpense = null;
	private bool isEditing = false;
	private View currentlyFocusedControl;
	public AddExpensePage()
	{
		InitializeComponent();
		SubToFocusControl();
		SetCatList();
	}
	// Overload for opening an already existing expense
    public AddExpensePage(Expense inputExpense)
    {
        InitializeComponent();
        SubToFocusControl();
        SetCatList();
		isEditing = true;

        entryWhere.Text = inputExpense.name;
        entryWhat.Text = inputExpense.details;
        entryCost.Value = inputExpense.cost;
        entryNotes.Text = inputExpense.notes;

		connectedExpense = inputExpense;
        btnRemove.IsVisible = true;
        Task.Run(async () =>
        {
            // Wait for the viewModel.catList to be loaded (adjust the time delay if needed)
            await Task.Delay(TimeSpan.FromSeconds(.5));

            // get index of category
            int ind = strCatList.IndexOf(inputExpense.category);
            // set index of picker
            pickerCat.Dispatcher.Dispatch(() => pickerCat.SelectedIndex = ind);
        });
    }

    private void EntryWhere_Completed(object sender, EventArgs e)
	{
		entryWhat.Focus();
	}

	private void EntryWhat_Completed(object sender, EventArgs e)
	{
		entryCost.Focus();
	}

	private async void BtnSaveOnClicked(object sender, EventArgs e) 
	{
		if (pickerCat.SelectedItem != null)
		{


			double _entryCost = entryCost.Value ?? 0.0;
			if (!isEditing)
			{
				await App.ExpenseRepo.AddNewExpense(new Expense
				{
					name = entryWhere.Text,
					details = entryWhat.Text,
					cost = _entryCost,
					category = pickerCat.SelectedItem.ToString(),
					notes = entryNotes.Text
				});
			}
			else
			{
				await App.ExpenseRepo.ReplaceExpense(connectedExpense, new Expense
				{
					name = entryWhere.Text,
					details = entryWhat.Text,
					cost = _entryCost,
					category = pickerCat.SelectedItem.ToString(),
					notes = entryNotes.Text
				});
			}

			if (currentlyFocusedControl != null)
				currentlyFocusedControl.Unfocus();

			await Navigation.PopAsync();
		}
		else { await Application.Current.MainPage.DisplayAlert("Error", "Please Select a Category", "OK"); }
	}

	private async void BtnRemoveOnClicked(object sender, EventArgs e) 
	{
        await App.ExpenseRepo.RemoveExpense(connectedExpense);
        if (currentlyFocusedControl != null)
            currentlyFocusedControl.Unfocus();
        await Navigation.PopAsync();
    }

	private void SubToFocusControl()
	{
        // Attach the focus event handlers to the controls
        entryWhere.Focused += OnControlFocused;
        entryWhat.Focused += OnControlFocused;
        entryCost.Focused += OnControlFocused;
        pickerCat.Focused += OnControlFocused;
        entryNotes.Focused += OnControlFocused;
    }

	private void OnControlFocused(object sender, FocusEventArgs e)
	{
		currentlyFocusedControl = sender as View;
	}

	public async void SetCatList()
	{
		// Get List of Categories
		catList = await App.CategoryRepo.GetAllCategories();
		// New List of Strings
		strCatList = new List<string>();
		// Convert to List of Strings
		foreach (var cat in catList) 
		{
			strCatList.Add(cat.name);
		}
		// Set as Item Source of Category Picker
		pickerCat.ItemsSource = strCatList;
    }

}