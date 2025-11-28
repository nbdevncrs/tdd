using System.Drawing;
using FluentAssertions;
using TagsCloudCore.CloudLayout.Extensions;

namespace TagsCloudTests;

public class IntersectsAnyTests
{
    private static IEnumerable<TestCaseData> IntersectionCases()
    {
        var baseRectangle = new Rectangle(new Point(50, 50), new Size(50, 50));

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(70, 70), new Size(30, 30)),
            true
        ).SetName("Overlap_CenterInside_ShouldCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(0, 0), new Size(30, 30)),
            false
        ).SetName("RectanglesThatFarAwayWithNoOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(100, 50), new Size(20, 20)),
            false
        ).SetName("RightSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(30, 50), new Size(20, 50)),
            false
        ).SetName("LeftSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(50, 100), new Size(40, 20)),
            false
        ).SetName("BottomSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(50, 30), new Size(20, 20)),
            false
        ).SetName("TopSideOverlap_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(100, 100), new Size(1, 1)),
            false
        ).SetName("TouchingCorner_ShouldNotCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(75, 75), new Size(2, 2)),
            true
        ).SetName("Inside_EntirelyContained_ShouldCountAsOverlap_Test");

        yield return new TestCaseData(
            baseRectangle,
            new Rectangle(new Point(40, 40), new Size(150, 150)),
            true
        ).SetName("Contains_BaseRectangleInsideCandidate_ShouldCountAsOverlap_Test");
    }

    [TestCaseSource(nameof(IntersectionCases))]
    public void IntersectsAny_ShouldWorkForVariousCases_Test(
        Rectangle existing,
        Rectangle candidate,
        bool expected)
    {
        var result = candidate.IntersectsAny([existing]);

        result.Should().Be(expected);
    }

    [Test]
    public void IntersectsAny_ShouldThrowException_IfExistingRectanglesAreNull_Test()
    {
        var rectangle = new Rectangle(new Point(50, 50), new Size(50, 50));
        Action act = () => rectangle.IntersectsAny(null);

        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Existing rectangles can't be null (Parameter 'rectangles')");
    }
}