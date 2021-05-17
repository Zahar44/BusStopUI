using MvvmCross.Platforms.Wpf.Views;
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
using System.Windows.Shapes;

namespace Simulation.Wpf
{
    /// <summary>
    /// Interaction logic for Dialoge.xaml
    /// </summary>
    public partial class Dialoge : MvxWindow
    {
        private static Dialoge current;
        public Dialoge(MvxWpfView view)
        {
            InitializeComponent();
            this.Content = view;
        }

        static public void ShowDialog(MvxWpfView view, string title)
        {
            current = new Dialoge(view);
            current.Title = title;
            current.Width = 250;
            current.Height = 300;
            current.ShowDialog();
        }

        static public void CloseDialog()
        {
            current?.Close();
        }
    }
}
