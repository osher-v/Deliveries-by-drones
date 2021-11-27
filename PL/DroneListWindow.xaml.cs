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

//using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        public IBL.IBL AccessIbl;
        public DroneListWindow(IBL.IBL bl)
        {
            InitializeComponent();
            AccessIbl = bl;
            DroneListView.ItemsSource = bl.GetDroneList();
            StatusSelector.SelectedIndex = 3;
            WeightSelctor.SelectedIndex = 3;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(PO.DroneStatuses));
            WeightSelctor.ItemsSource = Enum.GetValues(typeof(PO.WeightCategories)); 
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            PO.DroneStatuses enumm = (PO.DroneStatuses)StatusSelector.SelectedItem;
            PO.WeightCategories enumm2 = (PO.WeightCategories)WeightSelctor.SelectedItem;
            List<IBL.BO.DroneToList> droneToList = AccessIbl.GetDroneList().ToList();
            if (enumm2 == PO.WeightCategories.All && enumm == PO.DroneStatuses.All)
                DroneListView.ItemsSource = droneToList;
            else if(enumm != PO.DroneStatuses.All && enumm2 == PO.WeightCategories.All)
                DroneListView.ItemsSource = droneToList.FindAll(x => x.Statuses == (IBL.BO.DroneStatuses)enumm);
            else if(enumm == PO.DroneStatuses.All && enumm2 != PO.WeightCategories.All)
                DroneListView.ItemsSource = droneToList.FindAll(x => x.MaxWeight == (IBL.BO.WeightCategories)WeightSelctor.SelectedItem);
            else
                DroneListView.ItemsSource = droneToList.FindAll(x => x.MaxWeight == (IBL.BO.WeightCategories)WeightSelctor.SelectedItem &&
                x.Statuses == (IBL.BO.DroneStatuses)enumm);

            //PO.DroneStatuses.All? AccessIbl.GetDroneList().ToList()
            //: AccessIbl.GetDroneList(x => x.Statuses == (IBL.BO.DroneStatuses)enumm).ToList()
            //enumm == PO.DroneStatuses.All ? AccessIbl.GetDroneList().ToList()
            //: AccessIbl.GetDroneList(x => x.Statuses == (IBL.BO.DroneStatuses)enumm).ToList();
            //DroneListView.ItemsSource = enumm == PO.DroneStatuses.All ? AccessIbl.GetDroneList() : AccessIbl.GetDroneList(x=> x.Statuses == (IBL.BO.DroneStatuses)enumm);
            */
        }

        private void whigetSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            PO.WeightCategories enumm = (PO.WeightCategories)WeightSelctor.SelectedItem;
            List<IBL.BO.DroneToList> droneToList = AccessIbl.GetDroneList().ToList();

            if (enumm == PO.WeightCategories.All && (PO.DroneStatuses)StatusSelector.SelectedItem == PO.DroneStatuses.All)
                DroneListView.ItemsSource = droneToList;
            else if ((PO.DroneStatuses)StatusSelector.SelectedItem != PO.DroneStatuses.All && enumm == PO.WeightCategories.All)
                DroneListView.ItemsSource = droneToList.FindAll(x => x.Statuses == (IBL.BO.DroneStatuses)enumm);
            else if ((PO.DroneStatuses)StatusSelector.SelectedItem == PO.DroneStatuses.All && enumm != PO.WeightCategories.All)
                DroneListView.ItemsSource = droneToList.FindAll(x => x.MaxWeight == (IBL.BO.WeightCategories)WeightSelctor.SelectedItem);
            else
                DroneListView.ItemsSource = droneToList.FindAll(x => x.MaxWeight == (IBL.BO.WeightCategories)WeightSelctor.SelectedItem &&
                x.Statuses == (IBL.BO.DroneStatuses)enumm);
            */
            //PO.WeightCategories enumm = (PO.WeightCategories)WeightSelctor.SelectedItem;
            //DroneListView.ItemsSource = enumm == PO.WeightCategories.All ? AccessIbl.GetDroneList() : AccessIbl.GetDroneList(x => x.MaxWeight == (IBL.BO.WeightCategories)enumm);
        }
    }
}
