using MvvmCross.Platforms.Wpf.Views;
using Simulation.Core.ViewModels;
using Simulation.Wpf.Helpers;
using Simulation.Wpf.UI;
using System.Linq;
using System.Windows.Controls;

namespace Simulation.Wpf.Views
{
    /// <summary>
    /// Interaction logic for RouteDialogeView.xaml
    /// </summary>
    public partial class RouteDialogeView : MvxWpfView
    {
        public RouteDialogeView()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dialoge.CloseDialog();
        }

        private void Create_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var route = DataContext as RouteDialogeViewModel;
            if (route is null)
                throw new System.Exception();

            var ways = route.GetWays();
            var busCnt = route.BusCount;
            var delay = route.GetDelay();
            var routeCore = new RouteCore(ways, busCnt, delay);
            
            Dialoge.CloseDialog();
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var listView = sender as ListView;
            (DataContext as RouteDialogeViewModel).Set(listView.SelectedItem.ToString());
        }
    }
}
