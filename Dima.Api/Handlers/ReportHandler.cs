using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class ReportHandler(AppDbContext context) : IReportHandler
    {
        public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequests request)
        {
            try { 
            var data = await context
                .IncomesAndExpenses
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return new Response<List<IncomesAndExpenses>?>(data);
            }

            catch (Exception ex)
            {
                return new Response<List<IncomesAndExpenses>?>(null, 500, "Não foi possível obter as entradas e saídas");
            }
        }

        public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
        {
            try
            {
                var data = await context
                    .IncomesByCategories
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .OrderByDescending(x => x.Year)
                    .ThenBy(x => x.Category)
                    .ToListAsync();

                return new Response<List<IncomesByCategory>?>(data);
            }
            catch
            {
                return new Response<List<IncomesByCategory>?>(null, 500,
                    "Não foi possível obter as entradas por categoria");
            }
        }

        public Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<FinancialSummary?>>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
        {
            throw new NotImplementedException();
        }
       
    }
}
