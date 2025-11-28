using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout;

namespace TagsCloudTests;

public class CircularCloudLayouterTests
{
    [Test]
    public void CircularCloudLayouter_ShouldPlaceFirstRectangleCenterExactlyAtCloudCenter_Test()
    {
        var center = new Point(200, 200);
        var layouter = new CircularCloudLayouter(center);

        var size = new Size(40, 20);

        var rectangle = layouter.PutNextRectangle(size);

        var rectangleCenter = new Point(
            rectangle.X + rectangle.Width / 2,
            rectangle.Y + rectangle.Height / 2);

        rectangleCenter.Should().Be(center);
    }

    [Test]
    public void CircularCloudLayouter_ShouldNotProduceOverlappingRectangles_WhenPlacingMany_Test()
    {
        var center = new Point(500, 500);
        var layouter = new CircularCloudLayouter(center);

        const int rectanglesCount = 80;

        var random = new Random();

        var sizes = Enumerable.Range(0, rectanglesCount)
            .Select(_ => new Size(
                random.Next(10, 60),
                random.Next(10, 40)))
            .ToList();

        var placedRectangles = sizes
            .Select(s => layouter.PutNextRectangle(s))
            .ToList();

        var hasOverlap = false;

        for (var i = 0; i < placedRectangles.Count; i++)
        {
            for (var j = i + 1; j < placedRectangles.Count; j++)
            {
                if (!placedRectangles[i].IntersectsWith(placedRectangles[j])) continue;
                hasOverlap = true;
                break;
            }

            if (hasOverlap)
                break;
        }

        hasOverlap.Should().BeFalse();
    }


    [Test]
    public void CircularCloudLayouter_ShouldThrow_WhenSizeIsZeroOrNegative_Test()
    {
        var center = new Point(100, 100);
        var layouter = new CircularCloudLayouter(center);

        Action act = () => layouter.PutNextRectangle(new Size(0, 10));

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Size must be positive (Parameter 'rectangleSize')");
    }

    [Test]
    public void CircularCloudLayouter_ShouldReturnReasonablyCompactCloud_Test()
    {
        var center = new Point(500, 500);
        var layouter = new CircularCloudLayouter(center);

        var random = new Random();
        var sizes = Enumerable.Range(0, 200)
            .Select(_ => new Size(
                random.Next(10, 40),
                random.Next(10, 30)))
            .ToList();

        var rectangles = sizes
            .Select(s => layouter.PutNextRectangle(s))
            .ToList();

        var totalArea = rectangles.Sum(r => r.Width * r.Height);

        var maxRadius = rectangles.Max(MaxDistanceToCenter);

        var circleArea = Math.PI * maxRadius * maxRadius;
        var density = totalArea / circleArea;

        density.Should().BeGreaterThan(0.20);
        return;

        double MaxDistanceToCenter(Rectangle r)
        {
            var rectangleCenter = new Point(r.Left + r.Width / 2, r.Top + r.Height / 2);
            var dx = rectangleCenter.X - center.X;
            var dy = rectangleCenter.Y - center.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }


    [Test]
    public void CircularCloudLayouter_ShouldFormRoughCircularShape_Test()
    {
        var center = new Point(500, 500);
        var layouter = new CircularCloudLayouter(center);

        var random = new Random();
        var sizes = Enumerable.Range(0, 150)
            .Select(_ => new Size(
                random.Next(10, 50),
                random.Next(10, 35)))
            .ToList();

        var rectangles = sizes
            .Select(s => layouter.PutNextRectangle(s))
            .ToList();

        var distances = rectangles.Select(Distance).ToArray();

        var averageDistance = distances.Average();
        var variance = distances.Select(d => Math.Pow(d - averageDistance, 2)).Average();
        var standardDeviation = Math.Sqrt(variance);

        (standardDeviation / averageDistance).Should().BeLessThan(0.45);
        return;

        double Distance(Rectangle r)
        {
            var rc = new Point(r.Left + r.Width / 2, r.Top + r.Height / 2);
            var dx = rc.X - center.X;
            var dy = rc.Y - center.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}