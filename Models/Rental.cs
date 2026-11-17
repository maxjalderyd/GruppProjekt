public class Rental
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateOnly RentalDate { get; set; }

    public Rental(int id, int customerId, DateTime rentalDate)
    {
        Id = id;
        CustomerId = customerId;
        RentalDate = DateOnly.FromDateTime(rentalDate);
    }
}