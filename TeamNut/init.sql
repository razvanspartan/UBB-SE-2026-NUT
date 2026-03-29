
DROP TABLE IF EXISTS Messages;
DROP TABLE IF EXISTS Conversations;
DROP TABLE IF EXISTS Reminders;
DROP TABLE IF EXISTS Inventory;
DROP TABLE IF EXISTS UserBehaviour;
DROP TABLE IF EXISTS Favorites;
DROP TABLE IF EXISTS DailyLogs;
DROP TABLE IF EXISTS MealPlanMeal;
DROP TABLE IF EXISTS MealPlan;
DROP TABLE IF EXISTS MealsIngredients;
DROP TABLE IF EXISTS Ingredients;
DROP TABLE IF EXISTS Meals;
DROP TABLE IF EXISTS UserData;
DROP TABLE IF EXISTS Users;

CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL,
    role VARCHAR(50) NOT NULL
);

CREATE TABLE UserData (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE,
    weight FLOAT,
    height FLOAT,
    age INT,
    gender VARCHAR(20),
    goal VARCHAR(50),
    bmi FLOAT,
    calorie_needs INT,
    protein_needs INT,
    carb_needs INT,
    fat_needs INT
);

CREATE TABLE Meals (
    meal_id INT IDENTITY(1,1) PRIMARY KEY,
    imageUrl VARCHAR(255),
    name VARCHAR(100) NOT NULL,
    isKeto BIT DEFAULT 0,
    isLactoseFree BIT DEFAULT 0,
    isNutFree BIT DEFAULT 0,
    isVegan BIT DEFAULT 0,
    isGlutenFree BIT DEFAULT 0,
    description VARCHAR(255)
);

CREATE TABLE Ingredients (
    food_id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    calories_per_100g FLOAT,
    protein_per_100g FLOAT,
    carbs_per_100g FLOAT,
    fat_per_100g FLOAT
);

CREATE TABLE MealsIngredients (
    id INT IDENTITY(1,1) PRIMARY KEY,
    meal_id INT FOREIGN KEY REFERENCES Meals(meal_id) ON DELETE CASCADE,
    food_id INT FOREIGN KEY REFERENCES Ingredients(food_id) ON DELETE CASCADE,
    quantity FLOAT
);

CREATE TABLE MealPlan (
    mealplan_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE,
    created_at DATETIME DEFAULT GETDATE(),
    goal_type VARCHAR(50)
);

CREATE TABLE MealPlanMeal (
    id INT IDENTITY(1,1) PRIMARY KEY,
    mealPlanId INT FOREIGN KEY REFERENCES MealPlan(mealplan_id) ON DELETE CASCADE,
    mealId INT FOREIGN KEY REFERENCES Meals(meal_id) ON DELETE CASCADE,
    isConsumed BIT DEFAULT 0,
    mealType VARCHAR(50),
    assigned_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE DailyLogs (
    id INT IDENTITY(1,1) PRIMARY KEY,
    mealPlanId INT FOREIGN KEY REFERENCES MealPlan(mealplan_id) ON DELETE CASCADE,
    totalCalories FLOAT
);

CREATE TABLE Favorites (
    id INT IDENTITY(1,1) PRIMARY KEY,
    userId INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE,
    mealId INT FOREIGN KEY REFERENCES Meals(meal_id) ON DELETE CASCADE
);

CREATE TABLE UserBehaviour (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE,
    mealType VARCHAR(50),
    actionType VARCHAR(50),
    mealId INT FOREIGN KEY REFERENCES Meals(meal_id) ON DELETE CASCADE,
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE Inventory (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE,
    ingredient_id INT FOREIGN KEY REFERENCES Ingredients(food_id) ON DELETE CASCADE,
    quantity_grams FLOAT
);

CREATE TABLE Reminders (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE,
    name VARCHAR(100),
    has_sound BIT DEFAULT 1,
    time TIME,
    frequency VARCHAR(50)
);

CREATE TABLE Conversations (
    id INT IDENTITY(1,1) PRIMARY KEY,
    has_unanswered BIT DEFAULT 0,
    user_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE CASCADE
);

CREATE TABLE Messages (
    id INT IDENTITY(1,1) PRIMARY KEY,
    sent_at DATETIME DEFAULT GETDATE(),
    conversation_id INT FOREIGN KEY REFERENCES Conversations(id) ON DELETE CASCADE,
    sender_id INT FOREIGN KEY REFERENCES Users(id) ON DELETE NO ACTION,
    text_content VARCHAR(300)
);