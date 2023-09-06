using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BudgetSystem.Converters;
using BudgetSystem.Models;
using Syncfusion.Maui.DataSource.Extensions;

namespace BudgetSystem.ViewModels
{
    public class CatTotalsViewModel
    {
        public ObservableCollection<CatTotal> Data { get; set; }
        public ObservableCollection<Color> colors { get; set; }
        public List<Brush> CustomBrushes { get; set; }

        public CatTotalsViewModel()
        {
            StringToColorConverter converter = new StringToColorConverter();
            // Brushes \\
            CustomBrushes = new List<Brush>();
            // Gradients \\
            LinearGradientBrush gradientColor1 = new LinearGradientBrush();
            gradientColor1.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 1, Color = Color.FromRgb(219,62,37)},
                new GradientStop() { Offset = 0, Color = Color.FromRgb(107,27,15)} 
            };
            LinearGradientBrush gradientColor2 = new LinearGradientBrush();
            gradientColor2.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 1, Color = Color.FromRgb(54, 130, 48) }, 
                new GradientStop() { Offset = 0, Color = Color.FromRgb(10, 71, 6) } 
            };
            LinearGradientBrush gradientColor3 = new LinearGradientBrush();
            gradientColor3.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 1, Color = Color.FromRgb(6, 33, 71) },
                new GradientStop() { Offset = 0, Color = Color.FromRgb(42, 78, 130) }
            };
            LinearGradientBrush gradientColor4 = new LinearGradientBrush();
            gradientColor4.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 1, Color = Color.FromRgb(174, 191, 96) },
                new GradientStop() { Offset = 0, Color = Color.FromRgb(146, 76, 11) }
            };
            LinearGradientBrush gradientColor5 = new LinearGradientBrush();
            gradientColor5.GradientStops = new GradientStopCollection()
            {
                new GradientStop() { Offset = 1, Color = Color.FromRgb(97, 232, 214) },
                new GradientStop() { Offset = 0, Color = Color.FromRgb(78, 148, 139) }
            };

            CustomBrushes.Add(gradientColor1);
            CustomBrushes.Add(gradientColor2);
            CustomBrushes.Add(gradientColor3);
            CustomBrushes.Add(gradientColor4);
            CustomBrushes.Add(gradientColor5);


            colors = new ObservableCollection<Color>();


            


            // DATA \\
            Data = new ObservableCollection<CatTotal>();
            List<Expense> expenses = App.ExpenseRepoNA.GetAllExpenses();
            Data = expenses
                .GroupBy(e => e.category)
                .Select(group => new CatTotal { category = group.Key, total = group.Sum(e => (double)e.cost) })
                .ToObservableCollection<CatTotal>();   
        }
    }
}
