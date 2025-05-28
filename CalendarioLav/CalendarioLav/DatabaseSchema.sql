
CREATE TABLE WorkEntries (
    Id INT PRIMARY KEY IDENTITY,
    WorkDate DATE NOT NULL,
    EntryTime DATETIME NOT NULL,
    ExitTime DATETIME NOT NULL
);

CREATE TABLE WeeklyGoals (
    WeekStartDate DATE PRIMARY KEY,
    TargetHours FLOAT NOT NULL
);
