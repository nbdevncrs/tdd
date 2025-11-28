using System.Drawing;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudCore.CloudLayout;

public static class ToCenterTightener
{
    public static Rectangle Tighten(
        Rectangle rectangle,
        Point center,
        IEnumerable<Rectangle> existing)
    {
        var existingRectangles = existing.ToList();
        var current = rectangle;

        while (IsMovingForwardNeeded(current, center, existingRectangles, out var moved))
        {
            current = moved;
        }

        return current;
    }

    private static bool IsMovingForwardNeeded(
        Rectangle current,
        Point center,
        List<Rectangle> existing,
        out Rectangle moved)
    {
        moved = MoveOneStepTowardsCenter(current, center);
        var noMovementDone = moved == current;
        var wouldIntersect = moved.IntersectsAny(existing);
        var positionBecomesWorse = GetDistance(moved, center) > GetDistance(current, center);
        
        return !(noMovementDone || wouldIntersect || positionBecomesWorse);
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