namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class LinearBurnEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static LinearBurnEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/LinearBurnEffect.ps");
        }

        public LinearBurnEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
