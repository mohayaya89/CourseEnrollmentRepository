# Course Enrollment System
## Create Migration

```bash
dotnet ef migrations add InitialCreate \
  --project Infrastructure \
  --startup-project Web
```
## Update Migration
```bash
dotnet ef database update \
  --project Infrastructure \
  --startup-project Web
```
---
## 🔹 Example: C# Code
## DbContext Configuration
```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
```
---
## 🔹 Example: Project Structure
```md
## Project Structure
Solution
│
├── Web
├── API
├── Infrastructure
├── Application
├── Domain
```
---
### If 👉 Your application is using EF Core 10 (runtime) 👉 But your CLI tooling (dotnet ef) is still EF Core 8
Run
```bash
dotnet tool update --global dotnet-ef
```
Or explicitly match your runtime:
```bash
dotnet tool update --global dotnet-ef --version 10.0.5
```
---
### Installing Microsoft.EntityFrameworkCore
Web Project:
```
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" />
```
Infrastructure Project:
```
<PackageReference Include="Microsoft.EntityFrameworkCore" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
```
