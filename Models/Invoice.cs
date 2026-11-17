public class Invoice
{
    public string CustomerName { get; set; } = null;

    public decimal TotalCost { get; set; }

    List<MovieRental> movieRentals = new();
    // när rerutnmovies updaterar movierental då trigga funktionen som gör
    // for each row insert if curdate > expected returndate. 

    public Invoice(string name, decimal totalcost, List<MovieRental> movieRentals)
    {

    }


}