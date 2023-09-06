using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BudgetSystem.Models
{
    [Table("Category")]

    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(100),Unique]
        public string name { get; set; }
        [MaxLength(10)]
        public string color { get; set; }
       

    }
}
