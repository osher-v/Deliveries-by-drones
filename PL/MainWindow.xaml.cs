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
        private void Blogin_Click(object sender, RoutedEventArgs e)
        {
           
            switch (Blogin.Content)
            {
                case "כניסה כמנהל":
                    new DroneListWindow(AccessIbl).Show();
                    this.Close(); // we close the login window
                    break;
                case "הרשמת לקוח למערכת":
                    
                    break;
                case "כניסת לקוח":
                  
                    break;
                default:
                  
                    break;
            }
            //enter.Unloaded -= enter_Unloaded;
            //enter.Source = null;
            //enter.Close();
        }
        #endregion

       

        private void enter_Loaded(object sender, RoutedEventArgs e)
        {
            AddOn.Opacity = 0;
            DoubleAnimation Animmation = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(10.5));
            PBloding.BeginAnimation(ProgressBar.ValueProperty, Animmation);
        }

        private void PBloding_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(PBloding.Value == 100)
            {
                Blogin.IsEnabled = true;
                DoubleAnimation doubleAnimmation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(5));
                AddOn.BeginAnimation(Grid.OpacityProperty, doubleAnimmation);
                DoubleAnimation DSF = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(2));
                Disiaper.BeginAnimation(Grid.OpacityProperty, DSF);
                
            }
        }

        private void TCadmin_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Content = "כניסה כמנהל";
        }

        private void TIRegister_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Content = "הרשמת לקוח למערכת";

        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Content = "כניסת לקוח";

        }
    }
}
