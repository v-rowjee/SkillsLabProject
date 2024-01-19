USE master
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='SkillsLabDB')
BEGIN
	DROP DATABASE [SkillsLabDB]
END
GO

CREATE DATABASE [SkillsLabDB]
GO
USE [SkillsLabDB]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Role')
BEGIN
	CREATE TABLE Role
	(
		RoleId TINYINT NOT NULL IDENTITY(1,1),
		Type VARCHAR(MAX) NOT NULL,

		CONSTRAINT [PK_Role] PRIMARY KEY (RoleId),
	);
END


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Status')
BEGIN
	CREATE TABLE Status
	(
		StatusId TINYINT NOT NULL IDENTITY(1,1),
		Type VARCHAR(MAX) NOT NULL,

		CONSTRAINT [PK_Status] PRIMARY KEY (StatusId),
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Department')
BEGIN
	CREATE TABLE Department
	(
		DepartmentId TINYINT NOT NULL IDENTITY(1,1),
		Title NVARCHAR(MAX) NOT NULL,

		CONSTRAINT [PK_Department] PRIMARY KEY (DepartmentId),
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee')
BEGIN
	CREATE TABLE Employee
	(
		EmployeeId SMALLINT NOT NULL IDENTITY(1,1),
		FirstName NVARCHAR(MAX) NOT NULL,
		LastName NVARCHAR(MAX) NOT NULL,
		NIC NVARCHAR(48),
		PhoneNumber NVARCHAR(48),
		DepartmentId TINYINT NOT NULL,

		CONSTRAINT [PK_Employee] PRIMARY KEY (EmployeeId),
		CONSTRAINT [FK_Employee_DepartmentId_Department_DepartmentId] FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId),
		CONSTRAINT [UQ_Employee_NIC] UNIQUE (NIC),
		CONSTRAINT [UQ_Employee_PhoneNumber] UNIQUE (PhoneNumber)
	);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Notification')
BEGIN
	CREATE TABLE Notification
	(
		NotificationId SMALLINT NOT NULL IDENTITY(1,1),
		EmployeeId SMALLINT NOT NULL,
		EmployeeRoleId TINYINT NOT NULL,
		Message VARCHAR(MAX) NOT NULL,
		IsRead	BIT NOT NULL DEFAULT 0,
		ReceivedOn DATETIME NOT NULL DEFAULT GETDATE(),

		CONSTRAINT [PK_Notification] PRIMARY KEY (NotificationId),
		CONSTRAINT [FK_Notification_EmployeeId_Employee_EmployeeId] FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId),
		CONSTRAINT [FK_Notification_EmployeeRoleId_Role_RoleId] FOREIGN KEY (EmployeeRoleId) REFERENCES Role(RoleId)
	);
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AppUser')
BEGIN
	CREATE TABLE AppUser
	(
		AppUserId SMALLINT NOT NULL IDENTITY(1,1),
		Email NVARCHAR(255) NOT NULL,
		Password NVARCHAR(MAX) NOT NULL,
		EmployeeId SMALLINT NOT NULL,
		IsActive BIT NOT NULL CONSTRAINT [DF_AppUser_IsActive] DEFAULT 1,
		CreatedOn DATETIME NOT NULL CONSTRAINT [DF_AppUser_CreatedOn] DEFAULT GETDATE(),
		UpdatedOn DATETIME NOT NULL CONSTRAINT [DF_AppUser_UpdatedOn] DEFAULT GETDATE(),

		CONSTRAINT [PK_AppUser] PRIMARY KEY (AppUserId),
		CONSTRAINT [FK_AppUser_EmployeeId_Employee_EmployeeId] FOREIGN KEY (EmployeeId) REFERENCES Employee,
		CONSTRAINT [UQ_AppUser_Email] UNIQUE (Email)
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserRole')
BEGIN
	CREATE TABLE UserRole
	(
		AppUserId SMALLINT NOT NULL,
		RoleId TINYINT NOT NULL CONSTRAINT [DF_UserRole_RoleId] DEFAULT 1,

		CONSTRAINT [PK_UserRole] PRIMARY KEY (AppUserId, RoleId),
		CONSTRAINT [FK_UserRole_AppUserId_AppUser_AppUserId] FOREIGN KEY (AppUserId) REFERENCES AppUser,
		CONSTRAINT [FK_UserRole_RoleId_Role_RoleId] FOREIGN KEY (RoleId) REFERENCES Role,
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Training')
BEGIN
	CREATE TABLE Training
	(
		TrainingId SMALLINT NOT NULL IDENTITY(1,1),
		Title NVARCHAR(MAX) NOT NULL,
		Description NVARCHAR(MAX) NOT NULL,
		Deadline DATETIME NOT NULL,
		Capacity SMALLINT NOT NULL,
		PriorityDepartmentId TINYINT,
		IsClosed BIT NOT NULL CONSTRAINT [DF_Training_IsClosed] DEFAULT 0,
		IsActive BIT NOT NULL CONSTRAINT [DF_Training_IsActive] DEFAULT 1,
		CreatedOn DATETIME NOT NULL CONSTRAINT [DF_Training_CreatedOn] DEFAULT GETDATE(),
		UpdatedOn DATETIME NOT NULL CONSTRAINT [DF_Training_UpdatedOn] DEFAULT GETDATE(),

		CONSTRAINT [PK_Training] PRIMARY KEY (TrainingId),
		CONSTRAINT [FK_Training_DepartmentId_Department_DepartmentId] FOREIGN KEY (PriorityDepartmentId) REFERENCES Department(DepartmentId),
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PreRequisite')
BEGIN
	CREATE TABLE PreRequisite
	(
		PreRequisiteId SMALLINT NOT NULL IDENTITY(1,1),
		Detail NVARCHAR(MAX) NOT NULL,

		CONSTRAINT [PK_PreRequisite] PRIMARY KEY (PreRequisiteId),
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TrainingPreRequisite')
BEGIN
	CREATE TABLE TrainingPreRequisite
	(
		TrainingId SMALLINT NOT NULL,
		PreRequisiteId SMALLINT NOT NULL,

		CONSTRAINT [PK_Training_Prerequisite] PRIMARY KEY (TrainingId, PrerequisiteId),
		CONSTRAINT [FK_TrainingPreRequisite_TrainingId_PreRequisiteId] FOREIGN KEY (TrainingId) REFERENCES Training(TrainingId),
		CONSTRAINT [FK_TrainingPreRequisite_Department_DepartmentId] FOREIGN KEY (PreRequisiteId) REFERENCES PreRequisite(PreRequisiteId),
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enrollment')
BEGIN
	CREATE TABLE Enrollment
	(
		EnrollmentId SMALLINT NOT NULL IDENTITY(1,1),
		EmployeeId SMALLINT NOT NULL,
		TrainingId SMALLINT NOT NULL,
		StatusId TINYINT NOT NULL,
		IsActive BIT NOT NULL CONSTRAINT [DF_Enrollment_IsActive] DEFAULT 1,
		CreatedOn DATETIME NOT NULL CONSTRAINT [DF_Enrollment_CreatedOn] DEFAULT GETDATE(),
		UpdatedOn DATETIME NOT NULL CONSTRAINT [DF_Enrollment_UpdatedOn] DEFAULT GETDATE(),

		CONSTRAINT [PK_Enrollment] PRIMARY KEY (EnrollmentId),
		CONSTRAINT [FK_Enrollment_EmployeeId_Employee_EmployeeId] FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId),
		CONSTRAINT [FK_Enrollment_TrainingId_Training_TrainingId] FOREIGN KEY (TrainingId) REFERENCES Training(TrainingId),
		CONSTRAINT [FK_Enrollment_StatusId_Status_StatusId] FOREIGN KEY (StatusId) REFERENCES Status(StatusId)
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Proof')
BEGIN
	CREATE TABLE Proof
	(
		ProofId SMALLINT NOT NULL IDENTITY(1,1),
		EnrollmentId SMALLINT NOT NULL,
		Attachment NVARCHAR(MAX) NOT NULL,

		CONSTRAINT [PK_Proof] PRIMARY KEY (ProofId),
		CONSTRAINT [FK_Proof_EnrollmentId_Enrollment_EnrollmentId] FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(EnrollmentId)
	);
END



IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DeclinedEnrollment')
BEGIN
	CREATE TABLE DeclinedEnrollment
	(
		DeclinedEnrollmentId SMALLINT NOT NULL IDENTITY(1,1),
		EnrollmentId SMALLINT NOT NULL,
		Reason TEXT NOT NULL,

		CONSTRAINT [PK_DeclinedEnrollment] PRIMARY KEY (DeclinedEnrollmentId),
		CONSTRAINT [FK_DeclinedEnrollment_DeclinedEnrollmentId_Enrollment_EnrollmentId] FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(EnrollmentId),
	);
END















------------------------------
----------- INDEXES ----------
------------------------------


USE [SkillsLabDB]
GO

-- User logs in with email
CREATE UNIQUE NONCLUSTERED INDEX IX_AppUser_Email ON AppUser (Email);

-- Phone Number and NIC unique validation when registering
CREATE NONCLUSTERED INDEX IX_Employee_PhoneNumber ON Employee (PhoneNumber);
CREATE NONCLUSTERED INDEX IX_Employee_NIC ON Employee (NIC);

-- Manager changes status of enrollment only
CREATE NONCLUSTERED INDEX IX_Enrollment_StatusId ON Enrollment (StatusId);













------------------------------
---------- TRIGGERS ----------
------------------------------


USE [SkillsLabDB]
GO

CREATE TRIGGER TRG_AppUser_Update
ON AppUser
AFTER UPDATE
AS
BEGIN
    UPDATE AppUser
    SET UpdatedOn = GETDATE()
    FROM AppUser
    INNER JOIN inserted ON AppUser.AppUserId = inserted.AppUserId
END
GO
CREATE TRIGGER TRG_Employee_Update
ON Employee
AFTER UPDATE
AS
BEGIN
    UPDATE AppUser
    SET UpdatedOn = GETDATE()
    FROM AppUser
    INNER JOIN inserted ON AppUser.EmployeeId = inserted.EmployeeId
END
GO

CREATE TRIGGER TRG_Training_Update
ON Training
AFTER UPDATE
AS
BEGIN
    UPDATE Training
    SET UpdatedOn = GETDATE()
	WHERE TrainingId IN (SELECT DISTINCT TrainingId FROM inserted)
END
GO

CREATE TRIGGER TRG_Enrollment_Update
ON Enrollment
AFTER UPDATE
AS
BEGIN
    UPDATE Enrollment
    SET UpdatedOn = GETDATE()
    FROM Enrollment
    INNER JOIN inserted ON Enrollment.EnrollmentId = inserted.EnrollmentId
END
GO














------------------------------
------ STORED PROCEDURE ------
------------------------------


USE [SkillsLabDB]
GO

CREATE PROCEDURE dbo.ProcessEnrollments
    @TrainingId SMALLINT
AS
BEGIN
    DECLARE @Capacity SMALLINT;

    SELECT @Capacity = Capacity
    FROM Training
    WHERE TrainingId = @TrainingId;

    WITH SelectedEnrollments AS (
        SELECT TOP (@Capacity) 
            e.EnrollmentId,
            e.StatusId
        FROM
            Enrollment e
        INNER JOIN
            Employee emp ON e.EmployeeId = emp.EmployeeId
        INNER JOIN
            Training t ON e.TrainingId = t.TrainingId
        WHERE
            e.TrainingId = @TrainingId
            AND e.StatusId <> 3
        ORDER BY
            CASE 
                WHEN e.StatusId = 2 THEN 1 -- Manager approval required
                WHEN e.StatusId = 1 AND t.PriorityDepartmentId IS NOT NULL AND emp.DepartmentId = t.PriorityDepartmentId THEN 2 -- Priority department
                ELSE 3 -- First come first serve
            END,
            e.UpdatedOn
    )
	
    UPDATE Enrollment
    SET StatusId = CASE
                      WHEN o.StatusId <> 3 THEN 2 -- Approved
                      ELSE 3 -- Declined
                   END
    FROM
        Enrollment e
    LEFT JOIN
        SelectedEnrollments o ON e.EnrollmentId = o.EnrollmentId
    WHERE
        e.TrainingId = @TrainingId;

    UPDATE Training
    SET IsClosed = 1
    WHERE TrainingId = @TrainingId;

END
GO



CREATE PROCEDURE dbo.AutoProcessEnrollments
AS
BEGIN
    DECLARE @TrainingId SMALLINT;
    DECLARE @Capacity SMALLINT;

    DECLARE trainingCursor CURSOR FOR
    SELECT TrainingId, Capacity
    FROM Training
    WHERE Deadline < GETDATE() AND IsClosed = 0;

    OPEN trainingCursor;

    FETCH NEXT FROM trainingCursor INTO @TrainingId, @Capacity;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.ProcessEnrollments @TrainingId;

        FETCH NEXT FROM trainingCursor INTO @TrainingId, @Capacity;
    END

    CLOSE trainingCursor;
    DEALLOCATE trainingCursor;

END
GO





------------------------------
----- Server Agent Jobs ------
------------------------------

/*
USE msdb;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @schedule_id SMALLINT;

    SELECT @schedule_id = schedule_id
    FROM msdb.dbo.sysschedules
    WHERE name = 'DailySchedule';

    IF @schedule_id IS NOT NULL
    BEGIN
        EXEC dbo.sp_detach_schedule @job_name = 'EnrollmentJob', @schedule_id = @schedule_id;
    END

    IF EXISTS (SELECT 1 FROM msdb.dbo.sysjobs WHERE name = 'EnrollmentJob')
    BEGIN
        EXEC dbo.sp_delete_job @job_name = 'EnrollmentJob';
    END

    IF @schedule_id IS NOT NULL
    BEGIN
        EXEC dbo.sp_delete_schedule @schedule_id = @schedule_id;
    END

    EXEC dbo.sp_add_job
       @job_name = 'EnrollmentJob',
       @enabled = 1;

    EXEC dbo.sp_add_jobstep
       @job_name = 'EnrollmentJob',
       @step_name = 'RunProcessEnrollments',
       @subsystem = 'TSQL',
       @command = 'USE [SkillsLabDB]; EXEC dbo.AutoProcessEnrollments',
       @on_success_action = 1;

    EXEC dbo.sp_add_schedule
       @schedule_name = 'DailySchedule',
       @freq_type = 4,
       @freq_interval = 1, 
       @active_start_time = 135000;  -- midnight

    EXEC dbo.sp_attach_schedule
       @job_name = 'EnrollmentJob',
       @schedule_name = 'DailySchedule';

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'Error: ' + ERROR_MESSAGE();
END CATCH;
GO
*/


------------------------------
------------ DML -------------
------------------------------


 
 USE [SkillsLabDB]

INSERT Role (Type) VALUES ('Employee'), ('Manager'), ('Admin')

INSERT Status (Type) VALUES ('Pending'), ('Accepted'), ('Denied')

INSERT Department (Title) VALUES ('Product and Technology'), ('Human Resource'), ('Customer Support'), ('Cleaning Staff')

INSERT Employee (FirstName, LastName, NIC, PhoneNumber, DepartmentId) VALUES 
('Super', 'User', 'S0705035983519', '+230 5792216', 1),
('Admin', 'User', 'P0705035983510', '57920922', 1),
('Manager', 'User', 'A0705035983510', '5 6920 015', 1),
('Employee', 'User', 'P0205035983511', '57920031', 1),
('William', 'Taylor', 'T0705035983519', '+230 5782216', 1),
('Sophia', 'Jones', 'J0705035983510', '57920716', 1),
('Matthew', 'White', 'W0705035983510', '57920015', 1),
('Eva', 'Perez', 'P02050327893511', '57920011', 1),
('Alex', 'Brown', 'B0192093301298', '59830244', 1),
('Sophie', 'Miller', 'M0192093301298', '59830254', 1),
('Christopher', 'Harris', 'H0192093301298', '59830264', 1),
('Ava', 'Anderson', 'A0192093301298', '59830275', 1),
('Olivia', 'Moore', 'M0192093300298', '59834274', 1),
('Robert', 'Turner', 'T0192093300212', '59832272', 1),
('Aiden', 'Jackson', 'J1192093300298', '59835274', 2),
('Madison', 'Smith', 'S0192093320298', '+230 59830274', 2),
('Mark', 'Johnson', 'J0192093301298', '59830284', 2),
('Emily', 'Davis', 'D0192093301298', '59830294', 2),
('John', 'Doe', 'D0192023501298', '59830204', 2),
('Jane', 'Smith', 'S0192093301298', '59830214', 2),
('Michael', 'Lee', 'L0192093301298', '59830224', 3),
('Emma', 'Williams', 'W0192093301298', '59830234', 3),
('Daniel', 'Clark', 'C0192093301248', '59830304', 3),
('William', 'Clark', 'C0192093301298', '59830314', 3);

INSERT AppUser (Email, Password, EmployeeId) VALUES 
('ved.rowjee@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 1),
('admin@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 2),
('manager@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 3),
('employee@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 4),
('william@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 5),
('sophia@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 6),
('matthew@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 7),
('eva@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 8),
('alex@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 9),
('sophie@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 10),
('christopher@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 11),
('ava@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 12),
('olivia@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 13),
('robert@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 14),
('aiden@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 15),
('madison@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 16),
('mark@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 17),
('emily@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 18),
('john@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 19),
('jane@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 20),
('michael@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 21),
('emma@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 22),
('daniel@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 23),
('william2@ceridian.com', 'AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==', 24);

INSERT UserRole (AppUserId, RoleId) VALUES 
(1,1), (1,2), (1,3), (2,3), (3,2), (4,1)
INSERT UserRole (AppUserId) VALUES 
(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20),(21),(22),(23),(24)

INSERT PreRequisite (Detail) VALUES ('HSC') , ('SC'), ('BSc')

INSERT Training (Title, Description, Deadline, Capacity, PriorityDepartmentId) VALUES ('OOP', 'Learn OOP fundamentals, including classes, objects, methods, inheritance, polymorphism, and encapsulation. Practice implementing OOP in various programming languages and projects.', DATEADD(DAY, 100, GETDATE()), 10, 1)
INSERT Training (Title, Description, Deadline, Capacity, PriorityDepartmentId) VALUES ('Git', 'Understand Git as a distributed version control system, learn to track changes in source code, and collaborate with other developers using Git commands and tools.', DATEADD(DAY, 30, GETDATE()), 15, 1)
INSERT Training (Title, Description, Deadline, Capacity, PriorityDepartmentId) VALUES ('CSS', 'Master CSS to control the presentation, formatting, and layout of HTML elements, ensuring that websites are visually appealing and well-structured', DATEADD(DAY, 120, GETDATE()), 15, 1)
INSERT Training (Title, Description, Deadline, Capacity, PriorityDepartmentId) VALUES ('HTML', 'Learn to structure the content of websites using HTML, the basic language that provides the foundation for web pages', DATEADD(MINUTE, 15, GETDATE()), 5, 1)
INSERT Training (Title, Description, Deadline, Capacity, PriorityDepartmentId) VALUES ('JS', 'Explore JavaScript to make web pages more dynamic and interactive, allowing for tasks such as form validation, content changes, and image manipulation', DATEADD(DAY, 100, GETDATE()), 15, 2)
INSERT Training (Title, Description, Deadline, Capacity, PriorityDepartmentId) VALUES ('React', 'Discover React, a popular JavaScript library for building user interfaces. Learn to work with JSX, components, state, and props, and create dynamic and interactive web applications.', DATEADD(DAY, 10, GETDATE()), 15, null)

INSERT TrainingPreRequisite (TrainingId, PreRequisiteId) VALUES (1,1), (2,1), (2,2), (3,1), (4,3), (5,1), (5,2)

INSERT Enrollment (EmployeeId, TrainingId, StatusId) VALUES 
(1,1,1), (4,1,2),
(1,2,1), (4,2,1),
(4,3,2), (5,3,1), (6,3,1), (7,3,1), (8,3,1), (9,3,1), (10,3,1), (11,3,1), (12,3,1), (13,3,1), (14,3,1), (15,3,1), (16,3,1), (17,3,1),
(9,4,1), (10,4,1), (11,4,1), (12,4,1), (5,4,2),
(4,5,1), (21,5,1),
(5,6,1), (20,6,1)

INSERT Proof (EnrollmentId, Attachment) VALUES (1, 'https://source.unsplash.com/random/?technology')
INSERT Proof (EnrollmentId, Attachment) VALUES (2, 'https://source.unsplash.com/random/?dog')

INSERT Proof (EnrollmentId, Attachment) VALUES (3, 'https://source.unsplash.com/random/?code')
INSERT Proof (EnrollmentId, Attachment) VALUES (3, 'https://source.unsplash.com/random/?cat')

INSERT Proof (EnrollmentId, Attachment) VALUES (4, 'https://source.unsplash.com/random/?job')
INSERT Proof (EnrollmentId, Attachment) VALUES (4, 'https://source.unsplash.com/random/?cat')

INSERT Proof (EnrollmentId, Attachment) VALUES (5, 'https://source.unsplash.com/random/?css')
INSERT Proof (EnrollmentId, Attachment) VALUES (6, 'https://source.unsplash.com/random/?sky')
INSERT Proof (EnrollmentId, Attachment) VALUES (7, 'https://source.unsplash.com/random/?html')
INSERT Proof (EnrollmentId, Attachment) VALUES (8, 'https://source.unsplash.com/random/?lake')
INSERT Proof (EnrollmentId, Attachment) VALUES (9, 'https://source.unsplash.com/random/?drink')
INSERT Proof (EnrollmentId, Attachment) VALUES (10, 'https://source.unsplash.com/random/?tv')
INSERT Proof (EnrollmentId, Attachment) VALUES (11, 'https://source.unsplash.com/random/?react')
INSERT Proof (EnrollmentId, Attachment) VALUES (12, 'https://source.unsplash.com/random/?happy')
INSERT Proof (EnrollmentId, Attachment) VALUES (13, 'https://source.unsplash.com/random/?education')
INSERT Proof (EnrollmentId, Attachment) VALUES (14, 'https://source.unsplash.com/random/?learning')
INSERT Proof (EnrollmentId, Attachment) VALUES (15, 'https://source.unsplash.com/random/?food')
INSERT Proof (EnrollmentId, Attachment) VALUES (16, 'https://source.unsplash.com/random/?apple')
INSERT Proof (EnrollmentId, Attachment) VALUES (17, 'https://source.unsplash.com/random/?dog')
INSERT Proof (EnrollmentId, Attachment) VALUES (18, 'https://source.unsplash.com/random/?cat')
INSERT Proof (EnrollmentId, Attachment) VALUES (19, 'https://source.unsplash.com/random/?nature')

INSERT Proof (EnrollmentId, Attachment) VALUES (20, 'https://source.unsplash.com/random/?travel')
INSERT Proof (EnrollmentId, Attachment) VALUES (21, 'https://source.unsplash.com/random/?music')
INSERT Proof (EnrollmentId, Attachment) VALUES (22, 'https://source.unsplash.com/random/?art')
INSERT Proof (EnrollmentId, Attachment) VALUES (23, 'https://source.unsplash.com/random/?architecture')

INSERT Proof (EnrollmentId, Attachment) VALUES (23, 'https://source.unsplash.com/random/?architecture')
INSERT Proof (EnrollmentId, Attachment) VALUES (24, 'https://source.unsplash.com/random/?fashion')
INSERT Proof (EnrollmentId, Attachment) VALUES (24, 'https://source.unsplash.com/random/?fashion')
INSERT Proof (EnrollmentId, Attachment) VALUES (24, 'https://source.unsplash.com/random/?car')

INSERT DeclinedEnrollment (EnrollmentId, Reason) VALUES (1, 'Wrong doc')





------------------------------
--------- Hangfire -----------
------------------------------



USE master
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name='SkillsLabHangfire')
BEGIN
	DROP DATABASE SkillsLabHangfire
END
GO

CREATE DATABASE [SkillsLabHangfire]
GO

USE [SkillsLabDB]
GO