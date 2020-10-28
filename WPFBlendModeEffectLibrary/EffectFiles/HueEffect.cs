namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class HueEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static HueEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/HueEffect.ps");
        }

        public HueEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
