CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Tickets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    CategoryId INT NOT NULL,
    CreatedBy INT NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Open',
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Tickets_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT FK_Tickets_Users FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT CK_Tickets_Status CHECK (Status IN ('Open', 'InProgress', 'Closed'))
);
GO

CREATE TABLE TicketComments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TicketId INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedByU INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_TicketComments_Tickets FOREIGN KEY (TicketId) REFERENCES Tickets(Id),
    CONSTRAINT FK_TicketComments_Users FOREIGN KEY (CreatedByU) REFERENCES Users(Id)
);
GO

INSERT INTO Users (FullName, Email, PasswordHash, IsActive)
VALUES ('Mohammad Ayoub', 'mohammadayoub@helpdesk.com', 'Test@1234', 1);

INSERT INTO Users (FullName, Email, PasswordHash, IsActive)
VALUES ('Ali Mahmoud', 'alimahmoud@helpdesk.com', 'Test@1234', 1);
GO

INSERT INTO Categories (Name, IsActive) VALUES ('Software', 1);
INSERT INTO Categories (Name, IsActive) VALUES ('Hardware', 1);
INSERT INTO Categories (Name, IsActive) VALUES ('Network', 1);
GO

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('Email not working', 'Unable to send or receive emails since this morning. Outlook shows an error about server connection.', 1, 1, 'Open');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('Monitor flickering', 'The monitor on desk 4B is flickering intermittently. Already tried replacing the cable.', 2, 2, 'InProgress');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('VPN connection drops', 'VPN disconnects after about 10 minutes of inactivity. Need a permanent fix.', 3, 1, 'Open');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status)
VALUES ('Install Adobe Acrobat', 'Need Adobe Acrobat Pro installed on my workstation for PDF editing.', 1, 2, 'Closed');
GO

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU)
VALUES (1, 'I have checked the email server and it seems to be running fine. Can you try restarting Outlook?', 1);

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU)
VALUES (1, 'Restarted Outlook but still getting the same error. Screenshot attached.', 2);

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU)
VALUES (2, 'Ordered a replacement monitor. Should arrive by Friday.', 1);
GO

PRINT 'Database setup complete!';
PRINT 'Default login: mohammadayoub@helpdesk.com / Test@1234';
GO
