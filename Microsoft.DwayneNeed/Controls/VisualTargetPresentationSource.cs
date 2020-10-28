namespace Microsoft.DwayneNeed.Controls
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    ///     The VisualTargetPresentationSource represents the root
    ///     of a visual subtree owned by a different thread that the
    ///     visual tree in which is is displayed.
    /// </summary>
    /// <remarks>
    ///     A HostVisual belongs to the same UI thread that owns the
    ///     visual tree in which it resides.
    ///     
    ///     A HostVisual can reference a VisualTarget owned by another
    ///     thread.
    ///     
    ///     A VisualTarget has a root visual.
    ///     
    ///     VisualTargetPresentationSource wraps the VisualTarget and
    ///     enables basic functionality like Loaded, which depends on
    ///     a PresentationSource being available.
    /// </remarks>
    public class VisualTargetPresentationSource : PresentationSource
    {
        private readonly VisualTarget visualTarget;

        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            visualTarget = new VisualTarget(hostVisual);
        }

        public override Visual RootVisual
        {
            get => visualTarget.RootVisual;
            set
            {
                var oldRoot = visualTarget.RootVisual;

                // Set the root visual of the VisualTarget.  This visual will
                // now be used to visually compose the scene.
                visualTarget.RootVisual = value;

                // Tell the PresentationSource that the root visual has
                // changed.  This kicks off a bunch of stuff like the
                // Loaded event.
                RootChanged(oldRoot, value);

                // Kickoff layout...
                if (value is UIElement rootElement)
                {
                    rootElement.Measure(
                        new Size(double.PositiveInfinity, double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));
                }
            }
        }

        // We don't support disposing this object.
        public override bool IsDisposed => false;

        protected override CompositionTarget GetCompositionTargetCore() => visualTarget;
    }
}
