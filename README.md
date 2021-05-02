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
### 1. Odkaz
