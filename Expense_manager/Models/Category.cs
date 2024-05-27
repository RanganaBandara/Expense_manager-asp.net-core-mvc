using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_manager.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }


        [Column(TypeName = "nvarchar(50)")]
        public string title { get; set; }

        [Column(TypeName = "nvarchar(5)")]
        public string icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = "Expenses";
    }
}
