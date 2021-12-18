using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ListView.xaml
    /// </summary>
    public partial class ListView : Window
    {
        public BlApi.IBL AccessIbl=BlApi.BlFactory.GetBL();

        /// <summary> crate a observab list of type IBL.BO.DroneToList (to see changes in live) </summary>
        public ObservableCollection<BO.DroneToList> droneToLists;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        /// <summary>
        /// constractor for the DroneListWindow that will start the InitializeComponent ans fill the Observable Collection
        /// </summary>
        /// <param name="bl">get AccessIbl from main win</param>
        public ListView()
        {
            InitializeComponent();
            //AccessIbl = bl;
            //craet observer and set the list accordale to ibl drone list 
            droneToLists = new ObservableCollection<DroneToList>();
            List<BO.DroneToList> drones = AccessIbl.GetDroneList().ToList();
            foreach (var item in drones)
            {
                droneToLists.Add(item);
            }
            //new event that will call evre time that the ObservableCollection didact a change 
           // droneToLists.CollectionChanged += DroneToLists_CollectionChanged;
            //display the defult list 
            listss.ItemsSource = droneToLists;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            //WeightSelctor.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
        //public ListView()
        //{
        //    InitializeComponent();
        //}

    }
}
