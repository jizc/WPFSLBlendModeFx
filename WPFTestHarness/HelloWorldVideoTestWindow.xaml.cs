namespace WPFTestHarness
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for HelloWorldVideoTestWindow.xaml
    /// </summary>
    public partial class HelloWorldVideoTestWindow
    {
        public HelloWorldVideoTestWindow()
        {
            InitializeComponent();
        }

        private void mediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(0);
        }
    }
}
