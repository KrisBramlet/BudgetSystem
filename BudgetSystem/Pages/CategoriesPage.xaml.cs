using BudgetSystem.Models;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace BudgetSystem.Pages;

public partial class CategoriesPage : ContentPage
{
	public static ObservableCollection<Category> categories = new ObservableCollection<Category>();
	public CategoriesPage()
	{
		InitializeComponent();
		RefreshList();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		RefreshList();
    }

    private async void RemoveCategoryOnClicked(object sender, EventArgs e) 
	{
		var button = (Button)sender;
		var catToRemove = (Category)button.CommandParameter;
		// Check if Category Has Associated Expenses
		bool expenseFound = false;
		bool confirm = false;
		List<Expense> expenses = await App.ExpenseRepo.GetAllExpenses();
		List<Expense> expensesToRemove = new List<Expense>();
		foreach (Expense expense in expenses)
		{
			if (expense.category == catToRemove.name)
			{
				expenseFound = true;
				expensesToRemove.Add(expense);
			}
		}
		if (expenseFound)
		{
            if (confirm = await Application.Current.MainPage.DisplayAlert("Category Has Expenses", "This category has expenses associated with it. Removing this category will also remove the expenses.", "OK", "Cancel"))
			{
                await App.CategoryRepo.RemoveCategory(catToRemove);
				foreach (Expense expense in expensesToRemove)
				{
					await App.ExpenseRepo.RemoveExpense(expense);
				}
            }
        }

		RefreshList();
	}

	private async void AddCategoryOnClicked(Object sender, EventArgs e) 
	{ 
		await Navigation.PushAsync(new AddCategoryPage());
	}

	public async void RefreshList()
	{
		var newCategories = await App.CategoryRepo.GetAllCategories();
		categories.Clear();
		foreach (var category in newCategories)
		{
			categories.Add(category);
		}
		catList.ItemsSource = categories;
	}

	public event EventHandler ListNeedsRefresh;

	public void OnListNeedsRefresh()
	{
		
			ListNeedsRefresh?.Invoke(this, EventArgs.Empty);
		
	}
}