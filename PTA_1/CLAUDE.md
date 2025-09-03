# PTA_1 Project Information

## Project Type
- .NET 8.0 ASP.NET Core Web Application
- Uses Entity Framework Core with SQLite for development
- ASP.NET Core Identity for authentication

## Build Commands
- **Restore packages**: `dotnet restore`
- **Build**: `dotnet build`
- **Run**: `dotnet run` (from Web directory or solution root)
- **Run migrations**: `dotnet ef database update --project Web`

## Project Structure
- Solution file: `PTA_1.sln`
- Main web project: `Web/Web.csproj`
- Uses .NET 8.0 target framework

## Database Schema
The application includes a comprehensive user management system with the following entities:

### Core Tables
- **Person**: Base table with personal information (first/last/middle names, suffix, birthday, SSN)
- **Student**: Student-specific data (grade, student ID, enrollment date) - Foreign key to Person
- **Parent**: Parent-specific data (occupation, phone, email, address) - Foreign key to Person  
- **Teacher**: Teacher-specific data (grade, classroom, subject, employee ID, phone, email, hire date) - Foreign key to Person
- **ParentStudent**: Junction table linking parents to their children with relationship details

### Features Implemented
- **Student Registration**: Complete form to register new students with personal and academic information
- **Parent Registration**: Registration system for parents/guardians with contact information
- **Teacher Registration**: Employee registration with teaching assignment details
- **Search Functionality**: Search students, parents, and teachers by name and other relevant fields
- **Navigation**: Main navigation menu with links to Students, Parents, and Teachers sections
- **Details Views**: Comprehensive detail pages showing all related information

### Database Relationships
- One-to-One: Person ↔ Student, Person ↔ Parent, Person ↔ Teacher
- Many-to-Many: Parent ↔ Student (through ParentStudent junction table)
- ParentStudent table includes relationship type, primary contact flag, and pickup authorization

## Navigation URLs
- Students: `/Student` 
- Parents: `/Parent`
- Teachers: `/Teacher`
- Each section includes Index, Create, Details, and Search functionality

## Notes
- Database uses SQLite (app.db) for development environment
- First build may require package restore (can take 2+ minutes)
- Build succeeds with 0 warnings and 0 errors
- Remote repository uses HTTPS: https://github.com/jasonslemons/pta_1.git
- Application runs on http://localhost:5197 in development