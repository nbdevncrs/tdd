using System.Drawing;

namespace TagsCloudCore.CloudLayout;

public class CollisionIdentifier
{
    public bool IntersectsAny(Rectangle candidate, IEnumerable<Rectangle> rectangles)
    {
        return rectangles.Any(r => r.IntersectsWith(candidate));
    }
}