namespace Microsoft.DwayneNeed.Media.Imaging
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public class CustomBitmap : BitmapSource
    {
        private EventHandler downloadCompleted;
        private EventHandler<DownloadProgressEventArgs> downloadProgress;
        private EventHandler<ExceptionEventArgs> downloadFailed;
        private EventHandler<ExceptionEventArgs> decodeFailed;

        /// <summary>
        ///     Raised when the downloading of content is completed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation does nothing.
        /// </remarks>
        public override event EventHandler DownloadCompleted
        {
            add => downloadCompleted += value;
            remove => downloadCompleted -= value;
        }

        /// <summary>
        ///     Raised when the downloading of content has progressed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation does nothing.
        /// </remarks>
        public override event EventHandler<DownloadProgressEventArgs> DownloadProgress
        {
            add => downloadProgress += value;
            remove => downloadProgress -= value;
        }

        /// <summary>
        ///     Raised when the downloading of content has failed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation throws.
        /// </remarks>
        public override event EventHandler<ExceptionEventArgs> DownloadFailed
        {
            add => downloadFailed += value;
            remove => downloadFailed -= value;
        }

        /// <summary>
        ///     Raised when the downloading of content has progressed.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation does nothing.
        /// </remarks>
        public override event EventHandler<ExceptionEventArgs> DecodeFailed
        {
            add => decodeFailed += value;
            remove => decodeFailed -= value;
        }

        /// <summary>
        ///     Horizontal DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override double DpiX => 96.0;

        /// <summary>
        ///     Vertical DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override double DpiY => 96.0;

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override PixelFormat Format => PixelFormats.Bgr32;

        /// <summary>
        ///     Width of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override int PixelWidth => 0;

        /// <summary>
        ///     Height of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override int PixelHeight => 0;

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        /// </remarks>
        public override BitmapPalette Palette => null;

        /// <summary>
        ///     Whether or not the BitmapSource is downloading content.
        /// </summary>
        /// <remarks>
        ///     This is not applicable to all CustomBitmap classes, so
        ///     the base implementation return false.
        /// </remarks>
        public override bool IsDownloading => false;

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="pixels">
        ///     The destination array of pixels.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination array.
        /// </param>
        /// <param name="offset">
        ///     The starting index within the destination array to copy to.
        /// </param>
        /// <remarks>
        ///     Derived classes must override CopyPixelsCommon to implement
        ///     custom logic.
        /// </remarks>
        public sealed override void CopyPixels(Array pixels, int stride, int offset)
        {
            var sourceRect = new Int32Rect(0, 0, PixelWidth, PixelHeight);
            CopyPixels(sourceRect, pixels, stride, offset);
        }

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels to copy.
        /// </param>
        /// <param name="pixels">
        ///     The destination array of pixels.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination array.
        /// </param>
        /// <param name="offset">
        ///     The starting index within the destination array to copy to.
        /// </param>
        /// <remarks>
        ///     Derived classes must override CopyPixelsCommon to implement
        ///     custom logic.
        /// </remarks>
        public sealed override void CopyPixels(Int32Rect sourceRect, Array pixels, int stride, int offset)
        {
            ValidateArrayAndGetInfo(pixels, out var elementSize, out var bufferSize, out var elementType);

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            // We accept arrays of arbitrary value types - but not reference types.
            if (elementType is null || !elementType.IsValueType)
            {
                throw new ArgumentException(nameof(pixels));
            }

            checked
            {
                var offsetInBytes = offset * elementSize;
                if (offsetInBytes >= bufferSize)
                {
                    throw new IndexOutOfRangeException();
                }

                // Get the address of the data in the array by pinning it.
                var arrayHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                try
                {
                    // Adjust the buffer and bufferSize to account for the offset.
                    var buffer = arrayHandle.AddrOfPinnedObject();
                    buffer = new IntPtr((long)buffer + offsetInBytes);
                    bufferSize -= offsetInBytes;

                    CopyPixels(sourceRect, buffer, bufferSize, stride);
                }
                finally
                {
                    arrayHandle.Free();
                }
            }
        }

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The rectangle of pixels to copy.
        /// </param>
        /// <param name="buffer">
        ///     The destination buffer of pixels.
        /// </param>
        /// <param name="bufferSize">
        ///     The size of the buffer, in bytes.
        /// </param>
        /// <param name="stride">
        ///     The stride of the destination buffer.
        /// </param>
        /// <remarks>
        ///     Derived classes must override CopyPixelsCommon to implement
        ///     custom logic.
        /// </remarks>
        public sealed override void CopyPixels(Int32Rect sourceRect, IntPtr buffer, int bufferSize, int stride)
        {
            // WIC would specify NULL for the source rect to indicate that the
            // entire content should be copied.  WPF turns that into an empty
            // rect, which we inflate here to be the entire bounds.
            if (sourceRect.IsEmpty)
            {
                sourceRect.Width = PixelWidth;
                sourceRect.Height = PixelHeight;
            }

            if (sourceRect.X < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRect));
            }

            if (sourceRect.Width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRect));
            }

            if ((sourceRect.X + sourceRect.Width) > PixelWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRect));
            }

            if (sourceRect.Y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRect));
            }

            if (sourceRect.Height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRect));
            }

            if ((sourceRect.Y + sourceRect.Height) > PixelHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceRect));
            }

            if (buffer == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            checked
            {
                if (stride < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(stride));
                }

                var minStrideInBits = (uint)(sourceRect.Width * Format.BitsPerPixel);
                var minStrideInBytes = ((minStrideInBits + 7) / 8);
                if (stride < minStrideInBytes)
                {
                    throw new ArgumentOutOfRangeException(nameof(stride));
                }

                if (bufferSize < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(bufferSize));
                }

                var minBufferSize = (uint)((sourceRect.Height - 1) * stride) + minStrideInBytes;
                if (bufferSize < minBufferSize)
                {
                    throw new ArgumentOutOfRangeException(nameof(bufferSize));
                }
            }

            CopyPixelsCore(sourceRect, stride, bufferSize, buffer);
        }

        /// <summary>
        ///     Creates an instance of a CustomBitmap.
        /// </summary>
        /// <returns>
        ///     The new instance.
        /// </returns>
        /// <remarks>
        ///     The default implementation uses reflection to create a new
        ///     instance of this type.  Derived classes may override this
        ///     method if they wish to provide a more performant
        ///     implementation, or if their type does not have a public
        ///     default constructor.
        /// </remarks>
        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType());
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     When Freezable is cloned, WPF will make deep clones of all
        ///     writable, locally-set properties including expressions. The
        ///     property's base value is copied -- not the current value. WPF
        ///     skips read only DPs.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transfered to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void CloneCore(Freezable source)
        {
            base.CloneCore(source);

            var customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ false, /*willBeFrozen*/ false);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     When a Freezable's "current value" is cloned, WPF will make
        ///     deep clones of the "current values" of all writable,
        ///     locally-set properties. This has the effect of resolving
        ///     expressions to their values. WPF skips read only DPs.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transferred to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void CloneCurrentValueCore(Freezable source)
        {
            base.CloneCurrentValueCore(source);

            var customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ true, /*willBeFrozen*/ false);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     Freezable.GetAsFrozen is semantically equivalent to
        ///     Freezable.Clone().Freeze(), except that you can avoid copying
        ///     any portions of the Freezable graph which are already frozen.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transferred to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void GetAsFrozenCore(Freezable source)
        {
            base.GetAsFrozenCore(source);

            var customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ false, /*willBeFrozen*/ true);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="source">
        ///     The original instance to copy data from.
        /// </param>
        /// <remarks>
        ///     Freezable.GetCurrentValueAsFrozen is semantically equivalent to
        ///     Freezable.CloneCurrentValue().Freeze(), except that WPF will
        ///     avoid copying any portions of the Freezable graph which are
        ///     already frozen.
        ///     
        ///     If you derive from this class and have additional non-DP state
        ///     that should be transfered to copies, you should override the
        ///     CopyCommon method.
        /// </remarks>
        protected sealed override void GetCurrentValueAsFrozenCore(Freezable source)
        {
            base.GetCurrentValueAsFrozenCore(source);

            var customBitmapSource = (CustomBitmap)source;
            CopyCore(customBitmapSource, /*useCurrentValue*/ true, /*willBeFrozen*/ true);
        }

        /// <summary>
        ///     Copies data into a cloned instance.
        /// </summary>
        /// <param name="original">
        ///     The original instance to copy data from.
        /// </param>
        /// <param name="useCurrentValue">
        ///     Whether or not to copy the current value of expressions, or the
        ///     expressions themselves.
        /// </param>
        /// <param name="willBeFrozen">
        ///     Indicates whether or not the clone will be frozen.  If the
        ///     clone will be immediately frozen, there is no need to clone
        ///     data that is already frozen, you can just share the instance.
        /// </param>
        /// <remarks>
        ///     Override this method if you have additional non-DP state that
        ///     should be transferred to clones.
        /// </remarks>
        protected virtual void CopyCore(CustomBitmap original, bool useCurrentValue, bool willBeFrozen)
        {
        }

        /// <summary>
        ///     Requests pixels from this BitmapSource.
        /// </summary>
        /// <param name="sourceRect">
        ///     The caller can restrict the operation to a rectangle of
        ///     interest (ROI) using this parameter. The ROI sub-rectangle
        ///     must be fully contained in the bounds of the bitmap.
        ///     Specifying a null ROI implies that the whole bitmap should be
        ///     returned. Careful use of the ROI can be a significant
        ///     performance optimization when the pixel-production algorithm
        ///     is expensive - e.g. JPEG decoding. 
        /// </param>
        /// <param name="stride">
        ///     Defines the count of bytes between two vertically adjacent
        ///     pixels in the output buffer. 
        /// </param>
        /// <param name="bufferSize">
        ///     The size
        /// </param>
        /// <param name="buffer">
        ///     The caller controls the memory management and must provide an
        ///     output buffer of sufficient size to complete the call based on
        ///     the width, height and pixel format of the bitmap and the
        ///     sub-rectangle provided to the copy method. 
        /// </param>
        /// <remarks>
        ///     This is the main image processing routine. It instructs the
        ///     BitmapSource instance to produce pixels according to its
        ///     algorithm - this may involve decoding a portion of a JPEG
        ///     stored on disk, copying a block of memory, or even
        ///     analytically computing a complex gradient. The algorithm is
        ///     completely dependent on the implementation.
        ///     
        ///     Implementation of this method must only write to the first
        ///     (rc.Width*PixelFormat.BitsPerPixel+7)/8 bytes of each line of
        ///     the output buffer (in this case, a line is a consecutive string
        ///     of "stride" bytes).
        /// </remarks>
        protected virtual void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
        {
        }

        /// <summary>
        ///     Raises the download completed event.
        /// </summary>
        protected void RaiseDownloadCompleted()
        {
            downloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Raises the download progress event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDownloadProgress(DownloadProgressEventArgs e)
        {
            downloadProgress?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the download failed event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDownloadFailed(ExceptionEventArgs e)
        {
            downloadFailed?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the decode failed event.
        /// </summary>
        /// <param name="e"></param>
        protected void RaiseDecodeFailed(ExceptionEventArgs e)
        {
            decodeFailed?.Invoke(this, e);
        }

        /// <summary>
        ///     Get the size of the specified array and of the elements in it.
        /// </summary>
        /// <param name="pixels">
        ///     The array to get info about.
        /// </param>
        /// <param name="elementSize">
        ///     On output, will contain the size of the elements in the array.
        /// </param>
        /// <param name="sourceBufferSize">
        ///     On output, will contain the size of the array.
        /// </param>
        /// <param name="elementType">
        ///     On output, will contain the type of the elements in the array.
        /// </param>
        private static void ValidateArrayAndGetInfo(
            Array pixels,
            out int elementSize,
            out int sourceBufferSize,
            out Type elementType)
        {
            //
            // Assure that a valid pixels Array was provided.
            //
            if (pixels is null)
            {
                throw new ArgumentNullException(nameof(pixels));
            }

            if (pixels.Rank == 1)
            {
                if (pixels.GetLength(0) <= 0)
                {
                    throw new ArgumentException(nameof(pixels));
                }

                checked
                {
                    var exemplar = pixels.GetValue(0);
                    elementSize = Marshal.SizeOf(exemplar);
                    sourceBufferSize = pixels.GetLength(0) * elementSize;
                    elementType = exemplar.GetType();
                }
            }
            else if (pixels.Rank == 2)
            {
                if (pixels.GetLength(0) <= 0 || pixels.GetLength(1) <= 0)
                {
                    throw new ArgumentException(nameof(pixels));
                }

                checked
                {
                    var exemplar = pixels.GetValue(0, 0);
                    elementSize = Marshal.SizeOf(exemplar);
                    sourceBufferSize = pixels.GetLength(0) * pixels.GetLength(1) * elementSize;
                    elementType = exemplar.GetType();
                }
            }
            else
            {
                throw new ArgumentException(nameof(pixels));
            }
        }
    }
}
