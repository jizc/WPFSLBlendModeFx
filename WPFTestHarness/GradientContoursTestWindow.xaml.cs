namespace WPFTestHarness
{
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using BlendModeEffectLibrary;

    /// <summary>
    /// Interaction logic for GradientContoursTestWindow.xaml
    /// </summary>
    public partial class GradientContoursTestWindow
    {
        private bool swapped;

        public GradientContoursTestWindow()
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

            var brushA = (Brush)Resources["whiteToBlackTopToBottomGradientBrushOpacityBound"];
            var bindingA = new Binding
            {
                Source = brushA,
                Path = new PropertyPath(nameof(Opacity))
            };
            aOpacitySlider.SetBinding(RangeBase.ValueProperty, bindingA);

            var brushB = (Brush)Resources["blackToWhiteLeftToRightGradientBrushOpacityBound"];
            var bindingB = new Binding
            {
                Source = brushB,
                Path = new PropertyPath(nameof(Opacity))
            };
            bOpacitySlider.SetBinding(RangeBase.ValueProperty, bindingB);

            Loaded += (sender, e) => ShowGrayscaleBitmap();
        }

        private static BitmapSource CaptureScreen(Visual target, double dpiX, double dpiY)
        {
            if (target is null)
            {
                return null;
            }

            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            if (bounds.IsEmpty)
            {
                return null;
            }

            var rtb = new RenderTargetBitmap(
                (int)(bounds.Width * dpiX / 96.0),
                (int)(bounds.Height * dpiY / 96.0),
                dpiX,
                dpiY,
                PixelFormats.Pbgra32);

            var dv = new DrawingVisual();
            using (var ctx = dv.RenderOpen())
            {
                var vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            rtb.Render(dv);

            return rtb;
        }


        private void effectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is BlendModeEffect effect)
            {
                effect.BInput = (ImageBrush)Resources["imageBrushTexture"];
                resultBorder.Effect = effect;

                ShowGrayscaleBitmap();
            }
        }

        private void ShowGrayscaleBitmap()
        {
            var captureScreen = CaptureScreen(resultBorder, 96, 96);
            if (captureScreen is null)
            {
                return;
            }

            var grayscaleBitmap = new Grayscale4Bitmap { Source = captureScreen };
            resultImage.Source = grayscaleBitmap;
        }

        private void opacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)(ShowGrayscaleBitmap));
        }

        private void swap_Click(object sender, RoutedEventArgs e)
        {
            if (!swapped)
            {
                aLabel.Text = "B";
                aBorder.Fill = (Brush)Resources["blackToWhiteLeftToRightGradientBrush"];
                bLabel.Text = "A";
                bBorder.Fill = (Brush)Resources["whiteToBlackTopToBottomGradientBrush"];

                var brushA = (Brush)Resources["blackToWhiteLeftToRightGradientBrushOpacityBound"];
                var bindingA = new Binding
                {
                    Source = brushA,
                    Path = new PropertyPath(nameof(Opacity))
                };
                aOpacitySlider.SetBinding(RangeBase.ValueProperty, bindingA);
                resultBorder.Fill = brushA;

                var brushB = (Brush)Resources["whiteToBlackTopToBottomGradientBrushOpacityBound"];
                var bindingB = new Binding
                {
                    Source = brushB,
                    Path = new PropertyPath(nameof(Opacity))
                };
                bOpacitySlider.SetBinding(RangeBase.ValueProperty, bindingB);
                ((GeometryDrawing)Resources["geometryDrawing"]).Brush = brushB;
            }
            else
            {
                aLabel.Text = "A";
                aBorder.Fill = resultBorder.Fill = (Brush)Resources["whiteToBlackTopToBottomGradientBrush"];
                bLabel.Text = "B";
                bBorder.Fill = (Brush)Resources["blackToWhiteLeftToRightGradientBrush"];

                var brushA = (Brush)Resources["whiteToBlackTopToBottomGradientBrushOpacityBound"];
                var bindingA = new Binding
                {
                    Source = brushA,
                    Path = new PropertyPath(nameof(Opacity))
                };
                aOpacitySlider.SetBinding(RangeBase.ValueProperty, bindingA);
                resultBorder.Fill = brushA;

                var brushB = (Brush)Resources["blackToWhiteLeftToRightGradientBrushOpacityBound"];
                var bindingB = new Binding
                {
                    Source = brushB,
                    Path = new PropertyPath(nameof(Opacity))
                };
                bOpacitySlider.SetBinding(RangeBase.ValueProperty, bindingB);
                ((GeometryDrawing)Resources["geometryDrawing"]).Brush = brushB;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)(ShowGrayscaleBitmap));

            swapped = !swapped;
        }
    }
}
