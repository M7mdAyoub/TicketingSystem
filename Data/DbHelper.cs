using Microsoft.Data.Sqlite;

namespace HelpdeskApp.Data
{
    public static class DbHelper
    {
        private static string? _connectionString;

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static string ConnectionString
        {
            get => _connectionString ?? throw new InvalidOperationException("Connection string not configured. Call SetConnectionString first.");
            set => _connectionString = value;
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        /// <summary>
        /// Initialize the SQLite database with schema and seed data if tables don't exist.
        /// </summary>
        public static void InitializeDatabase()
        {
            using var conn = GetConnection();
            conn.Open();

            // Check if tables exist
            using var checkCmd = new SqliteCommand(
                "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Users'", conn);
            long tableCount = (long)checkCmd.ExecuteScalar()!;

            if (tableCount > 0) return; // Already initialized

            // Create tables
            string createSql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedDate TEXT NOT NULL DEFAULT (datetime('now'))
                );

                CREATE TABLE IF NOT EXISTS Categories (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    IsActive INTEGER NOT NULL DEFAULT 1,
                    CreatedDate TEXT NOT NULL DEFAULT (datetime('now'))
                );

                CREATE TABLE IF NOT EXISTS Tickets (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    CategoryId INTEGER NOT NULL,
                    CreatedBy INTEGER NOT NULL,
                    Status TEXT NOT NULL DEFAULT 'Open',
                    CreatedDate TEXT NOT NULL DEFAULT (datetime('now')),
                    IsDeleted INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
                    FOREIGN KEY (CreatedBy) REFERENCES Users(Id)
                );

                CREATE TABLE IF NOT EXISTS TicketComments (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    TicketId INTEGER NOT NULL,
                    CommentText TEXT NOT NULL,
                    CreatedByU INTEGER NOT NULL,
                    CreatedDate TEXT NOT NULL DEFAULT (datetime('now')),
                    FOREIGN KEY (TicketId) REFERENCES Tickets(Id),
                    FOREIGN KEY (CreatedByU) REFERENCES Users(Id)
                );
            ";

            using var createCmd = new SqliteCommand(createSql, conn);
            createCmd.ExecuteNonQuery();

            // Seed data
            string seedSql = @"
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Mohammad Ayoub', 'mohammadayoub@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Ali Mahmoud', 'alimahmoud@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Mohammad Al-Ahmad', 'mohammad@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Fatima Al-Zahra', 'fatima@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Omar Khaled', 'omar@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Aya Hassan', 'aya@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Yousef Al-Rashid', 'yousef@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Nour Al-Din', 'nour@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Layla Ibrahim', 'layla@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Ahmad Mansour', 'ahmad@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Sara Al-Harbi', 'sara@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Khaled Abu Hamza', 'khaled@helpdesk.com', 'Test@1234', 1);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Rania Taha', 'rania@helpdesk.com', 'Test@1234', 0);
                INSERT INTO Users (FullName, Email, PasswordHash, IsActive) VALUES ('Hussein Al-Jabri', 'hussein@helpdesk.com', 'Test@1234', 1);

                INSERT INTO Categories (Name, IsActive) VALUES ('Software', 1);
                INSERT INTO Categories (Name, IsActive) VALUES ('Hardware', 1);
                INSERT INTO Categories (Name, IsActive) VALUES ('Network', 1);

                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Email not working', 'Unable to send or receive emails since this morning. Outlook shows an error about server connection.', 1, 1, 'Open', '2026-02-01 08:30:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Monitor flickering', 'The monitor on desk 4B is flickering intermittently. Already tried replacing the cable.', 2, 2, 'InProgress', '2026-02-02 09:15:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('VPN connection drops', 'VPN disconnects after about 10 minutes of inactivity. Need a permanent fix.', 3, 1, 'Open', '2026-02-03 10:00:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Install Adobe Acrobat', 'Need Adobe Acrobat Pro installed on my workstation for PDF editing.', 1, 2, 'Closed', '2026-02-04 11:20:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Printer not responding', 'The HP LaserJet printer on the 3rd floor is not responding to any print jobs.', 2, 3, 'Open', '2026-02-05 08:30:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Cannot access shared folder', 'I cannot access the shared network folder. It gives me a permission denied error.', 3, 4, 'Open', '2026-02-06 09:15:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Outlook keeps crashing', 'Every time I open Outlook it crashes after 2 minutes. I have reinstalled it but the problem continues.', 1, 5, 'InProgress', '2026-02-07 10:00:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Request new laptop', 'My current laptop is very slow and freezes frequently. I need a new laptop for my daily work.', 2, 6, 'Open', '2026-02-08 11:20:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('WiFi disconnecting frequently', 'The WiFi keeps disconnecting every few minutes in Building B.', 3, 7, 'InProgress', '2026-02-09 13:45:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Install Microsoft Office', 'I need Microsoft Office 365 installed on my new workstation.', 1, 8, 'Closed', '2026-02-10 08:00:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Mouse not working', 'My wireless mouse stopped working. I replaced the batteries but it still does not connect.', 2, 9, 'Open', '2026-02-11 14:30:00');
                INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
                VALUES ('Slow internet speed', 'The internet speed is extremely slow since last week. Affecting video calls.', 3, 10, 'InProgress', '2026-02-12 09:50:00');

                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (1, 'I have checked the email server and it seems to be running fine. Can you try restarting Outlook?', 1, '2026-02-01 10:00:00');
                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (1, 'Restarted Outlook but still getting the same error. Screenshot attached.', 2, '2026-02-01 11:30:00');
                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (2, 'Ordered a replacement monitor. Should arrive by Friday.', 1, '2026-02-02 10:00:00');
                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (5, 'I have checked the printer and it seems to be a paper jam issue.', 1, '2026-02-05 10:00:00');
                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (7, 'This looks like an add-in conflict. Can you try starting Outlook in safe mode?', 1, '2026-02-07 11:00:00');
                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (7, 'Tried safe mode and it works fine. How do I find the problematic add-in?', 5, '2026-02-07 13:00:00');
                INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                VALUES (9, 'We are investigating the WiFi issue in Building B. A technician will visit today.', 1, '2026-02-09 15:00:00');
            ";

            using var seedCmd = new SqliteCommand(seedSql, conn);
            seedCmd.ExecuteNonQuery();
        }
    }
}
