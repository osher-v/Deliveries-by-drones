using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<IBL.BO.DroneToList> droneToLists;
        public bool ClosingWindow { get; private set; } = true;
        public DroneListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            AccessIbl = bl;
            droneToLists = new ObservableCollection<DroneToList>();
            List<IBL.BO.DroneToList> drones = bl.GetDroneList().ToList();
            //droneToLists.ToList().AddRange(drones);
            foreach (var item in drones)
            {
                droneToLists.Add(item);
            }
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
            DroneListView.ItemsSource = droneToLists;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

       

        private void DroneToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StatusSelectorChanged();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            //base.OnClosing(e);
            e.Cancel = ClosingWindow;
        }


        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorChanged();
        }

        private void StatusSelectorChanged()
        {
            if (WeightSelctor.SelectedItem == null)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(x => x.Statuses == (DroneStatuses)StatusSelector.SelectedIndex);
            else
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(x => x.Statuses == (DroneStatuses)StatusSelector.SelectedIndex && x.MaxWeight == (WeightCategories)WeightSelctor.SelectedIndex);
        }

        private void whigetSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedItem == null)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(x => x.MaxWeight == (WeightCategories)WeightSelctor.SelectedIndex);
            else
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(x => x.MaxWeight == (WeightCategories)WeightSelctor.SelectedIndex && x.Statuses == (DroneStatuses)StatusSelector.SelectedIndex);
        }

        private void res_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            WeightSelctor.SelectedItem = null;
            DroneListView.ItemsSource = droneToLists;
        }

        private void BAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(AccessIbl, this).Show();
        }

        private void Bclose_Click(object sender, RoutedEventArgs e)
        {
            //temp.Cancel = false;
            ClosingWindow = false;
            Close();
        }
    }
}
