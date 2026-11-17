using System.Text.RegularExpressions;
using Dapper;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.CRUD;

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
            Console.WriteLine("1. Hyra film"); //vanja - färdig (lägg till validering)
            Console.WriteLine("2. Visa alla filmer");
            Console.WriteLine("3. Lägg in ny film"); // peter
            Console.WriteLine("4. Lämna tillbaka film");
            Console.WriteLine("5. Lägg till ny kund"); //vanja - färdig (lägg till validering)
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
                    string name;
                    DateTime dateString;
                    decimal pricePerDay = 0.0m;
                    while (true)
                    {
                        System.Console.Write("Namn på film att lägga till?: ");
                        name = Console.ReadLine();
                        System.Console.Write("Releasedatum: ");
                        string dateInput = Console.ReadLine();
                        if (Regex.IsMatch(dateInput, @"^\d{4}-\d{2}-\d{2}$"))
                        {
                            dateString = DateTime.Parse(Console.ReadLine());
                            break;
                        }
                        else
                        {
                            System.Console.WriteLine("Wrong format");
                        }
                        System.Console.Write("Pris per dag: ");
                        pricePerDay = decimal.Parse(Console.ReadLine());
                    }
                    repo.AddNewMovie(name, dateString, pricePerDay);
                    break;
                case "4":
                    
                    break;
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

        List<MovieRental> movies = new();
        List<MovieReceiptRow> receiptRows = new();

        while (true)
        {
            //LÄGG TILL: Möjlighet att avbryta när som helst
            (MovieCopy? movieCopy, int rentalDays) = GetMovie();
            if (movieCopy == null)
                continue;

            //Skapa MovieRental objekt och spara i databasen samt uppdatera IsLoaned = true
            DateOnly dueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(rentalDays);
            MovieRental movieRental = new(
                null,
                movieCopy.Id,
                null,
                dueDate,
                movieCopy.Movie.PricePerDay
            );

            //Kontrollerar att filmen inte redan finns i listan
            if (movies.Any(movie => movie.MovieCopyId == movieRental.MovieCopyId))
            {
                Console.WriteLine("Film är redan tillagd.");
                continue;
            }

            movies.Add(movieRental);
            Console.WriteLine($"{movieCopy.Movie.Title} lades till.");

            //Spara kvittorad info
            MovieReceiptRow receiptRow = new(
                movieCopy.Barcode,
                movieCopy.Movie.Title,
                movieRental.PricePerDay,
                movieRental.ExpectedReturnDate,
                rentalDays
            );
            receiptRows.Add(receiptRow);

            Console.Write("Vill du lägga till fler filmer [Y] eller [N]?\n> ");
            string answer = Console.ReadLine() ?? "";
            if (answer.Equals("N", StringComparison.OrdinalIgnoreCase))
                break;
        }

        if (movies.Count == 0)
        {
            Console.WriteLine("Inga filmer las till. Återgår till huvudmenyn...");
            Thread.Sleep(3000);
            return;
        }
        
        //LÄGG TILL: Skriv ut order sammanfattning för bekräftning innan Rental skapas

        int rentalId;
        try
        {
            rentalId = repo.CreateRentalWithMovies(customer.Id, movies);
        }
        catch
        {
            Console.WriteLine("Beställningen kunde inte genomföras.. ");
            Thread.Sleep(3000);
            return;
        }

        Receipt receipt = new(customer.Name, rentalId);
        foreach (var row in receiptRows)
        {
            receipt.AddReceiptRow(row);
        }
        PrintReceipt(receipt);
        Thread.Sleep(3000);
    }

    public bool ConfirmOrder(List<MovieReceiptRow> receiptRows)
    {
        int totalPrice = 0;
        Console.WriteLine("====== SAMMANFATTNING ======");
        foreach (var row in receiptRows)
        {
            Console.WriteLine($"{row.MovieTitle} - {row.PricePerDay:C2} x {row.RentalDays} dagar");
            Console.WriteLine($"\tDue date: {row.ExpectedReturnDate}");
            Console.WriteLine("-------------------------------");
            totalPrice += row.PricePerDay * row.RentalDays;
        }
        Console.WriteLine($"Totalt: {totalPrice:C2}");

        Console.Write("Bekräfta och betala med [Y] ([N] för att avbryta):\n> ");
        string choice = Console.ReadLine() ?? "";
        if (choice.Equals("Y", StringComparison.OrdinalIgnoreCase))
            return true;
        
        Console.WriteLine("Order avbryts... ");
        Thread.Sleep(3000);
        return false;
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
        //LÄGG TILL: Kontrollera barcode formatet innan databasanrop

        MovieCopy? movieCopy = repo.GetMovieWithBarcode(barcode);

        //Kontrollerar om streckkoden är giltig
        if (movieCopy == null)
        {
            Console.WriteLine("Ingen film med den streckkoden finns.");
            return (null, 0);
        }

        //Kontrollerar om filmen har skickats tillbaka till utgivaren
        if (!movieCopy.IsAvailable)
        {
            Console.WriteLine("Filmen är inte längre tillgänglig.");
            return (null, 0);
        }

        //Kontrollerar om filmen redan är utlånad
        if (repo.IsMovieRented(movieCopy))
        {
            Console.WriteLine("Filmen är redan utlånad.");
            return (null, 0);
        }

        while (true)
        {
            Console.Write("I hur många dagar vill du hyra filmen? (1-30 dagar)\n> ");
            string strRentalDays = Console.ReadLine() ?? "";

            if (!int.TryParse(strRentalDays, out int rentalDays))
                Console.WriteLine("Felaktig inmatning. Ange endast heltal.");
            else if (rentalDays < 1 || rentalDays > 30)
                Console.WriteLine("Du kan endast hyra en film i 1-30 dagar.");
            else
                return (movieCopy, rentalDays);
        }
    }
    public void CreateNewCustomer()
    {
        Console.Write("Namn:\n> ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Födelsedatum (YYYY-MM-DD):\n> ");
        string strBirthDate = Console.ReadLine() ?? "";
        DateOnly birthDate;
        while (!DateOnly.TryParse(strBirthDate, out birthDate))
        {
            Console.Write("Ogiltigt datum. Ange i detta format YYYY-MM-DD:\n> ");
            strBirthDate = Console.ReadLine() ?? "";
        }

        Console.Write("Email:\n> ");
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