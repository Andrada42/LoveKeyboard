# Lovekeyboard
---
Convertor din text normal in font cursiv ([Caracterele Unicode U+1D4D0 - U+1D4E9](https://www.compart.com/en/unicode/block/U+1D4D0))

### Cum dai Build?
```cmd
dotnet build
```

### Cum dai Run?
```cmd
dotnet run
```

### Cum obtii un executabil?
```cmd
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```
=> executabil in .\bin\Release\net8.0-windows\win-x64\publish

- -c Release / Debug                            => stilul executabilului (de Release / Debug)
- -r win-x64 / win-x86 / linux-x64 / osx-x64    => sistem de operare
- --self-contained true                         => Ruleaza si pe PC-uri fara .NET instalat 
- /p:PublishSingleFile=true                     => creaza un singur fisier executabil

---
### Possible Improvements:
- Dark Theme (Buton jos de tip on/off)
- Sunet la toate butoanele + cand tastezi
- Animatia ferestrei cand o pui in bara/deschizi din taskbar
- Daca dai paste la link-uri, sa nu le converteasca in font (Poate un buton jos de tip on/off la convertirea textului)

- **Pentru a avea versiunea colora a emojiilor, aplicatia trebuie convertita in WinUI 3 / UWP / MAUI.**

---
### Good to know:
#### LoveKeyboard.csproj
- proprietatile aplicatiei (ex: output-ul aplicatiei (.exe), versiunea framework-ului folosit, daca foloseste WPF sau nu)

#### App.xaml
- stiluri teme si setari globale ale aplicatiei
- specifica fereastra ce se va deschide la launch (MainWindow)
#### App.xaml.cs
- logica in C# la deschiderea/inchiderea aplicatiei sau la modificari globale

#### MainWindow.xaml
- defineste interfata ferestrei
#### MainWindow.xaml.cs
- logica in C# a elementelor ferestrei

#### obj - DO NOT TOUCH
- generat automat
- folder cu fisiere compilate si artefacte intermediare ale buildului

#### AssemblyInfo.cs
- metadata aplicatiei (ce apare in Properties)
- pot fi precizate si in .csproj

---
### Credits
**Aplicatia a fost facuta de Andrada si Ciprian (see contribuitors).**

---
copy button icon designed by Smashicons from Flaticon
emoji button icon designed by Freepik from Flaticon
app icon designed by Vitaly Gorbachev from Flaticon