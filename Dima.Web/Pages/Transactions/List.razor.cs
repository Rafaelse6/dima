using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions
{
    public partial class ListTransactionsPage : ComponentBase
    {
        #region Properties

        public bool IsBusy { get; set; } = false;
        public List<Transaction> Transactions { get; set; } = [];
        public string SearchTerm { get; set; } = string.Empty;
        public int CurrentYear { get; set; } = DateTime.Now.Year;
        public int CurrentMonth { get; set; } = DateTime.Now.Month;

        public int[] Years { get; set; } =
        {
            DateTime.Now.Year,
            DateTime.Now.Year - 1,
            DateTime.Now.Year - 2,
            DateTime.Now.Year - 3,
        };

        #endregion

        #region Services

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        [Inject]
        public ITransactionHandler Handler { get; set; } = null!;
        #endregion

        #region Override 

        protected override async Task OnInitializedAsync()
        => await GetTransactions();

        #endregion

        #region Public Methods

        public Func<Transaction, bool> Filter => transaction =>
        {
            if (string.IsNullOrEmpty(SearchTerm))
                return true;

            return transaction.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                    || transaction.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
        };
        #endregion
        #region Private Methods

        private async Task GetTransactions()
        {
            IsBusy = true;

            try
            {
                var request = new GetTransactionsByPeriodRequest
                {
                    StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                    EndDate = DateTime.Now.GetLastDay(CurrentYear, CurrentMonth),
                    PageNumber = 1,
                    PageSize = 1000
                };
                var result = await Handler.GetTransactionsByPeriod(request);
                if (result.IsSuccess)
                    Transactions = result.Data ?? [];
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }

            IsBusy = false;
        }

        #endregion
    }
}
