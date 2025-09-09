LoveKeyboard.csproj
    - proprietatile aplicatiei (ex: output-ul aplicatiei (.exe), versiunea framework-ului folosit, daca foloseste WPF sau nu)

App.xaml
    - stiluri teme si setari globale ale aplicatiei
    - specifica fereastra ce se va deschide la launch (MainWindow)
App.xaml.cs
    - logica in C# la deschiderea/inchiderea aplicatiei sau la modificari globale

MainWindow.xaml
    - defineste interfata ferestrei
MainWindow.xaml.cs
    - logica in C# a elementelor ferestrei

obj - DO NOT TOUCH
    - generat automat
    - folder cu fisiere compilate si artefacte intermediare ale buildului

AssemblyInfo.cs
    - metadata aplicatiei (ce apare in Properties)
    - pot fi precizate si in .csproj