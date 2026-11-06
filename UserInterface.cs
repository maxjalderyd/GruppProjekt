using Dapper;
using MySql.Data.MySqlClient;
using UtilityLibraries;

namespace GruppProjekt;

public class UserInterface
{

    public void Start()
    {
        Repository repo = new();
        while (true)
        {
            Console.WriteLine("Hej vad vill du göra?");
            Console.WriteLine("1. Hyra en film");
            Console.WriteLine("2. Visa alla filmer");
            Console.WriteLine("3. ");
            Console.WriteLine("7. Avsluta");

            Console.Write("Välj: ");
            string val = Console.ReadLine();

            switch (val)
            {
                case "1":
                    //hämta en Customer som kan kopplas till Rental
                    //skapa en ny Rental kopplad till kundens Id, t ex: 
                    //Rental rental = repo.CreateNewRental(customer.Id))

                    //börja loopa här så man kan lägga in flera MovieCopy på en Rental
                    MovieCopy? movieCopy = GetMovie(repo);
                    if (movieCopy == null)
                        continue;
                    //skapa och hämta ny MovieRental (skicka in movieCopy.Id och rental.Id)
                    //börja om loop tills man inte vill lägga in mer filmer

                    repo.ShowAllMovies();
                    System.Console.WriteLine("vilken film vill du hyra?");
                    string movieToRent = Console.ReadLine();
                    repo.RentMovie(movieToRent);
                    System.Console.WriteLine("vad heter du?");
                    string name = Console.ReadLine();
                    System.Console.WriteLine("Vad är ditt person nummer?");
                    string birthdateString = Console.ReadLine();
                    DateOnly birthDate = DateOnly.Parse(birthdateString);
                    System.Console.WriteLine("vad är din mail?");
                    string mail = Console.ReadLine();
                    System.Console.WriteLine("vad är ditt telefon nummer?");
                    string phone = Console.ReadLine();
                    repo.CheckCustomerId(name, birthDate, mail, phone);
                    repo.RentMovie(movieToRent);
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
                    System.Console.WriteLine("avslutar");
                    Thread.Sleep(2000);
                    return;
            }
        }

    }

    public MovieCopy? GetMovie(Repository repo)
    {
        Console.Write("Ange streckkod:\n> ");
        string barcode = Console.ReadLine();
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