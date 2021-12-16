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

        /// <summary> open the drone list window  </summary>
        private void login_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(AccessIbl).Show();
            this.Close(); // we close the login window 
            //enter.Unloaded -= enter_Unloaded;
            //enter.Source = null;
            //enter.Close();
        }
        #endregion

       

        private void enter_Loaded(object sender, RoutedEventArgs e)
        {
            login.Opacity = 0;
            DoubleAnimation Animmation = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(10));
            PBloding.BeginAnimation(ProgressBar.ValueProperty, Animmation);
        }

        private void PBloding_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(PBloding.Value == 100)
            {
                DoubleAnimation doubleAnimmation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(5));
                login.BeginAnimation(Button.OpacityProperty, doubleAnimmation);
            }
        }
    }
}
