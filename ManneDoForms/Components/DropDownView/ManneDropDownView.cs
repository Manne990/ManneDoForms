using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ManneDoForms.Components.DropDownView
{
    public class ManneDropDownView : View
    {
        // Public Properties
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<string>), typeof(ManneDropDownView), null);
        public ObservableCollection<string> ItemsSource
        {
            get { return (ObservableCollection<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(ManneDropDownView), null);
        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }


        // ---------------------------------------------------------

        // Commands
        public static readonly BindableProperty SelectedItemChangedCommandProperty = BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(ManneDropDownView), null);
        public ICommand SelectedItemChangedCommand
        {
            get { return (ICommand)GetValue(SelectedItemChangedCommandProperty); }
            set { SetValue(SelectedItemChangedCommandProperty, value); }
        }


        // ---------------------------------------------------------

        // Public Methods
        public void SetSelected(string item)
        {
            SelectedItem = item;

            // Execute the command
            if (SelectedItemChangedCommand != null && SelectedItemChangedCommand.CanExecute(null))
            {
                SelectedItemChangedCommand.Execute(null);
            }
        }
    }
}