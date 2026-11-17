public class Receipt
{
    public string Customer { get; private set; } //Customer.Name
    public int OrderNr { get; private set; } //RentalId
    public List<MovieReceiptRow> ReceiptRows { get; private set; } = new(); //barcode, movieTitle, pricePerDay, expectedReturnDate, rentalDays
    public decimal TotalPrice => ReceiptRows.Sum(row => row.TotalPrice); //räknar ut totalpris för hela ordern

    private decimal PriceAfterDiscount => TotalPrice - DiscountPercentage * TotalPrice;
    private decimal _discountPercentage;
    public decimal DiscountPercentage
    {
        get
        {
            return _discountPercentage;
        }
        set
        {
            _discountPercentage = value;
        }
    }
    public Receipt(string customer, int orderNr)
    {
        Customer = customer;
        OrderNr = orderNr;
    }

    public void AddReceiptRow(MovieReceiptRow receiptRow)
    {
        ReceiptRows.Add(receiptRow);
    }
}