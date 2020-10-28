namespace WPFTestHarness
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.Source;
            if (button.Content.ToString() != "Close")
            {
                var type = GetType();
                var assembly = type.Assembly;
                var o = assembly.CreateInstance(type.Namespace + "." + button.Content);
                if (o is Window window)
                {
                    window.ShowDialog();
                }
                else if (o is Page p)
                {
                    var w = new Window { Content = p };
                    w.ShowDialog();
                }
            }
            else
            {
                Close();
            }
        }
    }
}
