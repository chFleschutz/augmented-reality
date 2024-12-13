using OpenCvSharp;
using OpenCvSharp.Demo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public class ContourFinder : WebCamera
{
    [SerializeField] private FlipMode flipMode;
    [SerializeField] private float threshold = 96.4f;
    [SerializeField] private bool showProcessed = true;
    [SerializeField] private float curveApproximation = 10.0f;
    [SerializeField] private float minArea = 5000.0f;
    [SerializeField] private float maxArea = 5000.0f;

    private Mat image;
    private Mat processedImage = new Mat();
    private Point[][] contours;
    private HierarchyIndex[] hierarchy;

    protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
    {
        image = OpenCvSharp.Unity.TextureToMat(input);
        Cv2.Flip(image, image, flipMode);
        Cv2.CvtColor(image, processedImage, ColorConversionCodes.BGR2GRAY);
        Cv2.Threshold(processedImage, processedImage, threshold, 255, ThresholdTypes.BinaryInv);
        Cv2.FindContours(processedImage, out contours, out hierarchy, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

        processContours();

        if (output == null)
        {
            output = OpenCvSharp.Unity.MatToTexture(showProcessed ? processedImage : image);
        }
        else
        {
            OpenCvSharp.Unity.MatToTexture(showProcessed ? processedImage : image, output);
        }

        return true;
    }

    private void processContours()
    {
        var validContours = new List<Point[]>();
        foreach (var contour in contours)
        {
            Point[] points = Cv2.ApproxPolyDP(contour, curveApproximation, true);
            var area = Cv2.ContourArea(points);

            if (area > minArea && area < maxArea)
            {
                validContours.Add(points);
            }
        }

        if (validContours.Count > 0)
            Cv2.DrawContours(image, validContours, -1, Scalar.Lime, -1);
    }

    private void OnDestroy()
    {
        image?.Dispose();
        processedImage?.Dispose();
    }
}
