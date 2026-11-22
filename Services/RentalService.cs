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
}