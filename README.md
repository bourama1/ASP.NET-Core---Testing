# ASP.NET-Core---Testing
## Views/Controller
### 1. Vybrání view v kontroleru
Stačí přidat do kontroleru metodu s názvem odpovídající danému view. Např. view s URL /Home/Index se nasměruje na metodu v HomeControlleru s názvem Index.

### 2. Parametry do view
V controlleru v metodě vracíme View(model) a ve view načítáme tento model: @model LineModel. Pokud je třeba předat nějaké dočasné data, které nejsou součástí modelů lze využít ViewBag, př. v kontroleru 
```cs
ViewBag.lines = new SelectList(lines, "ID", "Name");
```
a poté ve view
```cs
<select name="selected_line" asp-items="ViewBag.lines"></select>
```

## Gridy
S použitím EntityFrameworku lze vygenerovat controller a základní view(Create, Delete, Details, Edit, Index) automaticky z vytvořeného modelu. 
### 1. Odkaz na edit
```cs
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>
                    @Html.DisplayFor(modelItem => item.Name)
                </td>
                <td>
                    <a asp-action="Edit" asp-route-id="@item.ID">Edit</a>
                </td>
            </tr>
        }
    </tbody>
```
V odpovídajícím kontroleru je třeba mít GET metodu Edit.
```cs
public async Task<IActionResult> Edit(int? id)
```
### 2. Sort
Přidáme do kontroleru switch na různé možnosti sortování
```cs
public async Task<IActionResult> Index(string sortOrder)
{
    ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
    var students = from s in _context.Students
                   select s;
    switch (sortOrder)
    {
        case "name_desc":
            students = students.OrderByDescending(s => s.LastName);
            break;
        default:
            students = students.OrderBy(s => s.LastName);
            break;
    }
    return View(await students.AsNoTracking().ToListAsync());
}
```
Které předáváme z view 
  ```cs
<a asp-action="Index" asp-route-sortOrder="@ViewData["NameSortParm"]">@Html.DisplayNameFor(model => model.LastName)</a>
```
### 3. Strankování
#### Je třeba vytvořit třídu PaginatedList.cs
  ```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
```
#### Controller
Poté si do parametrů metody v controlleru zobrazující daný grid přídáme a také přidáme hodnotu aktuálního sortu
```cs
public async Task<IActionResult> Index(
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
```
a na konci této metody si určíme velikost stránkování a předáme view model
```cs
int pageSize = 3;
return View(await PaginatedList<LineModel>.CreateAsync(lines.AsNoTracking(), pageNumber ?? 1, pageSize));
```      
#### View
Nejdříve změníme typ modelu na
```cs
@model PaginatedList<WebApplication1.Models.LineModel>
```
a na konec view přidáme odkazy na pohybování mezi stránkami
```cs
@{
    var prevDisabled = !Model.HasPreviousPage ? "disabled" : "";
    var nextDisabled = !Model.HasNextPage ? "disabled" : "";
}

<a asp-action="Index"
   asp-route-sortOrder="@ViewData["CurrentSort"]"
   asp-route-pageNumber="@(Model.PageIndex - 1)"
   class="btn btn-default @prevDisabled">
    Previous
</a>
<a asp-action="Index"
   asp-route-sortOrder="@ViewData["CurrentSort"]"
   asp-route-pageNumber="@(Model.PageIndex + 1)"
   class="btn btn-default @nextDisabled">
    Next
</a>
```
### 4. Filtr
Do controlleru přidáme viewbag s výběrem itemů dle kterých chceme filtrovat a pak samotnou filtraci
```cs
ViewBag.lines = new SelectList(lines, "ID", "Name");

String selectedLine = Request.Query["selected_line"].ToString();

if (!String.IsNullOrEmpty(selectedLine))
{
    lines = lines.Where(s => s.ID.Equals(Convert.ToInt32(selectedLine)));
}
```
Do view poté přidáme formulář s daným viewbagem
```html
<form asp-action="Index" method="get">
    <div class="form-actions no-color">
        <p>
            Filters:
            <select name="selected_line" asp-items="ViewBag.lines"></select>
        </p>
    </div>
    <input type="submit" value="Search" class="btn btn-default" /> |
    <a asp-action="Index">Back to Full List</a>
</form>
```
## Auth
### 1. Kontrola práv - Anotace
Do Startup.cs přidáme k options autorizace danou roli
```cs
services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });
```
poté v controlleru ověřujeme anotací
```cs
[Authorize(Policy = "EmployeeOnly")]
```
### 2. Info o uživateli
Přímo ve view lze získat např. username takto
```cs
@Context.User.Identity.Name
```
V controlleru získáváme info obdobně
```cs
string username = HttpContext.User.Identity.Name;
string auth = HttpContext.User.Identity.AuthenticationType;
IEnumerable<Claim> claims = HttpContext.User.Claims;
```
K přidání informace o času přihlášení bude nejspíš potřeba vytvořit třídu z IdentityUser a přidat si do ní požadované vlastnosti, viz https://docs.microsoft.com/cs-cz/aspnet/core/security/authentication/add-user-data?view=aspnetcore-3.1&tabs=visual-studio

## Entity Framework
Je potřeba udělat implementaci DbContext a přidat do ní DbSet<> pro modely
```cs
public DbSet<LineModel> Lines{ get; set; }
```
### 1. Procedura jako zdroj dat
Poté lze v kontroleru využít DbSet<TEntity>.FromSqlRaw() nebo FromSqlInterpolated() metodu pro získání dat z uložených procedur. Např.
```cs
var name = "ex";

var lines = context.Lines
    .FromSqlRaw("EXECUTE dbo.Procedure {0}", name)
    .ToList();
```
zavolá proceduru Procedure s parametrem name.
Když je potřeba zobrazit hodnoty z více tabulek bude nutné vytvořit classu, která bude obsahovat všechny vlastnosti, které nám daná procedura vrací. Tu poté také přidáme do DbContextu a v kontroleru obdobně zavoláme.

### 2. Edit
    V GET získáme model toho co chceme upravovat takto
    ```cs
    var lineModel = await _context.Lines.FindAsync(id);
    ```
    V POST poté ukládáme opět přes DbContext
    ```cs
    _context.Update(lineModel);
    await _context.SaveChangesAsync();
    ```
