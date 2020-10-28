namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class AverageEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static AverageEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/AverageEffect.ps");
        }

        public AverageEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
