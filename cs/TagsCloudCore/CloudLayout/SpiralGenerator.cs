using System.Drawing;

namespace TagsCloudCore.CloudLayout;

public class SpiralGenerator(Point center, double angleStep = 0.1, double radiusStep = 0.5)
{
    private double angle;

    public Point GetNextSpiralPoint()
    {
        var radius = radiusStep * angle;

        var point = new Point(
            center.X + (int)(radius * Math.Cos(angle)),
            center.Y + (int)(radius * Math.Sin(angle)));

        angle += angleStep;

        return point;
    }
}