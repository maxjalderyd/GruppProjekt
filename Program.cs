using GruppProjekt;
Repository repo = new();
RentalService rentalService = new(repo);
MovieService movieService = new(repo);
UserInterface userInterface = new(repo, rentalService, movieService);
userInterface.Start();