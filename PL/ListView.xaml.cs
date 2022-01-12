using BO;
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
    /// Interaction logic for ListView.xaml.
    /// This window covers a Lists of all objects defined in this project.
    /// </summary>
    public partial class ListView : Window
    {
        public BlApi.IBL AccessIbl; //Access object to the BL class

        //crate a observabs list of type IBL.BO.object(to see changes in live)
        public ObservableCollection<BO.DroneToList> DroneToLists;
        public ObservableCollection<BO.BaseStationsToList> BaseStationToLists;
        public ObservableCollection<BO.CustomerToList> CustomerToLists;
        public ObservableCollection<BO.ParcelToList> ParcelToLists;

        public bool ClosingWindow { get; private set; } = true; //a bool to help us disable the x bootum.

        /// <summary>
        /// ctor for the ListWindow that will start the InitializeComponent ans fill the Observable Collection.
        /// </summary>
        /// <param name="bl">get AccessIbl from main win</param>
        public ListView(BlApi.IBL bl)
        {
            InitializeComponent();

            AccessIbl = bl;

            #region drone Observable and listOfDrones
            //craet observer and set the list accordale to ibl drone list.
            DroneToLists = new ObservableCollection<DroneToList>();
            List<BO.DroneToList> drones = AccessIbl.GetDroneList().ToList();//למה העברנו את זה ככה??
            foreach (var item in drones)
            {
                DroneToLists.Add(item);
            }

            //new event that will call evre time that the ObservableCollection didact a change 
            DroneToLists.CollectionChanged += DroneToLists_CollectionChanged;

            listOfDrones.ItemsSource = DroneToLists; //Connecting the data in ObservableCollection to listOfDrones.
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
            BaseStationToLists.CollectionChanged += BaseStationToLists_CollectionChanged;

            listOfBaseStations.ItemsSource = BaseStationToLists; //Connecting the data in ObservableCollection to listOfBaseStations.
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
            CustomerToLists.CollectionChanged += CustomerToLists_CollectionChanged;

            listOfCustomers.ItemsSource = CustomerToLists; //Connecting the data in ObservableCollection to listOfCustomers.
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
            ParcelToLists.CollectionChanged += ParcelToLists_CollectionChanged;

            listOfParcels.ItemsSource = ParcelToLists;//Connecting the data in ObservableCollection to listOfParcels.
            #endregion parcel Observable and listOfParcel

            //combobox of dronesList handling.
            CBStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            CBWeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            //combobox of parcelList handling.
            CBparcelWhaigt.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            CBparcelStatus.ItemsSource = Enum.GetValues(typeof(DeliveryStatus));
            CBparcelWprior.ItemsSource = Enum.GetValues(typeof(Priorities));
        }

        #region CollectionChanged of all Observas
        /// <summary>
        /// a new event that we crate in the intaklizer :DroneToLists_CollectionChanged:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        private void DroneToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StatusDroneSelectorChanged();
        }

        /// <summary>
        /// a new event that we crate in the intaklizer :BaseStationToLists_CollectionChanged:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        private void BaseStationToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            listOfBaseStations.ItemsSource = BaseStationToLists; //update Connecting the data in ObservableCollection to listOfBaseStations.
        }

        /// <summary>
        /// a new event that we crate in the intaklizer :CustomerToLists_CollectionChanged:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        private void CustomerToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            listOfCustomers.ItemsSource = CustomerToLists; //update Connecting the data in ObservableCollection to listOfCustomers.
        }

        /// <summary>
        /// a new event that we crate in the intaklizer :ParcelToLists_CollectionChanged:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        private void ParcelToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            listOfParcels.ItemsSource = ParcelToLists; //update Connecting the data in ObservableCollection to listOfParcels.
        }
        #endregion CollectionChanged of all Observas

        /// <summary>
        /// help fanction to choose what to show on the user side accordingly to user cohises(bonous).
        /// </summary>
        public void StatusDroneSelectorChanged()
        {
            if (CBWeightSelctor.SelectedItem == null && CBStatusSelector.SelectedItem == null)
            {
                listOfDrones.ItemsSource = DroneToLists.ToList();
            }
            else if (CBWeightSelctor.SelectedItem == null)
            {
                listOfDrones.ItemsSource = DroneToLists.ToList().FindAll(x => x.Statuses == (DroneStatuses)CBStatusSelector.SelectedIndex);
            }
            else if (CBStatusSelector.SelectedItem == null)
            {
                listOfDrones.ItemsSource = DroneToLists.ToList().FindAll(x => x.MaxWeight == (WeightCategories)CBWeightSelctor.SelectedIndex);
            }
            else //If 2 filters are enabled.
            {
                listOfDrones.ItemsSource = DroneToLists.ToList().FindAll(x => x.Statuses == (DroneStatuses)CBStatusSelector.SelectedIndex && x.MaxWeight == (WeightCategories)CBWeightSelctor.SelectedIndex);
            }    
        }

        #region combobox of dronesList handling
        /// <summary>
        /// show on the user side accordingly to user cohises.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusDroneSelectorChanged();
        }

        /// <summary>
        /// show on the user side accordingly to user cohises.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBWeightSelctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusDroneSelectorChanged();
        }

        /// <summary>
        /// restart modem to set option to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BResetStatus_Click(object sender, RoutedEventArgs e)
        {
            CBStatusSelector.SelectedItem = null;
            StatusDroneSelectorChanged();
        }

        /// <summary>
        /// restart modem to set option to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BResetWeight_Click(object sender, RoutedEventArgs e)
        {
            CBWeightSelctor.SelectedItem = null;
            StatusDroneSelectorChanged();
        }
        #endregion combobox of dronesList handling

        #region MouseDoubleClick evant of all the Lists
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
                this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
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
                this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
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
                this.IsEnabled = false; // to privent anotur click on the list window chosse we donr want alot of windows togter.
                new ParcelWindow(AccessIbl, this, parcel, indexParcel).ShowDialog();//open the drone windowon acction
            }
        }
        #endregion MouseDoubleClick evant of all the Lists

        /// <summary>
        /// Add objects button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            // we send ""this"" window becuse we want to use it in the new window
            switch (TCmenu.SelectedIndex)//According to the tab the user is in, add an object.
            {
                case 0:
                    new DroneWindow(AccessIbl, this).Show();
                    IsEnabled = false;

                    break;
                case 1:
                    new BaseStationWindow(AccessIbl, this).Show();
                    IsEnabled = false;

                    break;
                case 2:
                    new CustomerWindow(AccessIbl, this).Show();
                    IsEnabled = false;

                    break;
                case 3:
                    new ParcelWindow(AccessIbl, this).Show();
                    IsEnabled = false;

                    break;
                default:
                    break;
            }
        }

        #region combobox of parcelList handling
        /// <summary>
        /// Filter the list by weight.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBparcelWhaigt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusParcelSelectorChanged();
        }

        /// <summary>
        /// Reset the list from the weight filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRparcelWhaigt_Click(object sender, RoutedEventArgs e)
        {
            CBparcelWhaigt.SelectedItem = null;
            StatusParcelSelectorChanged();
        }

        /// <summary>
        /// Filter the list by status.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBparcelStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusParcelSelectorChanged();
        }

        /// <summary>
        /// Reset the list from the status filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRparcelStatus_Click(object sender, RoutedEventArgs e)
        {
            CBparcelStatus.SelectedItem = null;
            StatusParcelSelectorChanged();
        }

        /// <summary>
        /// Filter the list by priority.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBparcelWprior_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusParcelSelectorChanged();
        }

        /// <summary>
        /// Resetting the list from the priority filter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRparcelprior_Click(object sender, RoutedEventArgs e)
        {
            CBparcelWprior.SelectedItem = null;
            StatusParcelSelectorChanged();
        }
        #endregion combobox of parcelList handling

        /// <summary>
        /// The function calculates the list of packages according to all the filters.
        /// </summary>
        public void StatusParcelSelectorChanged()
        {
            if (CBparcelWhaigt.SelectedItem == null && CBparcelStatus.SelectedItem == null && CBparcelWprior.SelectedItem == null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList();
            }
            else if (CBparcelWhaigt.SelectedItem == null && CBparcelStatus.SelectedItem == null && CBparcelWprior.SelectedItem != null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll(x => x.Prior == (Priorities)CBparcelWprior.SelectedIndex);
            }
            else if (CBparcelWhaigt.SelectedItem == null && CBparcelStatus.SelectedItem != null && CBparcelWprior.SelectedItem == null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll(x => x.Status == (DeliveryStatus)CBparcelStatus.SelectedIndex);
            }
            else if (CBparcelWhaigt.SelectedItem == null && CBparcelStatus.SelectedItem != null && CBparcelWprior.SelectedItem != null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll(x => x.Status == (DeliveryStatus)CBparcelStatus.SelectedIndex && x.Prior == (Priorities)CBparcelWprior.SelectedIndex);
            }
            if (CBparcelWhaigt.SelectedItem != null && CBparcelStatus.SelectedItem == null && CBparcelWprior.SelectedItem == null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll(x => x.Weight == (WeightCategories)CBparcelWhaigt.SelectedIndex);
            }
            else if (CBparcelWhaigt.SelectedItem != null && CBparcelStatus.SelectedItem == null && CBparcelWprior.SelectedItem != null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll(x => x.Weight == (WeightCategories)CBparcelWhaigt.SelectedIndex && x.Prior == (Priorities)CBparcelWprior.SelectedIndex);
            }
            else if (CBparcelWhaigt.SelectedItem != null && CBparcelStatus.SelectedItem != null && CBparcelWprior.SelectedItem == null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll(x => x.Weight == (WeightCategories)CBparcelWhaigt.SelectedIndex && x.Status == (DeliveryStatus)CBparcelStatus.SelectedIndex);
            }
            else if (CBparcelWhaigt.SelectedItem != null && CBparcelStatus.SelectedItem != null && CBparcelWprior.SelectedItem != null)
            {
                listOfParcels.ItemsSource = ParcelToLists.ToList().FindAll
                    (x => x.Weight == (WeightCategories)CBparcelWhaigt.SelectedIndex &&
                    x.Status == (DeliveryStatus)CBparcelStatus.SelectedIndex &&
                    x.Prior == (Priorities)CBparcelWprior.SelectedIndex);
            }
        }

        #region grouping
        /// <summary>
        /// grouping items by the settings we need 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grouping_Click(object sender, RoutedEventArgs e)
        {         
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listOfDrones.ItemsSource);
            if (view.GroupDescriptions.Count < 1) //  not allow to set the style more then once 
            {
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Statuses");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void groupingBaseStaiton_Click(object sender, RoutedEventArgs e)
        {
            // we orgnize the collaction by our settings (according to the enums in this case) by grouping them 

            IEnumerable<IGrouping<int, BaseStationsToList>> baseGroup = from baseStation in AccessIbl.GetBaseStationList() group baseStation by baseStation.FreeChargeSlots;
            List<BaseStationsToList> baseToList = new();

            foreach (var group in baseGroup)
            {
                foreach (var baseStation in group)
                {
                    baseToList.Add(baseStation);
                }
            }
            listOfBaseStations.ItemsSource = baseToList;

            //from here is to set the xaml
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listOfBaseStations.ItemsSource);
            if (view.GroupDescriptions.Count < 1) // prevent from do it more then once 
            {
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("FreeChargeSlots");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        /// <summary>
        /// clear the gruop proprties 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRefresh_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listOfBaseStations.ItemsSource);
            view.GroupDescriptions.Clear();
        }

        private void BgroupingCustomar_Click(object sender, RoutedEventArgs e)
        {
            // we orgnize the collaction by our settings (according to the enums in this case) by grouping them 

            IEnumerable<IGrouping<string, ParcelToList>> parcelGroup = from par in AccessIbl.GetParcelList() group par by par.CustomerSenderName;
            List<ParcelToList> parcelToList = new();

            foreach (var group in parcelGroup)
            {
                foreach (var par in group)
                {
                    parcelToList.Add(par);
                }
            }
            listOfParcels.ItemsSource = parcelToList;

            //from here is to set the xaml
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listOfParcels.ItemsSource);
            if (view.GroupDescriptions.Count < 1) // prevent from do it more then once 
            {
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("CustomerSenderName");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        private void BRefreshPar_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listOfParcels.ItemsSource);
            view.GroupDescriptions.Clear();
        }

        private void BgroupingCustomar1_Click(object sender, RoutedEventArgs e)
        {
            // we orgnize the collaction by our settings (according to the enums in this case) by grouping them 

            IEnumerable<IGrouping<string, ParcelToList>> parcelGroup = from par in AccessIbl.GetParcelList() group par by par.CustomerReceiverName;
            List<ParcelToList> parcelToList = new();

            foreach (var group in parcelGroup)
            {
                foreach (var par in group)
                {
                    parcelToList.Add(par);
                }
            }
            listOfParcels.ItemsSource = parcelToList;

            //from here is to set the xaml
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listOfParcels.ItemsSource);
            if (view.GroupDescriptions.Count < 1) // prevent from do it more then once 
            {
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("CustomerReceiverName");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        #endregion grouping 

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