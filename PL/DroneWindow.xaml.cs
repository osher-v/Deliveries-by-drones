
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

using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        public IBL.IBL AccessIbl;
        public DroneWindow(IBL.IBL bl)
        {
            InitializeComponent();
            AccessIbl = bl;
            TBWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

    }
}
