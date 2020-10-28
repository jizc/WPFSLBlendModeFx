﻿namespace Microsoft.DwayneNeed.Media.Imaging
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    ///     The ChainedBitmap class is the base class for custom bitmaps that
    ///     processes the content of another source.
    /// </summary>
    /// <remarks>
    ///     The default implementation of the BitmapSource virtuals is to
    ///     delegate to the source.  This makes sense for most properties like
    ///     DpiX, DpiY, PixelWidth, PixelHeight, etc, as in many scenarios
    ///     these properties are the same for the entire chain of bitmap
    ///     sources.  However, derived classes should pay special attention to
    ///     the Format property.  Many bitmap processors only support a limited
    ///     number of pixel formats, and they should return this for the
    ///     Format property.  ChainedBitmap will take care of converting the
    ///     pixel format as needed in the base implementation of CopyPixels.
    /// </remarks>
    public class ChainedBitmap : CustomBitmap
    {
        /// <summary>
        ///     The DependencyProperty for the Source property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                nameof(Source),
                typeof(BitmapSource),
                typeof(ChainedBitmap),
                new FrameworkPropertyMetadata(OnSourcePropertyChanged));

        private PixelFormat formatConverterSourceFormat;
        private FormatConvertedBitmap formatConverter;

        /// <summary>
        ///     The BitmapSource to chain.
        /// </summary>
        public BitmapSource Source
        {
            get => (BitmapSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        ///     Horizontal DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override double DpiX
        {
            get
            {
                var source = Source;
                return source?.DpiX ?? base.DpiX;
            }
        }

        /// <summary>
        ///     Vertical DPI of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override double DpiY
        {
            get
            {
                var source = Source;
                return source?.DpiY ?? base.DpiY;
            }
        }

        /// <summary>
        ///     Pixel format of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override PixelFormat Format
        {
            get
            {
                var source = Source;
                return source?.Format ?? base.Format;
            }
        }

        /// <summary>
        ///     Width of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override int PixelWidth
        {
            get
            {
                var source = Source;
                return source?.PixelWidth ?? base.PixelWidth;
            }
        }

        /// <summary>
        ///     Height of the bitmap contents.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override int PixelHeight
        {
            get
            {
                var source = Source;
                return source?.PixelHeight ?? base.PixelHeight;
            }
        }

        /// <summary>
        ///     Palette of the bitmap.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override BitmapPalette Palette
        {
            get
            {
                var source = Source;
                return source?.Palette ?? base.Palette;
            }
        }

        /// <summary>
        ///     Whether or not the BitmapSource is downloading content.
        /// </summary>
        /// <remarks>
        ///     Derived classes can override this to specify their own value.
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        public override bool IsDownloading
        {
            get
            {
                var source = Source;
                return source != null && source.IsDownloading;
            }
        }

        /// <summary>
        ///     Creates an instance of the ChainedBitmap class.
        /// </summary>
        /// <returns>
        ///     The new instance.  If you derive from this class, you must
        ///     override this method to return your own type.
        /// </returns>
        protected override Freezable CreateInstanceCore() => new ChainedBitmap();

        /// <summary>
        ///     Transitions this instance into a thread-safe, read-only mode.
        /// </summary>
        /// <param name="isChecking">
        ///     Whether or not the transition should really happen, or just to
        ///     determine if the transition is possible.
        /// </param>
        /// <remarks>
        ///     Override this method if you have additional non-DP state that
        ///     should be frozen along with the instance.
        /// </remarks>
        protected override bool FreezeCore(bool isChecking)
        {
            if (formatConverter != null)
            {
                if (isChecking)
                {
                    return formatConverter.CanFreeze;
                }

                formatConverter.Freeze();
            }

            return true;
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
        ///     should be transfered to clones.
        /// </remarks>
        protected override void CopyCore(CustomBitmap original, bool useCurrentValue, bool willBeFrozen)
        {
            var originalChainedBitmap = (ChainedBitmap)original;
            if (originalChainedBitmap.formatConverter != null)
            {
                if (useCurrentValue)
                {
                    if (willBeFrozen)
                    {
                        formatConverter =
                            (FormatConvertedBitmap)originalChainedBitmap.formatConverter.GetCurrentValueAsFrozen();
                    }
                    else
                    {
                        formatConverter = originalChainedBitmap.formatConverter.CloneCurrentValue();
                    }
                }
                else
                {
                    if (willBeFrozen)
                    {
                        formatConverter = (FormatConvertedBitmap)originalChainedBitmap.formatConverter.GetAsFrozen();
                    }
                    else
                    {
                        formatConverter = originalChainedBitmap.formatConverter.Clone();
                    }
                }
            }
        }

        protected virtual void OnSourcePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            // Stop listening for the download and decode events on the old source.
            if (e.OldValue is BitmapSource oldValue)
            {
                oldValue.DownloadCompleted -= OnSourceDownloadCompleted;
                oldValue.DownloadProgress -= OnSourceDownloadProgress;
                oldValue.DownloadFailed -= OnSourceDownloadFailed;
                oldValue.DecodeFailed -= OnSourceDecodeFailed;
            }

            // Start listening for the download and decode events on the new source.
            if (e.NewValue is BitmapSource newValue)
            {
                newValue.DownloadCompleted += OnSourceDownloadCompleted;
                newValue.DownloadProgress += OnSourceDownloadProgress;
                newValue.DownloadFailed += OnSourceDownloadFailed;
                newValue.DecodeFailed += OnSourceDecodeFailed;
            }
        }

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
        ///     This implementation simply delegates to the source, if present.
        /// </remarks>
        protected override void CopyPixelsCore(Int32Rect sourceRect, int stride, int bufferSize, IntPtr buffer)
        {
            var source = Source;
            var convertedSource = source;

            if (source != null)
            {
                var sourceFormat = source.Format;
                var destinationFormat = Format;

                if (sourceFormat != destinationFormat)
                {
                    // We need a format converter.  Reuse the cached one if
                    // it still matches.
                    if (formatConverter is null
                        || formatConverter.Source != source
                        || formatConverter.Format != destinationFormat
                        || formatConverterSourceFormat != sourceFormat)
                    {
                        WritePreamble();
                        formatConverterSourceFormat = sourceFormat;
                        formatConverter = new FormatConvertedBitmap(source, destinationFormat, Palette, 0);
                        WritePostscript();
                    }

                    convertedSource = formatConverter;
                }

                convertedSource.CopyPixels(sourceRect, buffer, bufferSize, stride);
            }
        }

        private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chainedBitmap = (ChainedBitmap)d;
            chainedBitmap.OnSourcePropertyChanged(e);
        }

        private void OnSourceDownloadCompleted(object sender, EventArgs e)
        {
            RaiseDownloadCompleted();
        }

        private void OnSourceDownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            RaiseDownloadProgress(e);
        }

        private void OnSourceDownloadFailed(object sender, ExceptionEventArgs e)
        {
            RaiseDownloadFailed(e);
        }

        private void OnSourceDecodeFailed(object sender, ExceptionEventArgs e)
        {
            RaiseDecodeFailed(e);
        }
    }
}
