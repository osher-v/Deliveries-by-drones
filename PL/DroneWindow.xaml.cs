using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using IBL.BO;
using System.ComponentModel.DataAnnotations;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        public IBL.IBL AccessIbl;
        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;
        /// <summary> the calling window, becuse we want to use it here </summary>
        /// 
        private DroneListWindow DroneListWindow;
        #region בנאי להוספה 
        /// <summary>
        /// consractor for add drone option 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_DroneListWindow"></param>
        public DroneWindow(IBL.IBL bl, DroneListWindow _DroneListWindow)
        {
            InitializeComponent();

            this.Width = 440;
            addDrone.Visibility = Visibility.Visible;

            AccessIbl = bl;

            DroneListWindow = _DroneListWindow;

            // the combobox use it to show the Weight Categories
            TBWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            // the combobox use it to show the BaseStation ID
            BaseStationID.ItemsSource = AccessIbl.GetBaseStationList(x => x.FreeChargeSlots > 0);
            BaseStationID.DisplayMemberPath = "Id";          
        }


        /// <summary>
        /// disable the non numbers keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBID_KeyDown(object sender, KeyEventArgs e)
        {
            TBID.BorderBrush = Brushes.Gray;
            // take only the kyes we alowed 
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
            if (TBID.Text.Length > 8)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// linited the langth of the text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBModel_KeyDown(object sender, KeyEventArgs e)
        {
            TBID.BorderBrush = Brushes.Gray;
            if (TBModel.Text.Length > 5)
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// A function that sends the new drone and adds it to the data after tests in the logical layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToBl_Click(object sender, RoutedEventArgs e)
        {
            // If all the fields are full
            if (TBModel.Text.Length != 0 && TBID.Text.Length != 0 && BaseStationID.SelectedItem != null && TBWeight.SelectedItem != null)
            {
                DroneToList newdrone = new DroneToList
                {
                    Id = int.Parse(TBID.Text),
                    Model = TBModel.Text,
                    MaxWeight = (WeightCategories)TBWeight.SelectedIndex,
                };
                // try to add the drone if fals return a MessageBox
                try
                {
                    AccessIbl.AddDrone(newdrone, ((BaseStationsToList)BaseStationID.SelectedItem).Id);
                    MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            newdrone = AccessIbl.GetDroneList().ToList().Find(i => i.Id == newdrone.Id);
                            DroneListWindow.droneToLists.Add(newdrone);
                            DroneListWindow.IsEnabled = true;
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
                    TBID.Text = "";
                    TBID.BorderBrush = Brushes.Red; //בונוס 
                }
                catch (NonExistentObjectException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    TBID.Text = "";
                    TBID.BorderBrush = Brushes.Red;//בונוס 
                }
                catch (NoFreeChargingStations ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (NonExistentEnumException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("נא ודאו שכל השדות מלאים", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// to aloow closing again but just in the spcific close boutoon 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bclose_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow.IsEnabled = true;
            ClosingWindow = false;
            Close();
        }
        /// <summary>
        /// cancel the option to clik x to close the window 
        /// </summary>
        /// <param name="e">close window</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = ClosingWindow;
        }
        #endregion

        #region רחפן בפעולות 
        public Drone MyDrone;

        public int indexDrone;//indexe of the drone how chosse by doubly click 
        /// <summary>
        /// constractor for acction staet  And updates the views accordingly
        /// </summary>
        /// <param name="bl">accses to ibl</param>
        /// <param name="_DroneListWindow">the call window</param>
        /// <param name="id">the drone id that chosen</param>
        /// <param name="_indexDrone">/indexe of the drone in the list</param>
        public DroneWindow(IBL.IBL bl, DroneListWindow _DroneListWindow, DroneToList droneTo, int _indexDrone)
        {
            InitializeComponent();
            updateDrone.Visibility = Visibility.Visible; // open the grid for the user
            indexDrone = _indexDrone;
            AccessIbl = bl;

            DroneListWindow = _DroneListWindow;
            //to conecct the binding to set the value of my drone to the proprtis
            MyDrone = bl.GetDrone(droneTo.Id);
            DataContext = MyDrone;

            BModalUpdate.IsEnabled = false;

            //The switch checks the drone's status value and opens buttons 
            switch ((DroneStatuses)MyDrone.Statuses)
            {
                case DroneStatuses.free:
                    BSendToCharge.Visibility = Visibility.Visible;
                    BAssignPackage.Visibility = Visibility.Visible;
                    break;

                case DroneStatuses.inMaintenance:
                    BReleaseDrone.Visibility = Visibility.Visible;
                    BReleaseDrone.IsEnabled = false;
                    TimeChoose.Visibility = Visibility.Visible;
                    break;

                case DroneStatuses.busy:
                    GRIDparcelInDelivery.Visibility = Visibility.Visible;
                    TBnotAssigned.Visibility = Visibility.Hidden;
                    //check the status to open the right button
                    if (MyDrone.Delivery.OnTheWayToTheDestination)
                    {
                        BDeliveryPackage.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BPickedUp.Visibility = Visibility.Visible;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// close drone window And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BClose1_Click(object sender, RoutedEventArgs e)
        {
            DroneListWindow.IsEnabled = true;//allowd to use drone window list again
            ClosingWindow = false;
            Close();
        }
        /// <summary>
        /// the fanction update the modal of the drone And updates the views accordingly 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BModalUpdate_Click(object sender, RoutedEventArgs e)
        {
            AccessIbl.UpdateDroneName(MyDrone.Id, TBmodel.Text);
            MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
            switch (result)
            {
                case MessageBoxResult.OK:
                    BModalUpdate.IsEnabled = false;
                    DroneListWindow.StatusSelectorChanged();
                    //DroneListWindow.droneToLists[indexDrone].Model = TBmodel.Text;
                    //DroneListWindow.droneToLists[indexDrone] = DroneListWindow.droneToLists[indexDrone];
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// the fanction send the drone to charge And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.SendingDroneforCharging(MyDrone.Id);

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:

                        //DroneListWindow.droneToLists[indexDrone].Statuses = DroneStatuses.inMaintenance;
                        //DroneListWindow.droneToLists[indexDrone].CurrentLocation = AccessIbl.GetDrone(MyDrone.Id).CurrentLocation;
                        //DroneListWindow.droneToLists[indexDrone].BatteryStatus = AccessIbl.GetDrone(MyDrone.Id).BatteryStatus;
                        //DroneListWindow.droneToLists[indexDrone] = DroneListWindow.droneToLists[indexDrone];
                        DroneListWindow.StatusSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        BSendToCharge.Visibility = Visibility.Hidden;
                        BReleaseDrone.Visibility = Visibility.Visible;
                        BAssignPackage.Visibility = Visibility.Hidden;

                        BReleaseDrone.IsEnabled = false;
                        TimeChoose.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            catch (TheDroneCanNotBeSentForCharging ex)
            {
                MessageBox.Show(ex.Message, "info");
            }
        }
        /// <summary>
        ///  release the drone from charge And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BReleaseDrone_Click(object sender, RoutedEventArgs e)
        {
            DateTime time;
            string stringTime = $"{TBhours.Text}:{TBmin.Text}:{TBsec.Text}";//to convert the strings to a string that the datetime can hold
            DateTime.TryParse(stringTime, out time);
            AccessIbl.ReleaseDroneFromCharging(MyDrone.Id, time);

            MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
            switch (result)
            {
                case MessageBoxResult.OK:
                    DroneListWindow.StatusSelectorChanged();

                    //to conecct the binding to set the value of my drone to the proprtis
                    MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                    DataContext = MyDrone;

                    BSendToCharge.Visibility = Visibility.Visible;
                    BReleaseDrone.Visibility = Visibility.Hidden;
                    BAssignPackage.Visibility = Visibility.Visible;

                    //we set that back to the start posison for the next time.
                    BReleaseDrone.IsEnabled = false;
                    TBhours.Text = "00";
                    TBmin.Text = "00";
                    TBsec.Text = "00";
                    TimeChoose.Visibility = Visibility.Hidden;
                    CBtimeOk.IsChecked = false;

                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  Assign Package to the drone And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAssignPackage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.AssignPackageToDdrone(MyDrone.Id);

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
                switch (result)
                {
                    case MessageBoxResult.OK:
                        DroneListWindow.StatusSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        BSendToCharge.IsEnabled = false;

                        BAssignPackage.Visibility = Visibility.Hidden;
                        BPickedUp.Visibility = Visibility.Visible;
                        GRIDparcelInDelivery.Visibility = Visibility.Visible;
                        TBnotAssigned.Visibility = Visibility.Hidden;
                        break;
                    default:
                        break;
                }
            }   
            catch (NoSuitablePsrcelWasFoundToBelongToTheDrone ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DroneCantBeAssigend ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Picked Up the parcel And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BPickedUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        DroneListWindow.StatusSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        BPickedUp.Visibility = Visibility.Hidden;
                        BDeliveryPackage.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            catch (NonExistentObjectException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnableToCollectParcel ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BDeliveryPackage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        DroneListWindow.StatusSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        BDeliveryPackage.Visibility = Visibility.Hidden;
                        BAssignPackage.Visibility = Visibility.Visible;
                        BSendToCharge.IsEnabled = true;
                        GRIDparcelInDelivery.Visibility = Visibility.Hidden;
                        TBnotAssigned.Visibility = Visibility.Visible;

                        break;
                    default:
                        break;
                }
            }
            catch (DeliveryCannotBeMade ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        ///  prevent the user from type an non number key 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBmodel_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBmodel.Text.Length > 5)
            {
                e.Handled = true;
            }

            if (TBmodel.Text.Length != 0)
            {
                BModalUpdate.IsEnabled = true;
            }
            else
            {
                BModalUpdate.IsEnabled = false;
            }
        }

        #region מטפל בכפתורי זמן בטעינה
        private void TBhours_KeyDown(object sender, KeyEventArgs e)
        {
            //איך אני גורם שזה יהיה בפורמט של זמן
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                e.Handled = e.Key is < Key.NumPad0 or > Key.NumPad9;
            }
            if (TBhours.Text.Length > 1)
            {
                e.Handled = true;
            }
            TBhours.BorderBrush = Brushes.Gray; //בונוס 


        }
        private void TBmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                e.Handled = e.Key is < Key.NumPad0 or > Key.NumPad9;
            }
            if (TBmin.Text.Length > 1)
            {
                e.Handled = true;
            }
            TBmin.BorderBrush = Brushes.Gray; //בונוס 

        }

        private void TBsec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                e.Handled = e.Key is < Key.NumPad0 or > Key.NumPad9;
            }
            if (TBsec.Text.Length > 1)
            {
                e.Handled = true;
            }
            TBsec.BorderBrush = Brushes.Gray; //בונוס 

        }

        private void CBtimeOk_Checked(object sender, RoutedEventArgs e)
        {
            int hours, min, sec;
            int.TryParse(TBhours.Text, out hours);
            int.TryParse(TBmin.Text, out min);
            int.TryParse(TBsec.Text, out sec);


            if (hours > 23 || TBhours.Text == "")
            {
                MessageBox.Show("נא הכנס שעות בין 0-23", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                TBhours.BorderBrush = Brushes.Red; //בונוס 
                CBtimeOk.IsChecked = false;

            }
            else if (min > 59 || TBmin.Text == "")
            {
                MessageBox.Show("נא הכנס דקות בין 0-59", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                TBmin.BorderBrush = Brushes.Red; //בונוס 
                CBtimeOk.IsChecked = false;
            }
            else if (sec > 59 || TBsec.Text == "")
            {
                MessageBox.Show("נא הכנס שניות בין 0-59", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                TBsec.BorderBrush = Brushes.Red; //בונוס 
                CBtimeOk.IsChecked = false;
            }
            else
            {
                BReleaseDrone.IsEnabled = true;
                TBhours.IsReadOnly = true;
                TBmin.IsReadOnly = true;
                TBsec.IsReadOnly = true;
            }
        }
        private void CBtimeOk_Unchecked(object sender, RoutedEventArgs e)
        {
            BReleaseDrone.IsEnabled = false;
            TBhours.IsReadOnly = false;
            TBmin.IsReadOnly = false;
            TBsec.IsReadOnly = false;
        }
        #endregion

        #endregion רחפן בפעולות  
    }
}
