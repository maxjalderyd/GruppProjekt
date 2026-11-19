namespace GruppProjekt.Models;
public interface InMemoryDB
{

}

public class ClassInMemoryDB : InMemoryDB
{
    public List<Movie> movies = new();
    public List<MovieCopy> movieCopies = new();
}
// """
//                     SELECT Movie.Id, Movie.Title, Movie.ReleaseDate, Movie.PricePerDay
//                     FROM Movie
//                     INNER JOIN MovieCopy on Movie.Id = MovieCopy.MovieId
//                     WHERE MovieCopy.IsAvailable = false AND Movie.IsDeleted = 0
//                     """;