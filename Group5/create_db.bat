del src\infrastructure\data.db
rmdir /s /q Migrations

dotnet ef migrations add InitialCreate
dotnet ef database update