using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Wpf.Views;
using Simulation.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Simulation.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : MvxWpfView
    {
        private Point scrollOffset;
        private Point scrollMousePoint;
        private bool isScrolling = false;

        public ShellView()
        {
            InitializeComponent();
        }

        private void Quicker_Click(object sender, RoutedEventArgs e)
        {
            ShellViewModel.SpeedUpSimulation();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (ShellViewModel.Paused)
            {
                ShellViewModel.ContinueSimulation();
                b.Content = FindResource("Stop");
            }
            else
            {
                ShellViewModel.PauseSimulation();
                b.Content = FindResource("Play");
            }
        }

        private void Slow_Click(object sender, RoutedEventArgs e)
        {
            ShellViewModel.SlowSimulation();
        }

        private void ScrollViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl && !isScrolling)
            {
                isScrolling = true;
                var scroll = sender as ScrollViewer;
                Mouse.OverrideCursor = Cursors.Hand;

                scroll.PreviewMouseLeftButtonDown += ScrollViewer_MouseDown;
            }
        }

        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isScrolling)
            {
                var scroll = sender as ScrollViewer;
                Mouse.OverrideCursor = Cursors.Hand;
                scrollOffset.Y = scroll.VerticalOffset;
                scrollOffset.X = scroll.HorizontalOffset;
                scrollMousePoint = e.GetPosition(scroll);

                scroll.MouseMove += ScrollViewer_MouseMove;
            }
        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isScrolling)
            {
                var scroll = sender as ScrollViewer;
                var p = e.GetPosition(scroll);

                //scrollOffset.Y += p.Y - pos.Y > 0 ? 1 : -0.1;

                if (scrollOffset.Y < 0) scrollOffset.Y = 0;
                var offsetY = scrollOffset.Y + (scrollMousePoint.Y - e.GetPosition(scroll).Y);
                var offsetX = scrollOffset.X + (scrollMousePoint.X - e.GetPosition(scroll).X);
                scroll.ScrollToVerticalOffset(offsetY);
                scroll.ScrollToHorizontalOffset(offsetX);
            }
        }

        private void ScrollViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.LeftCtrl && isScrolling)
            {
                isScrolling = false;
                var scroll = sender as ScrollViewer;
                Mouse.OverrideCursor = Cursors.Arrow;

                scroll.MouseMove -= ScrollViewer_MouseMove;
                scroll.PreviewMouseLeftButtonDown -= ScrollViewer_MouseDown;
            }
        }
    }
}
