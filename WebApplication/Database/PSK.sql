CREATE TABLE "User" (
  "Id" int PRIMARY KEY,
  "Name" varchar,
  "Surname" varchar,
  "Email" varchar UNIQUE NOT NULL,
  "Role" enum
);

--COMMENT ON COLUMN "User"."Role" IS '
--    0 = Employee,
--    1 = Organizator,
--    2 = Administrator';

CREATE TABLE "Office" (
  "Id" int PRIMARY KEY,
  "Name" varchar UNIQUE NOT NULL,
  "Address" varchar,
  "City" varchar
);

CREATE TABLE "Apartment" (
  "Id" int PRIMARY KEY,
  "Title" varchar,
  "OfficeId" int,
  "RoomCount" int
);

CREATE TABLE "Calender" (
  "Id" int PRIMARY KEY,
  "StartDate" datetime NOT NULL,
  "EndDate" datetime NOT NULL,
  "BookingType" enum,
  "BookerId" int
);

--COMMENT ON COLUMN "Calender"."BookingType" IS '
--    0 = userEvent,
--    1 = userTrip';


CREATE TABLE "Trip" (
  "Id" int PRIMARY KEY,
  "Title" varchar,
  "Start" datetime NOT NULL,
  "End" datetime,
  "FromOffice" int,
  "ToOffice" int,
  "TripStatus" enum
);

--COMMENT ON COLUMN "Trip"."TripStatus" IS '
--    0 = Waiting for approval,
--    1 = Approved,
--    2 = In progress,
--    3 = Completed,
--    4 = Canceled';

CREATE TABLE "TripParticipator" (
  "UserId" int NOT NULL,
  "TripId" int NOT NULL
);

CREATE TABLE "FlightInformation" (
  "Id" int PRIMARY KEY,
  "TripId" int NOT NULL,
  "Cost" decimal,
  "Start" datetime,
  "End" datetime,
  "FlightTicketStatus" enum
);

--COMMENT ON COLUMN "FlightInformation"."FlightTicketStatus" IS '
--    0 = Not required,
--    1 = Required,
--    2 = Booked,';

CREATE TABLE "RentalCarInformation" (
  "Id" int PRIMARY KEY,
  "TripdId" int NOT NULL,
  "Cost" decimal,
  "Start" datetime,
  "End" datetime,
  "RentalCarStatus" enum
);

--COMMENT ON COLUMN "RentalCarInformation"."RentalCarStatus" IS '
--    0 = Not required,
--    1 = Required,
--    2 = Booked,';

CREATE TABLE "ApartmentRoom" (
  "Id" int PRIMARY KEY,
  "RoomNumber" int UNIQUE NOT NULL,
  "ApartmentId" int NOT NULL
);

ALTER TABLE "Apartment" ADD FOREIGN KEY ("OfficeId") REFERENCES "Office" ("Id");

ALTER TABLE "Calender" ADD FOREIGN KEY ("BookerId") REFERENCES "User" ("Id");

ALTER TABLE "Calender" ADD FOREIGN KEY ("BookerId") REFERENCES "Apartment" ("Id");

ALTER TABLE "Trip" ADD FOREIGN KEY ("FromOffice") REFERENCES "Office" ("Id");

ALTER TABLE "Trip" ADD FOREIGN KEY ("ToOffice") REFERENCES "Office" ("Id");

ALTER TABLE "TripParticipator" ADD FOREIGN KEY ("UserId") REFERENCES "User" ("Id");

ALTER TABLE "TripParticipator" ADD FOREIGN KEY ("TripId") REFERENCES "Trip" ("Id");

ALTER TABLE "FlightInformation" ADD FOREIGN KEY ("TripId") REFERENCES "Trip" ("Id");

ALTER TABLE "RentalCarInformation" ADD FOREIGN KEY ("TripdId") REFERENCES "Trip" ("Id");

ALTER TABLE "ApartmentRoom" ADD FOREIGN KEY ("ApartmentId") REFERENCES "Apartment" ("Id");