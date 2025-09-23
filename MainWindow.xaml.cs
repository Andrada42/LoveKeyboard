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
        var chrome = new WindowChrome //Reziable window function + drag function
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

}
