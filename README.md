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
