using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManneDoForms.Components.PhotoViewer.View
{
    public interface ICarouselLayoutChildDelegate
    {
        Task WillBeActive();
        Task GotActive();
        Task GotInactive();
        Task Refresh();
    }

    public class CarouselLayout : ScrollView
    {
        public enum IndicatorStyleEnum
        {
            None,
            Dots,
            Tabs
        }

        readonly StackLayout _stack;

        int _selectedIndex;

        public CarouselLayout()
        {
            Orientation = ScrollOrientation.Horizontal;

            _stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0
            };

            Content = _stack;
        }

        public IndicatorStyleEnum IndicatorStyle { get; set; }

        public IList<Xamarin.Forms.View> Children
        {
            get
            {
                return _stack.Children;
            }
        }

        private bool _layingOutChildren;
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);

            if (_layingOutChildren) return;

            _layingOutChildren = true;

            foreach (var child in Children)
            {
                child.WidthRequest = width;
            }

            _layingOutChildren = false;
        }

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create<CarouselLayout, int>(
                carousel => carousel.SelectedIndex,
                0,
                BindingMode.TwoWay,
                propertyChanged: async (bindable, oldValue, newValue) =>
                {
                    await ((CarouselLayout)bindable).UpdateSelectedItem();
                }
            );

        public int SelectedIndex
        {
            get
            {
                return (int)GetValue(SelectedIndexProperty);
            }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        async Task UpdateSelectedItem()
        {
            if (Children[SelectedIndex] is ICarouselLayoutChildDelegate)
            {
                await ((ICarouselLayoutChildDelegate)Children[SelectedIndex]).WillBeActive();
            }

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                Task.Factory.StartNew(async () =>
                {
                    SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;

                    if (Children[SelectedIndex] is ICarouselLayoutChildDelegate)
                    {
                        await ((ICarouselLayoutChildDelegate)Children[SelectedIndex]).GotActive();

                        for (int i = 0; i < Children.Count; i++)
                        {
                            if (i != SelectedIndex && Children[i] is ICarouselLayoutChildDelegate)
                            {
                                await ((ICarouselLayoutChildDelegate)Children[i]).GotInactive();
                            }
                        }
                    }
                });

                return false;
            });
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<CarouselLayout, IList>(
                view => view.ItemsSource,
                null,
                propertyChanging: (bindableObject, oldValue, newValue) =>
                {
                    ((CarouselLayout)bindableObject).ItemsSourceChanging();
                },
                propertyChanged: async (bindableObject, oldValue, newValue) =>
                {
                    await ((CarouselLayout)bindableObject).ItemsSourceChanged();
                }
            );

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public async void Refresh()
        {
            ItemsSourceChanging();
            await ItemsSourceChanged();
        }

        void ItemsSourceChanging()
        {
            if (ItemsSource == null)
            {
                return;
            }

            _selectedIndex = ItemsSource.IndexOf(SelectedItem);
        }

        async Task ItemsSourceChanged()
        {
            _stack.Children.Clear();

            foreach (var item in ItemsSource)
            {
                var view = (Xamarin.Forms.View)ItemTemplate.CreateContent();
                var bindableObject = view as BindableObject;

                if (bindableObject != null)
                {
                    bindableObject.BindingContext = item;
                }

                _stack.Children.Add(view);
            }

            if (_selectedIndex >= 0)
            {
                SelectedIndex = _selectedIndex;

                if (Children[SelectedIndex] is ICarouselLayoutChildDelegate)
                {
                    await ((ICarouselLayoutChildDelegate)Children[SelectedIndex]).WillBeActive();
                    await ((ICarouselLayoutChildDelegate)Children[SelectedIndex]).GotActive();

                    for (int i = 0; i < Children.Count; i++)
                    {
                        if (Children[i] is ICarouselLayoutChildDelegate)
                        {
                            await ((ICarouselLayoutChildDelegate)Children[i]).Refresh();

                            if (i != SelectedIndex)
                            {
                                await ((ICarouselLayoutChildDelegate)Children[i]).GotInactive();
                            }
                        }
                    }
                }
            }
        }

        public DataTemplate ItemTemplate
        {
            get;
            set;
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<CarouselLayout, object>(
                view => view.SelectedItem,
                null,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    ((CarouselLayout)bindable).UpdateSelectedIndex();
                }
            );

        public object SelectedItem
        {
            get
            {
                return GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        void UpdateSelectedIndex()
        {
            if (SelectedItem == BindingContext) return;

            SelectedIndex = Children
                .Select(c => c.BindingContext)
                .ToList()
                .IndexOf(SelectedItem);
        }
    }
}