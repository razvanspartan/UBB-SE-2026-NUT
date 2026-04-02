-- SQL script to create chat tables for NutritionistChat feature
CREATE TABLE IF NOT EXISTS ChatConversations (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    nutritionist_id INT NULL,
    created_at DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);

CREATE TABLE IF NOT EXISTS ChatMessages (
    id INT IDENTITY(1,1) PRIMARY KEY,
    conversation_id INT NOT NULL,
    sender_id INT NOT NULL,
    sender_role NVARCHAR(50) NOT NULL,
    text NVARCHAR(1000) NOT NULL,
    timestamp DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    is_read BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (conversation_id) REFERENCES ChatConversations(id)
);

CREATE INDEX IF NOT EXISTS IDX_ChatMessages_Conversation_Timestamp ON ChatMessages(conversation_id, timestamp);
