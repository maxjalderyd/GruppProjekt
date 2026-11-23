using GruppProjekt;
Repository repo = new();
RentalService rentalService = new(repo);
MovieService movieService = new(repo);
CustomerService customerService = new(repo);
UserInterface userInterface = new(rentalService, movieService, customerService);
userInterface.Start();