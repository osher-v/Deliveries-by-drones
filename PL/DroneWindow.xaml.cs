using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private DroneListWindow DroneListWindow;

        public DroneWindow(IBL.IBL bl, DroneListWindow _DroneListWindow) //, DroneListWindow _StatusSelector, DroneListWindow _WeightSelctor
        {
            InitializeComponent();

            AccessIbl = bl;

            DroneListWindow = _DroneListWindow;

            TBWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            BaseSTATION.ItemsSource = AccessIbl.GetBaseStationList();

            BaseStationID.ItemsSource = AccessIbl.GetBaseStationList();
            BaseStationID.DisplayMemberPath = "Id";
        }

        private void Bclose_Click(object sender, RoutedEventArgs e)
        {
            DroneOperation.Close();
        }

        private void TBID_KeyDown(object sender, KeyEventArgs e)
        {
            /////////TBModel.Text = count.ToString();
            // take only the kyes we alowed 
            if (e.Key < Key.D0 || e.Key > Key.D9 )
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
            if ( TBID.Text.Length > 8)
                e.Handled = true;
              
        }

        private void TBModel_KeyDown(object sender, KeyEventArgs e)
        {
            if (TBModel.Text.Length > 5)
                e.Handled = true;
        }

        private void SendToBl_Click(object sender, RoutedEventArgs e)
        {
            if(TBModel.Text.Length != 0 && TBID.Text.Length != 0 && BaseStationID.SelectedItem != null && TBWeight.SelectedItem!=null)
            {
                DroneToList newdrone = new DroneToList
                {
                    Id = int.Parse(TBID.Text), // כבר בדקנו שזה מספר על ידי זה שחסמנו את המקלדת 
                    Model = TBModel.Text,
                    MaxWeight = (WeightCategories)TBWeight.SelectedIndex,
                };

                try
                {
                    //AccessIbl.AddDrone(newdrone, (int)BaseStationID.SelectedItem);// כבר בדקנו שזה מספר על ידי זה שחסמנו את המקלדת 
                    //AccessIbl.AddDrone(newdrone, int.Parse(BaseStationID.SelectedItem.ToString()));
                    AccessIbl.AddDrone(newdrone, Convert.ToInt32(BaseStationID.SelectedValue));
                    //int.Parse(BaseStationID.SelectedItem.ToString());
                    MessageBoxResult result= MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                    switch (result)
                    {
                        case MessageBoxResult.OK:                          
                            newdrone = AccessIbl.GetDroneList().ToList().Find(i => i.Id == newdrone.Id);
                            DroneListWindow.droneToLists.Add(newdrone);
                            //DroneListWindow.DroneListView.Items.Refresh();
                            

                            Close();
                            break;         
                    }
                }
                catch (AddAnExistingObjectException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR" ,MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (NonExistentObjectException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (NoFreeChargingStations ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (NonExistentEnumException ex)
                {
                    MessageBox.Show(ex.ToString(), "ERROR" ,MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
                MessageBox.Show("נא ודאו שכל השדות מלאים","!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        
    }
}
