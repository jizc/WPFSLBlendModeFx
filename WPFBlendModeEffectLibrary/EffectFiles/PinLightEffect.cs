namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class PinLightEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static PinLightEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/PinLightEffect.ps");
        }

        public PinLightEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
