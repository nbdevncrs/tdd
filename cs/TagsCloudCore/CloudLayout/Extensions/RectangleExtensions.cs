using System.Drawing;

namespace TagsCloudCore.CloudLayout.Extensions;

public static class RectangleExtensions
{
    public static bool IntersectsAny(this Rectangle candidate, IEnumerable<Rectangle> rectangles)
    {
        return rectangles?.Any(r => r.IntersectsWith(candidate)) ??
               throw new ArgumentNullException(nameof(rectangles), "Existing rectangles can't be null");
    }
}