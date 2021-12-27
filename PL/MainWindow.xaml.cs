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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using System.Windows.Media.Animation;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region main window
        /// <summary> the constractor start the intlize consractor of the data </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        // we crate an obejt that give us accses to the ibl intrface  
        public BlApi.IBL AccessIbl = BlApi.BlFactory.GetBL();

        /// <summary>
        /// Login to a manager or client interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Blogin_Click(object sender, RoutedEventArgs e)
        {
           
            switch (Blogin.Content)
            {
                case "כניסה כמנהל":
                    new ListView(AccessIbl).Show();
                    break;
                case "כניסת לקוח":
                    try
                    {
                         AccessIbl.GetCustomer(int.Parse(TBuserID.Password)); //לשים לב שזה מקפיץ חריגה אם הלקוח נכנס ללא שם משתמש
                        new ClientWindow(AccessIbl, int.Parse(TBuserID.Password)).Show();
                    }
                    catch (BO.NonExistentObjectException ex)
                    {
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                default:   
                    break;
            }       
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_Loaded(object sender, RoutedEventArgs e)
        {
            AddOn.Opacity = 0;
            DoubleAnimation Animmation = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(10.5));
            PBloding.BeginAnimation(ProgressBar.ValueProperty, Animmation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PBloding_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(PBloding.Value == 100)
            {
                Blogin.IsEnabled = true;
                DoubleAnimation doubleAnimmation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(5));
                AddOn.BeginAnimation(Grid.OpacityProperty, doubleAnimmation);
                DoubleAnimation DSF = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                Disiaper.BeginAnimation(Grid.OpacityProperty, DSF);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BNewUser_Click(object sender, RoutedEventArgs e)
        {
            ListView listView = new ListView(AccessIbl);
            new CustomerWindow(AccessIbl, listView).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TICadmin_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Visibility = Visibility.Visible;
            Blogin.Content = "כניסה כמנהל";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TIUser_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Visibility = Visibility.Visible;
            Blogin.Content = "כניסת לקוח";
        }

     

        private void TBadmin_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBadmin.Text.Length != 0)
            {
                Blogin.IsEnabled = true;
            }
            else
            {
                Blogin.IsEnabled = false;
            }
        }

        private void TBuserID_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBuserID.Password.Length != 0)
            {
                Blogin.IsEnabled = true;
            }
            else
            {
                Blogin.IsEnabled = false;
            }
        }
    }
}
