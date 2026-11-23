-- MariaDB

DROP TABLE IF EXISTS Rental;
DROP TABLE IF EXISTS InvoiceToMovie;
DROP TABLE IF EXISTS InvoiceLogIfLate;
DROP TABLE IF EXISTS MovieRental;
DROP TABLE IF EXISTS Customer;
DROP TABLE IF EXISTS MovieCopy;
DROP TABLE IF EXISTS Movie;

CREATE TABLE Movie (
  Id          INT NOT NULL AUTO_INCREMENT,
  Title       VARCHAR(128) NOT NULL,
  ReleaseDate DATE NOT NULL,
  PricePerDay INT NOT NULL,
  IsDeleted   BOOL NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE MovieCopy (
  Id          INT NOT NULL AUTO_INCREMENT,
  MovieId     INT NOT NULL,
  IsAvailable BOOL NOT NULL,
  Barcode     VARCHAR(128) NOT NULL UNIQUE,
  PRIMARY KEY (Id),
  FOREIGN KEY (MovieId) REFERENCES Movie(Id)
);

CREATE TABLE Customer (
  Id          INT NOT NULL AUTO_INCREMENT,
  Name        VARCHAR(128) NOT NULL,
  BirthDate   DATE NOT NULL,
  Email       VARCHAR(128) NOT NULL,
  PhoneNumber VARCHAR(128) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE MovieRental (
  OrderId            INT NOT NULL,
  MovieCopyId        INT NOT NULL,
  ReturnDate         DATE NULL,
  ExpectedReturnDate DATE NOT NULL,
  PricePerDay        INT NOT NULL,
  PRIMARY KEY (OrderId, MovieCopyId),
  INDEX idx_MovieRental_MovieCopyId (MovieCopyId),
  FOREIGN KEY (MovieCopyId) REFERENCES MovieCopy(Id)
);

CREATE TABLE InvoiceLogIfLate (
  Id           INT NOT NULL AUTO_INCREMENT,
  CustomerName VARCHAR(64) NOT NULL,
  TotalCost    DECIMAL(10,2) NOT NULL,
  CreatedDate  DATE NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE InvoiceToMovie (
  MovieRentCopyId INT NOT NULL,
  InvoiceId       INT NOT NULL,
  FOREIGN KEY (MovieRentCopyId) REFERENCES MovieRental(MovieCopyId),
  FOREIGN KEY (InvoiceId) REFERENCES InvoiceLogIfLate(Id)
);

CREATE TABLE Rental (
  Id         INT NOT NULL AUTO_INCREMENT,
  CustomerId INT NOT NULL,
  RentalDate VARCHAR(128) NOT NULL,
  PRIMARY KEY (Id),
  FOREIGN KEY (Id) REFERENCES MovieRental(OrderId),
  FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);
