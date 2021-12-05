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
            addDrone.Visibility = Visibility.Visible;
            AccessIbl = bl;
            DroneListWindow = _DroneListWindow;
            // the combobox use it to show the Weight Categories
            TBWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            // the combobox use it to show the BaseStation ID
            BaseStationID.ItemsSource = AccessIbl.GetBaseStationList(x => x.FreeChargeSlots > 0);
            BaseStationID.DisplayMemberPath = "Id";
            //if (!AccessIbl.GetBaseStationList(x => x.FreeChargeSlots == 0).Any())
            //{
            //    MessageBox.Show("אין תחנות עם עמדות הטענה פנויות ","מידע", MessageBoxButton.OK, MessageBoxImage.None);              
            //}
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

        public int indexDrone;
        public DroneWindow(IBL.IBL bl, DroneListWindow _DroneListWindow, int id, int _indexDrone)
        {
            InitializeComponent();
            updateDrone.Visibility = Visibility.Visible;
            indexDrone = _indexDrone;
            AccessIbl = bl;

            DroneListWindow = _DroneListWindow;

            MyDrone = bl.GetDrone(id);
            DataContext = MyDrone;
            /*
            TBID2.Text = MyDrone.Id.ToString();
            TBmodel.Text = MyDrone.Model.ToString();
            TBWeightCategories.Text = MyDrone.MaxWeight.ToString();
            TBBatrryStatuses.Text = MyDrone.BatteryStatus.ToString();
            TBDroneStatuses.Text = MyDrone.Statuses.ToString();
            
            */
            TBLocation.Text = MyDrone.CurrentLocation.ToString(); //איך לעדכן ביידינג
            TBparcelInDelivery.Text = MyDrone.Delivery.ToString();

            BModalUpdate.IsEnabled = false;

            //הסוויץ בודק מה ערך הסטטוס של הרחפן ופותח כפתורים
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

        private void BClose1_Click(object sender, RoutedEventArgs e)
        {
            ClosingWindow = false;
            Close();
        }
        
        private void TBmodel_KeyDown_1(object sender, KeyEventArgs e)
        {
            
           if (TBmodel.Text.Length > 5)//הוא נותן 6 ספרות בגלל שהנעילה מתבצעת רק אחרי המעשה
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
            
            //BModalUpdate.Visibility = Visibility.Visible;
            
        }

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

                    //DroneListWindow.droneToLists.Insert(indexDrone, DroneListWindow.droneToLists[indexDrone]);
                    //DroneListWindow.StatusSelectorChanged();

                    break;
                default:
                    break;
            }
        }

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

                        MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        DataContext = MyDrone;

                        //בהמשך יהיו גם שינויים של מיקום ואולי של עוד דברים לכן חייבים משקיף
                        BSendToCharge.Visibility = Visibility.Hidden;
                        BReleaseDrone.Visibility = Visibility.Visible;

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

        private void BReleaseDrone_Click(object sender, RoutedEventArgs e)
        {
            //DateTime time = DateTime.Parse(TBtime.Text,time);
            DateTime time;
            string stringTime = $"{TBhours.Text}:{TBmin.Text}:{TBsec.Text}";
            DateTime.TryParse(stringTime, out time);//לבדוק מה עם paras
            AccessIbl.ReleaseDroneFromCharging(MyDrone.Id, time);

            MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
            switch (result)
            {
                case MessageBoxResult.OK:
                    DroneListWindow.StatusSelectorChanged();
                    //הבטריה לא מתעדכנת ברשימה
                    MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                    DataContext = MyDrone;

                    BSendToCharge.Visibility = Visibility.Visible;
                    BReleaseDrone.Visibility = Visibility.Hidden;

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

            //try
            //{    
            //Console.WriteLine("The operation was successful");
            //}
            //catch (NonExistentObjectException ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //catch (OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging ex)
            //{
            //    //Console.WriteLine(ex);
            //}

        }

        private void Stime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BReleaseDrone.IsEnabled = true;
        }

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
                        TBDroneStatuses.Text = "busy"; //לתקן כמה שיותר מהר לקרוא ליהודהההההההההההההההההההה
                                                       //בהמשך יהיו גם שינויים של מיקום ואולי של עוד דברים לכן חייבים משקיף
                        BSendToCharge.Visibility = Visibility.Visible;
                        BReleaseDrone.Visibility = Visibility.Hidden;

                        BReleaseDrone.IsEnabled = false;

                        TimeChoose.Visibility = Visibility.Hidden;
                        break;
                    default:
                        break;
                }
            }
            //catch (NonExistentObjectException ex)
            //{
            //    Console.WriteLine(ex);
            //}
            catch (NoSuitablePsrcelWasFoundToBelongToTheDrone ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DroneCantBeAssigend ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void TBmodel_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (TBmodel.Text.Length > 5)//הוא נותן 6 ספרות בגלל שהנעילה מתבצעת רק אחרי המעשה
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
            */
        }

        
    }
}
