//using System.Reflection.Metadata;
//using Dima.Core.Handlers;
//using Dima.Core.Requests.Reports;
//using Microsoft.AspNetCore.Components;
//using MudBlazor;

//namespace Dima.Web.Components.Reports;

//public partial class ExpensesByCategoryChartComponent : ComponentBase 
//{
//    #region Properties

//    public List<double> Data { get; set; } = [];
//    public List<string> Labels { get; set; } = [];

//    #endregion

//    #region Services

//    [Inject]
//    public IReportHandler Handler { get; set; } = null!;

//    [Inject]
//    public ISnackbar Snackbar { get; set; } = null!;

//    #endregion

//    #region Overrides

//    protected override async Task OnInitializedAsync()
//    {
//        await GetExpensesByCategoryAsync();
//    }

//    private async Task GetExpensesByCategoryAsync()
//    {
//        var request = new GetExpensesByCategoryRequest();
//        var result = await Handler.GetExpensesByCategoryReportAsync(request);
//        if (!result.IsSuccess || result.Data is null)
//        {
//            Snackbar.Add("Falha ao obter dados do relatório", Severity.Error);
//            return;
//        }

//        foreach (var item in result.Data)
//        {
//            // Mantém o label com sinal correto, se quiser
//            Labels.Add($"{item.Category} ({Math.Abs(item.Expenses):C})");

//            // O gráfico de pizza precisa de valor positivo
//            Data.Add((double)Math.Abs(item.Expenses));
//        }
//    }

//    #endregion
//}