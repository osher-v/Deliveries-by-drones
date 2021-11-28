using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        public IBL.IBL AccessIbl;
        //public CancelEventArgs temp = new CancelEventArgs();
        public DroneListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            AccessIbl = bl;
            DroneListView.ItemsSource = bl.GetDroneList();   
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /*
        protected override void OnClosing(CancelEventArgs e)
        {
            temp = e;
            //base.OnClosing(e);
            //e.Cancel = true;
        }
        */

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelctor.SelectedItem == null)
                DroneListView.ItemsSource = AccessIbl.GetDroneList(x => x.Statuses == (DroneStatuses)StatusSelector.SelectedIndex);
            else
                DroneListView.ItemsSource = AccessIbl.GetDroneList(x => x.Statuses == (DroneStatuses)StatusSelector.SelectedIndex && x.MaxWeight == (WeightCategories)WeightSelctor.SelectedIndex);
        }

        private void whigetSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem == null)
                DroneListView.ItemsSource = AccessIbl.GetDroneList(x => x.MaxWeight == (WeightCategories)WeightSelctor.SelectedIndex);
            else
                DroneListView.ItemsSource = AccessIbl.GetDroneList(x => x.MaxWeight == (WeightCategories)WeightSelctor.SelectedIndex && x.Statuses == (DroneStatuses)StatusSelector.SelectedIndex);
        }

        private void res_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            WeightSelctor.SelectedItem = null;
            DroneListView.ItemsSource = AccessIbl.GetDroneList();
        }

        private void BAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(AccessIbl).Show();
        }

        private void Bclose_Click(object sender, RoutedEventArgs e)
        {
            //temp.Cancel = false;
            list.Close();
        }
    }
}
