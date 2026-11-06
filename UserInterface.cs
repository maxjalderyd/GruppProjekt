using Dapper;
using MySql.Data.MySqlClient;
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
                    repo.ShowAllMovies();
                    return;
                case "3":
                    return;
                case "4":
                    return;
                case "5":
                    return;
                case "6":
                    return;
                case "7":
                    return;
                case "8":
                    repo.ShowAllLateMovies(repo.GetLateReturns());
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
        while (true)
        {
            MovieCopy? movieCopy = GetMovie();
            if (movieCopy == null)
                continue;
        }
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
    public MovieCopy? GetMovie()
    {
        Console.Write("Ange streckkod:\n> ");
        string barcode = Console.ReadLine() ?? "";
        MovieCopy? movieCopy = repo.GetMovieWithBarcode(barcode);
        if (movieCopy == null)
        {
            Console.WriteLine("Ingen film med den streckkoden finns.");
            return null;
        }
        if (movieCopy.IsLoaned)
        {
            Console.WriteLine("Film är redan utlånad.");
            return null;
        }
        return movieCopy;
    }
}