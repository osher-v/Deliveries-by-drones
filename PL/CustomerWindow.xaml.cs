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
    /// Interaction logic for CustomerWindow.xaml.
    /// This window covers a customer interface.
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public BlApi.IBL AccessIbl; //Access object to the BL class.

        public ListView ListWindow; //object of ListView window.

        public bool ClosingWindow { get; private set; } = true; //a bool to help us disable the x bootum.

        #region add situation
        /// <summary>
        /// add ctor.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_ListWindow"></param>
        public CustomerWindow(BlApi.IBL bl, ListView _ListWindow)
        {
            InitializeComponent();

            Width = 440;

            addCustomer.Visibility = Visibility.Visible; //open the Grid of add action.

            AccessIbl = bl;

            ListWindow = _ListWindow;
        }

        #region Handles text insert
        /// <summary>
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBcustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            TBcustomerId.BorderBrush = Brushes.Gray;
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
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBcustomerPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            TBcustomerId.BorderBrush = Brushes.Gray;
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
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBcustomerLattude_KeyDown(object sender, KeyEventArgs e)
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
        private void TBcustomerLongtude_KeyDown(object sender, KeyEventArgs e)
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
        #endregion 

        /// <summary>
        /// add Customer to data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            //Check that all fields are filled.
            if (TBcustomerId.Text.Length != 0 && TBcustomerName.Text.Length != 0 && TBcustomerPhoneNumber.Text.Length != 0 && TBcustomerLongtude.Text.Length != 0 && TBcustomerLattude.Text.Length != 0)
            {
                //Check that the location does not exceed the boundaries of Gush Dan.
                if (!(double.Parse(TBcustomerLongtude.Text) < 31.8 || double.Parse(TBcustomerLongtude.Text) > 32.2 || double.Parse(TBcustomerLattude.Text) < 34.6 || double.Parse(TBcustomerLattude.Text) > 35.1))
                {
                    BO.Customer customerAdd = new Customer() //Create an object to add to the data.
                    {
                        Id = int.Parse(TBcustomerId.Text),
                        Name = TBcustomerName.Text,
                        PhoneNumber = TBcustomerPhoneNumber.Text,
                        LocationOfCustomer = new Location() { longitude = double.Parse(TBcustomerLongtude.Text), latitude = double.Parse(TBcustomerLattude.Text) }
                    };

                    try
                    {
                        AccessIbl.AddCustomer(customerAdd); //update the logic layer.
                        MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                BO.CustomerToList customersToList = AccessIbl.GetCustomerList().ToList().Find(i => i.Id == customerAdd.Id);
                                ListWindow.CustomerToLists.Add(customersToList); //Updating the observer list of customers.

                                ListWindow.IsEnabled = true; //open the "ListWindow" window.

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
                        TBcustomerId.Text = "";
                        TBcustomerId.BorderBrush = Brushes.Red;
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

        #endregion add situation

        public Customer customer;

        public int indexSelected; //the location index in the observer of the customers in the ListView window.

        public ParcelWindow parcelWindow; //for using if we enter from parcel window.

        public ClientWindow clientWindow;//for using if we enter from client window.

        #region update customer 

        /// <summary>
        /// update ctor.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_ListWindow"></param>
        /// <param name="CustomerTo"></param>
        /// <param name="_indexCustomer"></param>
        public CustomerWindow(BlApi.IBL bl, ListView _ListWindow, CustomerToList CustomerTo, int _indexCustomer, ParcelWindow _parcelWindow = null, ClientWindow _clientWindow=null)
        {
            InitializeComponent();

            updateCustomer.Visibility = Visibility.Visible;

            AccessIbl = bl;

            ListWindow = _ListWindow;

            indexSelected = _indexCustomer;

            ////Connecting customer data.
            customer = AccessIbl.GetCustomer(CustomerTo.Id);
            DataContext = customer;

            listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer; //Connecting List of parcel from the customer.
            listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer; //Connecting List of parcel to the customer.

            parcelWindow = _parcelWindow;
            clientWindow = _clientWindow;
        }

        /// <summary>
        /// The function updates customer details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.UpdateCustomer(customer.Id, TBUpdateCustomerName.Text, TBUpdateCustomerPhoneNumber.Text); //update the logic layer.
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        ListWindow.CustomerToLists[indexSelected] = AccessIbl.GetCustomerList().FirstOrDefault(x => x.Id == customer.Id);//Updating the observer list of customers.

                        ListWindow.ParcelToLists.Clear();  //Update overlooking a new list for customers.
                        List<BO.ParcelToList> parcel = AccessIbl.GetParcelList().ToList();
                        foreach (var item in parcel)
                        {
                            ListWindow.ParcelToLists.Add(item);
                        }

                        if(parcelWindow != null)//in case opened from a parcel window.
                        {
                            parcelWindow.UpdateChangesFromCustomerWindow();
                        }

                        if (clientWindow != null)//in case opened from a client window.
                        {
                            clientWindow.UpdateChangesFromParcelWindow();
                        }

                        ListWindow.IsEnabled = true; //open the "ListWindow" window.

                        ClosingWindow = false; //to enable to close the "BaseStationWindow" window.
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                MessageBox.Show("ERROR");
            }
        }

        /// <summary>
        /// Open a parcel window from the List of "CustomeSend".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfCustomeSend_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ParcelAtCustomer)listOfCustomerReceive.SelectedItem) != null)// if the user click on empty space in the view list we dont open anything
            {
                int IdOfParcel = ((ParcelAtCustomer)listOfCustomeSend.SelectedItem).Id;
                int indexParcelInObservable = ListWindow.ParcelToLists.IndexOf(ListWindow.ParcelToLists.First(x => x.Id == IdOfParcel));
                ParcelToList customer = AccessIbl.GetParcelList().First(x => x.Id == IdOfParcel);
                new ParcelWindow(AccessIbl, ListWindow, customer, indexParcelInObservable, clientWindow).Show();

                // to close the window affter the opariton
                ClosingWindow = false;
                Close();
            }
        }

        /// <summary>
        /// Open a parcel window from the List of "CustomerReceive".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listOfCustomerReceive_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ParcelAtCustomer)listOfCustomerReceive.SelectedItem) != null)// if the user click on empty space in the view list we dont open anything
            {
                int IdOfParcel = ((ParcelAtCustomer)listOfCustomerReceive.SelectedItem).Id;
                int indexParcelInObservable = ListWindow.ParcelToLists.IndexOf(ListWindow.ParcelToLists.First(x => x.Id == IdOfParcel));
                ParcelToList customer = AccessIbl.GetParcelList().First(x => x.Id == IdOfParcel);
                new ParcelWindow(AccessIbl, ListWindow, customer, indexParcelInObservable, clientWindow).Show();

                // to close the window affter the opariton
                ClosingWindow = false;
                Close();
            }
        }

        #region Handles text insert and butones 

        /// <summary>
        /// Locks the keyboard for numbers only.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBUpdateCustomerPhoneNumber_KeyDown(object sender, KeyEventArgs e)
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

        /// <summary>
        /// The function opens the option to update as soon as the fields are full and closes when one of the fields is equal to 0.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBUpdateCustomerName_KeyUp(object sender, KeyEventArgs e)
        {

            if (TBUpdateCustomerPhoneNumber.Text.Length != 0 && TBUpdateCustomerName.Text.Length != 0)
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
        private void TBUpdateCustomerPhoneNumber_KeyUp(object sender, KeyEventArgs e)
        {

            if (TBUpdateCustomerPhoneNumber.Text.Length != 0 && TBUpdateCustomerName.Text.Length != 0)
            {
                if (!BUpdate.IsEnabled)
                    BUpdate.IsEnabled = true;
            }
            else //one of the fields is equal to 0.
            {
                BUpdate.IsEnabled = false;
            }
        }
        #endregion Handles text insert and butones 

        #endregion update customer 

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
        private void BcloseAdd_Click(object sender, RoutedEventArgs e)
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
