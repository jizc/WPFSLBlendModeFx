namespace BlendModeEffectLibrary
{
    using System.Windows.Media.Effects;

    public class DarkenEffect : BlendModeEffect
    {
        private static readonly PixelShader pixelShader = new PixelShader();

        static DarkenEffect()
        {
            pixelShader.UriSource = Global.MakePackUri("ShaderSource/DarkenEffect.ps");
        }

        public DarkenEffect()
        {
            PixelShader = pixelShader;
        }
    }
}
