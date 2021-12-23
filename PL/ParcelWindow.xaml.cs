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
        private void closeUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListWindow.IsEnabled = true;
            ClosingWindow = false; // we alowd the close option
            Close();
        }
        #endregion close  

        public ParcelWindow(BlApi.IBL bl, ListView _ListWindow, ParcelToList parcelTo, int _indexParcel)
        {
            InitializeComponent();
        }
    }
}
