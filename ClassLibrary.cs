using System;
using UtilityLibraries;
namespace UtilityLibraries;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int PricePerDay { get; set; }
    public Movie(int id, string title, DateTime releaseDate, int pricePerDay)
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        PricePerDay = pricePerDay;
    }
}

public class MovieCopy
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public bool IsLoaned { get; set; }
    public Movie Movie { get; set; }
    public MovieCopy(int id, string barcode, bool isLoaned, Movie movie)
    {
        Id = id;
        Barcode = barcode;
        IsLoaned = isLoaned;
        Movie = movie;
    }
}

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Customer(int id, string name, DateTime birthDate, string email, string phoneNumber)
    {
        Id = id;
        Name = name;
        BirthDate = DateOnly.FromDateTime(birthDate);
        Email = email;
        PhoneNumber = phoneNumber;
    }
}

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


public class DTO
{
    public string Name { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Barcode { get; private set; }
    public string MovieTitle { get; private set; }
    public int NumberOfDaysLate { get; private set; }
    public decimal DaysLatePrice { get; private set; }


    public DTO(string name, string phoneNumber, string barcode, string movieTitle,
       int numberOfDaysLate, decimal daysLatePrice)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Barcode = barcode;
        MovieTitle = movieTitle;
        NumberOfDaysLate = numberOfDaysLate;
        DaysLatePrice = daysLatePrice;
    }

    public interface IDiscounts
    {
        string Name { get; set; } // Namn På Rabbaten tex "BlackWeek 20"
        decimal Percentage { get; set; }    // procent att applicera på rabbaten
        bool IsApplicable { get; set; } // jämnför så att rabbaten är applicerbar tex 3 för 2. krävs att du har 3 filmer etc
        decimal CalculateDiscount(); // räknar ut hur mycket som skall dras av från original priset i kronor
        decimal PrivateSellerDeal(); // låter säljaren välja en procent.


    }
}