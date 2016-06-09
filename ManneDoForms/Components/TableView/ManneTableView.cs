using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ManneDoForms.Common;
using Xamarin.Forms;

namespace ManneDoForms.Components.TableView
{
	public class ManneTableView<T> : Grid
	{
		#region Overrides

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			CreateGrid();
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			if(propertyName == ManneTableView<T>.ItemsSourceProperty.PropertyName)
			{
				CreateGrid();
			}

			base.OnPropertyChanged(propertyName);
		}

		#endregion

		// ------------------------------------------------------------

		#region Public Properties

		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<IEnumerable<T>>), typeof(ManneTableView<T>), default(T));
		public IEnumerable<IEnumerable<T>> ItemsSource
		{
			get { return (IEnumerable<IEnumerable<T>>)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(ManneTableView<T>), null);
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}

		public static readonly BindableProperty VerticalContentAlignmentProperty = BindableProperty.Create(nameof(VerticalContentAlignment), typeof(ManneTableViewContentAlignment), typeof(ManneTableView<T>), ManneTableViewContentAlignment.Default);
		public ManneTableViewContentAlignment VerticalContentAlignment
		{
			get { return (ManneTableViewContentAlignment)GetValue(VerticalContentAlignmentProperty); }
			set { SetValue(VerticalContentAlignmentProperty, value); }
		}

		public static readonly BindableProperty HorizontalContentAlignmentProperty = BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(ManneTableViewContentAlignment), typeof(ManneTableView<T>), ManneTableViewContentAlignment.Default);
		public ManneTableViewContentAlignment HorizontalContentAlignment
		{
			get { return (ManneTableViewContentAlignment)GetValue(HorizontalContentAlignmentProperty); }
			set { SetValue(HorizontalContentAlignmentProperty, value); }
		}

		#endregion

		// ------------------------------------------------------------

		#region Commands

		public static readonly BindableProperty CellTappedCommandProperty = BindableProperty.Create(nameof(CellTappedCommand), typeof(ICommand), typeof(ManneTableView<T>), null);
		public ICommand CellTappedCommand
		{
			get { return (ICommand)GetValue(CellTappedCommandProperty); }
			set { SetValue(CellTappedCommandProperty, value); }
		}

		#endregion

		// ------------------------------------------------------------

		#region Events

		public event EventHandler<EventArgs<T>> CellTapped;

		#endregion

		// ------------------------------------------------------------

		#region Private Methods

		private void CreateGrid()
		{
			// Check for data
			if(this.ItemsSource == null || this.ItemsSource.Count() == 0 || this.ItemsSource.First().Count() == 0)
			{
				return;
			}

			// Create the grid
			this.RowDefinitions = CreateRowDefinitions();
			this.ColumnDefinitions = CreateColumnDefinitions();

			CreateCells();
		}

		private RowDefinitionCollection CreateRowDefinitions()
		{
			var rowDefinitions = new RowDefinitionCollection();

			if(this.VerticalContentAlignment == ManneTableViewContentAlignment.Center)
			{
				rowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}

			foreach(var row in this.ItemsSource)
			{
				rowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			}

			if(this.VerticalContentAlignment == ManneTableViewContentAlignment.Center)
			{
				rowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}

			return rowDefinitions;
		}

		private ColumnDefinitionCollection CreateColumnDefinitions()
		{
			var columnDefinitions = new ColumnDefinitionCollection();

			if(this.HorizontalContentAlignment == ManneTableViewContentAlignment.Center)
			{
				columnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}

			foreach(var column in this.ItemsSource.First())
			{
				columnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
			}

			if(this.HorizontalContentAlignment == ManneTableViewContentAlignment.Center)
			{
				columnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}

			return columnDefinitions;
		}

		private void CreateCells()
		{
			int startColIndex = this.HorizontalContentAlignment == ManneTableViewContentAlignment.Center ? 1 : 0;
			int rowIndex = this.VerticalContentAlignment == ManneTableViewContentAlignment.Center ? 1 : 0;

			foreach(var row in this.ItemsSource)
			{
				var colIndex = startColIndex;

				foreach(var item in row)
				{
					this.Children.Add(CreateCellView(item), colIndex, rowIndex);
					colIndex++;
				}

				rowIndex++;
			}
		}

		private Xamarin.Forms.View CreateCellView(T item)
		{
			var view = (Xamarin.Forms.View)this.ItemTemplate.CreateContent();
			var bindableObject = (BindableObject)view;

			if (bindableObject != null)
			{
				bindableObject.BindingContext = item;
			}

			var tapGestureRecognizer = new TapGestureRecognizer();

			tapGestureRecognizer.Tapped += CellTappedHandler;

			view.GestureRecognizers.Add(tapGestureRecognizer);

			return view;
		}

		private async void CellTappedHandler(object sender, EventArgs e)
		{
			if(sender is BindableObject)
			{
				var bindableObject = (BindableObject)sender;

				// Trigger Event
				if(CellTapped != null)
				{
					CellTapped(this, new EventArgs<T>((T)bindableObject.BindingContext));
				}

				// Trigger Command
				if (this.CellTappedCommand != null && this.CellTappedCommand.CanExecute((T)bindableObject.BindingContext))
				{
					this.CellTappedCommand.Execute((T)bindableObject.BindingContext);
				}
			}

			if(sender is Xamarin.Forms.View)
			{
				var view = (Xamarin.Forms.View)sender;

				// Animate
				await view.FadeTo(0.2, 100, Easing.Linear);
				await view.FadeTo(1.0, 100, Easing.Linear);
			}
		}

		#endregion
	}

	public enum ManneTableViewContentAlignment
	{
		Default,
		Center
	}
}