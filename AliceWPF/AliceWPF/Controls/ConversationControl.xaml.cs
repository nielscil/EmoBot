using AliceWPF.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AliceWPF.Controls
{
    /// <summary>
    /// Interaction logic for ConversationControl.xaml
    /// </summary>
    public partial class ConversationControl : UserControl, IDisposable
    {
        public ConversationControl()
        {
            InitializeComponent();

            Loaded += ConversationControl_Loaded;
        }

        private void ConversationControl_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
            if (dpd != null)
            {
                dpd.AddValueChanged(itemsControl, ItemsSource_Changed);
            }
        }

        private void ItemsSource_Changed(object sender, EventArgs e)
        {
            ObservableCollection<ConversationItem> items = itemsControl.ItemsSource as ObservableCollection<ConversationItem>;
            if (items != null)
            {
                items.CollectionChanged += Items_CollectionChanged;
            }
        }

        public void Dispose()
        {
            Loaded -= ConversationControl_Loaded;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                scrollView.ScrollToEnd();
            }
        }
    }
}
