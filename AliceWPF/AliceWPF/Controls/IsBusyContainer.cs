using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace AliceWPF.Controls
{
    [ContentProperty("Children")]
    public class IsBusyContainer : Grid, IDisposable
    {
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(IsBusyContainer), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsBusyPropertyChanged)));
        private static void OnIsBusyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IsBusyContainer container = d as IsBusyContainer;
            if (container != null)
            {
                if ((bool)e.NewValue)
                {
                    container.Overlay.Visibility = Visibility.Visible;
                }
                else
                {
                    container.Overlay.Visibility = Visibility.Collapsed;
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return (bool)GetValue(IsBusyProperty);
            }
            set
            {
                SetValue(IsBusyProperty, value);
            }
        }

        public Grid Overlay { get; private set; }
        public Grid View { get; private set; }

        public new ObservableCollection<UIElement> Children { get; private set; }

        public IsBusyContainer()
        {
            SetSubItems();
            Children = new ObservableCollection<UIElement>();
            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (UIElement elem in e.NewItems)
                    {
                        View.Children.Add(elem);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:

                    foreach (UIElement elem in e.OldItems)
                    {
                        View.Children.Remove(elem);
                    }
                    break;
                default:
                    break;
            }
        }

        ~IsBusyContainer()
        {
            Dispose();
        }

        public void Dispose()
        {
            Children.CollectionChanged -= Children_CollectionChanged;
        }

        private void SetSubItems()
        {
            SetOverlay();
            Overlay.Visibility = Visibility.Collapsed;
            InternalChildren.Add(Overlay);

            View = new Grid();
            InternalChildren.Add(View);
        }

        private SolidColorBrush GetOverlayColor()
        {
            var brush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            brush.Opacity = 0;
            return brush;
        }

        private void SetOverlay()
        {
            Overlay = new Grid();
            SetZIndex(Overlay, 9999);
            Overlay.Background = GetOverlayColor();
            Overlay.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Overlay.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            Overlay.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Overlay.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            Overlay.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            Overlay.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            Overlay.Children.Add(GetOverlayCard());
        }

        private Card GetOverlayCard()
        {
            Card card = new Card();
            card.UniformCornerRadius = 18;
            card.Padding = new Thickness(5);
            SetRow(card, 1);
            SetColumn(card, 1);
            card.VerticalAlignment = VerticalAlignment.Center;
            card.HorizontalAlignment = HorizontalAlignment.Center;
            Margin = new Thickness(12, 12, 12, 12);

            ProgressBar processbar = new ProgressBar();
            processbar.Style = (Style)FindResource("MaterialDesignCircularProgressBar");
            processbar.Value = 50;
            processbar.IsIndeterminate = true;
            processbar.Margin = new Thickness(2);
            card.Content = processbar;

            return card;
        }
    }
}
