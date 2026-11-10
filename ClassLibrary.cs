using System;
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
    public string Customer {get; private set;} //Customer.Name
    public int OrderNr {get; private set;} //RentalId
    public List<MovieReceiptRow> ReceiptRows {get; private set;} = new(); //barcode, movieTitle, pricePerDay, expectedReturnDate, rentalDays
    public decimal TotalPrice => ReceiptRows.Sum(row => row.TotalPrice); //räknar ut totalpris för hela ordern
    public Receipt (string customer, int orderNr)
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

// public class RentalContext // klass som håller information att föra in i discounts klasser.
// {
//     public string CustomerName { get; set; } = null!;
//     public List<MovieRental> items = new();
//     public decimal TotalCost;
//     public DateOnly RentalDate;
// }
// public interface IDiscounts
// {
//     string Name { get; } // Namn På Rabbaten tex "BlackWeek 20"
//     bool IsApplicable { get; set; } // jämnför så att rabbaten är applicerbar tex 3 för 2. krävs att du har 3 filmer etc
//     decimal CalculateDiscount(RentalContext context); // räknar ut hur mycket som skall dras av från original priset i kronor

// }

// public class SellersDiscount : IDiscounts
// {
//     private readonly decimal _percentage;
//     string IDiscounts.Name => "SellersDeal";
//     bool IDiscounts.IsApplicable { get; set; }
//     decimal IDiscounts.CalculateDiscount(RentalContext context)
//     {
//         return _percentage;
//     }
// }

/* public class Receipt
{
    string receiptFilePath = "Receipt.csv";
    private int _orderNumber;
    public int OrderNumber => _orderNumber;
    readonly string CustomerName;
    private int _totalPrice;
    decimal TotalPrice => _totalPrice;
    readonly List<MovieCopy> movies = new();


    public Receipt(Customer customer, List<MovieRental> movieRentals, List<Rental> rentals, List<MovieCopy> copy, Movie movie)
    {

        _orderNumber = Random.Shared.Next(111111, 999999);
        CustomerName = customer.Name;

        for (int i = 0; i < rentals.Count; i++)
    {
            Rental r = rentals[i];
            MovieRental m = movieRentals[i];

            int days = (int)(m.ExpectedReturnDate.DayNumber - r.RentalDate.DayNumber);

            _totalPrice += days * m.PricePerDay;
            // movies.Add(copy[i].Id == [i]);
        }

    }
    public override string ToString()
    {
        return $"{OrderNumber},{CustomerName},{string.Join(";", movies.ConvertAll(movie => movie.Barcode))},{string.Join(";", movies.ConvertAll(movie => movie.Movie.Title))},{string.Join(";", movies.ConvertAll(movie => movie.Movie.PricePerDay))},{TotalPrice}";
    }
    public void SaveReceipt(Receipt receipt)
    {
        File.AppendAllText(receiptFilePath, receipt.ToString() + Environment.NewLine);
    } 


}*/

