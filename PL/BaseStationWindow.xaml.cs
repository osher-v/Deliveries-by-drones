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
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        /// <summary>
        /// update constractor
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_DroneListWindow"></param>
        /// <param name="BaseStationTo"></param>
        /// <param name="_indexDrone"></param>
        public BaseStationWindow(BlApi.IBL bl, ListView _DroneListWindow, BaseStationsToList BaseStationTo, int _indexBaseStation)
        {
            InitializeComponent();
        }
    }
}
