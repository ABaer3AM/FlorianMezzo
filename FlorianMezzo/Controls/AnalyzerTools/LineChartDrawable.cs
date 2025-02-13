using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace FlorianMezzo.Controls.AnalyzerTools
{
    public class LineChartDrawable : IDrawable
    {

        public List<List<PointF>> Lines { get; set; } = new();
        public List<string> LineNames { get; set; } = new();
        private float Margin = 50; 
        private float GraphWidth = 300;
        private float GraphHeight = 200;

        public LineChartDrawable(float graphWidth, float graphHeight)
        {
            graphWidth = GraphWidth;
            graphHeight = GraphHeight;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeSize = 2;
            canvas.Antialias = true;
            float originX = Margin;
            float originY = dirtyRect.Height - Margin;

            // **Set Background to Black**
            canvas.FillColor = Colors.Black;
            canvas.FillRectangle(dirtyRect);

            // **Draw Axes**
            canvas.StrokeColor = Colors.White;
            canvas.DrawLine(originX, originY, originX, originY - GraphHeight); // Y-axis
            canvas.DrawLine(originX, originY, originX + GraphWidth, originY);  // X-axis

            // **Draw Grid & Labels**
            DrawGrid(canvas, originX, originY);
            DrawAxisLabel(canvas, dirtyRect);

            // **Draw Lines**
            Color[] colors = { Colors.Cyan, Colors.Magenta, Colors.Yellow, Colors.Orange };

            for (int i = 0; i < Lines.Count; i++)
            {
                var points = Lines[i];
                canvas.StrokeColor = colors[i % colors.Length];

                if (points.Count > 1)
                {
                    for (int j = 0; j < points.Count - 1; j++)
                    {
                        canvas.DrawLine(points[j].X + originX, originY - points[j].Y,
                                        points[j + 1].X + originX, originY - points[j + 1].Y);
                    }
                }
            }

            // **Draw Legend**
            DrawLegend(canvas, dirtyRect, colors);
        }

        private void DrawGrid(ICanvas canvas, float originX, float originY)
        {
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 1;
            int yMax = 100; // Max battery percentage

            float xMax = GetMaxX(); // Get the max X value from all lines

            // **Draw horizontal grid lines (Y-axis labels)**
            for (int i = 0; i <= 5; i++)
            {
                float y = originY - (i * GraphHeight / 5);
                int label = (yMax / 5) * i;
                canvas.DrawLine(originX, y, originX + GraphWidth, y);
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{label}%", 5, y, HorizontalAlignment.Left);
            }

            // **Draw vertical grid lines (X-axis labels in % of session time)**
            for (int i = 0; i <= 5; i++)
            {
                float x = originX + (i * GraphWidth / 5);
                int label = (int)((i / 5.0) * 100); // Convert to percentage
                canvas.DrawLine(x, originY, x, originY - GraphHeight);
                canvas.FontColor = Colors.White;
                canvas.DrawString($"{label}%", x, originY + 15, HorizontalAlignment.Center);
            }
        }

        private void DrawAxisLabel(ICanvas canvas, RectF dirtyRect)
        {
            // **Label X-Axis as "Session Time (%)"**
            canvas.FontColor = Colors.White;
            canvas.DrawString("Session Time (%)", dirtyRect.Width / 2, dirtyRect.Height - 5, HorizontalAlignment.Center);
        }

        private void DrawLegend(ICanvas canvas, RectF dirtyRect, Color[] colors)
        {
            float legendX = 20; // **Move to bottom-left**
            float legendY = 20; // **Below the X-axis label**
            float spacing = 20;

            canvas.FontColor = Colors.White;
            canvas.FontSize = 14;

            for (int i = 0; i < LineNames.Count; i++)
            {
                // **Draw line sample (matching color)**
                canvas.StrokeColor = colors[i % colors.Length];
                canvas.DrawLine(legendX, legendY + i * spacing, legendX + 20, legendY + i * spacing);

                // **Draw the label name beside it**
                canvas.DrawString(LineNames[i], legendX + 30, legendY + i * spacing, HorizontalAlignment.Left);
            }
        }
        /*
        private void DrawLegend(ICanvas canvas, RectF dirtyRect, Color[] colors)
        {
            float legendX = dirtyRect.Width - 120;
            float legendY = 20;
            float spacing = 20;

            canvas.FontColor = Colors.White;
            canvas.FontSize = 14;

            for (int i = 0; i < LineNames.Count; i++)
            {
                canvas.StrokeColor = colors[i % colors.Length];
                canvas.DrawLine(legendX, legendY + i * spacing, legendX + 20, legendY + i * spacing);
                canvas.DrawString(LineNames[i], legendX + 30, legendY + i * spacing, HorizontalAlignment.Left);
            }
        }
        */

        private float GetMaxX()
        {
            float maxX = 0;
            foreach (var line in Lines)
            {
                if (line.Count > 0)
                {
                    maxX = Math.Max(maxX, line[^1].X); // Get last X value
                }
            }
            return maxX;
        }

        public float getWidth()
        {
            return GraphWidth;
        }
        public float getHeight()
        {
            return GraphHeight;
        }
    }
}
