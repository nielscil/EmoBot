using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
    public partial class ConverstationItemControl : UserControl, IDisposable
    {

        public ConverstationItemControl()
        {
            InitializeComponent();
            DataContextChanged += ConverstationItemControl_DataContextChanged;
        }

        public void Dispose()
        {
            DataContextChanged -= ConverstationItemControl_DataContextChanged;
        }

        private void ConverstationItemControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Card card = new Card();
            //TODO: set good grid column
            //TOOD: add textblock

            Grid grid = Content as Grid;
            if(grid != null)
            {
                grid.Children.Add(card);
            }
        }
    }
}
