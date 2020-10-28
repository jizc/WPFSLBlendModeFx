namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class DifferenceEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static DifferenceEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/DifferenceEffect.ps");
        }

        public DifferenceEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
