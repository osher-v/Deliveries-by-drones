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

        //object of ListView window.
        public ListView ListWindow;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        public Customer customer;

        public int indexSelected;
        public ClientWindow(BlApi.IBL bl,int id)
        {
            InitializeComponent();
            AccessIbl = bl;

            customer = AccessIbl.GetCustomer(id);
            DataContext = customer;

            listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;
            listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer;


            // the combobox use it to show the BaseStation ID
            CBPickUpList.ItemsSource = AccessIbl.GetParcelList(x => x.CustomerSenderName == customer.Name && x.Status == DeliveryStatus.Assigned);
            CBPickUpList.DisplayMemberPath = "Id";

            // the combobox use it to show the BaseStation ID
            CBDeliverdList.ItemsSource = AccessIbl.GetParcelList(x => x.CustomerReceiverName == customer.Name && x.Status == DeliveryStatus.PickedUp);
            CBDeliverdList.DisplayMemberPath = "Id";
        }

        private void CBSendToPickUp_Checked(object sender, RoutedEventArgs e)
        {
            if(CBPickUpList.SelectedItem != null)
            {

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
                int id=((ParcelToList)CBDeliverdList.SelectedItem).Id;
                PickedUp(AccessIbl.GetParcel(id).MyDrone.Id);
               
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
                AccessIbl.PickedUpPackageByTheDrone(DroneId);

                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        //listWindow.StatusDroneSelectorChanged();

                        ////to conecct the binding to set the value of my drone to the proprtis
                        //MyDrone = AccessIbl.GetDrone(MyDrone.Id);
                        //DataContext = MyDrone;

                        ////עדכון רשימת החבילות
                        //int indexOfParcelInTheObservable = listWindow.ParcelToLists.IndexOf(listWindow.ParcelToLists.First(x => x.Id == MyDrone.Delivery.Id));
                        //listWindow.ParcelToLists[indexOfParcelInTheObservable] = AccessIbl.GetParcelList().First(x => x.Id == MyDrone.Delivery.Id);

                        ////עדכון השולח ברשימת הלקוחות
                        //int indexOfSenderCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Sender.Id));
                        //listWindow.CustomerToLists[indexOfSenderCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Sender.Id);

                        ////עדכון המקבל ברשימת הלקוחות
                        //int indexOfReceiverCustomerInTheObservable = listWindow.CustomerToLists.IndexOf(listWindow.CustomerToLists.First(x => x.Id == MyDrone.Delivery.Receiver.Id));
                        //listWindow.CustomerToLists[indexOfReceiverCustomerInTheObservable] = AccessIbl.GetCustomerList().First(x => x.Id == MyDrone.Delivery.Receiver.Id);

                        //BPickedUp.Visibility = Visibility.Hidden;
                        //BDeliveryPackage.Visibility = Visibility.Visible;

                        //if (parcelWindow != null)//עדכון שינוי מיקום וסוללת הרחפן אם נפתח חלון רחפן דרך חבילה
                        //{
                        //    parcelWindow.UpdateChangesFromDroneWindow();
                        //}
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

    }
}
