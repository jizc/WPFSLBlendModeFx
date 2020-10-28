namespace ColorPicker
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    public class ColorPicker : Control
    {
        /// <summary>
        /// Color Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(Color),
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(Colors.Black, OnColorChanged));

        /// <summary>
        /// ColorChanged Routed Event
        /// </summary>
        public static readonly RoutedEvent ColorChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(ColorChanged),
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<Color>),
                typeof(ColorPicker));

        /// <summary>
        /// Alpha Dependency Property
        /// </summary>
        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register(
                nameof(Alpha),
                typeof(byte),
                typeof(ColorPicker));

        /// <summary>
        /// Red Dependency Property
        /// </summary>
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register(
                nameof(Red),
                typeof(byte),
                typeof(ColorPicker));

        /// <summary>
        /// Green Dependency Property
        /// </summary>
        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register(
                nameof(Green),
                typeof(byte),
                typeof(ColorPicker));

        /// <summary>
        /// Blue Dependency Property
        /// </summary>
        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register(
                nameof(Blue),
                typeof(byte),
                typeof(ColorPicker));

        /// <summary>
        /// InternalColor Dependency Property
        /// </summary>
        private static readonly DependencyProperty InternalColorProperty =
            DependencyProperty.Register(
                nameof(InternalColor),
                typeof(Color),
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(Colors.Black, OnInternalColorChanged));

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker()
        {
            SetupColorBindings();
        }

        /// <summary>
        /// Occurs after Color has changed
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Color> ColorChanged
        {
            add => AddHandler(ColorChangedEvent, value);
            remove => RemoveHandler(ColorChangedEvent, value);
        }

        /// <summary>
        /// Gets or sets the Color property.
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the Alpha property.
        /// </summary>
        public byte Alpha
        {
            get => (byte)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }

        /// <summary>
        /// Gets or sets the Red property.
        /// </summary>
        public byte Red
        {
            get => (byte)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        /// <summary>
        /// Gets or sets the Green property.
        /// </summary>
        public byte Green
        {
            get => (byte)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        /// <summary>
        /// Gets or sets the Blue property.
        /// </summary>
        public byte Blue
        {
            get => (byte)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        /// <summary>
        /// Gets or sets the InternalColor property.
        /// </summary>
        private Color InternalColor
        {
            get => (Color)GetValue(InternalColorProperty);
            set => SetValue(InternalColorProperty, value);
        }

        protected virtual void OnColorChanged(Color oldValue, Color newValue)
        {
            var args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue)
            {
                RoutedEvent = ColorChangedEvent
            };
            RaiseEvent(args);
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPicker)d;

            var oldValue = (Color)e.OldValue;
            var newValue = (Color)e.NewValue;

            colorPicker.InternalColor = newValue;

            colorPicker.OnColorChanged(oldValue, newValue);
        }

        /// <summary>
        /// Synchronizes any change in the InternalColor property with the publicly 
        /// </summary>
        private static void OnInternalColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPicker)d;
            var newValue = (Color)e.NewValue;

            colorPicker.Color = newValue;
        }

        private void SetupColorBindings()
        {
            var multiBinding = new MultiBinding
            {
                Converter = new ByteColorMultiConverter(), Mode = BindingMode.TwoWay
            };

            var alphaBinding = new Binding(nameof(Alpha))
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };
            multiBinding.Bindings.Add(alphaBinding);

            var redBinding = new Binding(nameof(Red))
            {
                Source = this, 
                Mode = BindingMode.TwoWay
            };
            multiBinding.Bindings.Add(redBinding);

            var greenBinding = new Binding(nameof(Green))
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };
            multiBinding.Bindings.Add(greenBinding);

            var blueBinding = new Binding(nameof(Blue))
            {
                Source = this,
                Mode = BindingMode.TwoWay
            };
            multiBinding.Bindings.Add(blueBinding);

            SetBinding(InternalColorProperty, multiBinding);
        }
    }

    public class ByteColorMultiConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (values.Length != 4)
            {
                throw new ArgumentException(
                    "The ByteColorMultiConverter needs four byte values in order to convert them to a Color.");
            }

            var alpha = (byte)values[0];
            var red = (byte)values[1];
            var green = (byte)values[2];
            var blue = (byte)values[3];

            return Color.FromArgb(alpha, red, green, blue);
        }

        public object[] ConvertBack(
            object value,
            Type[] targetTypes,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            var color = (Color)value;
            return new object[] { color.A, color.R, color.G, color.B };
        }
    }

    [ValueConversion(typeof(byte), typeof(double))]
    public class ByteDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)(byte)value;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            return (byte)(double)value;
        }
    }

    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
