namespace Microsoft.DwayneNeed.Media.Imaging
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    ///     The GrayscaleBitmap class processes a source and converts the
    ///     image to a grayscale color scheme.
    /// </summary>
    /// <remarks>
    ///     For simplicity, we only process Bgr32 or Bgra32 formatted bitmaps,
    ///     anything else is converted to one of those formats. Bgr32 and
    ///     Bgra32 share the same memory layout for the RGB channels.
    /// </remarks>
    public class GrayscaleBitmap : ChainedBitmap
    {
        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     We will work natively with either Bgr32 or Bgra32, since both
        ///     formats have the same memory layout.
        /// </remarks>
        public sealed override PixelFormat Format
        {
            get
            {
                // Preserve an alpha channel, if there is one.
                var format = base.Format;
                if (format == PixelFormats.Bgra32
                    || format == PixelFormats.Prgba64
                    || format == PixelFormats.Pbgra32
                    || format == PixelFormats.Prgba128Float
                    || format == PixelFormats.Rgba128Float
                    || format == PixelFormats.Rgba64)
                {
                    format = PixelFormats.Bgra32;
                }
                else
                {
                    format = PixelFormats.Bgr32;
                }

                return format;
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     We only support Bgr32 and Bgra32 pixel formats, so a palette
        ///     is never needed, so we return null.
        /// </remarks>
        public override BitmapPalette Palette => null;

        /// <summary>
        ///     Requests pixels from the ChainedCustomBitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels being requested. 
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination buffer.
        /// </param>
        /// <param name="bufferSize">
        ///     The size of the destination buffer.
        /// </param>
        /// <param name="buffer">
        ///     The destination buffer.
        /// </param>
        /// <remarks>
        ///     Converts the contents of the source bitmap into a grayscale
        ///     color scheme.
        /// 
        ///     The algorithm is taken from here:
        ///     http://en.wikipedia.org/wiki/Grayscale
        /// </remarks>
        protected override void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
        {
            var source = Source;
            if (source != null)
            {
                // First defer to the base implementation, which will fill in
                // the buffer from the source and convert the pixel format as
                // needed.
                base.CopyPixelsCore(sourceRect, stride, bufferSize, buffer);

                // The buffer has been filled with Bgr32 or Bgra32 pixels.
                // Now process those pixels into grayscale.  Ignore the
                // alpha channel.
                //
                // Note: if this buffer pointer came from a managed array, the
                // array has already been pinned.
                unsafe
                {
                    var pBytes = (byte*)buffer.ToPointer();
                    for (var y = 0; y < sourceRect.Height; y++)
                    {
                        var pPixel = (Bgra32Pixel*)pBytes;

                        for (var x = 0; x < sourceRect.Width; x++)
                        {
                            // Get the linear color space values of this pixel.
                            var c = Color.FromRgb(pPixel->Red, pPixel->Green, pPixel->Blue);
                            var red = c.ScR;
                            var green = c.ScG;
                            var blue = c.ScB;

                            // Calculate the grayscale equivalent, taking into account
                            // the sensitivity of the human eye to the different primary
                            // colors (less sensitive to blue, more to green).
                            var gray = red * 0.30f + green * 0.59f + blue * 0.11f;

                            var cGray = Color.FromScRgb(1.0f, gray, gray, gray);

                            // Write sRGB (non-linear) since it is implied by
                            // the pixel format we chose.
                            pPixel->Red = cGray.R;
                            pPixel->Green = cGray.G;
                            pPixel->Blue = cGray.B;

                            pPixel++;
                        }

                        pBytes += stride;
                    }
                }
            }
        }
    }
}
