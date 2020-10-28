namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class NegationEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static NegationEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/NegationEffect.ps");
        }

        public NegationEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
