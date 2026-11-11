using System;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using UtilityLibraries;
using System.Net.Mime;
using System.Configuration;



public class Repository
{
    static string preConnectionString = File.ReadAllText("tokens.txt");
    static string[] connectionItems = preConnectionString.Split(';');

    static string connectionString = $"""
        Server={connectionItems[0]};
        Port={connectionItems[1]};
        Database={connectionItems[2]};
        User={connectionItems[3]};
        Password={connectionItems[4]};
    """;
    public void CheckDateAndPrice()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    UPDATE Movie
                    SET PricePerDay = CASE
                    WHEN ReleaseDate >= '2000-01-01' THEN 49
                    ELSE 29
                    END;
                    """;
        db.Execute(sql);
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

    public Rental CreateNewRental(int customerId)
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql =
        """
        INSERT INTO Rental (CustomerId, RentalDate) values
        (@CustomerId, CURRENT_TIMESTAMP);

        SELECT * FROM Rental
        WHERE Id = LAST_INSERT_ID()
        """;

        return db.QuerySingle<Rental>(sql, new { CustomerId = customerId });
    }

    public void ShowAllMovies()
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql = """
                    SELECT Movie.Id, Movie.Title, Movie.ReleaseDate, Movie.PricePerDay
                    FROM Movie
                    INNER JOIN MovieCopy on Movie.Id = MovieCopy.MovieId
                    WHERE MovieCopy.IsAvailable = false AND Movie.IsDeleted = 0
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
                    WHERE MovieCopy.IsLoaned = false AND Movie.IsDeleted = 0
                    """;
        IEnumerable<Movie> movies = db.Query<Movie>(sql).ToList();
        return movies;
    }

    public Customer? GetCustomer(string name, DateOnly birth, string mail, string phone)
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string CheckCustomerSql = """
                    SELECT Customer.Id, Customer.Name, Customer.BirthDate, Customer.Email, Customer.PhoneNumber
                    FROM Customer
                    """;
        IEnumerable<dynamic> customers = db.Query(CheckCustomerSql).ToList();

        foreach (var c in customers)
        {
            if (c.Name == name && DateOnly.FromDateTime(c.BirthDate) == birth && c.Email == mail && c.PhoneNumber == phone)
                return new Customer(
                    c.Id, c.Name, c.BirthDate, c.Email, c.PhoneNumber
                );
        }
        return null;
    }

    public List<LateRentalsDTO> GetLateReturns()
    {
        List<LateRentalsDTO> dTOs = new List<LateRentalsDTO>();
        using IDbConnection connection = new MySqlConnection(connectionString);
        string GetNotReturned =
"""
SELECT Customer.Name AS Name, Customer.PhoneNumber AS PhoneNumber, MovieCopy.Barcode AS Barcode, Movie.Title AS MovieTitle, datediff(curdate(), MovieRental.ExpectedReturnDate) AS NumberOfDaysLate,
sum(datediff(curdate(),MovieRental.ExpectedReturnDate) * MovieRental.PricePerDay) AS DaysLatePrice
FROM MovieCopy
INNER JOIN MovieRental ON MovieCopy.Id = MovieRental.MovieCopyId
INNER JOIN Rental ON MovieRental.OrderId = Rental.Id
inner join Movie on MovieCopy.MovieId = Movie.Id
Inner join Customer on Rental.CustomerId = Customer.Id
where curdate() > MovieRental.ExpectedReturnDate && ReturnDate is null AND Movie.IsDeleted = 0
group by Customer.Name, Customer.PhoneNumber, Customer.Id, MovieCopy.Barcode, Movie.Title, MovieRental.ExpectedReturnDate
order by Customer.Id desc      
""";
        IEnumerable<LateRentalsDTO> list = connection.Query<LateRentalsDTO>(GetNotReturned).ToList();
        foreach (LateRentalsDTO d in list)
        {
            dTOs.Add(new LateRentalsDTO(d.Name, d.PhoneNumber, d.Barcode, d.MovieTitle, d.NumberOfDaysLate, d.DaysLatePrice));
        }
        return dTOs;
    }

    public string DeleteMovie(string movieName)
    {
        // Hämta filmens Id
        using IDbConnection connection = new MySqlConnection(connectionString);
        string sqlGetMovieId = "SELECT Movie.Id FROM Movie WHERE IsDeleted = 0 AND Movie.Title = @Title";
        int? movieId = connection.QueryFirstOrDefault<int?>(sqlGetMovieId, new { Title = movieName });
        if (movieId == null)
        {
            return $"fanns ingen film med det namnet eller redan markerats som INAKTIV";

        }

        string CheckActiveRentals = @"SELECT EXISTS (
                                    SELECT 1
                                    FROM MovieRental
                                    INNER JOIN MovieCopy ON MovieRental.MovieCopyId = MovieCopy.Id
                                    INNER JOIN Movie ON MovieCopy.MovieId = Movie.Id
                                    WHERE Movie.Id = @movieId AND MovieRental.ReturnDate IS NULL
                                    );";
        bool hasActiveRentals = connection.ExecuteScalar<bool>(CheckActiveRentals, new { movieId });
        if (hasActiveRentals == true)
        {
            return $"kan ej radera {movieName} då det finns aktiva uthyrningar på den";
        }

        string softDeleteSql = @"UPDATE Movie
                                SET IsDeleted = 1
                                WHERE Movie.Id = @movieId";
        connection.Execute(softDeleteSql, new { movieId });
        return $"Filmen {movieName}: Har raderats från systemet";

    }
    public void CreateNewMovieRental(MovieRental movieRental)
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql =
        """
        INSERT INTO MovieRental(OrderId, MovieCopyId, ReturnDate, ExpectedReturnDate, PricePerDay)
        VALUES (@RentalId, @MovieCopyId, @ReturnDate, @ExpectedReturnDate, @PricePerDay);
        """;
        db.Execute(sql, movieRental);
    }

    public int CreateNewCustomer(string name, DateOnly birthDate, string email, string phoneNumber)
    {
        using IDbConnection db = new MySqlConnection(connectionString);
        string sql =
        """
        INSERT INTO Customer (Name, BirthDate, Email, PhoneNumber)
        VALUES (@name, @birthDate, @email, @phoneNumber)
        """;
        return db.Execute(sql, new { name, birthDate, email, phoneNumber });
    }

}