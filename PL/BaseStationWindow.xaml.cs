using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for BaseStationWindow.xaml.
    /// This window covers a baseStation interface.
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        public BlApi.IBL AccessIbl; //Access object to the BL class.

        public ListView ListWindow; //object of ListView window.

        public bool ClosingWindow { get; private set; } = true; //a bool to help us disable the x bootum

        #region add situation 
        /// <summary>
        /// adding constractor
        /// </summary>
        /// <param name="bl">Access object to the BL class</param>
        /// <param name="_ListWindow">object of ListView window</param>
        public BaseStationWindow(BlApi.IBL bl, ListView _ListWindow)
        {
            InitializeComponent();

            addBaseStation.Visibility = Visibility.Visible; //open the Grid of add action.

            AccessIbl = bl;

            ListWindow = _ListWindow;
        }

        /// <summary>
        /// The function handles adding a station.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            //Check that all fields are filled.
            if (TBstaitonId.Text.Length != 0 && TBstaitonName.Text.Length != 0 && TBstationChargeSlots.Text.Length != 0 && TBstaitonLongtude.Text.Length != 0 && TBstaitonLattude.Text.Length != 0)
            {
                //Check that the location does not exceed the boundaries of Gush Dan.
                if (!(double.Parse(TBstaitonLongtude.Text) < 31.8 || double.Parse(TBstaitonLongtude.Text) > 32.2 || double.Parse(TBstaitonLattude.Text) < 34.6 || double.Parse(TBstaitonLattude.Text) > 35.1))
                {
                    BO.BaseStation baseStationAdd = new BaseStation() //Create an object to add to the data.
                    {
                        Id = int.Parse(TBstaitonId.Text),
                        Name = TBstaitonName.Text,
                        FreeChargeSlots = int.Parse(TBstationChargeSlots.Text),
                        BaseStationLocation = new Location() { longitude = double.Parse(TBstaitonLongtude.Text), latitude = double.Parse(TBstaitonLattude.Text) },
                    };

                    try
                    {
                        AccessIbl.AddStation(baseStationAdd); //update the logic layer.
                        MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                BO.BaseStationsToList stationsToList = AccessIbl.GetBaseStationList().ToList().Find(i => i.Id == baseStationAdd.Id);
                                ListWindow.BaseStationToLists.Add(stationsToList); //Updating the observer list of stations.

                                ListWindow.IsEnabled = true; //to open the "ListWindow" window.

                                ClosingWindow = false; //to enable to close the "BaseStationWindow" window.
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (AddAnExistingObjectException ex) //The problem is with the ID number field.
                    {
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        TBstaitonId.Text = "";
                        TBstaitonId.BorderBrush = Brushes.Red;
                    }
                }
                else //if location exceed the boundaries of Gush Dan.
                {
                    MessageBox.Show(" מיקום קו האורך יכול להיות בין 31.8 ל32.2 וקו הרוחב בין34.6 ל35.1", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else //If not all fields are filled.
            {
                MessageBox.Show("נא ודאו שכל השדות מלאים", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region מטפל בבדיקות כפתורים
        /// <summary>
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBstaitonId_KeyDown(object sender, KeyEventArgs e)
        {
            TBstaitonId.BorderBrush = Brushes.Gray;
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // we want keys from the num pud too.
                {
                    e.Handled = true; //In this case, pressing the keyboard is not enabled.
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        /// <summary>
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBstaitonLattude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // we want keys from the num pud too
                {
                    if (e.Key == Key.Decimal) //Open option of "."
                        e.Handled = false;
                    else
                        e.Handled = true; //In this case, pressing the keyboard is not enabled.
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        /// <summary>
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBstaitonLongtude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // we want keys from the num pud too
                {
                    if (e.Key == Key.Decimal)//Open option of "."
                        e.Handled = false;
                    else
                        e.Handled = true; //In this case, pressing the keyboard is not enabled.
                }
                else
                {
                    e.Handled = false;
                }
            }
        }

        /// <summary>
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBstationChargeSlots_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // we want keys from the num pud too
                {
                    e.Handled = true; //In this case, pressing the keyboard is not enabled.
                }
                else
                {
                    e.Handled = false;
                }
            }
        }
        #endregion מטפל בבדיקות כפתורים

        #endregion add situation 

        public BaseStation baseStation;

        public int indexSelected; //the location index in the observer of the stations in the ListView window.

        #region update situation

        /// <summary>
        /// update constractor
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_DroneListWindow"></param>
        /// <param name="BaseStationTo"></param>
        /// <param name="_indexDrone"></param>
        public BaseStationWindow(BlApi.IBL bl, ListView _ListWindow, BaseStationsToList BaseStationTo, int _indexBaseStation)
        {
            InitializeComponent();

            updateBaseStation.Visibility = Visibility.Visible; //open the Grid of add action.

            AccessIbl = bl;

            ListWindow = _ListWindow;

            indexSelected = _indexBaseStation; //the location index in the observer of the stations in the ListView window.

            //Connecting station data.
            baseStation = AccessIbl.GetBaseStation(BaseStationTo.Id);
            DataContext = baseStation;

            TBstationFreeChargeSlotS.Text = (BaseStationTo.BusyChargeSlots + baseStation.FreeChargeSlots).ToString();//Connection given total ChargeSlots.

            listOfDronesInCahrge.ItemsSource = baseStation.DroneInChargsList; //Connection the List of DronesInCahrge.
        }

        /// <summary>
        /// The function handles adding a station.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.UpdateBaseStaison(baseStation.Id, TBUpdateStaitonName.Text, TBstationFreeChargeSlotS.Text); //update the logic layer.
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        ListWindow.BaseStationToLists[indexSelected] = AccessIbl.GetBaseStationList().First(x => x.Id == baseStation.Id);//Updating the observer list of stations.

                        ListWindow.IsEnabled = true; //to open the "ListWindow" window.

                        ClosingWindow = false; //to enable to close the "BaseStationWindow" window.
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch (MoreDroneInChargingThanTheProposedChargingStations ex) //The problem is with the stationFreeChargeSlots field.
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                TBstationFreeChargeSlotS.Text = "";
                TBstationFreeChargeSlotS.BorderBrush = Brushes.Red;
            }
        }

        /// <summary>
        /// Opening a drone window from the List of drone in Charge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfDronesInCahrge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((DroneInCharg)listOfDronesInCahrge.SelectedItem) != null)// if the user click on empty space in the view list we donr open anything
            {
                int IdOfDroneInCharg = ((DroneInCharg)listOfDronesInCahrge.SelectedItem).Id;
                int indexDroneInObservable = ListWindow.DroneToLists.IndexOf(ListWindow.DroneToLists.First(x => x.Id == IdOfDroneInCharg));
                DroneToList drone = AccessIbl.GetDroneList().First(x => x.Id == IdOfDroneInCharg);
                new DroneWindow(AccessIbl, ListWindow, drone, indexDroneInObservable).Show();

                ClosingWindow = false;
                Close();
            }
        }

        /// <summary>
        /// The function opens the option to update as soon as the fields are full and closes when one of the fields is equal to 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBUpdateStaitonName_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBstationFreeChargeSlotS.Text.Length != 0 && TBUpdateStaitonName.Text.Length != 0)
            {
                if (!BUpdate.IsEnabled)
                    BUpdate.IsEnabled = true;
            }
            else //one of the fields is equal to 0.
            {
                BUpdate.IsEnabled = false;
            }
        }

        /// <summary>
        /// The function opens the option to update as soon as the fields are full and closes when one of the fields is equal to 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBstationFreeChargeSlotS_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBstationFreeChargeSlotS.Text.Length != 0 && TBUpdateStaitonName.Text.Length != 0)
            {
                if (!BUpdate.IsEnabled)
                    BUpdate.IsEnabled = true;
            }
            else//one of the fields is equal to 0.
            {
                BUpdate.IsEnabled = false;
            }
        }

        #endregion update situation

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
        /// The function closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeAdd_Click(object sender, RoutedEventArgs e)
        {
            ListWindow.IsEnabled = true;
            ClosingWindow = false; // we alowd the close option
            Close();
        }

        /// <summary>
        /// The function closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListWindow.IsEnabled = true;
            ClosingWindow = false; // we alowd the close option
            Close();
        }
        #endregion close  
    }
}