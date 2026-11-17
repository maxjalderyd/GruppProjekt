public class Invoice
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime CreatedDate { get; set; }
    public int MovieRentCopyId { get; set; }

    List<MovieRental> movieRentals = new();
    // när rerutnmovies updaterar movierental då trigga funktionen som gör
    // for each row insert if curdate > expected returndate. 

    public Invoice()
    {

    }
    //fixme procedur och trigger för sen inlämning är klar
    //skapa ett objekt av invoice av informationen den skapar




}

public class LateInvoice : Invoice
{

}