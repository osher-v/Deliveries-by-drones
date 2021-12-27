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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        //Access object to the BL class.
        public BlApi.IBL AccessIbl;

        //object of ListView window.
        public ListView ListWindow;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        #region בנאי הוספה
        public CustomerWindow(BlApi.IBL bl, ListView _ListWindow)
        {
            InitializeComponent();

            Width = 440;

            addCustomer.Visibility = Visibility.Visible;

            AccessIbl = bl;

            ListWindow = _ListWindow;
        }

        #region מטפל בכפתורים
        private void TBcustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            TBcustomerId.BorderBrush = Brushes.Gray;
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

        private void TBcustomerPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {
            TBcustomerId.BorderBrush = Brushes.Gray;
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

        private void TBcustomerLattude_KeyDown(object sender, KeyEventArgs e)
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
            if (TBcustomerLattude.Text.Length > 10)
            {
                e.Handled = true;
            }
        }

        private void TBcustomerLongtude_KeyDown(object sender, KeyEventArgs e)
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
            if (TBcustomerLongtude.Text.Length > 10)
            {
                e.Handled = true;
            }
        }
        #endregion מטפל בכפתורים

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            //Check that all fields are filled.
            if (TBcustomerId.Text.Length != 0 && TBcustomerName.Text.Length != 0 && TBcustomerPhoneNumber.Text.Length != 0 && TBcustomerLongtude.Text.Length != 0 && TBcustomerLattude.Text.Length != 0)
            {
                //Check that the location does not exceed the boundaries of Gush Dan.
                if (!(double.Parse(TBcustomerLongtude.Text) < 31.8 || double.Parse(TBcustomerLongtude.Text) > 32.2 || double.Parse(TBcustomerLattude.Text) < 34.6 || double.Parse(TBcustomerLattude.Text) > 35.1))
                {
                    BO.Customer customerAdd = new Customer()
                    {
                        Id = int.Parse(TBcustomerId.Text),
                        Name = TBcustomerName.Text,
                        PhoneNumber = TBcustomerPhoneNumber.Text,
                        LocationOfCustomer = new Location() { longitude = double.Parse(TBcustomerLongtude.Text), latitude = double.Parse(TBcustomerLattude.Text) }
                    };

                    try
                    {
                        AccessIbl.AddCustomer(customerAdd);
                        MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                BO.CustomerToList customersToList = AccessIbl.GetCustomerList().ToList().Find(i => i.Id == customerAdd.Id);
                                ListWindow.CustomerToLists.Add(customersToList); //Updating the observer list of stations.

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
        
        #endregion בנאי הוספה

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

        public Customer customer;

        public int indexSelected;

        public CustomerWindow(BlApi.IBL bl, ListView _ListWindow, CustomerToList CustomerTo, int _indexCustomer)
        {
            InitializeComponent();

            updateCustomer.Visibility = Visibility.Visible;

            AccessIbl = bl;

            ListWindow = _ListWindow;

            indexSelected = _indexCustomer;

            customer = AccessIbl.GetCustomer(CustomerTo.Id);
            DataContext = customer;

            listOfCustomeSend.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelFromTheCustomer;
            listOfCustomerReceive.ItemsSource = AccessIbl.GetCustomer(customer.Id).ParcelToTheCustomer;
        }

        private void TBUpdateCustomerPhoneNumber_KeyDown(object sender, KeyEventArgs e)
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

        /// <summary>
        /// The function updates customer details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessIbl.UpdateCustomer(customer.Id, TBUpdateCustomerName.Text, TBUpdateCustomerPhoneNumber.Text);
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        ListWindow.CustomerToLists[indexSelected] = AccessIbl.GetCustomerList().FirstOrDefault(x => x.Id == customer.Id);//עדכון המשקיף

                        ListWindow.ParcelToLists.Clear();  //עדכון המשקיף לרשימה חדשה עבור הלקוחות
                        List<BO.ParcelToList> parcel = AccessIbl.GetParcelList().ToList();
                        foreach (var item in parcel)
                        {
                            ListWindow.ParcelToLists.Add(item);
                        }

                        ListWindow.IsEnabled = true;
                        ClosingWindow = false;
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
    }
}
