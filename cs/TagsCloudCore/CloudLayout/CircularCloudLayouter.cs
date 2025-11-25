using System.Drawing;

namespace TagsCloudCore.CloudLayout;

public class CircularCloudLayouter(
    Point center,
    CollisionIdentifier collisionIdentifier,
    ToCenterTightener toCenterTightener,
    SpiralGenerator spiral)
{
    private readonly List<Rectangle> rectangles = [];
    public IEnumerable<Rectangle> PlacedRectangles => rectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Size must be positive", nameof(rectangleSize));

        Rectangle currentRectangle;

        while (true)
        {
            var nextPossibleLocation = spiral.GetNextSpiralPoint();

            currentRectangle = new Rectangle(nextPossibleLocation, rectangleSize);

            if (!collisionIdentifier.IntersectsAny(currentRectangle, rectangles))
                break;
        }

        currentRectangle = toCenterTightener.Tighten(currentRectangle, center, rectangles, collisionIdentifier);

        rectangles.Add(currentRectangle);
        return currentRectangle;
    }
}