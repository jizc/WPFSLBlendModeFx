namespace Microsoft.DwayneNeed.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    /// <summary>
    ///     The VisualWrapper simply integrates a raw Visual child into a tree
    ///     of FrameworkElements.
    /// </summary>
    [ContentProperty("Child")]
    public class VisualWrapper : FrameworkElement
    {
        private Visual child;

        public Visual Child
        {
            get => child;
            set
            {
                if (child != null)
                {
                    RemoveVisualChild(child);
                }

                child = value;

                if (child != null)
                {
                    AddVisualChild(child);
                }
            }
        }

        protected override int VisualChildrenCount => child != null ? 1 : 0;

        protected override Visual GetVisualChild(int index)
        {
            if (child != null && index == 0)
            {
                return child;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}
