public class MovieRental
{
    public int RentalId { get; set; }
    public int MovieCopyId { get; set; }
    public DateOnly? ReturnDate { get; set; }
    public DateOnly ExpectedReturnDate { get; set; }
    public int PricePerDay { get; set; }
    public MovieRental(int rentalId, int movieCopyId, DateOnly? returnDate,
                        DateOnly expectedReturnDate, int pricePerDay)
    {
        RentalId = rentalId;
        MovieCopyId = movieCopyId;
        ReturnDate = returnDate;
        ExpectedReturnDate = expectedReturnDate;
        PricePerDay = pricePerDay;
    }
}