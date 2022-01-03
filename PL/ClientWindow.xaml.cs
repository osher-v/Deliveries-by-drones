using BO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public BlApi.IBL AccessIbl;


        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        public Customer customer;

        public int indexSelected;
        public int customerId;

        public ClientWindow(BlApi.IBL bl,int id)
        {
            InitializeComponent();
            AccessIbl = bl;
            customerId = id;
            customer = AccessIbl.GetCustomer(id);
            DataContext = customer;
            listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;
            listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer;

            // the combobox use it to show the parcel  ID
            CBPickUpList.ItemsSource = AccessIbl.GetParcelList(x => x.CustomerSenderName == customer.Name && x.Status == DeliveryStatus.Assigned);
            CBPickUpList.DisplayMemberPath = "Id";
            
            // the combobox use it to show the parcel  ID
            CBDeliverdList.ItemsSource = AccessIbl.GetParcelList(x => x.CustomerReceiverName == customer.Name && x.Status == DeliveryStatus.PickedUp);
            CBDeliverdList.DisplayMemberPath = "Id";
        }

        private void CBSendToPickUp_Checked(object sender, RoutedEventArgs e)
        {
            if(CBPickUpList.SelectedItem != null)
            {
                int id = ((ParcelToList)CBPickUpList.SelectedItem).Id;
                PickedUp(AccessIbl.GetParcel(id).MyDrone.Id);

            }
            else
            {
                MessageBox.Show(" לא נבחרה חבילה לאיסוף", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false;
            }
        }

        private void CBSendToDeliverd_Checked(object sender, RoutedEventArgs e)
        {
            if (CBDeliverdList.SelectedItem != null)
            {
                int id = ((ParcelToList)CBDeliverdList.SelectedItem).Id;
                DeliveryPackage(AccessIbl.GetParcel(id).MyDrone.Id);
            }
            else
            {
                MessageBox.Show(" לא נבחרה חבילה לאספקה", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToDeliverd.IsChecked = false;
            }
        }

        /// <summary>
        /// Picked Up the parcel And updates the views accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickedUp(int DroneId)
        {
            try
            {
                IsEnabled = false;
                AccessIbl.PickedUpPackageByTheDrone(DroneId);
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        IsEnabled = true;
                        CBSendToPickUp.IsChecked = false;

                        // the combobox use it to show the parcel  ID
                        CBPickUpList.ItemsSource = AccessIbl.GetParcelList(x => x.CustomerSenderName == customer.Name && x.Status == DeliveryStatus.Assigned);
                        CBPickUpList.DisplayMemberPath = "Id";

                        customer = AccessIbl.GetCustomer(customerId);
                        DataContext = customer;
                        listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;
                        break;
                    default:
                        break;
                }
            }
            catch (NonExistentObjectException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                IsEnabled = true;

            }
            catch (UnableToCollectParcel ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                IsEnabled = true;
                CBSendToDeliverd.IsChecked = false;
            }
        }

        /// <summary>
        /// The function handles the delivery of a package to the customer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeliveryPackage(int DroneId)
        {
            try
            {
                IsEnabled = false;
                AccessIbl.DeliveryPackageToTheCustomer(DroneId);
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        IsEnabled = true;
                        CBSendToDeliverd.IsChecked = false;

                        // the combobox use it to show the parcel  ID
                        CBDeliverdList.ItemsSource = AccessIbl.GetParcelList(x => x.CustomerReceiverName == customer.Name && x.Status == DeliveryStatus.PickedUp);
                        CBDeliverdList.DisplayMemberPath = "Id";

                        customer = AccessIbl.GetCustomer(customerId);
                        DataContext = customer;
                        listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer;
                        break;

                    default:
                        break;
                }
            }
            catch (DeliveryCannotBeMade ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                IsEnabled = true;
                CBSendToDeliverd.IsChecked = false;
            }
        }

        private void BRrestComboBox_Click(object sender, RoutedEventArgs e)
        {
            CBPickUpList.SelectedItem = null;
        }

        private void BRrestComboBox2_Click(object sender, RoutedEventArgs e)
        {
            CBDeliverdList.SelectedItem = null;
        }

        private void Bcustomer_Click(object sender, RoutedEventArgs e)
        {

        //object of ListView window.
         ListView ListWindow =new ListView(AccessIbl) ;
         int indexcustomerInObservable = ListWindow.CustomerToLists.IndexOf(ListWindow.CustomerToLists.First(x => x.Id == customerId));
         CustomerToList customers = AccessIbl.GetCustomerList().First(x => x.Id == customerId);
            new CustomerWindow(AccessIbl, ListWindow, customers, indexcustomerInObservable,null,this).Show();
          
        }

        /// <summary>
        /// Update changes from customer window(name).
        /// </summary>
        public void UpdateChangesFromCustomerWindow()
        {
            customer = AccessIbl.GetCustomer(customerId);
            DataContext = customer;
        }
        /// <summary>
        /// opend the window that adding parcels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAddParcel_Click(object sender, RoutedEventArgs e)
        {
            ListView listView = new ListView(AccessIbl);
            new  ParcelWindow(AccessIbl, listView,customer).Show();
        }
    }
}
