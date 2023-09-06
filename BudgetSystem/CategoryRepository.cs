using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetSystem.Models;

namespace BudgetSystem
{
    public class CategoryRepository
    {
        string _dbPath;

        private SQLiteAsyncConnection conn;

        async Task Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Category>();
        }

        public CategoryRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task AddNewCategory(Category category)
        {
            int result = 0;
            try
            {
                await Init();

                if (category.name == null)
                    throw new Exception("Name Cannot Be Empty");

                result = await conn.InsertAsync(new Category { name = category.name });
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Format("{0} record(s) added (Name: {1})", result, category.name), "OK");
            }
        }

        public async Task RemoveCategory(Category category)
        {
            try
            {
                if (category == null || category.id == 0)
                    throw new Exception("Invalid category");

                await Init();

                int result = await conn.DeleteAsync(category);

                if (result == 1)
                    await Application.Current.MainPage.DisplayAlert("Success", "Category deleted successfully", "OK");
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete category", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete category. " + ex.Message, "OK");
            }
        }


        public async Task<List<Category>> GetAllCategories()
        {
            try
            {
                await Init();
                return await conn.Table<Category>().ToListAsync();
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Format("Failed to retrieve data. {0}", e.Message), "OK");
            }
            return new List<Category>();
        }


    } 
}
