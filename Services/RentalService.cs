public class RentalService
{
    private readonly Repository repo;
    public RentalService (Repository repo)
    {
        this.repo = repo;
    }

    //Kan lägga till egna exceptions för specifika felmeddelanden
    public MovieCopy? GetAvailableMovie(string barcode)
    {
        MovieCopy? movieCopy = repo.GetMovieWithBarcode(barcode);

        //Ogiltig streckkod
        if (movieCopy == null)
            return null;
        
        //Exemplar skickats tillbaka till utgivaren
        if (!movieCopy.IsAvailable)
            return null;

        //Exemplar redan uthyrd
        if (repo.IsMovieRented(movieCopy))
            return null;
        
        return movieCopy;
    }

    //En film får hyras i 1-30 dagar
    public bool IsValidRentalDays(int rentalDays)
    {
        return rentalDays >= 1 && rentalDays <= 30;
    }

    //En film kan bara läggas till EN gång i EN uthyrning
    public bool IsMovieAlreadyAdded(List<MovieRental> movies, MovieRental movieRental)
    {
        return movies.Any(movie => movie.MovieCopyId == movieRental.MovieCopyId);
    }

    public MovieRental CreateMovieRental(MovieCopy movieCopy, int rentalDays)
    {
        DateOnly dueDate = DateOnly.FromDateTime(DateTime.Today).AddDays(rentalDays);

        return new MovieRental (
            null,
            movieCopy.Id,
            null,
            dueDate,
            movieCopy.Movie.PricePerDay
        );
    }

    public int CreateRental(int customerId, List<MovieRental> movies)
    {
        return repo.CreateRentalWithMovies(customerId, movies);
    }
}