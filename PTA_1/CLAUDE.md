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

#### User Management System
- **Student Registration**: Complete form to register new students with personal and academic information
- **Parent Registration**: Registration system for parents/guardians with contact information
- **Teacher Registration**: Employee registration with teaching assignment details
- **Search Functionality**: Search students, parents, and teachers by name and other relevant fields
- **Details Views**: Comprehensive detail pages showing all related information

#### Activity Management & Signup System
- **Activity Creation**: Full CRUD system for creating and managing activities with date ranges, locations, descriptions
- **Participant Signup**: Students, parents, and teachers can sign up as participants for activities
- **Volunteer Signup**: Students, parents, and teachers can sign up as volunteers for activities
- **Task Management**: Activities can have volunteer tasks with time slots, locations, and capacity limits
- **Station Management**: Activities can have participant stations with age restrictions and capacity limits
- **Signup Tracking**: Comprehensive tracking of who signed up for what role with status management
- **Emergency Contacts**: Participant signups can include emergency contact information

#### Database Schema - Activity Tables
- **Activity**: Main activity table with title, description, location, date range, participant/volunteer limits
- **Task**: Volunteer tasks associated with activities (time, location, max volunteers, required flag)
- **Station**: Participant stations for activities (age limits, capacity, requirements)
- **ActivitySignup**: General signup tracking (participant vs volunteer, status, emergency contacts)
- **TaskSignup**: Specific volunteer task assignments
- **StationSignup**: Specific participant station assignments

### Database Relationships
- One-to-One: Person ↔ Student, Person ↔ Parent, Person ↔ Teacher
- Many-to-Many: Parent ↔ Student (through ParentStudent junction table)
- One-to-Many: Activity ↔ Tasks, Activity ↔ Stations, Activity ↔ ActivitySignups
- One-to-Many: Task ↔ TaskSignups, Station ↔ StationSignups
- Many-to-One: All signup tables ↔ Person
- ParentStudent table includes relationship type, primary contact flag, and pickup authorization

## Navigation URLs
- Students: `/Student` 
- Parents: `/Parent`
- Teachers: `/Teacher`
- Activities: `/Activity`
- Each section includes Index, Create, Details, and Search functionality

### Activity System Features
- Create activities with date ranges and capacity limits
- Sign up users as participants or volunteers
- Track signup status (Pending, Confirmed, Cancelled, Waitlist)
- Emergency contact information for participants
- Real-time signup counts and availability
- Search and filter activities

#### Home Page Features
- **Activity Dashboard**: Front page displays upcoming activities with registration buttons
- **Quick Registration**: Direct signup from home page with person selection
- **Live Statistics**: Real-time display of total activities, participants, and volunteers
- **Quick Access Cards**: Direct links to manage Students, Parents, and Teachers
- **No Activity State**: Helpful messaging and links when no activities exist

## Notes
- Database uses SQLite (app.db) for development environment
- First build may require package restore (can take 2+ minutes)
- Build succeeds with 0 warnings and 0 errors
- Remote repository uses HTTPS: https://github.com/jasonslemons/pta_1.git
- Application runs on http://localhost:5197 in development