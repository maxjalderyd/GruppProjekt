using GruppProjekt;
Repository repo = new();
RentalService rentalService = new(repo);
UserInterface userInterface = new(repo, rentalService);
userInterface.Start();