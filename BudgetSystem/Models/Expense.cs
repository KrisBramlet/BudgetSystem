using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BudgetSystem.Models
{
    [Table("Expenses")]
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(250)]
        public string name { get; set; }

        [MaxLength(250)]
        public string details { get; set; }

        [Column("Cost")]
        public double cost { get; set; }

        [MaxLength(100)]
        public string category { get; set; }

        [MaxLength(500)]
        public string notes { get; set; }
    }


}
