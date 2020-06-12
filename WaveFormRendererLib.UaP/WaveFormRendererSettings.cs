using Windows.UI;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas;

namespace WaveFormRendererLib.UaP
{
    public class WaveFormRendererSettings : ICanvasResourceCreator
    {
        protected WaveFormRendererSettings()
        {
            Width = 800;
            TopHeight = 50;
            BottomHeight = 50;
            PixelsPerPeak = 1;
            SpacerPixels = 0;
            BackgroundColor = Colors.Beige;
            Device = CanvasDevice.GetSharedDevice();
        }

        // for display purposes only
        public string Name { get; set; }

        public int Width { get; set; }

        public int TopHeight { get; set; }
        public int BottomHeight { get; set; }
        public int PixelsPerPeak { get; set; }
        public int SpacerPixels { get; set; }
        public virtual Pen TopPeakPen { get; set; }
        public virtual Pen TopSpacerPen { get; set; }
        public virtual Pen BottomPeakPen { get; set; }
        public virtual Pen BottomSpacerPen { get; set; }
        public bool DecibelScale { get; set; }
        public Color BackgroundColor { get; set; }
        public CanvasBitmap BackgroundImage { get; set; }
        public ICanvasBrush BackgroundBrush {
            get
            {
                if (BackgroundImage == null) return new CanvasSolidColorBrush(this, BackgroundColor);
                return new CanvasImageBrush(this, BackgroundImage);
            }
        }

        protected Pen CreateGradientPen(int height, Color startColor, Color endColor)
        {
            var brush = new CanvasLinearGradientBrush(this, startColor, endColor);
            return new Pen(brush);
        }

        public CanvasDevice Device { get; }
    }
}