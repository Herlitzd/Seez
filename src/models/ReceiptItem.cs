namespace Seez.Models
{
  public class ReceiptItem
  {
    public string Name { get; set; }
    public double Price { get; set; }
    private int _quantity = 1;
    public int Quantity
    {
      get { return _quantity; }
      set { _quantity = value; }
    }
  }
}