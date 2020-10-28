namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class LinearLightEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static LinearLightEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/LinearLightEffect.ps");
        }

        public LinearLightEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
