namespace WPFTestHarness
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using BlendModeEffectLibrary;

    /// <summary>
    /// Interaction logic for ImageTestWindow.xaml
    /// </summary>
    public partial class ImageTestWindow
    {
        public ImageTestWindow()
        {
            InitializeComponent();

            effectsListBox.Items.Add(new NormalEffect());

            effectsListBox.Items.Add(new DarkenEffect());
            effectsListBox.Items.Add(new MultiplyEffect());
            effectsListBox.Items.Add(new ColorBurnEffect());
            effectsListBox.Items.Add(new LinearBurnEffect());

            effectsListBox.Items.Add(new LightenEffect());
            effectsListBox.Items.Add(new ScreenEffect());
            effectsListBox.Items.Add(new ColorDodgeEffect());
            effectsListBox.Items.Add(new LinearDodgeEffect());

            effectsListBox.Items.Add(new OverlayEffect());
            effectsListBox.Items.Add(new SoftLightEffect());
            effectsListBox.Items.Add(new HardLightEffect());
            effectsListBox.Items.Add(new VividLightEffect());
            effectsListBox.Items.Add(new LinearLightEffect());
            effectsListBox.Items.Add(new PinLightEffect());

            effectsListBox.Items.Add(new DifferenceEffect());
            effectsListBox.Items.Add(new ExclusionEffect());

            effectsListBox.Items.Add(new GlowEffect());
            effectsListBox.Items.Add(new ReflectEffect());

            effectsListBox.Items.Add(new HardMixEffect());
            effectsListBox.Items.Add(new NegationEffect());
            effectsListBox.Items.Add(new PhoenixEffect());

            effectsListBox.Items.Add(new AverageEffect());

            effectsListBox.Items.Add(new HueEffect());
            effectsListBox.Items.Add(new SaturationEffect());
            effectsListBox.Items.Add(new ColorEffect());
            effectsListBox.Items.Add(new LuminosityEffect());

            effectsListBox.SelectedIndex = 0;


            var images = new List<object>();
            var anImage = new
            {
                DisplayName = "image1",
                Image = new Image { Source = new BitmapImage(new Uri("pack://application:,,/Resources/image1_325x244.jpg")) }
            };
            images.Add(anImage);
            anImage = new
            {
                DisplayName = "image2",
                Image = new Image { Source = new BitmapImage(new Uri("pack://application:,,/Resources/image2_325x244.jpg")) }
            };
            images.Add(anImage);
            anImage = new
            {
                DisplayName = "summer",
                Image = new Image { Source = new BitmapImage(new Uri("pack://application:,,/Resources/summer_325x244.jpg")) }
            };
            images.Add(anImage);
            aImageComboBox.ItemsSource = images;

            images = new List<object>();
            anImage = new
            {
                DisplayName = "texture1",
                Image = new Image { Source = new BitmapImage(new Uri("pack://application:,,/Resources/texture1_325x244.jpg")) }
            };
            images.Add(anImage);
            anImage = new
            {
                DisplayName = "texture2",
                Image = new Image { Source = new BitmapImage(new Uri("pack://application:,,/Resources/texture2_325x244.jpg")) }
            };
            images.Add(anImage);
            anImage = new
            {
                DisplayName = "fall",
                Image = new Image { Source = new BitmapImage(new Uri("pack://application:,,/Resources/fall_325x244.jpg")) }
            };
            images.Add(anImage);
            bImageComboBox.ItemsSource = images;
        }

        private void effectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is BlendModeEffect effect)
            {
                effect.BInput = (ImageBrush)Resources["imageBrushTexture"];
                rImage.Effect = effect;
            }
        }
    }
}
