using BO;
using System;
using System.Collections.Generic;
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
        public BlApi.IBL AccessIbl; ///Access object to the BL class.
                                   
        private ListView listWindow; //object of ListView window.
   
        public bool ClosingWindow { get; private set; } = true; //a bool to help us disable the x bootun.

        #region add situation
        /// <summary>
        /// ctor for add drone option 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_DroneListWindow"></param>
        public DroneWindow(BlApi.IBL bl, ListView _DroneListWindow)
        {
            InitializeComponent();

            //set the Width & Height of window for add option.
            Width = 440;
            Height = 540;

            addDrone.Visibility = Visibility.Visible;
        
            AccessIbl = bl;

            listWindow = _DroneListWindow;

            //the combobox use it to show the Weight Categories.
            TBWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            //the combobox use it to show the BaseStation ID of BaseStation with free charge slots.
            BaseStationID.ItemsSource = AccessIbl.GetBaseStationList(x => x.FreeChargeSlots > 0);
            BaseStationID.DisplayMemberPath = "Id";
        }

        /// <summary>
        /// A function that sends the new drone and adds it to the data after tests in the logical layer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendToBl_Click(object sender, RoutedEventArgs e)
        {
            //Check that all fields are filled.
            if (TBModel.Text.Length != 0 && TBID.Text.Length != 0 && BaseStationID.SelectedItem != null && TBWeight.SelectedItem != null)
            {
                DroneToList newdrone = new DroneToList //Create an object to add to the data.
                {
                    Id = int.Parse(TBID.Text),
                    Model = TBModel.Text,
                    MaxWeight = (WeightCategories)TBWeight.SelectedIndex,
                };

                try // try to add the drone if fals return a MessageBox.
                {
                    AccessIbl.AddDrone(newdrone, ((BaseStationsToList)BaseStationID.SelectedItem).Id); //update the logic layer.
                    MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            newdrone = AccessIbl.GetDroneList().ToList().Find(i => i.Id == newdrone.Id);
                            listWindow.DroneToLists.Add(newdrone); //Updating the observer list of stations.
                            
                            listWindow.IsEnabled = true; //open the "ListWindow" window.

                            ClosingWindow = false; //to enable to close the "BaseStationWindow" window.
                            Close();
                            break;

                        default:
                            break;
                    }
                }
                // Area responsible for error capture
                catch (AddAnExistingObjectException ex) //in case ID of drone is already exists.
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

        #region keybord events
        /// <summary>
        /// disable the non numbers keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBID_KeyDown(object sender, KeyEventArgs e)
        {
            if (TBID.BorderBrush != Brushes.Gray)
            {
                TBID.BorderBrush = Brushes.Gray;
            }

            // take only the kyes we alowed 
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

        /// <summary>
        /// set the color back to gray.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBModel_KeyDown(object sender, KeyEventArgs e)
        { 
            if (TBID.BorderBrush != Brushes.Gray) 
            {
                TBID.BorderBrush = Brushes.Gray;
            }
        }
        #endregion add situation

        #region close
        /// <summary>
        /// cancel the option to clik x to close the window.
        /// </summary>
        /// <param name="e">close window</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = ClosingWindow;
        }

        /// <summary>
        /// The function closes the window.
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

        #endregion
        
        public Drone MyDrone; //hold the drone that we send for update or work on. 
        
        public int indexDrone; //the location index in the observer of the drones in the ListView window.

        ParcelWindow parcelWindow; //Update to the package window.

        #region drone in operations

        /// <summary>
        /// ctor for acction staet And updates the views accordingly
        /// </summary>
        /// <param name="bl">accses to ibl</param>
        /// <param name="_DroneListWindow">the call window</param>
        /// <param name="id">the drone id that chosen</param>
        /// <param name="_indexDrone">/indexe of the drone in the list</param>
        public DroneWindow(BlApi.IBL bl, ListView _DroneListWindow, DroneToList droneTo, int _indexDrone, ParcelWindow _parcelWindow = null)
        {
            InitializeComponent();
           
            updateDrone.Visibility = Visibility.Visible; //open the grid for update action.

            //save the variables that we get in the fanction

            indexDrone = _indexDrone;

            AccessIbl = bl;

            listWindow = _DroneListWindow;

            parcelWindow = _parcelWindow;

            //to conect the binding to set the value of my drone to the proprtis.
            MyDrone = bl.GetDrone(droneTo.Id);
            DataContext = MyDrone;

            //set the color of the battry
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

            //The switch checks the drone's status value and opens buttons.
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
                    if (MyDrone.Delivery.OnTheWayToTheDestination)//if the parcel is Already collected from the sender.
                    {
                        BDeliveryPackage.Visibility = Visibility.Visible;
                    }
                    else //In case the parcel has not yet been collected from the sender.
                    {
                        BPickedUp.Visibility = Visibility.Visible;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// prevent the user from sending empty string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBmodel_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBmodel.Text.Length != 0)
            {
                if (!BModalUpdate.IsEnabled)
                    BModalUpdate.IsEnabled = true;
            }
            else
            {
                BModalUpdate.IsEnabled = false;
            }
        }

        /// <summary>
        /// the fanction update the modal of the drone And updates the views accordingly 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BModalUpdate_Click(object sender, RoutedEventArgs e)
        {
            AccessIbl.UpdateDroneName(MyDrone.Id, TBmodel.Text); //update the logic layer.
            MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.OK:
                    BModalUpdate.IsEnabled = false;
                    listWindow.StatusDroneSelectorChanged(); //update the List of drones.
                    break;
                default:
                    break;
            }
        }

        #region operation functions of the drone evants
        /// <summary>
        /// the fanction send the drone to charge And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.SendingDroneforCharging(MyDrone.Id); //update the logic layer.

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        //update conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        listWindow.StatusDroneSelectorChanged();//update the List of drones.

                        //Update the list observer of BaseStations.
                        int IdOfBaseStation = AccessIbl.GetBaseCharge(MyDrone.Id);
                        int indexOfBaseStationInTheObservable = listWindow.BaseStationToLists.IndexOf(listWindow.BaseStationToLists.First(x => x.Id == IdOfBaseStation));
                        listWindow.BaseStationToLists[indexOfBaseStationInTheObservable] = AccessIbl.GetBaseStationList().First(x => x.Id == IdOfBaseStation);//Update the list observer

                        //display update
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
            int IdOfBaseStation = AccessIbl.GetBaseCharge(MyDrone.Id);//Saving the station tax for the purpose of updating the list of stations

            AccessIbl.ReleaseDroneFromCharging(MyDrone.Id); //no need for try becuse we chose drone from the list and the exption for non exsit item cant be trown 

            MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
            switch (result)
            {
                case MessageBoxResult.OK:
                    listWindow.StatusDroneSelectorChanged();//update the List of drones.

                    //Update the list observer of BaseStations.
                    int indexOfBaseStationInTheObservable = listWindow.BaseStationToLists.IndexOf(listWindow.BaseStationToLists.First(x => x.Id == IdOfBaseStation));
                    listWindow.BaseStationToLists[indexOfBaseStationInTheObservable] = AccessIbl.GetBaseStationList().First(x => x.Id == IdOfBaseStation);

                    //to conecct the binding to set the value of my drone to the proprtis
                    MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                    DataContext = MyDrone;

                    BSendToCharge.Visibility = Visibility.Visible;
                    BReleaseDrone.Visibility = Visibility.Hidden;
                    BAssignPackage.IsEnabled = true;

                    //battery colors.
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
                    else //MyDrone.BatteryStatus > 50
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

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        listWindow.StatusDroneSelectorChanged(); //update the List of drones.

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //Update the list observer of Parcel.
                        int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);

                        // disply update affter the oppertion
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
                AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);//update the logic layer.

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        listWindow.StatusDroneSelectorChanged(); //update the List of drones.

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //Update the list of parcel
                        int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);

                        //Sender update in customer list
                        int indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Sender.Id));
                        listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Sender.Id);

                        //Recipient update in the customer list
                        int indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Receiver.Id));
                        listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Receiver.Id);

                        BPickedUp.Visibility = Visibility.Hidden;
                        BDeliveryPackage.Visibility = Visibility.Visible;

                        if (parcelWindow != null)//update the parcelWindow if a drone window opens through a parcelWindow.
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
                int IdOfDeliveryInMyDrone = MyDrone.Delivery.Id;
                int IdOfSenderCustomerInMyDrone = MyDrone.Delivery.Sender.Id;
                int IdOfReceiverCustomerInMyDrone = MyDrone.Delivery.Receiver.Id;

                AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);//update the logic layer.

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        listWindow.StatusDroneSelectorChanged();//update the List of drones.

                        //to conecct the binding to set the value of my drone to the proprtis
                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //Update the list of packages
                        int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == IdOfDeliveryInMyDrone));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == IdOfDeliveryInMyDrone);

                        //Sender update in customer list
                        int indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == IdOfSenderCustomerInMyDrone));
                        listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == IdOfSenderCustomerInMyDrone);

                        //Recipient update in the customer list
                        int indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == IdOfReceiverCustomerInMyDrone));
                        listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == IdOfReceiverCustomerInMyDrone);

                        //disply update affter the oppertion
                        BDeliveryPackage.Visibility = Visibility.Hidden;
                        BAssignPackage.Visibility = Visibility.Visible;
                        BSendToCharge.Visibility = Visibility.Visible;
                        BSendToCharge.IsEnabled = true;
                        GRIDparcelInDelivery.Visibility = Visibility.Hidden;
                        TBnotAssigned.Visibility = Visibility.Visible;

                        if (parcelWindow != null) //update the parcelWindow if a drone window opens through a parcelWindow.
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
        #endregion operation functions of the drone evants

        /// <summary>
        /// close drone window And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BClose1_Click(object sender, RoutedEventArgs e)
        {
            if (Bsimoltor.Visibility == Visibility.Visible) //In case simulator mode is not turned on.
            {
                listWindow.IsEnabled = true;//allowd to use drone window list again.
                ClosingWindow = false;
                Close();
            }
            else if (BstopSimoltor.Visibility == Visibility.Visible) //In case the simulator mode is active. In this case the window should close only when the process is complete.
            {
                BstopSimoltor.Visibility = Visibility.Hidden;
                DroneSimultor.CancelAsync(); //Closing the process.
                Cursor = Cursors.Wait;
            }
        }
        #endregion drone in operations  

        //////////////////////////////////////////////////////////Simultor//////////////////////////////////////////////
        
        #region Simultor

        internal BackgroundWorker DroneSimultor; //defining the process(Worker).

        /// <summary>
        /// The function creates the process.
        /// </summary>
        private void Simultor()
        {
            DroneSimultor = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            DroneSimultor.DoWork += DroneSimultor_DoWork; //Operation function.
            DroneSimultor.ProgressChanged += DroneSimultor_ProgressChanged; //changed function.
            DroneSimultor.RunWorkerCompleted += DroneSimultor_RunWorkerCompleted; //end of process function.
        }

        /// <summary>
        /// The function reports a change made by the process.
        /// </summary>
        public void ReportProgressInSimultor()
        {
            DroneSimultor.ReportProgress(0);
        }

        /// <summary>
        /// The function returns a Boolean value whether the process should end.
        /// </summary>
        /// <returns></returns>
        public bool IsTimeRun()
        {
            return DroneSimultor.CancellationPending;
        }

        /// <summary>
        /// The function calls the simulator class as soon as the process start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneSimultor_DoWork(object sender, DoWorkEventArgs e)
        {
            AccessIbl.sim(MyDrone.Id, ReportProgressInSimultor, IsTimeRun);
        }

        /// <summary>
        /// The function is activated when the process ends.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneSimultor_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BstopSimoltor.Visibility == Visibility.Visible) //If the process closes because of the stop button.
            {
                Cursor = Cursors.Arrow;

                BstopSimoltor.Visibility = Visibility.Hidden;
                BstopSimoltor.IsEnabled = true;

                Bsimoltor.Visibility = Visibility.Visible;

                TBmodel.IsEnabled = true;
            }
            else //If the process closes because the user clicks a close button on the window.
            {
                listWindow.IsEnabled = true;//allowd to use drone window list again
                ClosingWindow = false;
                Close();
                return;
            }

            //The switch checks the drone's status value and opens buttons/
            switch ((DroneStatuses)MyDrone.Statuses)
            {
                case DroneStatuses.free:
                    BSendToCharge.Visibility = Visibility.Visible;
                    BAssignPackage.Visibility = Visibility.Visible;
                    BSendToCharge.IsEnabled = true;
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
                    if (MyDrone.Delivery.OnTheWayToTheDestination)//if the parcel is Already collected from the sender.
                    {
                        BDeliveryPackage.Visibility = Visibility.Visible;
                        BSendToCharge.Visibility = Visibility.Visible;
                        BSendToCharge.IsEnabled = false;
                    }
                    else //In case the parcel has not yet been collected from the sender.
                    {
                        BPickedUp.Visibility = Visibility.Visible;
                        BSendToCharge.Visibility = Visibility.Visible;
                        BSendToCharge.IsEnabled = false;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The function handles in case the user selects the automatic process button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bsimoltor_Click(object sender, RoutedEventArgs e)
        {
            listWindow.IsEnabled = true;

            Simultor(); //call to function who creates the process.

            DroneSimultor.RunWorkerAsync(); //Run the process.

            //Hiding the other buttons in the background.
            BSendToCharge.Visibility = Visibility.Hidden;
            BReleaseDrone.Visibility = Visibility.Hidden;
            BAssignPackage.Visibility = Visibility.Hidden;
            BPickedUp.Visibility = Visibility.Hidden;
            BDeliveryPackage.Visibility = Visibility.Hidden;

            //Hiding the automatic process button and opening a manually process button.
            Bsimoltor.Visibility = Visibility.Hidden;
            BstopSimoltor.Visibility = Visibility.Visible;

            TBmodel.IsEnabled = false; //to prevent modal changing
        }

        //int to help us save the ID.
        private int IdOfDeliveryInMyDrone;
        private int IdOfSenderCustomerInMyDrone;
        private int IdOfReceiverCustomerInMyDrone;

        /// <summary>
        /// The function handles the display when changes made in the process are received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneSimultor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //to update conect the binding to set the value of my drone to the proprtis.
            MyDrone = AccessIbl.GetDrone(MyDrone.Id);
            DataContext = MyDrone;

            listWindow.StatusDroneSelectorChanged(); //update the List of drones.

            // to find the index when the fanc need to find in the observer collaction and update.
            int indexOfParcelInTheObservable;
            int indexOfSenderCustomerInTheObservable;
            int indexOfReceiverCustomerInTheObservable;

            //switch betwen drone status and according to that update the display.
            switch (MyDrone.Statuses)
            {
                case DroneStatuses.free:
                    if (GRIDparcelInDelivery.Visibility == Visibility.Visible) //the drone is free cuse he just done (we know that becuse the grid is opend) it is affter deliverd.
                    {
                        //update the parcels list
                        indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == IdOfDeliveryInMyDrone));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == IdOfDeliveryInMyDrone);

                        //update spasice customer in the Customer list (sender)
                        indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == IdOfSenderCustomerInMyDrone));
                        listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == IdOfSenderCustomerInMyDrone);

                        //update the reciver
                        indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == IdOfReceiverCustomerInMyDrone));
                        listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == IdOfReceiverCustomerInMyDrone);

                        //display changes for thois stage
                        GRIDparcelInDelivery.Visibility = Visibility.Hidden;
                        TBnotAssigned.Visibility = Visibility.Visible;
                    }
                    else //the drone is in a free state that has come out of charge and not like before (not affter deliver).
                    {
                        //Update the list observer of BaseStations.
                        listWindow.BaseStationToLists.Clear();
                        List<BO.BaseStationsToList> baseStations1 = AccessIbl.GetBaseStationList().ToList();
                        foreach (var item in baseStations1)
                        {
                            listWindow.BaseStationToLists.Add(item);
                        }
                    }

                    break;

                case DroneStatuses.inMaintenance:
                    //Update the list observer of BaseStations.
                    listWindow.BaseStationToLists.Clear();
                    List<BO.BaseStationsToList> baseStations = AccessIbl.GetBaseStationList().ToList();
                    foreach (var item in baseStations)
                    {
                        listWindow.BaseStationToLists.Add(item);
                    }
                    break;

                case DroneStatuses.busy:
                    IdOfDeliveryInMyDrone = MyDrone.Delivery.Id;
                    IdOfSenderCustomerInMyDrone = MyDrone.Delivery.Sender.Id;
                    IdOfReceiverCustomerInMyDrone = MyDrone.Delivery.Receiver.Id;

                    if (AccessIbl.GetParcel(MyDrone.Delivery.Id).PickedUp == null)
                    {
                        IdOfDeliveryInMyDrone = MyDrone.Delivery.Id;
                        IdOfSenderCustomerInMyDrone = MyDrone.Delivery.Sender.Id;
                        IdOfReceiverCustomerInMyDrone = MyDrone.Delivery.Receiver.Id;

                        //update list of parcels
                        indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);

                        GRIDparcelInDelivery.Visibility = Visibility.Visible;
                        TBnotAssigned.Visibility = Visibility.Hidden;
                    }
                    else if (AccessIbl.GetParcel(MyDrone.Delivery.Id).Delivered == null)
                    {
                        //update the parcels list
                        indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);
                        //update spasice customer in the Customer list (sender)
                        indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Sender.Id));
                        listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Sender.Id);
                        //update the reciver
                        indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Receiver.Id));
                        listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Receiver.Id);
                    }
                    break;

                default:
                    break;
            }

            //battery colors.
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
            else //MyDrone.BatteryStatus > 50
            {
                PBbatr.Foreground = Brushes.LimeGreen;

            }
        }

        /// <summary>
        /// stop the simoltor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BstopSimoltor_Click(object sender, RoutedEventArgs e)
        {
            DroneSimultor.CancelAsync(); //close the prosses.
            BstopSimoltor.IsEnabled = false;
            Cursor = Cursors.Wait; //change the mouse to "Wait".
        }
        #endregion
    }
}
