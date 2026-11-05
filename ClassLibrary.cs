using System;
using UtilityLibraries;
namespace UtilityLibraries;

public class Movie
{
    public int Id {get; set;}
    public string Title {get; set;}
    public DateOnly ReleaseDate {get; set;}
    public int PricePerDay {get; set;}
    public Movie (int id, string title, DateOnly releaseDate, int pricePerDay)
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        PricePerDay = pricePerDay;
    }
}

public class MovieCopy
{
    public int Id {get; set;}
    public string Barcode {get; set;}
    public bool IsRented {get; set;}
    public Movie Movie {get; set;}
    public MovieCopy (int id, string barcode, bool isRented, Movie movie)
    {
        Id = id;
        Barcode = barcode;
        Movie = movie;
    }
}