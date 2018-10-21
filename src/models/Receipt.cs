using System.Collections.Generic;
namespace Seez.Models
{
  public class Receipt
  {
    public IEnumerable<ReceiptItem> Items { get; set; }

  }
}