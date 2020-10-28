namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class ColorBurnEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static ColorBurnEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/ColorBurnEffect.ps");
        }

        public ColorBurnEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
