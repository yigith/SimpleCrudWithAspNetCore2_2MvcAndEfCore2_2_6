# Getting Ready
Open **Visual Studio 2019**

**Create a new project** > **ASP.NET Core Web Application** [Next]

Project name: **SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6** [Create]

Select: **.NET Core** | **ASP.NET Core 2.2** | **Web Application (Model-View-Controller)** [Create]

Open Package Manager Console and type:
**Install-Package Microsoft.EntityFrameworkCore -Version 2.2.6**

# Publish to GitHub
**File** Menu > **Add to Source Control**
(Now, you should be able to see blue lock symbols next to each item on the solution explorer)

Open the following address on your browser and **create an empty repository**:
https://github.com/new

**Copy the git URL** of the newly created empty repository.
(For me, it is: https://github.com/yigith/SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.git)

Open** Team Explorer - Home** > **Sync** > **Push to Remote Repository**

Click on **Publish Git Repo**

**Enter the URL** of the empty Git repo and click **Publish**

# Setting Up Entity Framework
**Add a connection string** to **appsettins.json**:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
# Create an Entity Data Model
Create a class file named **Student.cs** inside the **Models** folder.

```csharp
public class Student
{
    public int Id { get; set; }

    [Required, Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required, Display(Name = "Last Name")]
    public string LastName { get; set; }
}
```
# Create the Database Context
Create a class file named **SchoolContext.cs** inside the **Models** folder.

```csharp
public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
}
```
# Register the SchoolContext
Open **Startup.cs** and add the **SchoolContext** to the services.
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<CookiePolicyOptions>(options =>
    {
        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
        options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
    });

    services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
}
```
# Add Seed Data
Override the method **OnModelCreating** in the **SchoolContext**.
```csharp
public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, FirstName = "Angelina", LastName = "Jolie" },
            new Student { Id = 2, FirstName = "Brad", LastName = "Pitt" });
    }

    public DbSet<Student> Students { get; set; }
}
```
# Update the Database
Run the following commands in the **Package Manager Console**:
```bash
Add-Migration Initial
Update-Database
```
And now, your database is ready and seeded with sample data.

# Create the Students Controller
**Create an empty controller** inside the folder **Controllers** by right clicking on the **Controllers** > **Add** > **Controller...** > **MVC Controller - Empty** [**Add**] and place the following action methods **Index**, **Create**, **Edit** and **Delete** along with a **constructor** method.
```csharp
public class StudentsController : Controller
{
    private readonly SchoolContext db;

    public StudentsController(SchoolContext context)
    {
        db = context;
    }

    // GET: Students
    public IActionResult Index()
    {
        return View(db.Students.ToList());
    }

    // GET: Students/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Students/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Id,FirstName,LastName")] Student student)
    {
        if (ModelState.IsValid)
        {
            db.Add(student);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: Students/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = db.Students.Find(id);
        if (student == null)
        {
            return NotFound();
        }
        return View(student);
    }

    // POST: Students/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Id,FirstName,LastName")] Student student)
    {
        if (id != student.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                db.Update(student);
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (db.Students.Any(x => x.Id == student.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: Students/Delete/5
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = db.Students
            .FirstOrDefault(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var student = db.Students.Find(id);
        db.Students.Remove(student);
        db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
```
# Create the Views
 Create **Index.cshtml**, **Create.cshtml**, **Edit.cshtml** and **Delete.cshtml** inside the folder **Views/Students**.

 Index.cshtml:
```razor
@model List<Student>

@{
    ViewData["Title"] = "Index";
}

<h1>Index</h1>

<p>
    <a asp-action="Create">Create New</a>
</p>
<table class="table">
    <thead>
        <tr>
            <th>
                First Name
            </th>
            <th>
                Last Name
            </th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model) {
        <tr>
            <td>
                @item.FirstName
            </td>
            <td>
                @item.LastName
            </td>
            <td>
                <a asp-action="Edit" asp-route-id="@item.Id">Edit</a> |
                <a asp-action="Delete" asp-route-id="@item.Id">Delete</a>
            </td>
        </tr>
}
    </tbody>
</table>
```

Create.cshtml
```razor
@model Student

@{
    ViewData["Title"] = "Create";
}

<h1>Create</h1>

<h4>Student</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group">
                <label asp-for="FirstName" class="control-label"></label>
                <input asp-for="FirstName" class="form-control" />
                <span asp-validation-for="FirstName" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="LastName" class="control-label"></label>
                <input asp-for="LastName" class="form-control" />
                <span asp-validation-for="LastName" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Create" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-action="Index">Back to List</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

Edit.cshtml:
```razor
@model Student

@{
    ViewData["Title"] = "Edit";
}

<h1>Edit</h1>

<h4>Student</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form asp-action="Edit">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <input type="hidden" asp-for="Id" />
            <div class="form-group">
                <label asp-for="FirstName" class="control-label"></label>
                <input asp-for="FirstName" class="form-control" />
                <span asp-validation-for="FirstName" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="LastName" class="control-label"></label>
                <input asp-for="LastName" class="form-control" />
                <span asp-validation-for="LastName" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Save" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-action="Index">Back to List</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
```

Delete.cshtml:
```razor
@model Student

@{
    ViewData["Title"] = "Delete";
}

<h1>Delete</h1>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>Student</h4>
    <hr />
    <dl class="row">
        <dt class = "col-sm-2">
            First Name
        </dt>
        <dd class = "col-sm-10">
            @Model.FirstName
        </dd>
        <dt class = "col-sm-2">
            Last Name
        </dt>
        <dd class = "col-sm-10">
            @Model.LastName
        </dd>
    </dl>
    
    <form asp-action="Delete">
        <input type="hidden" asp-for="Id" />
        <input type="submit" value="Delete" class="btn btn-danger" /> |
        <a asp-action="Index">Back to List</a>
    </form>
</div>
```

# Small Changes in _Layout.cshtml
```html
...

<title>@ViewData["Title"] - Simple CRUD With Asp.Net Core2.2 MVC and Entity Framework Core 2.2.6</title>

...

<a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">School</a>

...

<ul class="navbar-nav flex-grow-1">
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Students" asp-action="Index">Students</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
    </li>
</ul>

...

<footer class="border-top footer text-muted">
    <div class="container">
        &copy; 2019 - Simple CRUD With Asp.Net Core 2.2 MVC and Entity Framework Core 2.2.6 - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
    </div>
</footer>
```


# Resources
- https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-2.2
- https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx
- https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding