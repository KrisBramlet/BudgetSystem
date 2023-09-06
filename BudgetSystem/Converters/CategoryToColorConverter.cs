using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetSystem.Models;
using BudgetSystem.ViewModels;

namespace BudgetSystem.Converters
{
    namespace BudgetSystem.Converters
    {
        public class CategoryToColorConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Category category)
                {
                    // Access the colors collection from the CatTotalsViewModel
                    var catTotalsViewModel = Application.Current.Resources["CatTotalsViewModel"] as CatTotalsViewModel;
                    // Access the Categories Table
                    //List<Category> categories = App.CategoryRepo.GetAllCategories();
                    // Find the matching color for the category
                    //foreach (Category cat in categories) 
                    { }
                }

                // Return a default color gray
                return Color.FromArgb("#646464");
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}

