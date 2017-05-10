using AliceWPF.Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AliceWPF.Controls
{
    public partial class ConverstationItemControl : UserControl
    {

        public ConverstationItemControl()
        {
            InitializeComponent();
            DataContextChanged += ConverstationItemControl_DataContextChanged;
        }

        private void ConverstationItemControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            DataContextChanged -= ConverstationItemControl_DataContextChanged;

            ConversationItem item = DataContext as ConversationItem;
            DockPanel grid = Content as DockPanel;

            if (item != null && grid != null)
            {
                grid.Children.Add(CreateCard(item));
            }
        }

        private Card CreateCard(ConversationItem item)
        {
            Card card = new Card();

            SetGridColumn(card, item);
            card.Content = CreateTextBlock(item);

            return card;
        }

        private void SetGridColumn(Card card, ConversationItem item)
        {
            if(item.Sender == SenderEnum.Bot)
            {
                card.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                card.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        private TextBlock CreateTextBlock(ConversationItem item)
        {
            TextBlock block = new TextBlock();
            block.Text = item.Content;
            return block;
        }
    }
}
