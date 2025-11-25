using System.Drawing;

namespace TagsCloudCore.CloudLayout;

public class ToCenterTightener
{
    public Rectangle Tighten(
        Rectangle rect,
        Point center,
        IEnumerable<Rectangle> existing,
        CollisionIdentifier collisionIdentifier)
    {
        var current = rect;

        while (true)
        {
            var moved = MoveOneStepTowardsCenter(current, center);

            if (moved == current ||
                collisionIdentifier.IntersectsAny(moved, existing) ||
                GetDistance(moved, center) > GetDistance(current, center))
            {
                return current;
            }

            current = moved;
        }
    }

    private static Rectangle MoveOneStepTowardsCenter(Rectangle r, Point center)
    {
        var cx = r.X + r.Width / 2;
        var cy = r.Y + r.Height / 2;

        var dx = Math.Sign(center.X - cx);
        var dy = Math.Sign(center.Y - cy);

        if (dx == 0 && dy == 0)
            return r;

        return r with { X = r.X + dx, Y = r.Y + dy };
    }

    private static double GetDistance(Rectangle r, Point center)
    {
        var cx = r.X + r.Width / 2;
        var cy = r.Y + r.Height / 2;

        var dx = cx - center.X;
        var dy = cy - center.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }
}