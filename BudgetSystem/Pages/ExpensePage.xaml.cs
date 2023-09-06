using Microsoft.Maui.Controls;

using BudgetSystem.Models;

namespace BudgetSystem.Pages;

public partial class ExpensePage : ContentPage
{
	public static List<Expense> expenseList;
    private List<Category> catList;
    private List<string> strCatList;
    public ExpensePage()
	{
		InitializeComponent();
		RefreshList();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshList();
    }

    private async void OnExpenseTapped(object sender, EventArgs e) 
	{
        var button = (Button)sender;
		var expToOpen = (Expense)button.CommandParameter;

		await Navigation.PushAsync(new AddExpensePage(expToOpen));
    }


    private async void BtnAddOnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddExpensePage());
	}

	private async void FSOnClicked(object sender, EventArgs e) 
    {
        if (chkSort.IsChecked)
        {
            if(pickerSortBy != null)
            {
                expenseList = await App.ExpenseRepo.SortExpensesByProperty(pickerSortBy.SelectedItem.ToString(),true);
                cvExpenseList.ItemsSource = expenseList;
            }
        }
        else if (chkFilter.IsChecked)
        {
            if (pickerFilterBy.SelectedItem != null)
            {
                if (pickerFilterBy.SelectedItem.ToString().Equals("All"))
                {
                    RefreshList();
                }
                else
                {
                    expenseList = await App.ExpenseRepo.GetExpensesByCategory(pickerFilterBy.SelectedItem.ToString());
                    cvExpenseList.ItemsSource = expenseList;
                }
            }
        }
        //Set add expense btn visible
        btnAdd.IsVisible = true;
        // Hide Anything else still standing from filter/sort
        btnFilter.IsVisible = false;
        btnSort.IsVisible = false;
        chkFilter.IsChecked = false;
        pickerFilterBy.IsVisible = false;
        chkSort.IsChecked = false;
        pickerSortBy.IsVisible = false;
    }

	private void ChkSortOnCheckedChanged(object sender, EventArgs e) 
    {
       if (chkSort.IsChecked) // if checked
       {
            if(chkFilter.IsChecked) // if filter checked
            {
                chkFilter.IsChecked = false; // un check filter
            }
            //Set Visibilities
            pickerSortBy.IsVisible = true;
            btnSort.IsVisible = true;
            btnAdd.IsVisible = false;
            btnFilter.IsVisible=false;
       }
        else // not checked
        {
            if (!chkFilter.IsChecked) // neither checked
            {
                btnAdd.IsVisible = true;
                btnFilter.IsVisible = false;
                btnSort.IsVisible = false;
            }
            pickerSortBy.IsVisible = false;
        }
    }

	private void ChkFilterOnCheckedChanged(Object sender, EventArgs e) 
    {
        if (chkFilter.IsChecked) // if checked
        {
            if (chkSort.IsChecked) // if sort checked
            {
                chkSort.IsChecked = false; // un check sort
            }
            // Set Visibilities
            pickerFilterBy.IsVisible = true;
            btnFilter.IsVisible = true;
            btnAdd.IsVisible = false;
            btnSort.IsVisible = false;
        }
        else // if not checked
        {
            if (!chkSort.IsChecked) // neither checked 
            {
                btnAdd.IsVisible = true;
                btnFilter.IsVisible= false;
                btnSort.IsVisible = false;
            }

            pickerFilterBy.IsVisible = false;
        }
    }

	public async void RefreshList()
	{
		expenseList = await App.ExpenseRepo.GetAllExpenses();
		cvExpenseList.ItemsSource = expenseList;

        // Set Category List for Filter By Picker
        // Get List of Categories
        catList = await App.CategoryRepo.GetAllCategories();
        // New List of Strings
        strCatList = new List<string>();
        // Convert to List of Strings
        foreach (var cat in catList)
        {
            strCatList.Add(cat.name);
        }
        // Add "All" option to bottom
        strCatList.Add("All");
        // Set as Item Source of Category Picker
        pickerFilterBy.ItemsSource = strCatList;
    }

    
}