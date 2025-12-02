using System.Drawing;
using TagsCloudCore.CloudLayout;
using TagsCloudCore.CloudVisualization;

namespace TagsCloudRunner;

internal static class Program
{
    public static void Main()
    {
        GenerateTagsCloud(
            filePath: "cloud_small.png",
            center: new Point(0, 0),
            rectanglesCount: 75);

        GenerateTagsCloud(
            filePath: "cloud_medium.png",
            center: new Point(0, 0),
            rectanglesCount: 250);

        GenerateTagsCloud(
            filePath: "cloud_big.png",
            center: new Point(0, 0),
            rectanglesCount: 450);
    }

    private static void GenerateTagsCloud(string filePath, Point center, int rectanglesCount)
    {
        var layouter = new CircularCloudLayouter(center);

        var random = new Random();

        for (var i = 0; i < rectanglesCount; i++)
        {
            var size = new Size(
                random.Next(20, 100),
                random.Next(15, 40));

            layouter.PutNextRectangle(size);
        }

        CloudVisualizer.SaveLayoutToFile(filePath, layouter.PlacedRectangles.ToArray());
    }
}