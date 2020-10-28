namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class HardLightEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static HardLightEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/HardLightEffect.ps");
        }

        public HardLightEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
