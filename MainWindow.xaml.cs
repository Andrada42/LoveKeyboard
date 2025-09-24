using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;
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
    private const int WM_GETMINMAXINFO = 0x0024;

    [StructLayout(LayoutKind.Sequential)]
    struct POINT { public int x, y; }
    [StructLayout(LayoutKind.Sequential)]
    struct MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct MONITORINFO
    {
        public int cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }
    [StructLayout(LayoutKind.Sequential)]
    struct RECT { public int left, top, right, bottom; }
    [DllImport("user32.dll")]
    static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
    private const uint MONITOR_DEFAULTTONEAREST = 2;

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_GETMINMAXINFO)
        {
            var mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

            var monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            var info = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            GetMonitorInfo(monitor, ref info);

            // Set maximized size/position to the monitor's working area
            mmi.ptMaxPosition.x = info.rcWork.left;
            mmi.ptMaxPosition.y = info.rcWork.top;
            mmi.ptMaxSize.x = info.rcWork.right - info.rcWork.left;
            mmi.ptMaxSize.y = info.rcWork.bottom - info.rcWork.top;

            Marshal.StructureToPtr(mmi, lParam, true);
            handled = true;
        }

        return IntPtr.Zero;
    }

    public MainWindow()
    {
        InitializeComponent();
        // Pt. a inchide popup-ul cand defocalizam, mutam sau redimensionam fereastra
        this.Deactivated += MainWindow_SomethingChanged;
        this.LocationChanged += MainWindow_SomethingChanged;
        this.SizeChanged += MainWindow_SomethingChanged;

        Loaded += Window_Loaded;

        var chrome = new WindowChrome //Resizable window function + drag function
        {
            ResizeBorderThickness = new Thickness(5), // Thickness of resizable border
            CaptionHeight = 30,                       // Height of draggable area (title bar)
            GlassFrameThickness = new Thickness(0),
            UseAeroCaptionButtons = false
        };
        WindowChrome.SetWindowChrome(this, chrome);
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        MainTextBox.Focus();
        var hwnd = new WindowInteropHelper(this).Handle;
        HwndSource.FromHwnd(hwnd)?.AddHook(WndProc);
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

    private void MainTextBox_TextChanged(object sender, RoutedEventArgs e)
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

    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(MainTextBox.Text);
        MainTextBox.Focus();
    }

    private void EmojiButton_Click(object sender, RoutedEventArgs e)
    {
        EmojiPopup.IsOpen = !EmojiPopup.IsOpen;
        MainTextBox.Focus();
    }
    private void Emoji_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b)
            return;
        
        string emoji = b.Content?.ToString() ?? "";
        // ?. => daca b.Content == null, nu apeleaza ToString() si returneaza null
        // ?? => daca b.Content?.ToString() e null, emoji = ""
        if (string.IsNullOrEmpty(emoji))
            return;
        
        // Indecsii selectiei, daca e cazul
        // (daca nu sunt caractere selectate, length = 0 iar start = pozitia caret-ului)
        int start = MainTextBox.SelectionStart;
        int length = MainTextBox.SelectionLength;

        MainTextBox.Text = MainTextBox.Text.Remove(start, length); // șterge selecția
        MainTextBox.Text = MainTextBox.Text.Insert(start, emoji);  // inserează emoji-ul

        MainTextBox.CaretIndex = start + emoji.Length;
        MainTextBox.Focus();
        
    }

    private void ClosePopup()
    {
        // Inchide popup-ul cand aplicatia nu mai e activa
        if (EmojiPopup.IsOpen)
        {
            // Retinem animatia de deschidere/inchidere a popup-ului
            var oldAnimation = EmojiPopup.PopupAnimation; 
            // O dezactivam/stergem temporar
            EmojiPopup.PopupAnimation = PopupAnimation.None;
            // Inchidem popup-ul
            EmojiPopup.IsOpen = false;
            // Restauram animatia pentru urmatoarea deschidere
            EmojiPopup.PopupAnimation = oldAnimation;
        }
    }

    // SomethingChanged = Fereastra s-a dezactivat / a fost mutata / a fost redimensionata
    private void MainWindow_SomethingChanged(object? sender, EventArgs e) // EventHandler
    {
        ClosePopup();
    }
}
