using System;
using Google.Protobuf.WellKnownTypes;
using Mysqlx.Resultset;
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
public record MovieReceiptRow(string Barcode, string MovieTitle, int PricePerDay, DateOnly ExpectedReturnDate, int RentalDays)
{ public decimal TotalPrice => PricePerDay * RentalDays; } //räknar ut totalpris per film

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

