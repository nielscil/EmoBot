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
                SetCard(item);
            }
        }

        private void SetCard(ConversationItem item)
        {
            SetCardAlignment(card, item);
            SetText(item);
        }

        private void SetCardAlignment(Card card, ConversationItem item)
        {
            if(item.Sender == SenderEnum.Bot)
            {
                card.HorizontalAlignment = HorizontalAlignment.Left;
                card.Background = Brushes.MediumPurple;
                card.Foreground = Brushes.White;
            }
            else
            {
                card.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }

        private void SetText(ConversationItem item)
        {
            content.Text = item.Content;
        }
    }
}
