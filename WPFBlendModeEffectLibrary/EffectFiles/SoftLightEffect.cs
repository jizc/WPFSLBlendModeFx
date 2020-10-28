namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class SoftLightEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static SoftLightEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/SoftLightEffect.ps");
        }

        public SoftLightEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
