namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class LinearDodgeEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static LinearDodgeEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/LinearDodgeEffect.ps");
        }

        public LinearDodgeEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
