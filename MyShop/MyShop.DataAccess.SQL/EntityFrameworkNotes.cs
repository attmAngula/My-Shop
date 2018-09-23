/*

THIS DOCUMENT CONTAINS GENERAL NOTES

ENTITY FRAMEWORK:
- Install (via NuGet Packages or Package Manager Console)
- For Code First MIgrations:
    - Define the connectionString in both web.config and app.config
        - app.config in the projects which will contain migrations
        - webconfig in the project to be run
    - In Package Manager Console:
        - Run: Enable-Migrations (to enable migrations)
            - This creates a Migrations folder and a Configuration.cs file
        - Run: Add-Migration <MigrationName>
            - This prepares a .cs file to help writing/uopdating the db
        - Run: Update-Database
            - This implements the migration


 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 */