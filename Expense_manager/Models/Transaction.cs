using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_manager.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        //categoryId

        [Range (1,int.MaxValue,ErrorMessage ="please enter valid Category Id")]
        public int CategoryId { get; set; }
       
        public Category? Category { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should not less than 0")]

        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]

        public string Note { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category == null ? "" : Category.icon + " " + Category.title;
            }
        }
        [NotMapped]
        public string? plusorminusamount
        {
            get
            {
                return (Category == null || Category.Type=="Expense" ? "-" :"+")+(Amount.ToString());

            }
        }

    }

}
