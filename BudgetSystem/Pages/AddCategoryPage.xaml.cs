using BudgetSystem.Models;
using BudgetSystem.Pages;
using CommunityToolkit.Maui.Core.Extensions;
using Syncfusion.Maui.Core;

namespace BudgetSystem.Pages;

public partial class AddCategoryPage : ContentPage
{
	CategoriesPage categoriesPage;
	Color attachedColor;
    public AddCategoryPage()
	{
		InitializeComponent();
        categoriesPage = new CategoriesPage();
        categoriesPage.ListNeedsRefresh += CategoriesPage_ListNeedsRefresh;
        categoriesPage.OnListNeedsRefresh();
    }

	private async void BtnSubmitOnClicked(object sender, EventArgs e)
	{
		if (newCat.Text == "")
		{
            await Application.Current.MainPage.DisplayAlert("Error", "Please Name Your Category", "OK");
        }
		else if (attachedColor == null)
		{
            await Application.Current.MainPage.DisplayAlert("Error", "Please Select a Color for your Category", "OK");
        }
        else
        {
            string c = attachedColor.ToString();
			try
			{
				App.CategoryRepoNA.AddNewCategory(new Category { name = newCat.Text, color = c });
			} 
			catch (Exception ex) 
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			categoriesPage.OnListNeedsRefresh();
		}
    }

	private async void CategoriesPage_ListNeedsRefresh(object sender, EventArgs e) 
	{
		categoriesPage.RefreshList();
		await Navigation.PopAsync();
	}

	private void chipClicked(object sender, EventArgs e)
	{
		var chip = (SfChip)sender;
        attachedColor = chip.BackgroundColor;
		btnSubmit.BackgroundColor = attachedColor;
	}
}