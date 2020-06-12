using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas;
using NAudio.Wave;
using Windows.Storage;
using Windows.UI;

namespace WaveFormRendererLib.UaP
{
    public class WaveFormRenderer
    {
        public ICanvasImage Render(string selectedFile, WaveFormRendererSettings settings)
        {
            return Render(selectedFile, new MaxPeakProvider(), settings);
        }

        public ICanvasImage Render(string selectedFile, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            
            using (var reader = new MediaFoundationReader(selectedFile))
            {
                int bytesPerSample = (reader.WaveFormat.BitsPerSample / 8);
                var samples = reader.Length / (bytesPerSample);
                var samplesPerPixel = (int)(samples / settings.Width);
                var stepSize = settings.PixelsPerPeak + settings.SpacerPixels;
                peakProvider.Init(reader.ToSampleProvider(), samplesPerPixel * stepSize);
                return Render(peakProvider, settings);
            }
        }

        public Task<ICanvasImage> RenderAsync(StorageFile selectedFile, WaveFormRendererSettings settings)
        {
            return RenderAsync(selectedFile, new MaxPeakProvider(), settings);
        }

        public async Task<ICanvasImage> RenderAsync(StorageFile selectedFile, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            var randomAcessStream = await selectedFile.OpenReadAsync();
            using (var reader = new StreamMediaFoundationReader(randomAcessStream.AsStream()))
            {
                int bytesPerSample = (reader.WaveFormat.BitsPerSample / 8);
                var samples = reader.Length / (bytesPerSample);
                var samplesPerPixel = (int)(samples / settings.Width);
                var stepSize = settings.PixelsPerPeak + settings.SpacerPixels;
                peakProvider.Init(reader.ToSampleProvider(), samplesPerPixel * stepSize);
                return Render(peakProvider, settings);
            }
        }

        private static ICanvasImage Render(IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            if (settings.DecibelScale)
                peakProvider = new DecibelPeakProvider(peakProvider, 48);

            var b = new CanvasRenderTarget(settings.Device, settings.Width, settings.TopHeight + settings.BottomHeight, 96);
            using(var ds = b.CreateDrawingSession())
            {
                if (settings.BackgroundColor == Colors.Transparent)
                {
                    ds.Clear(Colors.Transparent);
                }

                ds.FillRectangle(0, 0, (float)b.Bounds.Width, (float)b.Bounds.Height, settings.BackgroundBrush);
                var midPoint = settings.TopHeight;

                int x = 0;
                var currentPeak = peakProvider.GetNextPeak();
                while (x < settings.Width)
                {
                    var nextPeak = peakProvider.GetNextPeak();
                    
                    for (int n = 0; n < settings.PixelsPerPeak; n++)
                    {
                        var lineHeight = settings.TopHeight * currentPeak.Max;
                        ds.DrawPen(settings.TopPeakPen, x, midPoint, x, midPoint - lineHeight);
                        lineHeight = settings.BottomHeight * currentPeak.Min;
                        ds.DrawPen(settings.BottomPeakPen, x, midPoint, x, midPoint - lineHeight);
                        x++;
                    }

                    for (int n = 0; n < settings.SpacerPixels; n++)
                    {
                        // spacer bars are always the lower of the 
                        var max = Math.Min(currentPeak.Max, nextPeak.Max);
                        var min = Math.Max(currentPeak.Min, nextPeak.Min);

                        var lineHeight = settings.TopHeight * max;
                        ds.DrawPen(settings.TopSpacerPen, x, midPoint, x, midPoint - lineHeight);
                        lineHeight = settings.BottomHeight * min;
                        ds.DrawPen(settings.BottomSpacerPen, x, midPoint, x, midPoint - lineHeight);
                        x++;
                    }
                    currentPeak = nextPeak;
                }
            }
            
            return b;
        }


    }
}
