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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        //Access object to the BL class.
        public BlApi.IBL AccessIbl;

        //object of ListView window.
        public ListView ListWindow;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        /// <summary>
        /// Add constractor.
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_ListWindow"></param>
        public ParcelWindow(BlApi.IBL bl, ListView _ListWindow)
        {
            InitializeComponent();

            addParcel.Visibility = Visibility.Visible;

            AccessIbl = bl;

            ListWindow = _ListWindow;

            // the combobox use it to show the Weight Categories
            CBWeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            // the combobox use it to show the Priorities Categories
            CBPriorSelector.ItemsSource = Enum.GetValues(typeof(Priorities));
        }

        /// <summary>
        /// disable the non numbers keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBParcelSenderId_KeyDown(object sender, KeyEventArgs e)
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
        /// disable the non numbers keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBParcelReciverId_KeyDown(object sender, KeyEventArgs e)
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
        private void BcloseUpdate_Click(object sender, RoutedEventArgs e)//חריגהההההההההההההה
        {
            ListWindow.IsEnabled = true;
            ClosingWindow = false; // we alowd the close option
            Close();
        }
        #endregion close  

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
                BO.Parcel parcelAdd = new Parcel()
                {
                    Sender = new CustomerInDelivery() { Id = int.Parse(TBParcelSenderId.Text) },
                    Receiver = new CustomerInDelivery() { Id = int.Parse(TBParcelReciverId.Text) },
                     Prior = (Priorities)CBPriorSelector.SelectedItem,
                    Weight = (WeightCategories)CBWeightSelctor.SelectedItem
                };

                try
                {
                    AccessIbl.AddParcel(parcelAdd);
                    MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            BO.ParcelToList parcelsToList = AccessIbl.GetParcelList().ToList().Find(i => i.Id == parcelAdd.Id);
                            ListWindow.ParcelToLists.Add(parcelsToList); //Updating the observer list of stations.

                            ListWindow.IsEnabled = true;
                            ClosingWindow = false;
                            Close();
                            break;
                        default:
                            break;
                    }
                }
                catch (NonExistentObjectException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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

        public ParcelWindow(BlApi.IBL bl, ListView _ListWindow, ParcelToList parcelTo, int _indexParcel)
        {
            InitializeComponent();

            updateParcel.Visibility = Visibility.Visible;

        }
    }
}
