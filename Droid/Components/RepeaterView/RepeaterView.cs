using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ManneDoForms.Droid.Components.RepeaterView
{
    public sealed class RepeaterView : LinearLayout
    {
        private Adapter _adapter;

        [Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;)V", "")]
        public RepeaterView(Context context, IAttributeSet attrs) : base(context, attrs)
        { }

        [Register(".ctor", "(Landroid/content/Context;)V", "")]
        public RepeaterView(Context context) : base(context)
        { }

        [Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;I)V", "", ApiSince = 11)]
        public RepeaterView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        { }

        [Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;II)V", "", ApiSince = 21)]
        public RepeaterView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        { }

        public void SetAdapter(Adapter adapter)
        {
            _adapter = adapter;

            _adapter.OnAttachedToRepeaterView(this);
            _adapter.NotifyDataSetChanged();
        }

        protected override void Dispose(bool disposing)
        {
            _adapter.Dispose();
            _adapter = null;
            
            base.Dispose(disposing);
        }

        public abstract class Adapter : IDisposable
        {
            private WeakReference<RepeaterView> _repeaterView;

            protected virtual int ItemCount { get; }
            protected abstract ViewHolder OnCreateViewHolder(ViewGroup parent);
            protected abstract void OnBindViewHolder(ViewHolder holder, int position);

            public void NotifyDataSetChanged()
            {
                if (_repeaterView == null)
                {
                    return;
                }

                RepeaterView repeaterView;
                if (_repeaterView.TryGetTarget(out repeaterView) == false)
                {
                    return;
                }

                repeaterView.RemoveAllViews();

                if (ItemCount <= 0)
                {
                    return;
                }

                for (int i = 0; i < ItemCount; i++)
                {
                    var viewHolder = OnCreateViewHolder(repeaterView);

                    OnBindViewHolder(viewHolder, i);

                    repeaterView.AddView(viewHolder.ItemView);
                }
            }

            public virtual void OnAttachedToRepeaterView(RepeaterView view)
            {
                _repeaterView = new WeakReference<RepeaterView>(view);
            }

            public void Dispose()
            {
                _repeaterView = null;
            }
        }

        public abstract class ViewHolder : IDisposable
        {
            public View ItemView { get; private set; }

            protected ViewHolder(View itemView)
            {
                ItemView = itemView;
            }

            public void Dispose()
            {
                ItemView.Dispose();
                ItemView = null;
            }
        }
    }
}