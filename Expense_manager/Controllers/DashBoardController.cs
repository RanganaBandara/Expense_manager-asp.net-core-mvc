using Expense_manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_manager.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashBoardController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public async Task<IActionResult> Index()
        {
            //starting date to track

            DateTime StartDate=DateTime.Today.AddDays(-6);
            //ending date
            DateTime EndDate = DateTime.Today;
            List<Transaction> SelectedTransactions = await _context.Transactions.Include(

                x => x.Category).Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            //TOtal income
            int TotalIncome = SelectedTransactions.Where(i => i.Category.Type == "Income")
                .Sum(s => s.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");
           

            //TOtal expense
            int Totalexpense = SelectedTransactions.Where(i => i.Category.Type == "Expense")
             .Sum(s => s.Amount);
            ViewBag.Totalexpense = Totalexpense.ToString("C0");
           

            //balance

            int balance = TotalIncome - Totalexpense;
            ViewBag.balance = balance.ToString("C0");

            //donut chart settup

            ViewBag.DonutChart = SelectedTransactions.Where(
                i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categorywithicon = k.First().Category.icon + "" + k.First().Category.title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0")
                }


                ).OrderByDescending(p => p.amount)
                .ToList();



            //spline chart income vs expense    

            //income
            List<Splinechart> Incomesummery = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new Splinechart()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            //expense

            //income
            List<Splinechart> Expensesummery = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new Splinechart()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            //combine incomee and expenses


            string[] last7dates = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MMM")).ToArray();

            ViewBag.splinedata = from day in last7dates
                                 join income in Incomesummery on day equals income.day into dayincomejoined
                                 from income in dayincomejoined.DefaultIfEmpty()
                                 join expense in Expensesummery on day equals expense.day into dayexpensejoined
                                 from expense in dayexpensejoined.DefaultIfEmpty()

                                 select new
                                 {
                                     day=day,
                                     income = income == null ? 0 : income.income,
                                     expense = expense == null ? 0 : expense.expense,
                                 };



            return View();

        }

    }
    public class Splinechart
    {
        public string day;
        public int income;
        public int expense;
    }

}
