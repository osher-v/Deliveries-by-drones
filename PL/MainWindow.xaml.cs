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
        public IBL.IBL AccessIbl = new IBL.BL();

        /// <summary> open the drone list window  </summary>
        private void ShowDroneList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(AccessIbl).Show();
            this.Close(); // we close the login window 
        }
        #endregion
    }
}
