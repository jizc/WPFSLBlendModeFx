namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class VividLightEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static VividLightEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/VividLightEffect.ps");
        }

        public VividLightEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
