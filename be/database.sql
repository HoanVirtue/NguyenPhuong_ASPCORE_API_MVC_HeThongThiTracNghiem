-- Table Subject
CREATE DATABASE MultipleChoiceTestDB;

USE MultipleChoiceTestDB;

select * from exam

-- Table Subject
CREATE TABLE Subject (
    Id INT PRIMARY KEY IDENTITY(1,1),
	Code varchar(100),
    SubjectName NVARCHAR(255),
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

-- Table Lesson
CREATE TABLE Lesson (
    Id INT PRIMARY KEY IDENTITY(1,1),
	Code varchar(100),
    LessonName NVARCHAR(255),
    SubjectId INT, -- Foreign key linked to the Subject table
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (SubjectId) REFERENCES Subject(Id)
);

CREATE TABLE QuestionType (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50),  -- Tên loại câu hỏi (Trắc nghiệm, Tự luận, v.v.)
    Description NVARCHAR(255),  -- Mô tả về loại câu hỏi (Tuỳ chọn)
	CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

-- Table Question
CREATE TABLE Question (
    Id INT PRIMARY KEY IDENTITY(1,1),
    QuestionText NVARCHAR(MAX),
    Choices NVARCHAR(MAX),
    CorrectAnswer NVARCHAR(MAX),
	AnswerExplanation NVARCHAR(MAX),
    SubjectId INT, -- Foreign key linked to the Subject table
    LessonId INT, -- Foreign key linked to the Lesson table
	QuestionTypeId INT,
	AudioFilePath NVARCHAR(255),
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (SubjectId) REFERENCES Subject(Id),
    FOREIGN KEY (LessonId) REFERENCES Lesson(Id),
	FOREIGN KEY (QuestionTypeId) REFERENCES QuestionType(Id)
);


-- Table Exam
CREATE TABLE Exam (
    Id INT PRIMARY KEY IDENTITY(1,1),
	Code varchar(100),
    ExamName NVARCHAR(255),
    Duration INT, -- Duration in minutes
    TotalQuestions INT,
    SubjectId INT, -- Foreign key linked to the Subject table
    LessonId INT, -- Foreign key linked to the Lesson table
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
    FOREIGN KEY (SubjectId) REFERENCES Subject(Id),
    FOREIGN KEY (LessonId) REFERENCES Lesson(Id)
);

-- Table User
CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(255),
    Email NVARCHAR(255),
    Gender NVARCHAR(10),
    DateOfBirth DATE,
    Phone NVARCHAR(15),
    AccountName NVARCHAR(50),
    PasswordHash NVARCHAR(255),
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0, -- Cột IsDeleted
	IsAdmin BIT
);


-- Table ExamAttempt
CREATE TABLE ExamAttempt (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT, -- Foreign key linked to the User table
    ExamId INT, -- Foreign key linked to the Exam table
    QuestionId INT, -- Foreign key linked to the Question table
    Answer NVARCHAR(MAX),
    IsCorrect BIT,
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
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
    Rank NVARCHAR(50),
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
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
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
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
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

-- Table Feedback
CREATE TABLE Feedback (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255),
    FullName NVARCHAR(255),
    Email NVARCHAR(255),
    Phone NVARCHAR(15),
    Address NVARCHAR(255),
    Content NVARCHAR(MAX),
    CreatedDate DATE,
    CreatedBy NVARCHAR(100),
    UpdatedDate DATE,
    UpdatedBy NVARCHAR(100),
    IsDeleted BIT DEFAULT 0 -- Cột IsDeleted
);

INSERT INTO QuestionType(TypeName)
VAlues
(N'Câu hỏi trắc nghiệm'),
(N'Câu hỏi tự luận'),
(N'Câu hỏi âm thanh');