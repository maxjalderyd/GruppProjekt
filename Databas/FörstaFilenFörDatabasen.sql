DROP TABLE IF EXISTS MovieRental;
DROP TABLE IF EXISTS Rental;
DROP TABLE IF EXISTS Customer;
DROP TABLE IF EXISTS MovieCopy;
DROP TABLE IF EXISTS Movie;

CREATE TABLE Movie (
Id          INT NOT NULL AUTO_INCREMENT,
Title       VARCHAR(128) NOT NULL,
ReleaseDate DATE NOT NULL,
PricePerDay INT NOT NULL,
PRIMARY KEY (Id)
);

CREATE TABLE MovieCopy (
Id       INT NOT NULL AUTO_INCREMENT,
MovieId  INT NOT NULL,
IsLoaned BOOL NOT NULL,
Barcode  VARCHAR(128) NOT NULL UNIQUE,
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

CREATE TABLE Rental (
Id         INT NOT NULL AUTO_INCREMENT,
CustomerId INT NOT NULL,
RentalDate DATE NOT NULL,
PRIMARY KEY (Id),
FOREIGN KEY (CustomerId) REFERENCES Customer(Id)
);

CREATE TABLE MovieRental (
OrderId            INT NOT NULL,
MovieCopyId        INT NOT NULL,
ReturnDate         DATE NULL,
ExpectedReturnDate DATE NOT NULL,
PricePerDay        INT NOT NULL,
FOREIGN KEY (OrderId) REFERENCES Rental(Id),
FOREIGN KEY (MovieCopyId) REFERENCES MovieCopy(Id)
);
-- 1. Lägg till filmer (Movie)
INSERT INTO Movie (Title, ReleaseDate, PricePerDay) VALUES
('Inception', '2010-07-16', 39),
('The Matrix', '1999-03-31', 29),
('Interstellar', '2014-11-07', 49),
('Oppenheimer', '2023-07-21', 59);

-- 2. Lägg till fysiska filmkopior (MovieCopy)
INSERT INTO MovieCopy (MovieId, IsLoaned, Barcode) VALUES
(1, TRUE,  'BC-INCEPTION-001'),
(1, FALSE, 'BC-INCEPTION-002'),
(2, FALSE, 'BC-MATRIX-001'),
(3, TRUE,  'BC-INTERSTELLAR-001'),
(4, FALSE, 'BC-OPPENHEIMER-001');

-- 3. Lägg till kunder (Customer)
INSERT INTO Customer (Name, BirthDate, Email, PhoneNumber) VALUES
('Anna Andersson', '1992-05-14', 'anna.a@example.com', '0701234567'),
('Erik Karlsson', '1985-10-22', 'erik.k@example.com', '0729876543'),
('Maria Nilsson', '2000-01-08', 'maria.n@example.com', '0735551234');

-- 4. Lägg till uthyrningstillfällen / ordrar (Rental)
INSERT INTO Rental (CustomerId, RentalDate) VALUES
(1, '2026-03-01'), -- Uthyrning 1: Anna
(2, '2026-03-03'); -- Uthyrning 2: Erik

-- 5. Koppla filmkopior till ordrarna (MovieRental)
INSERT INTO MovieRental (OrderId, MovieCopyId, ReturnDate, ExpectedReturnDate, PricePerDay) VALUES
-- Anna hyrde Inception (pågående)
(1, 1, NULL, '2026-03-04', 39),

-- Anna hyrde även Interstellar (pågående)
(1, 4, NULL, '2026-03-04', 49),

-- Erik hyrde Matrix och har redan återlämnat den
(2, 3, '2026-03-05', '2026-03-06', 29);