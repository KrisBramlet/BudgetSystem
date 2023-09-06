using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetSystem.Models;

namespace BudgetSystem
{
    public class CategoryRepositoryNA
    {
        string _dbPath;

        private SQLiteConnection conn;

        public CategoryRepositoryNA(string dbPath)
        {
            _dbPath = dbPath;
            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Category>();
        }

        public void AddNewCategory(Category category)
        {
            int result = 0;
            try
            {
                if (category.name == null)
                    throw new Exception("Name Cannot Be Empty");

                result = conn.Insert(new Category { name = category.name, color = category.color });
            }
            catch (Exception ex)
            {
                if (result == 1)
                    Application.Current.MainPage.DisplayAlert("Success", string.Format("{0} record(s) added (Name: {1}) color string = {2}", result, category.name, category.color), "OK");
                else
                    Application.Current.MainPage.DisplayAlert("Error", string.Format("No Records Added: {0}",ex.Message),"OK");
            }
        }

        public void RemoveCategory(Category category)
        {
            try
            {
                if (category == null || category.id == 0)
                    throw new Exception("Invalid category");

                int result = conn.Delete(category);

                if (result == 1)
                    Application.Current.MainPage.DisplayAlert("Success", "Category deleted successfully", "OK");
                else
                    Application.Current.MainPage.DisplayAlert("Error", "Failed to delete category", "OK");
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Failed to delete category. " + ex.Message, "OK");
            }
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                return conn.Table<Category>().ToList();
            }
            catch (Exception e)
            {
                Application.Current.MainPage.DisplayAlert("Error", string.Format("Failed to retrieve data. {0}", e.Message), "OK");
            }
            return new List<Category>();
        }

        
    }
}
