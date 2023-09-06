using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using BudgetSystem.Models;
using System.Linq.Expressions;

namespace BudgetSystem
{
    public class ExpenseRepositoryNA
    {
        private bool debug = false;

        private string _dbPath;

        private SQLiteConnection conn;

        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Expense>();
        }

        public ExpenseRepositoryNA(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void AddNewExpense(Expense expense)
        {
            int result = 0;
            try
            {
                Init();

                // DATA VALIDATION HERE
                /*if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");*/

                result = conn.Insert(new Expense
                {
                    name = expense.name,
                    details = expense.details,
                    cost = expense.cost,
                    category = expense.category,
                    notes = expense.notes
                });

                if (debug)
                    Application.Current.MainPage.DisplayAlert("Status Message", $"{result} record(s) added (Name: {expense.name})", "OK");
            }
            catch (Exception ex)
            {
                if (debug)
                    Application.Current.MainPage.DisplayAlert("Status Message", $"Failed to add {expense.name}. Error: {ex.Message}", "OK");
            }
        }

        public List<Expense> GetAllExpenses()
        {
            try
            {
                Init();
                return conn.Table<Expense>().ToList();
            }
            catch (Exception e)
            {
                if (debug)
                    Application.Current.MainPage.DisplayAlert("Status Message", $"Failed to retrieve data. {e.Message}", "OK");
            }

            return new List<Expense>();
        }

        public void RemoveExpense(Expense expense)
        {
            try
            {
                if (expense == null || expense.id == 0)
                    throw new Exception("Invalid expense");

                Init();

                int result = conn.Delete(expense);

                if (result == 1)
                    if (debug)
                        Application.Current.MainPage.DisplayAlert("Status Message", "Expense deleted successfully", "OK");
                    else
                    if (debug)
                        Application.Current.MainPage.DisplayAlert("Status Message", "Failed to delete expense", "OK");
            }
            catch (Exception ex)
            {
                if (debug)
                    Application.Current.MainPage.DisplayAlert("Status Message", "Failed to delete expense. " + ex.Message, "OK");
            }
        }

        public void ReplaceExpense(Expense replaceThis, Expense withThis)
        {
            try
            {
                if (replaceThis == null || replaceThis.id == 0)
                    throw new Exception("Invalid expense to replace");

                if (withThis == null || string.IsNullOrEmpty(withThis.name))
                    throw new Exception("Invalid expense data for replacement");

                Init();

                withThis.id = replaceThis.id; // Preserve the existing primary key

                int result = conn.Update(withThis);

                if (result == 1)
                    if (debug)
                        Application.Current.MainPage.DisplayAlert("Status Message", "Expense replaced successfully", "OK");
                    else
                    if (debug)
                        Application.Current.MainPage.DisplayAlert("Status Message", "Failed to replace expense", "OK");
            }
            catch (Exception ex)
            {
                if (debug)
                    Application.Current.MainPage.DisplayAlert("Status Message", "Failed to replace expense. " + ex.Message, "OK");
            }
        }

        public List<Expense> GetExpensesByCategory(string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category))
                    throw new Exception("Invalid category");

                Init();

                // Use LINQ to query the expenses with the specified category
                var expenses = conn.Table<Expense>().Where(e => e.category == category).ToList();

                return expenses;
            }
            catch (Exception ex)
            {
                if (debug)
                    Application.Current.MainPage.DisplayAlert("Status Message", "Failed to retrieve expenses by category. " + ex.Message, "OK");
            }

            return new List<Expense>();
        }

        public List<Expense> SortExpensesByProperty(string propertyName, bool isAscending = true)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                    throw new Exception("Invalid property name");

                Init();

                // Use LINQ to dynamically build a sorting expression based on the property name
                var parameter = Expression.Parameter(typeof(Expense), "e");
                var property = Expression.Property(parameter, propertyName);
                var lambda = Expression.Lambda<Func<Expense, object>>(Expression.Convert(property, typeof(object)), parameter);

                // Perform the sorting
                var expenses = isAscending
                    ? conn.Table<Expense>().OrderBy((Expression<Func<Expense, object>>)lambda).ToList()
                    : conn.Table<Expense>().OrderByDescending((Expression<Func<Expense, object>>)lambda).ToList();

                return expenses;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Status Message", "Failed to sort expenses. " + ex.Message, "OK");
            }

            return new List<Expense>();
        }
    }
}
