using System.Drawing;

namespace TagsCloudCore.CloudVisualization
{
    public static class CloudVisualizer
    {
        public static void SaveLayoutToFile(string filePath, Rectangle[] rectangles, int padding = 50)
        {
            if (rectangles.Length == 0)
                throw new ArgumentException("No rectangles to visualize");

            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach (var r in rectangles)
            {
                if (r.Left < minX) minX = r.Left;
                if (r.Right > maxX) maxX = r.Right;
                if (r.Top < minY) minY = r.Top;
                if (r.Bottom > maxY) maxY = r.Bottom;
            }

            var width = (maxX - minX) + padding * 2;
            var height = (maxY - minY) + padding * 2;

            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);

            var random = new Random();

            foreach (var rectangle in rectangles)
            {
                var shifted = rectangle with
                {
                    X = rectangle.Left - minX + padding, Y = rectangle.Top - minY + padding
                };

                var color = Color.FromArgb(
                    255,
                    random.Next(40, 255),
                    random.Next(40, 255),
                    random.Next(40, 255));

                var brush = new SolidBrush(color);
                var pen = new Pen(Color.White, 1);

                graphics.FillRectangle(brush, shifted);
                graphics.DrawRectangle(pen, shifted);
            }

            bitmap.Save(filePath);
        }
    }
}