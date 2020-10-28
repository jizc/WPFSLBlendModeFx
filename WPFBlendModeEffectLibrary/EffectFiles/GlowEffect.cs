namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class GlowEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static GlowEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/GlowEffect.ps");
        }

        public GlowEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
