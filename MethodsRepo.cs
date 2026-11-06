using System;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using UtilityLibraries;



public class Repository
{

    string connectionString =
    """
    Server=comet-direct.usbx.me;
            Port=17815;
            Database=yh_max_Videoteket;
            User=yh_max;
            Password=C7AKQfWB#sha7n;
    """;


    public void CheckDateAndPrice()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    UPDATE Movie
                    SET PricePerDay = CASE
                    WHEN ReleaseDate >= '2000-01-01' THEN 47
                    ELSE 27
                    END;
                    """;
        db.Execute(sql);
    }

    public void SendBackOldBooks()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    
                     """;
    }

    public void RentMovie(string movieToRent)
    {
        // ej klar
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql =
    }

    public void ShowAllMovies()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    SELECT Movie.Id, Movie.Title, Movie.ReleaseDate, Movie.PricePerDay
                    FROM Movie
                    INNER JOIN MovieCopy on Movie.Id = MovieCopy.MovieId
                    WHERE MovieCopy.IsLoaned = false
                    """;
        IEnumerable<Movie> movies = db.Query<Movie>(sql).ToList();
        foreach (Movie m in movies)
        {
            System.Console.WriteLine($"{m.Title}, {m.PricePerDay}");
        }
    }
    public IEnumerable<Movie> GetAllNotRentedMovies()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    SELECT Movie.Id, Movie.Title, Movie.ReleaseDate, Movie.PricePerDay
                    FROM Movie
                    INNER JOIN MovieCopy on Movie.Id = MovieCopy.MovieId
                    WHERE MovieCopy.IsLoaned = false
                    """;
        IEnumerable<Movie> movies = db.Query<Movie>(sql).ToList();
        return movies;
    }

    public bool CheckCustomerId(string name, DateOnly birth, string mail, string phone)
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        var customers = db.Query<Customer>("""
                                    SELECT Customer.Name, Customer.BirthDate, Customer.Email, Customer.PhoneNumber
                                    FROM CUSTOMER
                                    """).ToList();
        foreach (Customer c in customers)
        {
            if (c.Name == name && c.BirthDate == birth && c.Email == mail && c.PhoneNumber == phone)
            {
                System.Console.WriteLine("Det stämde överens"); return true;
            }
            else System.Console.WriteLine("det stämde inte överäns"); return false;
        }
        return false;
    }
}