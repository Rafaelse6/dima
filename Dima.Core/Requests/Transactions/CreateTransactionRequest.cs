using Dima.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Transactions
{
    public class CreateTransactionRequest : Request
    {
        [Required(ErrorMessage = "Título Inválido")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo Inválido")]
        public ETransactionType Type { get; set; }
        
        [Required(ErrorMessage = "Quantidade Inválida")]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = "Categoria Inválida")]
        public long CategoryId { get; set; }
        
        [Required(ErrorMessage = "Data Inválida")]
        public DateTime? PaidOrReceivedAt { get; set; }

    }
}
