using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        BlApi.IBL AccessIbl;
        private ListView ListWindow;
        /// <summary>
        /// update constractor
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="_DroneListWindow"></param>
        /// <param name="BaseStationTo"></param>
        /// <param name="_indexDrone"></param>
        public BaseStationWindow(BlApi.IBL bl, ListView _ListWindow, BaseStationsToList BaseStationTo, int _indexBaseStation)
        {
            InitializeComponent();
            AccessIbl = bl;
            ListWindow = _ListWindow;
            
        }

        private void TBstaitonId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (true)
            {

            }
        }


      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TBstaitonId.Text.Length!=0 && TBstaitonName.Text.Length != 0 && TBstationChargeSlots.Text.Length != 0 && TBstaitonLongtude.Text.Length != 0 && TBstaitonLattude.Text.Length != 0)
            {
                if (!(double.Parse(TBstaitonLongtude.Text) < 31.8 || double.Parse(TBstaitonLongtude.Text) > 32.2 || double.Parse(TBstaitonLattude.Text) < 34.6 || double.Parse(TBstaitonLattude.Text) > 35.1))
                {


                    BO.BaseStation baseStationAdd = new BaseStation()
                    {
                        Id = int.Parse(TBstaitonId.Text),
                        Name = TBstaitonName.Text,
                        FreeChargeSlots = int.Parse(TBstationChargeSlots.Text),
                        BaseStationLocation = new Location() { longitude = double.Parse(TBstaitonLongtude.Text), latitude = double.Parse(TBstaitonLattude.Text) },
                    };
                    try
                    {
                        AccessIbl.AddStation(baseStationAdd);
                        MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);//לטלפל בX
                        switch (result)
                        {
                            case MessageBoxResult.OK:
                                BO.BaseStationsToList stationsToList = AccessIbl.GetBaseStationList().ToList().Find(i => i.Id == baseStationAdd.Id);
                                ListWindow.BaseStationToLists.Add(stationsToList);
                                ListWindow.IsEnabled = true;
                                //ClosingWindow = false;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (AddAnExistingObjectException ex)
                    {
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        TBstaitonId.Text = "";
                        TBstaitonId.BorderBrush = Brushes.Red;
                    }
                }
                else
                {
                    MessageBox.Show(" מיקום קו האורך יכול להיות בין 31.8 ל32.2 וקו הרוחב בין34.6 ל35.1", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("נא ודאו שכל השדות מלאים", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region מטפל בבדיקות כפתורים
        private void TBstaitonName_KeyDown(object sender, KeyEventArgs e)
        {
            if (TBstaitonName.Text.Length > 20)
            {
                e.Handled = true;
            }
        }


        private void TBstaitonId_KeyDown(object sender, KeyEventArgs e)
        {
            TBstaitonId.BorderBrush = Brushes.Gray;
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
            if (TBstaitonId.Text.Length > 8)
            {
                e.Handled = true;
            }
        }
        private void TBstaitonLattude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) // we want keys from the num pud too
                {
                    if (e.Key == Key.Decimal)
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            if (TBstaitonLattude.Text.Length > 10)
            {
                e.Handled = true;
            }
        }

        private void TBstaitonLongtude_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key < Key.D0 || e.Key > Key.D9)
            {
                if (e.Key < Key.NumPad0 || e.Key > Key.NumPad9 ) // we want keys from the num pud too
                {
                    if (e.Key == Key.Decimal)
                        e.Handled = false;
                    else
                        e.Handled = true;

                }
                else
                {
                    e.Handled = false;
                }
            }
            if (TBstaitonLongtude.Text.Length > 10)
            {
                e.Handled = true;
            }
        }
        #endregion

        private void TBstationChargeSlots_KeyDown(object sender, KeyEventArgs e)
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
            if (TBstationChargeSlots.Text.Length > 8)
            {
                e.Handled = true;
            }
        }
        public BaseStationWindow(BlApi.IBL bl, ListView _ListWindow)
        {
            InitializeComponent();
            Width = 440;
            AccessIbl = bl;
            ListWindow = _ListWindow;

        }
    }
}
