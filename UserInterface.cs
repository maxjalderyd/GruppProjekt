using Dapper;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.CRUD;
using UtilityLibraries;

namespace GruppProjekt;

public class UserInterface
{
    private readonly Repository repo;
    public UserInterface(Repository repo)
    {
        this.repo = repo;
    }

    public void Start()
    {
        repo.CheckDateAndPrice();
        while (true)
        {
            Console.WriteLine("Hej vad vill du göra?");
            Console.WriteLine("1. Hyra film"); //vanja
            Console.WriteLine("2. Visa alla filmer");
            Console.WriteLine("3. Lägg in ny film"); // peter
            Console.WriteLine("4. Lämna tillbaka film");
            Console.WriteLine("5. Lägg till ny kund"); //vanja
            Console.WriteLine("6. Ta bort film ur katalog");
            Console.WriteLine("7. Skicka faktura");
            Console.WriteLine("8. Visa försenade lån"); //max
            Console.WriteLine("9. Avsluta");
            Console.Write("Välj: ");
            string val = Console.ReadLine() ?? "";

            switch (val)
            {
                case "1":
                    RentMovie();
                    break;
                case "2":
                    return;
                case "3":
                    return;
                case "4":
                    return;
                case "5":
                    CreateNewCustomer();
                    break;
                case "6":
                    System.Console.WriteLine("vilken film vill du radera? Namn");
                    repo.ShowAllMovies();
                    DeleteMovieUi(repo.DeleteMovie(Console.ReadLine()));
                    return;
                case "7":
                    return;
                case "8":
                    ShowAllLateMovies(repo.GetLateReturns());
                    return;
                case "9":
                    System.Console.WriteLine("avslutar");
                    Thread.Sleep(2000);
                    return;
            }
        }
    }

    public void RentMovie()
    {
        Customer? customer = GetCustomer();
        if (customer == null)
            return;

        Rental? rental = null;

        Receipt? receipt = null;

        while (true)
        {
            (MovieCopy? movieCopy, int rentalDays) = GetMovie();
            if (movieCopy == null)
                continue;

            if (rental == null)
                rental = repo.CreateNewRental(customer.Id);
            
            if (receipt == null)
                receipt = new(customer.Name, rental.Id);

            //Skapa MovieRental objekt och spara i databasen samt uppdatera IsLoaned = true
            DateOnly dueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(rentalDays);
            MovieRental movieRental = new(
                rental.Id,
                movieCopy.Id,
                null,
                dueDate,
                movieCopy.Movie.PricePerDay
            );
            repo.CreateNewMovieRental(movieRental);
            Console.WriteLine($"{movieCopy.Movie.Title} lades till.");

            //Lägg till infon i Receipt
            MovieReceiptRow receiptRow = new(
                movieCopy.Barcode,
                movieCopy.Movie.Title,
                movieRental.PricePerDay,
                movieRental.ExpectedReturnDate,
                rentalDays
            );
            receipt.AddReceiptRow(receiptRow);

            Console.Write("Vill du lägga till fler filmer [Y] eller [N]?\n> ");
            string answer = Console.ReadLine() ?? "";
            if (answer.Equals("N", StringComparison.OrdinalIgnoreCase))
                break;
        }

        if (receipt != null)
            PrintReceipt(receipt);
        else
            Console.WriteLine("Inga filmer las till.");
    }


    public void PrintReceipt(Receipt receipt)
    {
        Console.WriteLine("\n==============================================");
        Console.WriteLine("              VIDEOTEKET");
        Console.WriteLine("                 KVITTO");
        Console.WriteLine("==============================================");

        Console.WriteLine($"Kund: {receipt.Customer}");
        Console.WriteLine($"Kvittonummer: {receipt.OrderNr}");
        
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine($"{"Film",-20} {"Dagar",5} {"Pris/dag",10} {"Totalt",10}");
        Console.WriteLine("----------------------------------------------");

        foreach (var row in receipt.ReceiptRows)
        {
            Console.WriteLine($"{row.MovieTitle,-20} {row.RentalDays,5} {row.PricePerDay,10:C2} {row.TotalPrice,10:C2}");
            Console.WriteLine($"  Returneras senast: {row.ExpectedReturnDate:yyyy-MM-dd}");
        }

        Console.WriteLine("----------------------------------------------");
        Console.WriteLine($"{"TOTALPRIS:",-36} {receipt.TotalPrice,10:C2}");
        Console.WriteLine("==============================================");
    }

    public Customer? GetCustomer()
    {
        while (true)
        {
            Console.WriteLine("Tryck [Q] för att avsluta.");
            Console.WriteLine("vad heter du?");
            string name = Console.ReadLine() ?? "";

            if (name.Equals("Q", StringComparison.OrdinalIgnoreCase))
                return null;

            Console.WriteLine("Vad är ditt person nummer?");
            string birthdateString = Console.ReadLine() ?? "";
            DateOnly birthDate = DateOnly.Parse(birthdateString);

            Console.WriteLine("vad är din mail?");
            string mail = Console.ReadLine() ?? "";

            Console.WriteLine("vad är ditt telefon nummer?");
            string phone = Console.ReadLine() ?? "";

            Customer? customer = repo.GetCustomer(name, birthDate, mail, phone);
            if (customer == null)
            {
                Console.WriteLine("Det finns ingen kund som matchar.");
                continue;
            }
            return customer;
        }
    }
    public (MovieCopy? movieCopy, int rentalDays) GetMovie()
    {
        Console.Write("Ange streckkod:\n> ");
        string barcode = Console.ReadLine() ?? "";

        MovieCopy? movieCopy = repo.GetMovieWithBarcode(barcode);
        if (movieCopy == null)
        {
            Console.WriteLine("Ingen film med den streckkoden finns.");
            return (null, 0);
        }
        if (movieCopy.IsLoaned)
        {
            Console.WriteLine("Film är redan utlånad.");
            return (null, 0);
        }

        Console.Write("I hur många dagar vill du hyra filmen?\n> ");

        int rentalDays = int.Parse(Console.ReadLine()!);
        return (movieCopy, rentalDays);
    }
    public void CreateNewCustomer()
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Födelsedatum (YYYY-MM-DD): ");
        string strBirthDate = Console.ReadLine() ?? "";
        DateOnly birthDate;
        while (!DateOnly.TryParse(strBirthDate, out birthDate))
        {
            Console.Write("Ogiltigt datum. YYYY-MM-DD: ");
            strBirthDate = Console.ReadLine() ?? "";
        }

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Telefonnummer: ");
        string phoneNumber = Console.ReadLine() ?? "";

        int rowsAffected = repo.CreateNewCustomer(name, birthDate, email, phoneNumber);
        if (rowsAffected == 1)
            Console.WriteLine("Kunden las till.");
        else
            Console.WriteLine("Kunde inte lägga till kund.");

        Thread.Sleep(3000);
    }

    public void ShowAllLateMovies(List<LateRentalsDTO> DtoList)
    {
        foreach (LateRentalsDTO d in DtoList)
        {
            System.Console.WriteLine($"Name: {d.Name} Phone: {d.PhoneNumber} Barcode: {d.Barcode}, Movietitle: {d.MovieTitle} Days to late: {d.NumberOfDaysLate} Price: {d.DaysLatePrice}");
        }
    }
    public void DeleteMovieUi(string delMovie)
    {
        System.Console.WriteLine(delMovie);
    }
}