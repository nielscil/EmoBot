using AliceWPF.ViewModels;
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
using System.Windows.Shapes;

namespace AliceWPF.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window, IDisposable
    {
        public MainView()
        {
            InitializeComponent();
            Input.KeyDown += Input_KeyDown;
        }

        public void Dispose()
        {
            Input.KeyDown -= Input_KeyDown;
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainViewModel viewModel = DataContext as MainViewModel;
                if(viewModel != null)
                {
                    viewModel.SendContent();
                }
            }
        }
    }
}
