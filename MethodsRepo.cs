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

    }

    public MovieCopy? GetMovieWithBarcode(string barcode)
    {
        using MySqlConnection db = new(connectionString);
        db.Open();

        using MySqlCommand command = new(
        """
        SELECT 
            m.Id as MovieId, m.Title, m.ReleaseDate, m.PricePerDay, mc.Id, mc.Barcode, mc.IsLoaned
        FROM Movie m 
        JOIN MovieCopy mc on m.Id = mc.MovieId
        WHERE mc.Barcode = @barcode
        """, db);
        command.Parameters.AddWithValue("@barcode", barcode);

        using MySqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;
        
        Movie movie = new(
            reader.GetInt32("MovieId"),
            reader.GetString("Title"),
            reader.GetDateTime("ReleaseDate"),
            reader.GetInt32("PricePerDay")
        );
        MovieCopy movieCopy = new(
            reader.GetInt32("Id"), 
            barcode, 
            reader.GetBoolean("IsLoaned"),
            movie
        );

        return movieCopy;
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
        string CheckCustomerSql = """
                    SELECT Customer.Id, Customer.Name, Customer.BirthDate, Customer.Email, Customer.PhoneNumber
                    FROM Customer
                    """;
        IEnumerable<Customer> customers = db.Query<Customer>(CheckCustomerSql).ToList();
        foreach (Customer c in customers)
        {
            if (c.Name == name && c.BirthDate == birth && c.Email == mail && c.PhoneNumber == phone)
            {
                System.Console.WriteLine("Det stämde överens"); return true;
            }
        }
        System.Console.WriteLine("det stämde inte överäns"); return false;
    }
}