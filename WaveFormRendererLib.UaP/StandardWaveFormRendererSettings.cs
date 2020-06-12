using Windows.UI;

namespace WaveFormRendererLib.UaP
{
    public class StandardWaveFormRendererSettings : WaveFormRendererSettings
    {
        public StandardWaveFormRendererSettings()
        {
            PixelsPerPeak = 1;
            SpacerPixels = 0;
            TopPeakPen = new Pen(Colors.Maroon, 1);
            BottomPeakPen = new Pen(Colors.Peru, 1);
        }


        public override Pen TopPeakPen { get; set; }

        // not needed
        public override Pen TopSpacerPen { get; set; }
        
        public override Pen BottomPeakPen { get; set; }
        
        // not needed
        public override Pen BottomSpacerPen { get; set; }
    }
}