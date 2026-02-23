-- ============================================
-- Helpdesk Seed Data - Arabic Names (English transliteration)
-- Adds users, tickets, and comments for pagination testing
-- ============================================

USE HelpdeskDB;
GO

-- Insert more users
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
GO

-- Insert more tickets (need 20+ so pagination shows multiple pages at pageSize=5)
-- Users IDs: 1=Admin, 2=Test, 3=Mohammad, 4=Fatima, 5=Omar, 6=Aya, 7=Yousef, 8=Nour, 9=Layla, 10=Ahmad, 11=Sara, 12=Khaled, 13=Rania, 14=Hussein

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Printer not responding', 'The HP LaserJet printer on the 3rd floor is not responding to any print jobs. I tried restarting it but still nothing works.', 2, 3, 'Open', '2026-02-01 08:30:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Cannot access shared folder', 'I cannot access the shared network folder \\server\finance. It gives me a permission denied error.', 3, 4, 'Open', '2026-02-02 09:15:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Outlook keeps crashing', 'Every time I open Outlook it crashes after 2 minutes. I have reinstalled it but the problem continues.', 1, 5, 'InProgress', '2026-02-03 10:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Request new laptop', 'My current laptop is very slow and freezes frequently. I need a new laptop for my daily work.', 2, 6, 'Open', '2026-02-04 11:20:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('WiFi disconnecting frequently', 'The WiFi keeps disconnecting every few minutes in Building B. Multiple employees are affected.', 3, 7, 'InProgress', '2026-02-05 13:45:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Install Microsoft Office', 'I need Microsoft Office 365 installed on my new workstation. Desk number 2A-15.', 1, 8, 'Closed', '2026-02-06 08:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Mouse not working', 'My wireless mouse stopped working. I replaced the batteries but it still does not connect.', 2, 9, 'Open', '2026-02-07 14:30:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Slow internet speed', 'The internet speed is extremely slow since last week. It is affecting our video calls and uploads.', 3, 10, 'InProgress', '2026-02-08 09:50:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Software license expired', 'The AutoCAD license has expired and I cannot open my project files. Urgent renewal needed.', 1, 3, 'Open', '2026-02-09 07:30:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Keyboard keys stuck', 'Several keys on my keyboard are stuck and not responding. It is a mechanical keyboard model K95.', 2, 4, 'Open', '2026-02-10 16:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Cannot connect to VPN', 'When I try to connect to the company VPN from home it shows authentication failed error.', 3, 5, 'Open', '2026-02-11 10:30:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Windows update failed', 'Windows update has been failing for the past 3 days. Error code 0x80070002 keeps appearing.', 1, 6, 'InProgress', '2026-02-12 11:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Projector not displaying', 'The meeting room projector in Room 5B is not displaying anything. The HDMI cable seems fine.', 2, 7, 'Open', '2026-02-13 08:45:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Email storage full', 'My mailbox is full and I cannot receive new emails. Please increase the storage limit.', 1, 8, 'Open', '2026-02-14 15:20:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Network cable damaged', 'The network cable at desk 4C is damaged and I have no wired internet connection.', 3, 9, 'Closed', '2026-02-15 12:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Install antivirus software', 'My PC does not have any antivirus installed. Please install the company approved antivirus.', 1, 10, 'Open', '2026-02-16 09:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Monitor no display', 'The Dell monitor suddenly went black and the power light is blinking orange. No display at all.', 2, 12, 'InProgress', '2026-02-17 13:10:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Firewall blocking website', 'I cannot access the supplier portal website. It seems the firewall is blocking it. Need access urgently.', 3, 14, 'Open', '2026-02-18 10:40:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Backup not running', 'The daily backup job has not run for the past 5 days. The backup logs show a timeout error.', 1, 3, 'Open', '2026-02-19 07:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Headset microphone issue', 'My headset microphone is not being detected by the system. I tried different USB ports.', 2, 4, 'Open', '2026-02-20 14:00:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('File server access denied', 'Getting access denied on the main file server. My permissions were working fine yesterday.', 3, 5, 'InProgress', '2026-02-21 08:20:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('Install Visual Studio', 'I need Visual Studio 2022 Professional installed for the new development project starting next week.', 1, 7, 'Open', '2026-02-22 09:30:00');

INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate)
VALUES ('UPS beeping continuously', 'The UPS under my desk is beeping non-stop. It might need a battery replacement.', 2, 8, 'Open', '2026-02-23 11:15:00');
GO

-- Insert more comments on various tickets
-- Ticket IDs for new tickets start at 5 (since we had 4 from initial seed)

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (5, 'I have checked the printer and it seems to be a paper jam issue. Cleared it now.', 1, '2026-02-01 10:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (5, 'Thank you, it is printing now but the quality is very low. Can you check the toner?', 3, '2026-02-01 11:30:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (6, 'I will check the permissions on the file server. What is your username?', 1, '2026-02-02 10:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (6, 'My username is fatima.zahra. I need access to the quarterly reports folder.', 4, '2026-02-02 10:45:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (7, 'This looks like an add-in conflict. Can you try starting Outlook in safe mode?', 1, '2026-02-03 11:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (7, 'Tried safe mode and it works fine. How do I find the problematic add-in?', 5, '2026-02-03 13:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (7, 'Disable all add-ins and enable them one by one to find the cause.', 1, '2026-02-03 14:30:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (9, 'We are investigating the WiFi issue in Building B. A technician will visit today.', 1, '2026-02-05 15:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (9, 'Found a faulty access point on the 2nd floor. Replacing it now.', 10, '2026-02-05 17:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (12, 'Internet speed test shows 5 Mbps. It should be 100 Mbps. Escalating to ISP.', 1, '2026-02-08 11:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (13, 'The license renewal request has been sent to procurement. Expected in 2 days.', 1, '2026-02-09 09:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (13, 'Procurement approved the renewal. License key should arrive by email today.', 10, '2026-02-10 08:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (16, 'Windows update fix: try running sfc /scannow and then retry the update.', 1, '2026-02-12 14:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (21, 'I have replaced the monitor with a spare one. The original is sent for repair.', 1, '2026-02-17 15:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (22, 'The website URL has been whitelisted. Please try again and confirm.', 1, '2026-02-18 12:00:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (22, 'Confirmed, I can access the supplier portal now. Thank you!', 14, '2026-02-18 13:30:00');

INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
VALUES (25, 'The access issue is related to a group policy change. I will restore your permissions.', 1, '2026-02-21 09:30:00');
GO

PRINT 'Additional seed data inserted successfully!';
PRINT 'Total: 12 new users, 23 new tickets, 17 new comments';
GO
