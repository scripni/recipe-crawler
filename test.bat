dotnet restore app/project.json
dotnet build app/project.json
dotnet restore app.tests/project.json
dotnet test app.tests/project.json