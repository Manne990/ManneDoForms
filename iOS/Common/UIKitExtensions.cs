using System;
using System.Runtime.InteropServices;
using CoreGraphics;
using Foundation;
using UIKit;

namespace ManneDoForms.iOS.Common
{
    public static class UIKitExtensions
    {
        /// <summary>
        /// Sets a callback for registering text changed notifications on a UITextField
        /// </summary>
        public static void SetDidChangeNotification(this UITextField textField, Action<UITextField> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification, _ => callback(textField), textField);
        }

        /// <summary>
        /// Sets a callback for registering text changed notifications on a UITextField
        /// </summary>
        public static void SetDidChangeNotification(this UITextView textView, Action<UITextView> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            NSNotificationCenter.DefaultCenter.AddObserver(UITextView.TextDidChangeNotification, _ => callback(textView), textView);
        }

        /// <summary>
        /// Returns true if is landscape
        /// </summary>
        public static bool IsLandscape(this UIInterfaceOrientation orientation)
        {
            return orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight;
        }

        /// <summary>
        /// Returns true if is portrait
        /// </summary>
        public static bool IsPortrait(this UIInterfaceOrientation orientation)
        {
            return orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown;
        }

        /// <summary>
        /// Loads a UIImage from a byte array
        /// </summary>
        public static UIImage ToUIImage(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            using (var data = NSData.FromArray(bytes))
                return UIImage.LoadFromData(data);
        }

        /// <summary>
        /// Loads a UIImage from a Stream
        /// </summary>
        public static UIImage ToUIImage(this System.IO.Stream stream)
        {
            if (stream == null)
                return null;

            using (var data = NSData.FromStream(stream))
                return UIImage.LoadFromData(data);
        }

        /// <summary>
        /// Converts a UIImage to a byte array
        /// </summary>
        public static byte[] ToByteArray(this UIImage image)
        {
            if (image == null)
                return null;

            using (image)
            using (var data = image.AsJPEG())
            {
                byte[] bytes = new byte[data.Length];
                Marshal.Copy(data.Bytes, bytes, 0, (int)data.Length);
                return bytes;
            }
        }

        /// <summary>
        /// Creates a 1x1 image that is a certain color
        /// </summary>
        public static UIImage ToImage(this UIColor color)
        {
            try
            {
                UIGraphics.BeginImageContext(new CGSize(1, 1));

                using (var context = UIGraphics.GetCurrentContext())
                {
                    context.SetFillColor(color.CGColor);
                    context.FillRect(new CGRect(0, 0, 1, 1));

                    return UIGraphics.GetImageFromCurrentImageContext();
                }
            }
            finally
            {
                UIGraphics.EndImageContext();
            }
        }

        /// <summary>
        /// Awesome helper method to instantiate a view controller and use the type name for the id
        /// </summary>
        public static T InstantiateViewController<T>(this UIStoryboard storyboard)
            where T : UIViewController
        {
            return (T)storyboard.InstantiateViewController(typeof(T).Name);
        }
    }
}