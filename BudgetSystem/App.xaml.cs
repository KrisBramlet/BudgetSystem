using System.Collections.ObjectModel;
using BudgetSystem.Models;
using BudgetSystem.ViewModels;
using Syncfusion.Maui.DataSource.Extensions;

namespace BudgetSystem;

public partial class App : Application
{
	public static ExpenseRepository ExpenseRepo { get; set; }
    public static ExpenseRepositoryNA ExpenseRepoNA { get; set; }
    public static CategoryRepository CategoryRepo { get; set; }
	public static CategoryRepositoryNA CategoryRepoNA { get; set; }	
	
	public App(ExpenseRepository expRepo,ExpenseRepositoryNA expRepoNA,CategoryRepository catRepo, CategoryRepositoryNA catRepoNA)
	{
		InitializeComponent();

		MainPage = new AppShell();

		ExpenseRepo = expRepo;
		ExpenseRepoNA = expRepoNA;
		CategoryRepo = catRepo;
		CategoryRepoNA = catRepoNA; 
    }
    

}
