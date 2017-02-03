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

            _adapter.SetParentView(this);
            _adapter.NotifyDataSetChanged();
        }

        public abstract class Adapter
        {
            private WeakReference<RepeaterView> _view;

            // ReSharper disable once UnassignedGetOnlyAutoProperty
            protected virtual int ItemCount { get; }
            protected abstract ViewHolder OnCreateViewHolder(ViewGroup parent);
            protected abstract void OnBindViewHolder(ViewHolder holder, int position);

            public void NotifyDataSetChanged()
            {
                if (_view == null)
                {
                    return;
                }

                RepeaterView repeaterView;
                if (_view.TryGetTarget(out repeaterView) == false)
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

            public void SetParentView(RepeaterView view)
            {
                _view = new WeakReference<RepeaterView>(view);
            }
        }

        public abstract class ViewHolder
        {
            public View ItemView { get; }

            protected ViewHolder(View itemView)
            {
                ItemView = itemView;
            }
        }
    }
}