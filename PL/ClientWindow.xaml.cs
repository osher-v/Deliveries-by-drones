using BO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ClientWindow.xaml.
    /// This window covers a client interface.
    /// </summary>
    public partial class ClientWindow : Window
    {
        public BlApi.IBL AccessIbl; //Access object to the BL class.

        public bool ClosingWindow { get; private set; } = true; //bool object to help us disable the x button. 

        public Customer customer;

        //public int indexSelected;
        //public int customerId;

        /// <summary>
        /// ctor of ClientWindow.
        /// </summary>
        /// <param name="bl">Access object to the BL class</param>
        /// <param name="id">Id of Client</param>
        public ClientWindow(BlApi.IBL bl, int id)
        {
            InitializeComponent();

            AccessIbl = bl;

            //customerId = id;

            //to the Biding of client details.
            customer = AccessIbl.GetCustomer(id);
            DataContext = customer;

            listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;//Connecting the listview to the list of parcels who sent by the client.
            listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer;//Connecting the listview to the list of parcels that the client should receive.

            //Connecting the the combobox to parcels who sent by the client, and show the parcel ID.
            CBPickUpList.ItemsSource = AccessIbl.GetParcelList(x => x.Status == DeliveryStatus.Assigned &&
                      AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer.ToList().Exists(item => item.Id == x.Id));
            CBPickUpList.DisplayMemberPath = "Id";

            //Connecting the the combobox to parcels that the client should receive, and show the parcel ID.
            CBDeliverdList.ItemsSource = AccessIbl.GetParcelList(x => x.Status == DeliveryStatus.PickedUp &&
                      AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer.ToList().Exists(item => item.Id == x.Id));
            CBDeliverdList.DisplayMemberPath = "Id";

            //Connecting the the combobox to parcels who sent by the client parcels that have not yet been associated and show the parcel ID. 
            CBdeleteList.ItemsSource = AccessIbl.GetParcelList(x => x.Status == DeliveryStatus.created &&
                      AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer.ToList().Exists(item => item.Id == x.Id));
            CBdeleteList.DisplayMemberPath = "Id";
        }


        #region PickUp combobox

        /// <summary>
        /// The function handles in case a package is selected for collection confirmation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBSendToPickUp_Checked(object sender, RoutedEventArgs e)
        {
            if (CBPickUpList.SelectedItem != null)//check if a package has been selected in combobox.
            {
                //int id = ((ParcelToList)CBPickUpList.SelectedItem).Id;
                PickedUp(AccessIbl.GetParcel(((ParcelToList)CBPickUpList.SelectedItem).Id).MyDrone.Id);
            }
            else
            {
                MessageBox.Show(" לא נבחרה חבילה לאיסוף", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// picked up the parcel and updates the views accordingly.
        /// </summary>
        /// <param name="DroneId">drone Id</param>
        private void PickedUp(int DroneId)
        {
            try
            {
                //IsEnabled = false;
                AccessIbl.PickedUpPackageByTheDrone(DroneId); //Activation of the PickedUp function in the BL layer.
                MessageBoxResult result = MessageBox.Show("הפעולה הצליחה", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        //IsEnabled = true;
                        CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.

                        //Update the combobox of parcels who sent by the client and have not yet been pickup.
                        CBPickUpList.ItemsSource = AccessIbl.GetParcelList(x => x.Status == DeliveryStatus.Assigned &&
                                AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer.ToList().Exists(item => item.Id == x.Id));
                        CBPickUpList.DisplayMemberPath = "Id";

                        //Update the Biding of client details.
                        customer = AccessIbl.GetCustomer(customer.Id);
                        DataContext = customer;

                        //Update the list of parcels from the customer.
                        listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;
                        break;

                    default:
                        break;
                }
            }
            catch (NonExistentObjectException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.
            }
            catch (UnableToCollectParcel ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// Reset the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRrestComboBox_Click(object sender, RoutedEventArgs e)
        {
            CBPickUpList.SelectedItem = null;
        }
        #endregion PickUp combobox

        #region Deliverd combobox

        /// <summary>
        /// The function handles in case a package is selected for deliverd confirmation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBSendToDeliverd_Checked(object sender, RoutedEventArgs e)
        {
            if (CBDeliverdList.SelectedItem != null)//check if a package has been selected in combobox.
            {
                //int id = ((ParcelToList)CBDeliverdList.SelectedItem).Id;
                DeliveryPackage(AccessIbl.GetParcel(((ParcelToList)CBDeliverdList.SelectedItem).Id).MyDrone.Id);
            }
            else
            {
                MessageBox.Show(" לא נבחרה חבילה לאספקה", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToDeliverd.IsChecked = false;//update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// deliverd the parcel and updates the views accordingly.
        /// </summary>
        /// <param name="DroneId"></param>
        private void DeliveryPackage(int DroneId)
        {
            try
            {
                //IsEnabled = false;
                AccessIbl.DeliveryPackageToTheCustomer(DroneId);//Activation of the Delivery function in the BL layer.
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        //IsEnabled = true;
                        CBSendToDeliverd.IsChecked = false;//update the CheckBox to uncheck.

                        //Update the combobox of parcels that the client should receive and have not yet been Delivered.
                        CBDeliverdList.ItemsSource = AccessIbl.GetParcelList(x => x.Status == DeliveryStatus.PickedUp &&
                                 AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer.ToList().Exists(item => item.Id == x.Id));
                        CBDeliverdList.DisplayMemberPath = "Id";

                        //Update the Biding of client details.
                        customer = AccessIbl.GetCustomer(customer.Id);
                        DataContext = customer;

                        //Update the list of parcels to the customer.
                        listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer;
                        break;

                    default:
                        break;
                }
            }
            catch (DeliveryCannotBeMade ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                //IsEnabled = true;
                CBSendToDeliverd.IsChecked = false;
            }
        }

        /// <summary>
        /// Reset the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRrestComboBox2_Click(object sender, RoutedEventArgs e)
        {
            CBDeliverdList.SelectedItem = null;
        }
        #endregion Deliverd combobox

        #region Delete combobox

        /// <summary>
        /// The function handles in case a package is selected for delete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBdeleteParcel_Checked(object sender, RoutedEventArgs e)
        {
            if (CBdeleteList.SelectedItem != null)//check if a package has been selected in combobox.
            {
                MessageBoxResult result = MessageBox.Show("האם אתה בטוח שאתה רוצה לבצע מחיקה", "מצב מחיקה", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        deleteParcel(((ParcelToList)CBdeleteList.SelectedItem).Id);
                        break;
                    case MessageBoxResult.No: //in case that the user dont want to delete he have the option to abort withot any change.
                        CBdeleteParcel.IsChecked = false; //update the CheckBox to uncheck.
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show(" לא נבחרה חבילה למחיקה", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                CBdeleteParcel.IsChecked = false; //update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// delete the parcel and updates the views accordingly.
        /// </summary>
        /// <param name="Id"></param>
        private void deleteParcel(int Id)
        {
            //IsEnabled = false;
            AccessIbl.RemoveParcel(Id);
            MessageBoxResult result = MessageBox.Show("הפעולה הצליחה", "מידע", MessageBoxButton.OK, MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.OK:
                    //IsEnabled = true;
                    CBdeleteParcel.IsChecked = false; //update the CheckBox to uncheck.

                    //Update the combobox of parcels who have not yet been associated.
                    CBdeleteList.ItemsSource = AccessIbl.GetParcelList(x => x.Status == DeliveryStatus.created &&
                       AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer.ToList().Exists(item => item.Id == x.Id));
                    CBdeleteList.DisplayMemberPath = "Id";

                    //Update the list of parcels from the customer.
                    listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Reset the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRrestComboBoxDelete_Click(object sender, RoutedEventArgs e)
        {
            CBdeleteList.SelectedItem = null;
        }
        #endregion Delete combobox


        /// <summary>
        /// opend a customer update window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bcustomer_Click(object sender, RoutedEventArgs e)
        {
            ListView ListWindow = new ListView(AccessIbl); //object of ListView window.
            int indexcustomerInObservable = ListWindow.CustomerToLists.IndexOf(ListWindow.CustomerToLists.First(x => x.Id == customer.Id));
            CustomerToList customers = AccessIbl.GetCustomerList().First(x => x.Id == customer.Id);
            new CustomerWindow(AccessIbl, ListWindow, customers, indexcustomerInObservable, null, this).Show();
        }

        /// <summary>
        /// opend the window that adding parcels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAddParcel_Click(object sender, RoutedEventArgs e)
        {
            ListView listView = new ListView(AccessIbl);
            new ParcelWindow(AccessIbl, listView, customer, this).Show();
        }

        /// <summary>
        /// Update changes from Parcel window.
        /// </summary>
        public void UpdateChangesFromParcelWindow()
        {
            //Update the Biding of client details.
            customer = AccessIbl.GetCustomer(customer.Id);
            DataContext = customer;

            listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer; //update the listview to the list of parcels who sent by the client. 
            listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer; //update the listview to the list of parcels that the client should receive.
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
        private void closeUpdate_Click(object sender, RoutedEventArgs e)
        {
            ClosingWindow = false; // we alowd the close option
            Close();
        }
        #endregion close  
    }
}
