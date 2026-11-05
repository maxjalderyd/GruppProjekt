using System;
using UtilityLibraries;
namespace UtilityLibraries;

public class Movie
{
    public int Id {get; set;}
    public string Title {get; set;}
    public DateOnly ReleaseDate {get; set;}
    public int PricePerDay {get; set;}
    public Movie (int id, string title, DateOnly releaseDate, int pricePerDay)
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        PricePerDay = pricePerDay;
    }
}

public class MovieCopy
{
    public int Id {get; set;}
    public string Barcode {get; set;}
    public bool IsLoaned {get; set;}
    public Movie Movie {get; set;}
    public MovieCopy (int id, string barcode, bool isLoaned, Movie movie)
    {
        Id = id;
        Barcode = barcode;
        IsLoaned = isLoaned;
        Movie = movie;
    }
}

public class Customer
{
    public int Id {get; set;}
    public string Name {get; set;}
    public DateOnly BirthDate {get; set;}
    public string Email {get; set;}
    public string PhoneNumber {get; set;}
    public Customer (int id, string name, DateOnly birthDate, string email, string phoneNumber)
    {
        Id = id;
        Name = name;
        BirthDate = birthDate;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}

public class MovieRental
{
    public int RentalId {get; set;}
    public int MovieCopyId {get; set;}
    public DateOnly? ReturnDate {get; set;}
    public DateOnly ExpectedReturnDate {get; set;}
    public int PricePerDay {get; set;}
    public MovieRental (int rentalId, int movieCopyId, DateOnly? returnDate,
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
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public DateOnly RentalDate {get; set;}

    public Rental (int id, int customerId, DateOnly rentalDate)
    {
        Id = id;
        CustomerId = customerId;
        RentalDate = rentalDate;
    }
}