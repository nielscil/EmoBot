using AliceWPF.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            ObservableCollection<ConversationItem> items = itemsControl.ItemsSource as ObservableCollection<ConversationItem>;
            if(items != null)
            {
                items.CollectionChanged += Items_CollectionChanged;
            }
        }

        public void Dispose()
        {
            ObservableCollection<ConversationItem> items = itemsControl.ItemsSource as ObservableCollection<ConversationItem>;
            if (items != null)
            {
                items.CollectionChanged -= Items_CollectionChanged;
            }          
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
