using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;

namespace LoveKeyboard;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        
        var chrome = new WindowChrome //Resizable window function + drag function
        {
            ResizeBorderThickness = new Thickness(5), // Thickness of resizable border
            CaptionHeight = 30,                       // Height of draggable area (title bar)
            GlassFrameThickness = new Thickness(0),
            UseAeroCaptionButtons = false
        };
        WindowChrome.SetWindowChrome(this, chrome);
    }

    private void ToggleMaximizeRestore() //Toggles between maximized and normal
    {
        WindowState = WindowState == WindowState.Normal
        ? WindowState.Maximized 
        : WindowState.Normal;
    }
    
    private void MinimizeButton_Click(object sender, RoutedEventArgs e)     //Minimize button
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeButton_Click(object sender, RoutedEventArgs e)     //Maximize button
    {
        if (WindowState == WindowState.Normal)
            WindowState = WindowState.Maximized;
        else
            WindowState = WindowState.Normal;
    }
    private void CloseButton_Click(object sender, RoutedEventArgs e)     //Close window button
    {
        Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MainTextBox.Focus();
    }

    private void MainTextBox_Update(object sender, RoutedEventArgs e)
    {
        if (sender is not TextBox tb)   // daca sender nu e TextBox => return
            return;                     // altfel incarca tb cu TextBox-ul din sender

        // Retinem pozitia cursorului
        int selectionStart = tb.SelectionStart;

        string input = tb.Text;
        string output = "";

        // Fontul are caractere cu Unicode-uri intre U+1D400 si U+1D7FF
        // A - Z <=> U+1D4D0 - U+1D4E9 (1D4D0(16) + 26(10) = 1D4E9(16))
        // a - z <=> U+1D4EA - U+1D503

        // OBS: https://www.compart.com/en/unicode/block/U+1D4D0 - caractere Unicode
        // caracterul U+1D400 are codul 1D400(16) = 119808(10) care are 17 cifre in baza 2 
        // Asadar, un char = 1 byte = 8 biti = 8 cifre in baza 2 nu poate retine acest cod
        // Folosim: char.ConvertFromUtf32

        // OBS: Daca concatenam litera cu litera la output,
        // in C# se creeaza de fiecare data cate un obiect nou,
        // asa ca folosim clasa StringBuilder.
        StringBuilder sb = new StringBuilder();
        foreach (char c in input)
        {
            if (c >= 'A' && c <= 'Z')
                sb.Append(char.ConvertFromUtf32(0x1D4D0 + c - 'A'));
            else if (c >= 'a' && c <= 'z')
                sb.Append(char.ConvertFromUtf32(0x1D4EA + c - 'a'));
            else
                sb.Append(c);
        }

        output = sb.ToString();
        tb.Text = output;
        tb.SelectionStart = selectionStart;
    }
}
