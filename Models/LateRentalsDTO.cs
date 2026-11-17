public class LateRentalsDTO
{
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Barcode { get; private set; }
    public string MovieTitle { get; private set; }
    public int NumberOfDaysLate { get; private set; }
    public decimal DaysLatePrice { get; private set; }


    public LateRentalsDTO(string name, string phoneNumber, string barcode, string movieTitle,
       int numberOfDaysLate, decimal daysLatePrice)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Barcode = barcode;
        MovieTitle = movieTitle;
        NumberOfDaysLate = numberOfDaysLate;
        DaysLatePrice = daysLatePrice;
    }
}