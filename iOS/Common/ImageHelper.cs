using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ManneDoForms.iOS.Common
{
    internal static class ImageHelper
    {
        internal enum CropOrigoTypes
        {
        	Top,
        	Center
        }

        internal static UIImage ScaleImage(UIImage sourceImage, CGSize targetSize)
        {
        	// Init
        	var thumbnailPoint = new CGPoint(0.0, 0.0);
        	var scaledWidth = targetSize.Width;
        	var scaledHeight = targetSize.Height;

        	// Scale
        	if (sourceImage.Size.Width != targetSize.Width || sourceImage.Size.Height != targetSize.Height)
        	{
        		var widthFactor = targetSize.Width / sourceImage.Size.Width;
        		var heightFactor = targetSize.Height / sourceImage.Size.Height;
        		var scaleFactor = widthFactor < heightFactor ? widthFactor : heightFactor;

        		scaledWidth = sourceImage.Size.Width * scaleFactor;
        		scaledHeight = sourceImage.Size.Height * scaleFactor;

        		// center the image
        		if (widthFactor < heightFactor)
        		{
        			thumbnailPoint.Y = (targetSize.Height - scaledHeight) * 0.5f;
        		}
        		else
        		{
        			if (widthFactor > heightFactor)
        			{
        				thumbnailPoint.X = (targetSize.Width - scaledWidth) * 0.5f;
        			}
        		}
        	}

        	// Draw the scaled image
        	UIGraphics.BeginImageContextWithOptions(targetSize, false, 0f);

        	var thumbnailRect = CGRect.Empty;

        	thumbnailRect.Location = thumbnailPoint;
        	thumbnailRect.Size = new CGSize(scaledWidth, scaledHeight);

        	sourceImage.Draw(thumbnailRect);

        	var newImage = UIGraphics.GetImageFromCurrentImageContext();

        	UIGraphics.EndImageContext();

        	return newImage;
        }

        internal static UIImage CropAndScaleImage(UIImage sourceImage, CGSize targetSize, CropOrigoTypes cropOrigo)
        {
        	// Init
        	var thumbnailPoint = new CGPoint(0.0, 0.0);
        	var scaledWidth = targetSize.Width;
        	var scaledHeight = targetSize.Height;

        	// Scale
        	if (sourceImage.Size.Width != targetSize.Width || sourceImage.Size.Height != targetSize.Height)
        	{
        		var widthFactor = targetSize.Width / sourceImage.Size.Width;
        		var heightFactor = targetSize.Height / sourceImage.Size.Height;
        		var scaleFactor = widthFactor > heightFactor ? widthFactor : heightFactor;

        		scaledWidth = sourceImage.Size.Width * scaleFactor;
        		scaledHeight = sourceImage.Size.Height * scaleFactor;

        		// center the image
        		if (widthFactor > heightFactor)
        		{
        			switch (cropOrigo)
        			{
        				case CropOrigoTypes.Top:
        					break;
        				case CropOrigoTypes.Center:
        					thumbnailPoint.Y = (targetSize.Height - scaledHeight) * 0.5f;
        					break;
        			}
        		}
        		else
        		{
        			if (widthFactor < heightFactor)
        			{
        				thumbnailPoint.X = (targetSize.Width - scaledWidth) * 0.5f;
        			}
        		}
        	}

        	// Draw the scaled image
        	UIGraphics.BeginImageContextWithOptions(targetSize, false, 0f);

        	var thumbnailRect = CGRect.Empty;

        	thumbnailRect.Location = thumbnailPoint;
        	thumbnailRect.Size = new CGSize(scaledWidth, scaledHeight);

        	sourceImage.Draw(thumbnailRect);

        	var newImage = UIGraphics.GetImageFromCurrentImageContext();

        	UIGraphics.EndImageContext();

        	return newImage;
        }

        internal static UIImage ImageFromBytes(byte[] imageBytes)
        {
        	using (var data = NSData.FromArray(imageBytes))
        	{
        		return UIImage.LoadFromData(data);
        	}
        }

        internal static byte[] ImageToBytes(UIImage image, string fileType)
        {
        	if (image == null)
        	{
        		return null;
        	}

        	return fileType == "png" ? PngImageToBytes(image) : JpegImageToBytes(image);
        }

        private static byte[] PngImageToBytes(UIImage image)
        {
        	using (var imageData = image.AsPNG())
        	{
        		var myByteArray = new byte[imageData.Length];
        		System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));

        		return myByteArray;
        	}
        }

        private static byte[] JpegImageToBytes(UIImage image)
        {
        	using (var imageData = image.AsJPEG())
        	{
        		var myByteArray = new byte[imageData.Length];
        		System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));

        		return myByteArray;
        	}
        }
    }
}