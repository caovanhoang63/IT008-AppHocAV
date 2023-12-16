using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IT008_AppHocAV.Components.RadioButtonMenuItem
{
    public class RadioButtonMenuItem : MenuItem
    {
        /// <summary>
        /// GroupName 
        /// </summary>
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(RadioButtonMenuItem), new PropertyMetadata(default(string)));

        static RadioButtonMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadioButtonMenuItem), new FrameworkPropertyMetadata(typeof(RadioButtonMenuItem)));
        }

        /// <summary>
        /// GroupName
        /// </summary>
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        /// <inheritdoc />
        protected override void OnClick()
        {
            base.OnClick();
            IsChecked = true;
        }

        /// <inheritdoc />
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new RadioButtonMenuItem();
        }

        /// <inheritdoc />
        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            if (this.Parent is ContextMenu parent)
            {
                foreach (var menuItem in parent.Items.OfType<RadioButtonMenuItem>())
                {
                    if (menuItem != this && menuItem.GroupName == GroupName && (menuItem.DataContext == parent.DataContext || menuItem.DataContext != DataContext))
                    {
                        menuItem.IsChecked = false;
                    }
                }
            }
        }
    }
}