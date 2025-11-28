using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudTests;

public class ToCenterTightenerTests
{
    [Test]
    public void ToCenterTightener_ShouldMoveRectangleCloserToCenter_WhenNoObstacles_Test()
    {
        var center = new Point(0, 0);
        var rectangle = new Rectangle(50, 50, 20, 20);

        var result = ToCenterTightener.Tighten(rectangle, center, []);

        Distance(result, center).Should().BeLessThan(Distance(rectangle, center));
    }

    [Test]
    public void ToCenterTightener_ShouldStopBeforeCollision_WhenObstacleIsPresent_Test()
    {
        var center = new Point(0, 0);
        var rectangle = new Rectangle(40, 0, 20, 20);
        var obstacle = new Rectangle(10, 0, 20, 20);

        var result = ToCenterTightener.Tighten(rectangle, center, [obstacle]);

        result.IntersectsAny([obstacle]).Should().BeFalse();
        Distance(result, center).Should().BeGreaterThan(0);
    }
    
    private static double Distance(Rectangle rect, Point center)
    {
        var cx = rect.Left + rect.Width / 2.0;
        var cy = rect.Top + rect.Height / 2.0;
        return Math.Sqrt((cx - center.X) * (cx - center.X) + (cy - center.Y) * (cy - center.Y));
    }
}