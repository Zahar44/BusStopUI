using MvvmCross.Platforms.Wpf.Views;
using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simulation.Wpf.Views
{
    /// <summary>
    /// Interaction logic for RoadDialogeView.xaml
    /// </summary>
    public partial class RoadDialogeView : MvxWpfView
    {
        public RoadDialogeView()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dialoge.CloseDialog();
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as RoadDialogeViewModel).Length = 0;
            Dialoge.CloseDialog();
        }
    }
}
