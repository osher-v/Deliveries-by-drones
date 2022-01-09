using BO;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml.
    /// This window covers a drone interface.
    /// </summary>
    public partial class DroneWindow : Window
    {
        ///Access object to the BL class.
        public BlApi.IBL AccessIbl;
        /// <summary> the calling window, becuse we want to use it here </summary> 
        private ListView listWindow;
        /// <summary> a bool to help us disable the x bootun  </summary>
        public bool ClosingWindow { get; private set; } = true;

        #region drone to add
        /// <summary>
        /// consractor for add drone option 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_DroneListWindow"></param>
        public DroneWindow(BlApi.IBL bl, ListView _DroneListWindow)
        {
            InitializeComponent();
            //set the Width & Height for this window
            Width = 440;
            Height = 540;
            addDrone.Visibility = Visibility.Visible;
            // set the bl obeject and the drone list window that we got from the priviose window
            AccessIbl = bl;
            listWindow = _DroneListWindow;
            // the combobox use it to show the Weight Categories
            TBWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            // the combobox use it to show the BaseStation ID
            BaseStationID.ItemsSource = AccessIbl.GetBaseStationList(x => x.FreeChargeSlots > 0);
            BaseStationID.DisplayMemberPath = "Id";
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
                            listWindow.DroneToLists.Add(newdrone); //Observer update
                            //Handles required window updates
                            listWindow.IsEnabled = true;
                            ClosingWindow = false;
                            Close();
                            break;
                        default:
                            break;
                    }
                }
                // Area responsible for error capture
                catch (AddAnExistingObjectException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    TBID.Text = "";
                    TBID.BorderBrush = Brushes.Red; //bonus 
                }
                catch (NonExistentObjectException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    TBID.Text = "";
                    TBID.BorderBrush = Brushes.Red;//bonus 
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
        }

        /// <summary>
        /// set the color back to gray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBModel_KeyDown(object sender, KeyEventArgs e)
        {
            TBID.BorderBrush = Brushes.Gray;
        }
        #endregion

        #region drone in operations
        //hold the drone that we send for update or work on 
        public Drone MyDrone;
        //indexe of the drone how chosse by doubly click
        public int indexDrone;
        //Update to the package window
        ParcelWindow parcelWindow;

        /// <summary>
        /// constractor for acction staet  And updates the views accordingly
        /// </summary>
        /// <param name="bl">accses to ibl</param>
        /// <param name="_DroneListWindow">the call window</param>
        /// <param name="id">the drone id that chosen</param>
        /// <param name="_indexDrone">/indexe of the drone in the list</param>
        public DroneWindow(BlApi.IBL bl, ListView _DroneListWindow, DroneToList droneTo, int _indexDrone, ParcelWindow _parcelWindow = null)
        {
            InitializeComponent();
            // open the grid for the user
            updateDrone.Visibility = Visibility.Visible;

            indexDrone = _indexDrone;
            AccessIbl = bl;
            listWindow = _DroneListWindow;

            //to conect the binding to set the value of my drone to the proprtis
            MyDrone = bl.GetDrone(droneTo.Id);
            DataContext = MyDrone;

            if (MyDrone.BatteryStatus < 50)
            {
                if (MyDrone.BatteryStatus > 10)
                {
                    PBbatr.Foreground = Brushes.YellowGreen;
                }
                else
                {
                    PBbatr.Foreground = Brushes.Red;
                }
            }

            parcelWindow = _parcelWindow;

            //The switch checks the drone's status value and opens buttons 
            switch ((DroneStatuses)MyDrone.Statuses)
            {
                case DroneStatuses.free:
                    BSendToCharge.Visibility = Visibility.Visible;
                    BAssignPackage.Visibility = Visibility.Visible;
                    break;

                case DroneStatuses.inMaintenance:
                    BReleaseDrone.Visibility = Visibility.Visible;
                    BAssignPackage.Visibility = Visibility.Visible;
                    BAssignPackage.IsEnabled = false;
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
            listWindow.IsEnabled = true;//allowd to use drone window list again
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
                    listWindow.StatusDroneSelectorChanged();
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
                        listWindow.StatusDroneSelectorChanged(); //עקיפת המשקיף/עדכון הרשימה

                        ////עדכון משקיף הרשימ
                        int IdOfBaseStation = AccessIbl.GetBaseCharge(MyDrone.Id);
                        int indexOfBaseStationInTheObservable = listWindow.BaseStationToLists.IndexOf(listWindow.BaseStationToLists.First(x => x.Id == IdOfBaseStation));
                        listWindow.BaseStationToLists[indexOfBaseStationInTheObservable] = AccessIbl.GetBaseStationList().First(x => x.Id == IdOfBaseStation);//עדכון משקיף הרשימות

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        BSendToCharge.Visibility = Visibility.Hidden;
                        BReleaseDrone.Visibility = Visibility.Visible;
                        BAssignPackage.IsEnabled = false;
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
            int IdOfBaseStation = AccessIbl.GetBaseCharge(MyDrone.Id);//שמירת מס התחנה לצורך עדכון רשימת התחנות בשורה 350

            AccessIbl.ReleaseDroneFromCharging(MyDrone.Id);

            MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
            switch (result)
            {
                case MessageBoxResult.OK:
                    listWindow.StatusDroneSelectorChanged();

                    ////עדכון משקיף הרשימ
                    int indexOfBaseStationInTheObservable = listWindow.BaseStationToLists.IndexOf(listWindow.BaseStationToLists.First(x => x.Id == IdOfBaseStation));
                    listWindow.BaseStationToLists[indexOfBaseStationInTheObservable] = AccessIbl.GetBaseStationList().First(x => x.Id == IdOfBaseStation);//עדכון משקיף הרשימות

                    //to conecct the binding to set the value of my drone to the proprtis
                    MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                    DataContext = MyDrone;

                    BSendToCharge.Visibility = Visibility.Visible;
                    BReleaseDrone.Visibility = Visibility.Hidden;
                    BAssignPackage.IsEnabled = true;

                    if (MyDrone.BatteryStatus < 50)
                    {
                        if (MyDrone.BatteryStatus > 10)
                        {
                            PBbatr.Foreground = Brushes.YellowGreen;
                        }
                        else
                        {
                            PBbatr.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        PBbatr.Foreground = Brushes.LimeGreen;

                    }
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
                        listWindow.StatusDroneSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //עדכון רשימת החבילות
                        int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);

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
                        listWindow.StatusDroneSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //עדכון רשימת החבילות
                        int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);

                        //עדכון השולח ברשימת הלקוחות
                        int indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Sender.Id));
                        listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Sender.Id);

                        //עדכון המקבל ברשימת הלקוחות
                        int indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Receiver.Id));
                        listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Receiver.Id);

                        BPickedUp.Visibility = Visibility.Hidden;
                        BDeliveryPackage.Visibility = Visibility.Visible;

                        if (parcelWindow != null)//עדכון שינוי מיקום וסוללת הרחפן אם נפתח חלון רחפן דרך חבילה
                        {
                            parcelWindow.UpdateChangesFromDroneWindow();
                        }
                        //set the battry color 
                        if (MyDrone.BatteryStatus < 50)
                        {
                            if (MyDrone.BatteryStatus > 10)
                            {
                                PBbatr.Foreground = Brushes.YellowGreen;
                            }
                            else
                            {
                                PBbatr.Foreground = Brushes.Red;
                            }
                        }
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

        /// <summary>
        /// The function handles the delivery of a package to the customer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BDeliveryPackage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //חייבים לעשות את זה כאן כי כאשר מופעל אספקה זה משחרר את ה Delivery
                int IdOfDeliveryInMyDrone = MyDrone.Delivery.Id;
                int IdOfSenderCustomerInMyDrone = MyDrone.Delivery.Sender.Id;
                int IdOfReceiverCustomerInMyDrone = MyDrone.Delivery.Receiver.Id;

                AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        listWindow.StatusDroneSelectorChanged();

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //עדכון רשימת החבילות
                        int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == IdOfDeliveryInMyDrone));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == IdOfDeliveryInMyDrone);

                        //עדכון השולח ברשימת הלקוחות
                        int indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == IdOfSenderCustomerInMyDrone));
                        listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == IdOfSenderCustomerInMyDrone);

                        //עדכון המקבל ברשימת הלקוחות
                        int indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == IdOfReceiverCustomerInMyDrone));
                        listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == IdOfReceiverCustomerInMyDrone);

                        BDeliveryPackage.Visibility = Visibility.Hidden;
                        BAssignPackage.Visibility = Visibility.Visible;
                        BSendToCharge.IsEnabled = true;
                        GRIDparcelInDelivery.Visibility = Visibility.Hidden;
                        TBnotAssigned.Visibility = Visibility.Visible;

                        if (parcelWindow != null)//עדכון שינוי מיקום וסוללת הרחפן אם נפתח חלון רחפן דרך חבילה
                        {
                            parcelWindow.UpdateChangesFromDroneWindow();
                            ClosingWindow = false;
                            Close();
                        }
                        //set the battry color 
                        if (MyDrone.BatteryStatus < 50)
                        {
                            if (MyDrone.BatteryStatus > 10)
                            {
                                PBbatr.Foreground = Brushes.YellowGreen;
                            }
                            else
                            {
                                PBbatr.Foreground = Brushes.Red;
                            }
                        }
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

            if (TBmodel.Text.Length != 0)
            {
                BModalUpdate.IsEnabled = true;
            }
            else
            {
                BModalUpdate.IsEnabled = false;
            }
        }

        /// <summary>
        /// this event is checking tje limit for the contexet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBmodel_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (TBmodel.Text.Length > 5)
            {
                e.Handled = true;
            }
        }

        #endregion drone in operations  

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
        private void Bclose_Click(object sender, RoutedEventArgs e)
        {
            listWindow.IsEnabled = true;
            ClosingWindow = false;
            Close();
        }
        #endregion close

        public BackgroundWorker DroneSimultor;

        bool isTimeRun;
        public void Simultor()
        {
            DroneSimultor = new BackgroundWorker();
            DroneSimultor.DoWork += DroneSimultor_DoWork;
            DroneSimultor.ProgressChanged += DroneSimultor_ProgressChanged;

            DroneSimultor.WorkerReportsProgress = true;
        }

        private void Bsimoltor_Click(object sender, RoutedEventArgs e)
        {
            isTimeRun = true;
            Simultor();
            //DroneSimultor = new BackgroundWorker();
            DroneSimultor.RunWorkerAsync();
        }

        private void DroneSimultor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //to conect the binding to set the value of my drone to the proprtis
            MyDrone = AccessIbl.GetDrone(MyDrone.Id);
            DataContext = MyDrone;
        }

        private void DroneSimultor_DoWork(object sender, DoWorkEventArgs e)
        {
            while(isTimeRun)
            {
                switch (MyDrone.Statuses)
                {
                    case DroneStatuses.free:
                        try
                        {
                            AccessIbl.AssignPackageToDdrone(MyDrone.Id);

                        }
                        catch
                        {
                            AccessIbl.SendingDroneforCharging(MyDrone.Id);
                        }
                        break;
                    case DroneStatuses.inMaintenance:
                        AccessIbl.ReleaseDroneFromCharging(MyDrone.Id);
                        break;
                    case DroneStatuses.busy:
                        if (AccessIbl.GetParcel(MyDrone.Delivery.Id).PickedUp == null)
                        {   
                            AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);
                        }
                        else
                        {
                            AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);
                        }
                        break;
                    default:
                        break;
                }
                DroneSimultor.ReportProgress(1);
                Thread.Sleep(5000);
            }
        }

        private void BstopSimoltor_Click(object sender, RoutedEventArgs e)
        {
            //while(MyDrone.Statuses!=DroneStatuses.free)
            //{ }
            isTimeRun = false;
        }

        //public void Help()
        //{
        //    DroneSimultor.ReportProgress(0);
        //}

        //public bool Help2()
        //{
        //    return DroneSimultor.CancellationPending;
        //}

        //private void Bsimoltor_Click(object sender, RoutedEventArgs e)
        //{
        //    DroneSimultor = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };

        //    DroneSimultor.DoWork += (sender, e) => AccessIbl.sim(MyDrone.Id, Help, Help2);

        //}


    }
}
