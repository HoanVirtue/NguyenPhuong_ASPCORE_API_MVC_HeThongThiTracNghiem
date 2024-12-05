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







INSERT INTO Subject (Code, SubjectName, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('SUB001', 'Toán học', GETDATE(), 'admin', GETDATE(), 'admin', 0),
('SUB002', 'Vật lý', GETDATE(), 'admin', GETDATE(), 'admin', 0),
('SUB003', 'Hóa học', GETDATE(), 'admin', GETDATE(), 'admin', 0),
('SUB004', 'Sinh học', GETDATE(), 'admin', GETDATE(), 'admin', 0);



-- Bài học cho Môn học Toán học (SubjectId = 1)
INSERT INTO Lesson (Code, LessonName, SubjectId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('L001', 'Đại số', 1, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L002', 'Giải tích', 1, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L003', 'Hình học', 1, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L004', 'Thống kê', 1, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L005', 'Lượng giác', 1, GETDATE(), 'admin', GETDATE(), 'admin', 0);

-- Bài học cho Môn học Vật lý (SubjectId = 2)
INSERT INTO Lesson (Code, LessonName, SubjectId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('L006', 'Cơ học', 2, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L007', 'Nhiệt học', 2, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L008', 'Quang học', 2, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L009', 'Điện từ học', 2, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L010', 'Lý thuyết hạt cơ bản', 2, GETDATE(), 'admin', GETDATE(), 'admin', 0);

-- Bài học cho Môn học Hóa học (SubjectId = 3)
INSERT INTO Lesson (Code, LessonName, SubjectId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('L011', 'Hóa học đại cương', 3, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L012', 'Hóa hữu cơ', 3, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L013', 'Hóa vô cơ', 3, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L014', 'Hóa phân tích', 3, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L015', 'Hóa lý', 3, GETDATE(), 'admin', GETDATE(), 'admin', 0);

-- Bài học cho Môn học Sinh học (SubjectId = 4)
INSERT INTO Lesson (Code, LessonName, SubjectId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('L016', 'Giải phẫu học', 4, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L017', 'Sinh lý học', 4, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L018', 'Di truyền học', 4, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L019', 'Sinh học tế bào', 4, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('L020', 'Thực vật học', 4, GETDATE(), 'admin', GETDATE(), 'admin', 0);



-- Exam cho Bài học Toán học (LessonId = 1 đến 5)
INSERT INTO Exam (Code, ExamName, Duration, TotalQuestions, SubjectId, LessonId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('EX001', 'Kiểm tra Đại số', 60, 20, 1, 1, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX002', 'Kiểm tra Giải tích', 60, 20, 1, 2, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX003', 'Kiểm tra Hình học', 60, 20, 1, 3, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX004', 'Kiểm tra Thống kê', 60, 20, 1, 4, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX005', 'Kiểm tra Lượng giác', 60, 20, 1, 5, GETDATE(), 'admin', GETDATE(), 'admin', 0);

-- Exam cho Bài học Vật lý (LessonId = 6 đến 10)
INSERT INTO Exam (Code, ExamName, Duration, TotalQuestions, SubjectId, LessonId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('EX006', 'Kiểm tra Cơ học', 60, 20, 2, 6, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX007', 'Kiểm tra Nhiệt học', 60, 20, 2, 7, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX008', 'Kiểm tra Quang học', 60, 20, 2, 8, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX009', 'Kiểm tra Điện từ học', 60, 20, 2, 9, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX010', 'Kiểm tra Lý thuyết hạt cơ bản', 60, 20, 2, 10, GETDATE(), 'admin', GETDATE(), 'admin', 0);

-- Exam cho Bài học Hóa học (LessonId = 11 đến 15)
INSERT INTO Exam (Code, ExamName, Duration, TotalQuestions, SubjectId, LessonId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('EX011', 'Kiểm tra Hóa học đại cương', 60, 20, 3, 11, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX012', 'Kiểm tra Hóa hữu cơ', 60, 20, 3, 12, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX013', 'Kiểm tra Hóa vô cơ', 60, 20, 3, 13, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX014', 'Kiểm tra Hóa phân tích', 60, 20, 3, 14, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX015', 'Kiểm tra Hóa lý', 60, 20, 3, 15, GETDATE(), 'admin', GETDATE(), 'admin', 0);

-- Exam cho Bài học Sinh học (LessonId = 16 đến 20)
INSERT INTO Exam (Code, ExamName, Duration, TotalQuestions, SubjectId, LessonId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy, IsDeleted)
VALUES
('EX016', 'Kiểm tra Giải phẫu học', 60, 20, 4, 16, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX017', 'Kiểm tra Sinh lý học', 60, 20, 4, 17, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX018', 'Kiểm tra Di truyền học', 60, 20, 4, 18, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX019', 'Kiểm tra Sinh học tế bào', 60, 20, 4, 19, GETDATE(), 'admin', GETDATE(), 'admin', 0),
('EX020', 'Kiểm tra Thực vật học', 60, 20, 4, 20, GETDATE(), 'admin', GETDATE(), 'admin', 0);
