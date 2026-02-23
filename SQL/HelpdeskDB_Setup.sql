-- ============================================
-- Helpdesk Ticketing System - Database Script
-- Creates database, tables, constraints, and seed data
-- ============================================

-- 1. Create the database (run this separately if needed)
-- CREATE DATABASE HelpdeskDB;
-- GO
-- USE HelpdeskDB;
-- GO

-- ============================================
-- 2. Create Tables
-- ============================================

-- Users Table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Categories Table
CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Tickets Table
CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    CategoryId INT NOT NULL,
    CreatedBy INT NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Open',
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,

    -- Foreign Keys
    CONSTRAINT FK_Tickets_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Tickets_Users FOREIGN KEY (CreatedBy) REFERENCES Users(Id),

    -- CHECK constraint: Status must be Open, InProgress, or Closed
    CONSTRAINT CK_Tickets_Status CHECK (Status IN ('Open', 'InProgress', 'Closed'))
);
GO

-- TicketComments Table
CREATE TABLE TicketComments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TicketId INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedByU INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),

    -- Foreign Keys
    CONSTRAINT FK_TicketComments_Tickets FOREIGN KEY (TicketId) REFERENCES Tickets(Id),
    CONSTRAINT FK_TicketComments_Users FOREIGN KEY (CreatedByU) REFERENCES Users(Id)
);
GO

-- ============================================
-- 3. Seed Data
-- ============================================

-- Insert Admin User (Password: Admin@123)
INSERT INTO Users (FullName, Email, PasswordHash, IsActive)
VALUES ('Mohammad Ayoub', 'admin@helpdesk.com', 'Admin@123', 1);

-- Insert Test User (Password: Test@123)
INSERT INTO Users (FullName, Email, PasswordHash, IsActive)
VALUES ('Ali Mahmoud', 'test@helpdesk.com', 'Test@123', 1);
GO

-- Insert Categories
INSERT INTO Categories (Name, IsActive) VALUES ('Software', 1);
INSERT INTO Categories (Name, IsActive) VALUES ('Hardware', 1);
INSERT INTO Categories (Name, IsActive) VALUES ('Network', 1);
GO

-- Insert Sample Tickets
INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('Email not working', 'Unable to send or receive emails since this morning. Outlook shows an error about server connection.', 1, 1, 'Open');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('Monitor flickering', 'The monitor on desk 4B is flickering intermittently. Already tried replacing the cable.', 2, 2, 'InProgress');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('VPN connection drops', 'VPN disconnects after about 10 minutes of inactivity. Need a permanent fix.', 3, 1, 'Open');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('Install Adobe Acrobat', 'Need Adobe Acrobat Pro installed on my workstation for PDF editing.', 1, 2, 'Closed');
GO

-- Insert Sample Comments
INSERT INTO TicketComments (TicketId, CommentText, CreatedByU)
VALUES (1, 'I have checked the email server and it seems to be running fine. Can you try restarting Outlook?', 1);

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU)
VALUES (1, 'Restarted Outlook but still getting the same error. Screenshot attached.', 2);

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU)
VALUES (2, 'Ordered a replacement monitor. Should arrive by Friday.', 1);
GO

PRINT 'Database setup complete!';
PRINT 'Default login: admin@helpdesk.com / Admin@123';
GO
