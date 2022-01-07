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
        //Access object to the BL class.
        public BlApi.IBL AccessIbl;

        //object of ListView window.
        public ListView ListWindow;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        #region בנאי הוספה 
        /// <summary>
        /// adding constractor
        /// </summary>
        /// <param name="bl">Access object to the BL class</param>
        /// <param name="_ListWindow">object of ListView window</param>
        public BaseStationWindow(BlApi.IBL bl, ListView _ListWindow)
        {
            InitializeComponent();

            addBaseStation.Visibility = Visibility.Visible;

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
                    BO.BaseStation baseStationAdd = new BaseStation()
                    {
                        Id = int.Parse(TBstaitonId.Text),
                        Name = TBstaitonName.Text,
                        FreeChargeSlots = int.Parse(TBstationChargeSlots.Text),
                        BaseStationLocation = new Location() { longitude = double.Parse(TBstaitonLongtude.Text), latitude = double.Parse(TBstaitonLattude.Text) },
                    };

                    try
                    {
                        AccessIbl.AddStation(baseStationAdd);
                        MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                BO.BaseStationsToList stationsToList = AccessIbl.GetBaseStationList().ToList().Find(i => i.Id == baseStationAdd.Id);
                                ListWindow.BaseStationToLists.Add(stationsToList); //Updating the observer list of stations.
                                ListWindow.IsEnabled = true;
                                ClosingWindow = false;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (AddAnExistingObjectException ex)
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
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // we want keys from the num pud too
                {
                    e.Handled = true;
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
                    if (e.Key == Key.Decimal)
                        e.Handled = false;
                    else
                        e.Handled = true;
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
                    if (e.Key == Key.Decimal)
                        e.Handled = false;
                    else
                        e.Handled = true;
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
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
        }
        #endregion מטפל בבדיקות כפתורים

        #endregion בנאי הוספה

        public BaseStation baseStation;

        public int indexSelected;

        #region בנאי לעדכון
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

            updateBaseStation.Visibility = Visibility.Visible;

            AccessIbl = bl;

            ListWindow = _ListWindow;

            indexSelected = _indexBaseStation;

            baseStation = AccessIbl.GetBaseStation(BaseStationTo.Id);
            DataContext = baseStation;

            TBstationFreeChargeSlotS.Text = (BaseStationTo.BusyChargeSlots + baseStation.FreeChargeSlots).ToString();

            listOfDronesInCahrge.ItemsSource = baseStation.DroneInChargsList;
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
                AccessIbl.UpdateBaseStaison(baseStation.Id, TBUpdateStaitonName.Text, TBstationFreeChargeSlotS.Text);
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        ListWindow.BaseStationToLists[indexSelected] = AccessIbl.GetBaseStationList().First(x => x.Id == baseStation.Id);//עדכון המשקיף

                        ListWindow.IsEnabled = true;
                        ClosingWindow = false;
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch (MoreDroneInChargingThanTheProposedChargingStations ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                TBstationFreeChargeSlotS.Text = "";
                TBstationFreeChargeSlotS.BorderBrush = Brushes.Red;
            }
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

        #endregion

        /// <summary>
        /// Opening a drone window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfDronesInCahrge_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int IdOfDroneInCharg = ((DroneInCharg)listOfDronesInCahrge.SelectedItem).Id;
            int indexDroneInObservable = ListWindow.DroneToLists.IndexOf(ListWindow.DroneToLists.First(x => x.Id == IdOfDroneInCharg));
            DroneToList drone = AccessIbl.GetDroneList().First(x => x.Id == IdOfDroneInCharg); 
            new DroneWindow(AccessIbl, ListWindow, drone, indexDroneInObservable).Show();

            ClosingWindow = false; // עקרונית צריכים לעדכן את החלון הזה השאלה איך עושים
            Close();         
        }
    }
}