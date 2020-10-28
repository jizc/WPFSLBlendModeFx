namespace BlendModeEffectLibrary
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;

    public class BlendModeEffect : ShaderEffect
    {
        public static readonly DependencyProperty AInputProperty =
            RegisterPixelShaderSamplerProperty(
                nameof(AInput),
                typeof(BlendModeEffect),
                0);

        public static readonly DependencyProperty BInputProperty =
            RegisterPixelShaderSamplerProperty(
                nameof(BInput),
                typeof(BlendModeEffect),
                1);

        public BlendModeEffect()
        {
            UpdateShaderValue(AInputProperty);
            UpdateShaderValue(BInputProperty);
        }

        [System.ComponentModel.Browsable(false)]
        public Brush AInput
        {
            get => (Brush)GetValue(AInputProperty);
            set => SetValue(AInputProperty, value);
        }

        public Brush BInput
        {
            get => (Brush)GetValue(BInputProperty);
            set => SetValue(BInputProperty, value);
        }
    }
}
