namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class ScreenEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static ScreenEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/ScreenEffect.ps");
        }

        public ScreenEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
