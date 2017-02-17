using System;
using System.Collections.ObjectModel;
using CoreGraphics;
using Foundation;
using ManneDoForms.Components.DropDownView;
using ManneDoForms.iOS.Renderers.DropDownView;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ManneDropDownView), typeof(ManneDropDownViewRenderer))]
namespace ManneDoForms.iOS.Renderers.DropDownView
{
    public class ManneDropDownViewRenderer : ViewRenderer
    {
        // Private Members
        private ManneDropDownView _formsView;
        private UIImageView _iconImageView;
        private UILabel _selectedValueLabel;
        private UIView _dropDownView;
        private UITableView _dropDownTableView;
        private DropDownTableSource _dropDownTableSource;


        // -----------------------------------------------------------------------------

        // Overrides
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                // Init
                _formsView = e.NewElement as ManneDropDownView;

                // Create Native View
                var view = new UIView { BackgroundColor = _formsView.BackgroundColor.ToUIColor() };

                _iconImageView = new UIImageView(new UIImage("icon_dropdown-arr.png"));
                _selectedValueLabel = new UILabel();

                _dropDownView = new UIView { BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0), Hidden = true };
                _dropDownTableView = new UITableView
                {
                    SeparatorStyle = UITableViewCellSeparatorStyle.None,
                    BackgroundColor = _formsView.BackgroundColor.ToUIColor()
                };

                _dropDownTableSource = new DropDownTableSource(ItemSelected);
                _dropDownTableView.Source = _dropDownTableSource;

                view.AddSubviews(_selectedValueLabel, _iconImageView);

                SetNativeControl(view);

                // Add Gesture Recognizers
                Control.AddGestureRecognizer(new UITapGestureRecognizer(ViewSelected));
                _dropDownView.AddGestureRecognizer(new UITapGestureRecognizer(HideDropDown));

                // Handle TableView
                UpdateDropDownTable();
                SelectItem();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == nameof(ManneDropDownView.ItemsSource))
            {
                UpdateDropDownTable();
                return;
            }

            if (e.PropertyName == nameof(ManneDropDownView.SelectedItem))
            {
                SelectItem();
                return;
            }

            if (e.PropertyName == nameof(ManneDropDownView.BackgroundColor))
            {
                Control.BackgroundColor = _formsView.BackgroundColor.ToUIColor();
                _dropDownTableView.BackgroundColor = _formsView.BackgroundColor.ToUIColor();
                return;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _selectedValueLabel.Frame = new CGRect(10, 5, Control.Frame.Width - 30f, Control.Frame.Height - 10);
            _iconImageView.Frame = new CGRect(Control.Frame.Width - 27f, (Control.Frame.Height - 9f) / 2f, 17f, 9f);
        }


        // -----------------------------------------------------------------------------

        // Private Methods
        private void ViewSelected()
        {
            if (_dropDownView == null)
            {
                return;
            }

            if (_dropDownView.Hidden)
            {
                ShowDropDown();
                return;
            }

            HideDropDown();
        }

        private void ShowDropDown()
        {
            if (_dropDownView == null || _dropDownTableView == null)
            {
                return;
            }

            _dropDownView.Hidden = false;

            var topView = FindTopView(Control);

            topView.AddSubview(_dropDownView);
            _dropDownView.Frame = topView.Bounds;
            topView.BringSubviewToFront(_dropDownView);

            var convertedControlFrame = Control.ConvertRectToView(Control.Frame, topView);

            topView.AddSubview(_dropDownTableView);
            _dropDownTableView.Frame = new CGRect(convertedControlFrame.X, convertedControlFrame.Bottom, convertedControlFrame.Width, _formsView?.ItemsSource?.Count * 30f ?? 30f);
            topView.BringSubviewToFront(_dropDownTableView);
        }

        private void HideDropDown()
        {
            if (_dropDownView == null && _dropDownTableView == null)
            {
                return;
            }

            _dropDownView.RemoveFromSuperview();
            _dropDownTableView.RemoveFromSuperview();

            if (_dropDownView == null)
            {
                return;
            }

            _dropDownView.Hidden = true;
        }

        private UIView FindTopView(UIView child)
        {
            var parentView = child.Superview;

            while (parentView?.Superview != null)
            {
                parentView = parentView.Superview;
            }

            return parentView;
        }

        private void SelectItem()
        {
            _selectedValueLabel.Text = _formsView.SelectedItem;

            var index = _formsView?.ItemsSource?.IndexOf(_formsView.SelectedItem) ?? -1;

            if (index > 0)
            {
                _dropDownTableView.SelectRow(NSIndexPath.FromRowSection(index, 0), false, UITableViewScrollPosition.None);
            }
        }

        private void ItemSelected(string item)
        {
            _formsView?.SetSelected(item);

            HideDropDown();
        }

        private void UpdateDropDownTable()
        {
            if (_formsView?.ItemsSource == null)
            {
                return;
            }

            _dropDownTableSource?.UpdateItems(_formsView.ItemsSource);
            _dropDownTableView?.ReloadData();
        }


        // -----------------------------------------------------------------------------

        // Private Classes
        private class DropDownTableSource : UITableViewSource
        {
            // Private Members
            public const string CellId = "CellId";

            private ObservableCollection<string> _items;
            private readonly Action<string> _itemListener;


            // -----------------------------------------------------------------------------

            // Constructors
            public DropDownTableSource(Action<string> itemListener)
            {
                _items = new ObservableCollection<string>();
                _itemListener = itemListener;
            }


            // -----------------------------------------------------------------------------

            // Public Methods
            public void UpdateItems(ObservableCollection<string> items)
            {
                _items = items;
            }


            // -----------------------------------------------------------------------------

            // Overrides
            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return _items.Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CellId);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellId);
                }

                cell.TextLabel.Text = _items[indexPath.Row];

                return cell;
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return 30f;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                _itemListener(_items[indexPath.Row]);
            }
        }
    }
}