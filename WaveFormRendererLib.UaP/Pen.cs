using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace WaveFormRendererLib.UaP
{
    public class Pen
    {
        public Pen(ICanvasBrush brush, double width = 1)
        {
            Brush = brush;
            Width = width;
        }

        public Pen(Color color, double width = 1)
        {
            Color = color;
            Width = width;
        }

        public ICanvasBrush Brush { get; set; }

        public Color Color
        {
            get
            {
                if (Brush is CanvasSolidColorBrush colorBrush)
                {
                    return colorBrush.Color;
                }
                return Colors.Transparent;
            }
            set
            {
                Brush?.Dispose();
                Brush = new CanvasSolidColorBrush(CanvasDevice.GetSharedDevice(), value);
            }
        }

        public double Width { get; set; }

    }

    internal static class DrawingSessionPenExtensions
    {
        internal static void DrawPen(this CanvasDrawingSession ds, Pen pen, float x1, float y1, float x2, float y2)
        {
            ds.DrawLine(x1, y1, x2, y2, pen.Brush, (float)pen.Width);
        }

        internal static void DrawPen(this CanvasDrawingSession ds, Pen pen, Point p1, Point p2)
        {
            ds.DrawPen(pen, p1.ToVector2(), p2.ToVector2());
        }

        internal static void DrawPen(this CanvasDrawingSession ds, Pen pen, Vector2 p1, Vector2 p2)
        {
            ds.DrawLine(p1, p2, pen.Brush, (float)pen.Width);
        }
    }
}
