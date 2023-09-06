using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using BudgetSystem.Models;
using System.Linq.Expressions;

namespace BudgetSystem
{
    public class ExpenseRepository
    {
        private bool debug = false;

        string _dbPath;

        private SQLiteAsyncConnection conn;

        async Task Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Expense>();
        }

        public ExpenseRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task AddNewExpense(Expense expense)
        {
            int result = 0;
            try
            {
                await Init();

                // DATA VALIDATION HERE
                /*if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");*/

                result = await conn.InsertAsync(new Expense
                {
                    name = expense.name,
                    details = expense.details,
                    cost = expense.cost,
                    category = expense.category,
                    notes = expense.notes
                });

                if (debug)
                    await Application.Current.MainPage.DisplayAlert("Status Message", $"{result} record(s) added (Name: {expense.name})", "OK");
            }
            catch (Exception ex)
            {
                if (debug)
                    await Application.Current.MainPage.DisplayAlert("Status Message", $"Failed to add {expense.name}. Error: {ex.Message}", "OK");
            }
        }

        public async Task<List<Expense>> GetAllExpenses()
        {
            try
            {
                await Init();
                return await conn.Table<Expense>().ToListAsync();
            }
            catch (Exception e)
            {
                if (debug)
                    await Application.Current.MainPage.DisplayAlert("Status Message", $"Failed to retrieve data. {e.Message}", "OK");
            }

            return new List<Expense>();
        }

        public async Task RemoveExpense(Expense expense)
        {
            try
            {
                if (expense == null || expense.id == 0)
                    throw new Exception("Invalid expense");

                await Init();

                int result = await conn.DeleteAsync(expense);

                if (result == 1)
                    if (debug)
                        await Application.Current.MainPage.DisplayAlert("Status Message", "Expense deleted successfully", "OK");
                else
                    if (debug)
                        await Application.Current.MainPage.DisplayAlert("Status Message", "Failed to delete expense", "OK");
            }
            catch (Exception ex)
            {
                if (debug)
                    await Application.Current.MainPage.DisplayAlert("Status Message", "Failed to delete expense. " + ex.Message, "OK");
            }
        }

        public async Task ReplaceExpense(Expense replaceThis, Expense withThis)
        {
            try
            {
                if (replaceThis == null || replaceThis.id == 0)
                    throw new Exception("Invalid expense to replace");

                if (withThis == null || string.IsNullOrEmpty(withThis.name))
                    throw new Exception("Invalid expense data for replacement");

                await Init();

                withThis.id = replaceThis.id; // Preserve the existing primary key

                int result = await conn.UpdateAsync(withThis);

                if (result == 1)
                    if (debug)
                        await Application.Current.MainPage.DisplayAlert("Status Message", "Expense replaced successfully", "OK");
                else
                    if (debug)
                        await Application.Current.MainPage.DisplayAlert("Status Message", "Failed to replace expense", "OK");
            }
            catch (Exception ex)
            {
                if (debug)
                    await Application.Current.MainPage.DisplayAlert("Status Message", "Failed to replace expense. " + ex.Message, "OK");
            }
        }

        public async Task<List<Expense>> GetExpensesByCategory(string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category))
                    throw new Exception("Invalid category");

                await Init();

                // Use LINQ to query the expenses with the specified category
                var expenses = await conn.Table<Expense>().Where(e => e.category == category).ToListAsync();

                return expenses;
            }
            catch (Exception ex)
            {
                if (debug)
                    await Application.Current.MainPage.DisplayAlert("Status Message", "Failed to retrieve expenses by category. " + ex.Message, "OK");
            }

            return new List<Expense>();
        }


        public async Task<List<Expense>> SortExpensesByProperty(string propertyName, bool isAscending = true)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                    throw new Exception("Invalid property name");

                await Init();

                // Use LINQ to dynamically build a sorting expression based on the property name
                var parameter = Expression.Parameter(typeof(Expense), "e");
                var property = Expression.Property(parameter, propertyName);
                var lambda = Expression.Lambda<Func<Expense, object>>(Expression.Convert(property, typeof(object)), parameter);

                // Perform the sorting
                var expenses = isAscending
                    ? await conn.Table<Expense>().OrderBy((Expression<Func<Expense, object>>)lambda).ToListAsync()
                    : await conn.Table<Expense>().OrderByDescending((Expression<Func<Expense, object>>)lambda).ToListAsync();

                return expenses;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Status Message", "Failed to sort expenses. " + ex.Message, "OK");
            }

            return new List<Expense>();
        }




    }
}
