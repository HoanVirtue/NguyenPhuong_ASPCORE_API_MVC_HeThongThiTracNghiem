-- Table Subject
CREATE DATABASE MultipleChoiceTestDB;

USE MultipleChoiceTestDB;
Scaffold-DbContext "Data Source=.;Initial Catalog=MultipleChoiceTestDB;Integrated Security=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

-- Table Subject
CREATE TABLE Subject (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SubjectName VARCHAR(255),
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

-- Table Lesson
CREATE TABLE Lesson (
    Id INT PRIMARY KEY IDENTITY(1,1),
    LessonName VARCHAR(255),
    SubjectId INT, -- Foreign key linked to the Subject table
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (SubjectId) REFERENCES Subject(Id)
);

-- Table Question
CREATE TABLE Question (
    Id INT PRIMARY KEY IDENTITY(1,1),
    QuestionText TEXT,
    Choices TEXT,
    CorrectAnswer TEXT,
    SubjectId INT, -- Foreign key linked to the Subject table
    LessonId INT, -- Foreign key linked to the Lesson table
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (SubjectId) REFERENCES Subject(Id),
    FOREIGN KEY (LessonId) REFERENCES Lesson(Id)
);

-- Table Exam
CREATE TABLE Exam (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ExamName VARCHAR(255),
    Duration INT, -- Duration in minutes
    TotalQuestions INT,
    SubjectId INT, -- Foreign key linked to the Subject table
    LessonId INT, -- Foreign key linked to the Lesson table
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (SubjectId) REFERENCES Subject(Id),
    FOREIGN KEY (LessonId) REFERENCES Lesson(Id)
);

-- Table User
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(255),
    Email VARCHAR(255),
    Gender VARCHAR(10),
    DateOfBirth DATE,
    Phone VARCHAR(15),
    AccountName VARCHAR(50),
    PasswordHash VARCHAR(255),
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

-- Table ExamAttempt
CREATE TABLE ExamAttempt (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT, -- Foreign key linked to the User table
    ExamId INT, -- Foreign key linked to the Exam table
    QuestionId INT, -- Foreign key linked to the Question table
    Answer TEXT,
    IsCorrect BIT,
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (UserId) REFERENCES [User](Id),
    FOREIGN KEY (ExamId) REFERENCES Exam(Id),
    FOREIGN KEY (QuestionId) REFERENCES Question(Id)
);

-- Table ExamResult
CREATE TABLE ExamResult (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ExamId INT, -- Foreign key linked to the Exam table
    UserId INT, -- Foreign key linked to the User table
    CompletionTime DATE,
    CorrectAnswersCount INT,
    IncorrectAnswersCount INT,
    UnansweredQuestionsCount INT,
    Score DECIMAL(5, 2),
    Rank VARCHAR(50),
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (ExamId) REFERENCES Exam(Id),
    FOREIGN KEY (UserId) REFERENCES [User](Id)
);

-- Table Session
CREATE TABLE Session (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT, -- Foreign key linked to the User table
    SessionStart DATETIME,
    SessionEnd DATETIME,
    IsActive BIT,
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (UserId) REFERENCES [User](Id)
);

-- Table AccessStatistics
CREATE TABLE AccessStatistics (
    Id INT PRIMARY KEY IDENTITY(1,1),
    StatisticsDate DATE,
    DailyAccessCount INT,
    MonthlyAccessCount INT,
    YearlyAccessCount INT,
    TotalAccessCount INT,
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

-- Table Feedback
CREATE TABLE Feedback (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(255),
    FullName VARCHAR(255),
    Email VARCHAR(255),
    Phone VARCHAR(15),
    Address VARCHAR(255),
    Content TEXT,
    CreatedDate DATE,
    CreatedBy VARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy VARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);