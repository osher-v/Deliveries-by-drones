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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        public CustomerWindow(BlApi.IBL bl, ListView _DroneListWindow, CustomerToList CustomerTo, int _indexCustomer)
        {
            InitializeComponent();
        }
        private void BAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BcloseAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TBcustomerId_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TBcustomerPhoneNumber_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TBcustomerLattude_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TBcustomerLongtude_KeyDown(object sender, KeyEventArgs e)
        {

        }

       
    }
}
