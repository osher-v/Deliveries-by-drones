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
    /// Interaction logic for ParcelWindow.xaml.
    /// This window covers a parcel interface.
    /// </summary>
    public partial class ParcelWindow : Window
    {
        public BlApi.IBL AccessIbl; //Access object to the BL class.

        public ListView ListWindow; //object of ListView window.

        public ClientWindow clientWindow;//for using if we enter from customer window
    
        public bool ClosingWindow { get; private set; } = true; //a bool to help us disable the x bootum.

        #region add situation
        /// <summary>
        /// Add ctor.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_ListWindow"></param>
        public ParcelWindow(BlApi.IBL bl, ListView _ListWindow, Customer customerFromClientWindow = null, ClientWindow _clientWindow = null)
        {
            InitializeComponent();

            Width = 440;

            addParcel.Visibility = Visibility.Visible; //open the Grid of add action.

            AccessIbl = bl;

            ListWindow = _ListWindow;

            clientWindow = _clientWindow;

            if (clientWindow != null) //in case "ParcelWindow" opened from client Window.
            {
                TBParcelSenderId.Text = customerFromClientWindow.Id.ToString(); //Placement in the field of sender ID.
                TBParcelSenderId.IsEnabled = false;
            }

            CBWeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories)); //the combobox use it to show the Weight Categories.
            CBPriorSelector.ItemsSource = Enum.GetValues(typeof(Priorities)); //the combobox use it to show the Priorities Categories
        }

        /// <summary>
        /// disable the non numbers keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBParcelSenderId_KeyDown(object sender, KeyEventArgs e)
        {
            TBParcelSenderId.BorderBrush = Brushes.Gray;
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
        /// disable the non numbers keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBParcelReciverId_KeyDown(object sender, KeyEventArgs e)
        {
            TBParcelReciverId.BorderBrush = Brushes.Gray;
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
        /// The function handles adding a parcel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAddParcel_Click(object sender, RoutedEventArgs e)
        {
            //Check that all fields are filled.
            if (TBParcelSenderId.Text.Length != 0 && TBParcelReciverId.Text.Length != 0 && CBPriorSelector.SelectedItem != null && CBWeightSelctor.SelectedItem != null)
            {           
                BO.Parcel parcelAdd = new Parcel() //Create an object to add to the data.
                {
                    Sender = new CustomerInDelivery() { Id = int.Parse(TBParcelSenderId.Text) },
                    Receiver = new CustomerInDelivery() { Id = int.Parse(TBParcelReciverId.Text) },
                    Prior = (Priorities)CBPriorSelector.SelectedItem,
                    Weight = (WeightCategories)CBWeightSelctor.SelectedItem
                };

                try
                {
                    int IdOfParcel = AccessIbl.AddParcel(parcelAdd); //update the logic layer.
                    MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            BO.ParcelToList parcelsToList = AccessIbl.GetParcelList().ToList().Find(i => i.Id == IdOfParcel);
                            ListWindow.ParcelToLists.Add(parcelsToList); //Updating the observer list of parcels.

                            if (clientWindow != null)//in case "ParcelWindow" opened from client Window.
                            {
                                clientWindow.UpdateChangesFromParcelWindow(); //update clientWindow.
                            }

                            ListWindow.IsEnabled = true; //open the "ListWindow" window.

                            ClosingWindow = false; //to enable to close the "BaseStationWindow" window.
                            Close();
                            break;
                        default:
                            break;
                    }
                }
                catch (NonExistentObjectException ex) //The problem is with the ID number field.
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    switch (ex.Message)
                    {
                        case "Erorr: is no Customer Sender id":
                            TBParcelSenderId.Text = "";
                            TBParcelSenderId.BorderBrush = Brushes.Red;
                            break;
                        case "Erorr: is no Customer Receiver id":
                            TBParcelReciverId.Text = "";
                            TBParcelReciverId.BorderBrush = Brushes.Red;
                            break;
                        default: 
                            break;
                    }         
                }
            }
            else //If not all fields are filled.
            {
                MessageBox.Show("נא ודאו שכל השדות מלאים", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion add situation

        public Parcel parcel;

        public ClientWindow Client;

        public int indexSelected; //the location index in the observer of the parcels in the ListView window.

        #region update situation
        /// <summary>
        /// update ctor.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_ListWindow"></param>
        /// <param name="parcelTo"></param>
        /// <param name="_indexParcel"></param>
        public ParcelWindow(BlApi.IBL bl, ListView _ListWindow, ParcelToList parcelTo, int _indexParcel, ClientWindow _client = null)
        {
            InitializeComponent();

            updateParcel.Visibility = Visibility.Visible; //open the Grid of update action.

            AccessIbl = bl;

            ListWindow = _ListWindow;

            indexSelected = _indexParcel;

            Client = _client;

            //Connecting customer data.
            parcel = AccessIbl.GetParcel(parcelTo.Id);
            DataContext = parcel;

            if (Client == null)//in case "ParcelWindow" NOT opened from client Window.
            {
                Breciver.Visibility = Visibility.Visible;
                Bsender.Visibility = Visibility.Visible;
            }

            //If the parcel is not associated then it will be possible to delete it.
            if (parcelTo.Status == DeliveryStatus.created && Client == null)
            {
                BDelete.Visibility = Visibility.Visible; //we can only delete if the package is not associated.
            }

            //If the parcel has already been associated then it will be possible to open drone details.
            if ((parcelTo.Status == DeliveryStatus.Assigned || parcelTo.Status == DeliveryStatus.PickedUp) && Client == null)
            {
                Bdrone.Visibility = Visibility.Visible;         
            };
        }

        /// <summary>
        /// The function deletes a parcel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("האם אתה בטוח שאתה רוצה לבצע מחיקה", "מצב מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    AccessIbl.RemoveParcel(parcel.Id); //update the logic layer.

                    ListWindow.ParcelToLists.RemoveAt(indexSelected); //Update the observer list of parcels.

                    ListWindow.IsEnabled = true; //open the "ListWindow" window.

                    ClosingWindow = false; //allowd to close the window 
                    Close();

                    break;
                case MessageBoxResult.No: //in case that the user dont want to delete he have the option to abort withot any change. 
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// open window for more informiton about the sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bsender_Click(object sender, RoutedEventArgs e)
        {
            int IdOfcustomer = parcel.Sender.Id;
            int indexcustomerInObservable = ListWindow.CustomerToLists.IndexOf(ListWindow.CustomerToLists.First(x => x.Id == IdOfcustomer));
            CustomerToList customer = AccessIbl.GetCustomerList().First(x => x.Id == IdOfcustomer);
            new CustomerWindow(AccessIbl, ListWindow, customer, indexcustomerInObservable, this).ShowDialog();
        }

        /// <summary>
        /// open window for more informiton about the reciver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Breciver_Click(object sender, RoutedEventArgs e)
        {
            int IdOfcustomer = parcel.Receiver.Id;
            int indexcustomerInObservable = ListWindow.CustomerToLists.IndexOf(ListWindow.CustomerToLists.First(x => x.Id == IdOfcustomer));
            CustomerToList customer = AccessIbl.GetCustomerList().First(x => x.Id == IdOfcustomer);
            new CustomerWindow(AccessIbl, ListWindow, customer, indexcustomerInObservable, this).ShowDialog();  
        }

        /// <summary>
        /// open window for more informiton about the linked drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bdrone_Click(object sender, RoutedEventArgs e)
        {
            int IdOfDrone = parcel.MyDrone.Id;
            int indexDroneInObservable = ListWindow.DroneToLists.IndexOf(ListWindow.DroneToLists.First(x => x.Id == IdOfDrone));
            DroneToList drone = AccessIbl.GetDroneList().First(x => x.Id == IdOfDrone);
            new DroneWindow(AccessIbl, ListWindow, drone, indexDroneInObservable, this).ShowDialog();
        }

        /// <summary>
        /// Update changes from customer window.
        /// </summary>
        public void UpdateChangesFromCustomerWindow()
        {
            //update connecting customer data.
            parcel = AccessIbl.GetParcel(parcel.Id);
            DataContext = parcel;
        }

        /// <summary>
        /// Update changes from drone window.
        /// </summary>
        public void UpdateChangesFromDroneWindow()
        {
            //update connecting customer data.
            parcel = AccessIbl.GetParcel(parcel.Id);
            DataContext = parcel;

            if(parcel.Delivered != null) //In case the parcl was provided hidden the operation skimmer details.
            {
                Bdrone.Visibility = Visibility.Hidden;
            }
        }
        #endregion update situation

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
        /// The function closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BcloseAdd_Click(object sender, RoutedEventArgs e) //חריגהההההההההההההה
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
        private void BcloseUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListWindow.IsEnabled = true;
            ClosingWindow = false; // we alowd the close option
            Close();
        }
        #endregion close  
    }
}
