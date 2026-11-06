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

    public void RentMovie()
    {
        System.Console.WriteLine("vilken film vill du hyra?");
        ShowAllMovies();
        Console.ReadLine();
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
}