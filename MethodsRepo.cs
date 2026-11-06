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

    public List<DTO> GetLateReturns()
    {
        List<DTO> dTOs = new List<DTO>();
        using IDbConnection connection = new MySqlConnection(connectionString);
        string GetNotReturned =
"""
SELECT Customer.Name AS Name, MovieCopy.Barcode AS Barcode, Movie.Title AS MovieTitle, datediff(curdate(), MovieRental.ExpectedReturnDate) AS NumberOfDaysLate,
sum(datediff(curdate(),MovieRental.ExpectedReturnDate) * MovieRental.PricePerDay) AS DaysLatePrice
FROM MovieCopy
INNER JOIN MovieRental ON MovieCopy.Id = MovieRental.MovieCopyId
INNER JOIN Rental ON MovieRental.OrderId = Rental.Id
inner join Movie on MovieCopy.MovieId = Movie.Id
Inner join Customer on Rental.CustomerId = Customer.Id
where curdate() > MovieRental.ExpectedReturnDate && ReturnDate is null
group by Customer.Name,Customer.Id, MovieCopy.Barcode, Movie.Title, MovieRental.ExpectedReturnDate
order by Customer.Id desc      
""";
        IEnumerable<DTO> list = connection.Query<DTO>(GetNotReturned).ToList();
        foreach (DTO d in list)
        {
            dTOs.Add(new DTO(d.Name, d.Barcode, d.MovieTitle, d.NumberOfDaysLate, d.DaysLatePrice));
        }
        return dTOs;
    }

    public void ShowAllLateMovies(List<DTO> DtoList)
    {
        foreach (DTO d in DtoList)
        {
            System.Console.WriteLine($"Name: {d.Name} Barcode: {d.Barcode}, Movietitle: {d.MovieTitle} Days to late: {d.NumberOfDaysLate} Price: {d.DaysLatePrice}  ");
        }
    }
}