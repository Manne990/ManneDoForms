using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ManneDoForms.Droid.Components.RepeaterView;

namespace ManneDoForms.Droid.Samples
{
    [Activity(Label = "RepeaterView")]
    public class RepeaterViewActivity : Activity
    {
        private RepeaterViewAdapter _adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RepeaterViewLayout);

            var repeaterView = FindViewById<RepeaterView>(Resource.Id.repeaterView);

            _adapter = new RepeaterViewAdapter();

            repeaterView.SetAdapter(_adapter);
        }

        protected override void OnResume()
        {
            base.OnResume();

            _adapter.Update(new List<string> { "Item 1", "Item 2", "Item 3", "Item 4" });
        }

        private class RepeaterViewAdapter : RepeaterView.Adapter
        {
            private List<string> _content;

            protected override int ItemCount => _content?.Count ?? 0;

            protected override RepeaterView.ViewHolder OnCreateViewHolder(ViewGroup parent)
            {
                var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RepeaterViewItemLayout, parent, false);
                return new RepeaterViewViewHolder(view);
            }

            protected override void OnBindViewHolder(RepeaterView.ViewHolder holder, int position)
            {
                var viewHolder = holder as RepeaterViewViewHolder;
                viewHolder?.Update(_content[position]);
            }

            public void Update(List<string> content)
            {
                _content = content;

                NotifyDataSetChanged();
            }
        }

        private class RepeaterViewViewHolder : RepeaterView.ViewHolder
        {
            private readonly TextView _messageTextView;

            public RepeaterViewViewHolder(View itemView) : base(itemView)
            {
                _messageTextView = itemView.FindViewById<TextView>(Resource.Id.messageTextView);
            }

            public void Update(string text)
            {
                _messageTextView.Text = text;
            }
        }
    }
}