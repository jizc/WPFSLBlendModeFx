namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class ColorDodgeEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static ColorDodgeEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/ColorDodgeEffect.ps");
        }

        public ColorDodgeEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
