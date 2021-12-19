﻿using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        public BlApi.IBL AccessIbl = BlApi.BlFactory.GetBL();

        /// <summary> crate a observabs list of type IBL.BO.object (to see changes in live) </summary>
        public ObservableCollection<BO.DroneToList> droneToLists;
        public ObservableCollection<BO.BaseStationsToList> BaseStationToLists;
        public ObservableCollection<BO.CustomerToList> CustomerToLists;
        public ObservableCollection<BO.ParcelToList> ParcelToLists;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        /// <summary>
        /// constractor for the ListWindow that will start the InitializeComponent ans fill the Observable Collection.
        /// </summary>
        /// <param name="bl">get AccessIbl from main win</param>
        public ListView()
        {
            InitializeComponent();

            //AccessIbl = bl;

            #region drone Observable and listOfDrones
            //craet observer and set the list accordale to ibl drone list.
            droneToLists = new ObservableCollection<DroneToList>();
            List<BO.DroneToList> drones = AccessIbl.GetDroneList().ToList();//למה העברנו את זה ככה??
            foreach (var item in drones)
            {
                droneToLists.Add(item);
            }

            //new event that will call evre time that the ObservableCollection didact a change 
            droneToLists.CollectionChanged += DroneToLists_CollectionChanged;

            //display the defult list 
            listOfDrones.ItemsSource = droneToLists;
            #endregion drone Observable and listOfDrones

            #region baseStations Observable and listOfBaseStations
            //craet observer and set the list accordale to ibl BaseStation list. 
            BaseStationToLists = new ObservableCollection<BaseStationsToList>();
            List<BO.BaseStationsToList> baseStations = AccessIbl.GetBaseStationList().ToList();//למה העברנו את זה ככה??
            foreach (var item in baseStations)
            {
                BaseStationToLists.Add(item);
            }

            //new event that will call evre time that the ObservableCollection didact a change 
            //BaseStationToLists.CollectionChanged += BaseStationToLists_CollectionChanged;

            //display the defult list 
            listOfBaseStations.ItemsSource = BaseStationToLists;
            #endregion baseStations Observable and listOfBaseStations

            #region customer Observable and listOfCustomer
            //craet observer and set the list accordale to ibl BaseStation list. 
            CustomerToLists = new ObservableCollection<CustomerToList>();
            List<BO.CustomerToList> customer = AccessIbl.GetCustomerList().ToList();//למה העברנו את זה ככה??
            foreach (var item in customer)
            {
                CustomerToLists.Add(item);
            }

            //new event that will call evre time that the ObservableCollection didact a change 
            //BaseStationToLists.CollectionChanged += BaseStationToLists_CollectionChanged;

            //display the defult list 
            listOfCustomers.ItemsSource = CustomerToLists;
            #endregion customer Observable and listOfCustomer

            #region parcel Observable and listOfParcel
            //craet observer and set the list accordale to ibl BaseStation list. 
            ParcelToLists = new ObservableCollection<ParcelToList>();
            List<BO.ParcelToList> parcel = AccessIbl.GetParcelList().ToList();//למה העברנו את זה ככה??
            foreach (var item in parcel)
            {
                ParcelToLists.Add(item);
            }

            //new event that will call evre time that the ObservableCollection didact a change 
            //BaseStationToLists.CollectionChanged += BaseStationToLists_CollectionChanged;

            //display the defult list 
            listOfParcels.ItemsSource = ParcelToLists;
            #endregion parcel Observable and listOfParcel

            //combobox of dronesList handling.
            CBStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            CBWeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

        /// <summary>
        /// /// a new event that we crate in the intaklizer :DroneToLists_CollectionChanged:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        private void DroneToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StatusSelectorChanged();
        }

        /// <summary>
        /// help fanction to choose what to show on the user side accordingly to user cohises ( bonous)
        /// </summary>
        public void StatusSelectorChanged()
        {
            if (CBWeightSelctor.SelectedItem == null && CBStatusSelector.SelectedItem == null)
            {
                listOfDrones.ItemsSource = droneToLists.ToList();
            }
            else if (CBWeightSelctor.SelectedItem == null)
            {
                listOfDrones.ItemsSource = droneToLists.ToList().FindAll(x => x.Statuses == (DroneStatuses)CBStatusSelector.SelectedIndex);
            }
            else if (CBStatusSelector.SelectedItem == null)
            {
                listOfDrones.ItemsSource = droneToLists.ToList().FindAll(x => x.MaxWeight == (WeightCategories)CBWeightSelctor.SelectedIndex);
            }
            else //
            {
                listOfDrones.ItemsSource = droneToLists.ToList().FindAll(x => x.Statuses == (DroneStatuses)CBStatusSelector.SelectedIndex && x.MaxWeight == (WeightCategories)CBWeightSelctor.SelectedIndex);
            }
        }

        #region combobox of dronesList handling
        /// <summary>
        /// show on the user side accordingly to user cohises
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorChanged();
        }

        /// <summary>
        /// show on the user side accordingly to user cohises
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBWeightSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorChanged();
        }

        /// <summary>
        /// restart modem to set option to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BResetStatus_Click(object sender, RoutedEventArgs e)
        {
            CBStatusSelector.SelectedItem = null;
            StatusSelectorChanged();
        }

        /// <summary>
        /// restart modem to set option to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BResetWeight_Click(object sender, RoutedEventArgs e)
        {
            CBWeightSelctor.SelectedItem = null;
            StatusSelectorChanged();
        }




        #endregion combobox of dronesList handling

        /// <summary>
        /// open drone window in acction when didect double clik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfDrones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList drone = (DroneToList)listOfDrones.SelectedItem;
            if (drone != null)// if the user click on empty space in the view list we donr open anything
            {
                int indexDrone = listOfDrones.SelectedIndex;
                this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
                new DroneWindow(AccessIbl, this, drone, indexDrone).Show();//open the drone windowon acction
            }
        }



        /// <summary>
        /// open BaseStation window in acction when didect double clik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfBaseStations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationsToList baseStations = (BaseStationsToList)listOfBaseStations.SelectedItem;
            if (baseStations != null)// if the user click on empty space in the view list we donr open anything
            {
                int indexBaseStation = listOfBaseStations.SelectedIndex;
                //this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
                new BaseStationWindow(AccessIbl, this, baseStations, indexBaseStation).Show();//open the drone windowon acction
            }
        }

        /// <summary>
        /// open Customer window in acction when didect double clik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfCustomers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)listOfCustomers.SelectedItem;
            if (customer != null)// if the user click on empty space in the view list we donr open anything
            {
                int indexCustomer = listOfCustomers.SelectedIndex;
                //this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
                new CustomerWindow(AccessIbl, this, customer, indexCustomer).Show();//open the drone windowon acction
            }
        }

        /// <summary>
        /// open Parcel window in acction when didect double clik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfParcels_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)listOfParcels.SelectedItem;
            if (parcel != null)// if the user click on empty space in the view list we donr open anything
            {
                int indexParcel = listOfParcels.SelectedIndex;
                //this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
                new ParcelWindow(AccessIbl, this, parcel, indexParcel).Show();//open the drone windowon acction
            }
        }















        /// <summary>
        /// Add drone button which activates the builder in the Add drone window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            // we send ""this"" window becuse we want to use it in the new window
            new DroneWindow(AccessIbl, this).Show();
            this.IsEnabled = false;
            //switch (switch_on)
            //{
            //    default:
            //}
        }

        #region close
        /// <summary>
        /// cancel the option to clik x to close the window 
        /// </summary>
        /// <param name="e">close window</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = ClosingWindow;
        }

        /// <summary>
        /// to aloow closing again but just in the spcific close boutoon 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bclose_Click_1(object sender, RoutedEventArgs e)
        {
            ClosingWindow = false; // we alowd the close option
            Close();
        }
        #endregion close     

    }
}

