public class Invoice
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime CreatedDate { get; set; }
    public int MovieRentCopyId { get; set; }
    // när rerutnmovies updaterar movierental då trigga funktionen som gör
    // for each row insert if curdate > expected returndate. 

    public Invoice(int id, string customerName, decimal totalCost, DateTime date, int MovieRentCopy)
    {
        Id = id;
        CustomerName = customerName;
        TotalCost = totalCost;
        CreatedDate = date;
        MovieRentCopyId = MovieRentCopy;
    }
    //fixme procedur och trigger för sen inlämning är klar
    //skapa ett objekt av invoice av informationen den skapar
    public Invoice()
    {
        //parameterlös för att slippa dapper crash
    }



}

public class LateInvoice : Invoice
{
    public LateInvoice(int id, string customerName, decimal totalCost, DateTime date, int MovieRentCopy) : base(id, customerName, totalCost, date, MovieRentCopy)
    {
    }

    public LateInvoice()
    {
        //parameterlös för att slippa dapper crash
    }
}