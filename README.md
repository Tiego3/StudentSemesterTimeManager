# ModuleTracker

A simple WPF desktop application for tracking study modules, self-study requirements, and study hours for university students.  
The solution is built with .NET 6, Entity Framework Core, SQL Server LocalDB, and follows the MVVM pattern with logic in a separate class library.

## Features

- User registration and login (with password hashing)
- Add multiple modules per semester (code, name, credits, class hours/week)
- Record number of weeks and semester start date
- Calculation of required self-study hours per module per week
- Record hours spent per module per day
- Real-time calculation of remaining self-study hours for current week
- Data persistence via SQL Server and Entity Framework Core
- Each user only sees their data
- Multi-threaded DB access for responsive UI
- LINQ used for data manipulation
- All business/data logic lives in a custom class library (`SemesterCore`)

## Solution Structure

```
StudentSemesterManager.sln
├─ SemesterCore         # Class library for models, EF Core context, calculations, utilities
├─ StudentSemesterManager          # WPF MVVM application
```

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 6 SDK
- SQL Server LocalDB (usually included with Visual Studio)

### How to Run

1. Open `StudentSemesterManager.sln` in Visual Studio.
2. Build the solution to restore NuGet packages.
3. Ensure `StudentSemesterManager` is set as the startup project.
4. Run the application (`F5` or `Ctrl+F5`).

The database will be created automatically in LocalDB when you first run the app.

### Usage

1. **Register** a new user with a username and password.
2. **Login** with your credentials.
3. Enter semester information (number of weeks, start date).
4. Add your modules with code, name, credits, and class hours/week.
5. The app calculates how many hours of self-study you need per module per week.
6. Record the hours you study for each module per day.
7. See how many hours remain for self-study for each module in the current week.

## Technical Notes

- **DB Creation:** The app uses `Database.EnsureCreated()` (no migrations).
- **Multi-threading:** All database operations are performed on background threads; the UI remains responsive.
- **Password Security:** Only password hashes are stored, using SHA-256.
- **Data Isolation:** Each user only accesses their own modules and study sessions.
- **LINQ:** Used for all calculations and queries.
- **Code Separation:** Models and logic are in a class library (`SemesterCore`), UI in the WPF app.

## Planned/Future Features

Here are some simple features that could improve the application in the future:

- **Logout functionality**: Allow users to securely log out of the application.
- **Password reset**: Add a feature to reset passwords using a verification step.
- **Stricter password requirements**: Enforce passwords to have at least:
  - One number
  - One special character
  - Minimum 8 characters
- **Delete/Remove module**: Enable users to remove modules from their list.
- **Edit module details**: Allow updating module info after creation.
- **View study history**: See a log of past study sessions per module.
- **Export data**: Export your module and study session data to CSV.
- **Dark mode/theme support**: UI improvement for accessibility.
- **Notifications/reminders**: Let users set reminders for study hours or deadlines.
- **Improved error messages and UI feedback**
- **Show progress bars for self-study completion**
- **Support for multiple semesters per user**
- **Basic analytics**: Show trends or weekly summaries for study habits.

Feel free to contribute or suggest other improvements!

## Customization

- Change the database connection string in `StudentSemesterManager/LoginWindow.xaml.cs` and `MainWindow.xaml.cs` if you use a different SQL Server instance.
- Expand models/business logic in `SemesterCore` for more features.

