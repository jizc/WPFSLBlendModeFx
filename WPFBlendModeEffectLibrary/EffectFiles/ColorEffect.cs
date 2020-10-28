namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class ColorEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static ColorEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/ColorEffect.ps");
        }

        public ColorEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
