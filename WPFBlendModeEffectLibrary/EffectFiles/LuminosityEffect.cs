namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class LuminosityEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static LuminosityEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/LuminosityEffect.ps");
        }

        public LuminosityEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
