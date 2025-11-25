using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout;

namespace TagsCloudTests;

public class CloudLayoutTests
{
    [Test]
    public void SpiralGenerator_ShouldReturnCenterPoint_OnFirstCall_Test()
    {
        var center = new Point(100, 100);
        var spiralGenerator = new SpiralGenerator(center);

        var firstPoint = spiralGenerator.GetNextSpiralPoint();

        firstPoint.Should().Be(center);
    }

    [Test]
    public void SpiralGenerator_ShouldEventuallyMoveAwayFromCenter_Test()
    {
        var center = new Point(0, 0);
        var spiralGenerator = new SpiralGenerator(center);
        
        var spiralPoint = spiralGenerator.GetNextSpiralPoint();;
        
        for (var i = 0; i < 200; i++)
            spiralPoint = spiralGenerator.GetNextSpiralPoint();

        Distance(spiralPoint, center).Should().BeGreaterThan(0);
    }
    
    [Test]
    public void CollisionIdentifier_ShouldDetectIntersection_WhenRectanglesOverlap_Test()
    {
        var collisionIdentifier = new CollisionIdentifier();
        var existing = new[]
        {
            new Rectangle(new Point(0, 0), new Size(50, 50))
        };

        var candidate = new Rectangle(new Point(30, 30), new Size(40, 40));

        var result = collisionIdentifier.IntersectsAny(candidate, existing);

        result.Should().BeTrue();
    }

    [Test]
    public void CollisionIdentifier_ShouldNotDetectIntersection_WhenRectanglesDoNotOverlap_Test()
    {
        var collisionIdentifier = new CollisionIdentifier();
        var existing = new[]
        {
            new Rectangle(new Point(0, 0), new Size(50, 50))
        };

        var candidate = new Rectangle(new Point(100, 100), new Size(20, 20));

        var result = collisionIdentifier.IntersectsAny(candidate, existing);

        result.Should().BeFalse();
    }

    [Test]
    public void ToCenterTightener_ShouldMoveRectangleCloserToCenter_WhenNoObstacles_Test()
    {
        var center = new Point(0, 0);
        var rectangle = new Rectangle(50, 50, 20, 20);
        var tightener = new ToCenterTightener();
        var collisionIdentifier = new CollisionIdentifier();

        var result = tightener.Tighten(rectangle, center, [], collisionIdentifier);

        Distance(result, center).Should().BeLessThan(Distance(rectangle, center));
    }

    [Test]
    public void ToCenterTightener_ShouldStopBeforeCollision_WhenObstacleIsPresent_Test()
    {
        var center = new Point(0, 0);
        var rectangle = new Rectangle(40, 0, 20, 20);

        var obstacle = new Rectangle(10, 0, 20, 20);

        var tightener = new ToCenterTightener();
        var collisionIdentifier = new CollisionIdentifier();

        var result = tightener.Tighten(rectangle, center, [obstacle], collisionIdentifier);

        collisionIdentifier.IntersectsAny(result, [obstacle]).Should().BeFalse();
        Distance(result, center).Should().BeGreaterThan(0);
    }

    [Test]
    public void CircularCloudLayouter_ShouldPlaceFirstRectangleExactlyAtCenter_Test()
    {
        var center = new Point(200, 200);
        var layouter = new CircularCloudLayouter(
            center,
            new CollisionIdentifier(),
            new ToCenterTightener(),
            new SpiralGenerator(center)
        );

        var size = new Size(40, 20);
        var rectangle = layouter.PutNextRectangle(size);

        var rectangleCenter = new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);

        rectangleCenter.Should().Be(center);
    }

    [Test]
    public void CircularCloudLayouter_ShouldNotProduceOverlappingRectangles_WhenPlacingMany_Test()
    {
        var center = new Point(500, 500);
        var layouter = new CircularCloudLayouter(
            center,
            new CollisionIdentifier(),
            new ToCenterTightener(),
            new SpiralGenerator(center)
        );

        var sizes = Enumerable.Range(0, 50)
            .Select(_ => new Size(20, 10))
            .ToList();

        var placedRectangles = sizes.Select(s => layouter.PutNextRectangle(s)).ToList();

        foreach (var r1 in placedRectangles)
        foreach (var r2 in placedRectangles.Where(r => r != r1))
            r1.IntersectsWith(r2).Should().BeFalse();
    }

    [Test]
    public void CircularCloudLayouter_ShouldThrow_WhenSizeIsZeroOrNegative_Test()
    {
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(
            center,
            new CollisionIdentifier(),
            new ToCenterTightener(),
            new SpiralGenerator(center)
        );

        Action act = () => layouter.PutNextRectangle(new Size(0, 10));

        act.Should().Throw<ArgumentException>();
    }

    private static double Distance(Point a, Point b)
        => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

    private static double Distance(Rectangle rect, Point center)
    {
        var cx = rect.Left + rect.Width / 2.0;
        var cy = rect.Top + rect.Height / 2.0;
        return Math.Sqrt((cx - center.X) * (cx - center.X) + (cy - center.Y) * (cy - center.Y));
    }
}