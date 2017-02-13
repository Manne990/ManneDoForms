using System;
using Android.Graphics;
using Android.Widget;
using Android.Views;

namespace ManneDoForms.Droid.Common.PhotoViewDroid
{
	public interface IPhotoViewDroid
	{

		bool CanZoom();


		RectF GetDisplayRect();

		bool SetDisplayMatrix(Matrix finalMatrix);

		Matrix GetDisplayMatrix();


		[Obsolete]
		float GetMinScale();


		float GetMinimumScale();


		[Obsolete]
		float GetMidScale();

		float GetMediumScale();


		[Obsolete]
		float GetMaxScale();


		float GetMaximumScale();


		float GetScale();


		ImageView.ScaleType GetScaleType();

		void SetAllowParentInterceptOnEdge(bool allow);

		[Obsolete]
		void SetMinScale(float minScale);

		void SetMinimumScale(float minimumScale);


		[Obsolete]
		void SetMidScale(float midScale);


		void SetMediumScale(float mediumScale);


		[Obsolete]
		void SetMaxScale(float maxScale);


		void SetMaximumScale(float maximumScale);

	
		void SetOnLongClickListener(Android.Views.View.IOnLongClickListener listener);


		void SetOnMatrixChangeListener(PhotoViewDroidAttacher.IOnMatrixChangedListener listener);


		void SetOnPhotoTapListener(PhotoViewDroidAttacher.IOnPhotoTapListener listener);


		PhotoViewDroidAttacher.IOnPhotoTapListener GetOnPhotoTapListener();


		void SetOnViewTapListener(PhotoViewDroidAttacher.IOnViewTapListener listener);


		void SetRotationTo(float rotationDegree);


		void SetRotationBy(float rotationDegree);

		PhotoViewDroidAttacher.IOnViewTapListener GetOnViewTapListener();

		void SetScale(float scale);

	
		void SetScale(float scale, bool animate);

	
		void SetScale(float scale, float focalX, float focalY, bool animate);

		void SetScaleType(ImageView.ScaleType scaleType);

	
		void SetZoomable(bool zoomable);

	
		void SetPhotoViewRotation(float rotationDegree);

	
		Bitmap GetVisibleRectangleBitmap();

		void SetZoomTransitionDuration(int milliseconds);

	
		IPhotoViewDroid GetIPhotoViewImplementation();

	
		void SetOnDoubleTapListener(GestureDetector.IOnDoubleTapListener newOnDoubleTapListener);
	}
}

