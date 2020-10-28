namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class OverlayEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static OverlayEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/OverlayEffect.ps");
        }

        public OverlayEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
