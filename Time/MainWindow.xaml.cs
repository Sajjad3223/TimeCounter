using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Time
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseManager databaseManager;
        DispatcherTimer timer;
        int id = 1;
        int selectedWorkId = 1;
        int seconds = 0;
        int minutes = 0;
        int hours = 0;

        public MainWindow()
        {
            InitializeComponent();
            databaseManager = new DatabaseManager();
            ReadData();
            ReadWorksData();
        }
        
        private void StartCounting(object sender, RoutedEventArgs e)
        {
            if (selectedWorkId == 0)
            {
                MessageBox.Show("لطفا ابتدا یک کار را انتخاب کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RtlReading);
            }
            else
            {
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(TimerCounter);
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Start();
                StartButton.Visibility = Visibility.Collapsed;
                StopButton.Visibility = Visibility.Visible;
            }
        }

        private void TimerCounter(object sender, EventArgs e)
        {
            seconds++;
            if (seconds >= 60)
            {
                minutes++;
                seconds = 0;
            }
            if (minutes >= 60)
            {
                hours++;
                minutes = 0;
            }
            SetTimerText();
        }

        private void SetTimerText()
        {
            TimerText.Text = $"{ToDigit(hours)}:{ToDigit(minutes)}:{ToDigit(seconds)}";
        }

        private void StopCounting(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                timer.Stop();
                databaseManager.SetDataSqlite(id, selectedWorkId, hours, minutes, seconds);
                
                StartButton.Visibility = Visibility.Visible;
                StopButton.Visibility = Visibility.Collapsed;
            }
        }

        private string ToDigit(int num)
        {
            return num.ToString("00");
        }

        private void ReadData()
        {
            var data = databaseManager.ReadDataSqlite(selectedWorkId);
            if (data != null)
            {
                id = data.Id;
                hours = data.Hours;
                minutes = data.Minutes;
                seconds = data.Seconds;
                selectedWorkId = data.WorkId;
            }
            SetTimerText();
        }
        private void ReadWorksData()
        {
            var data = databaseManager.ReadWorksSqlite();
            if (data.Any())
            {
                WorksDropdown.Items.Clear();
                foreach (var work in data)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = work.WorkName;
                    WorksDropdown.Items.Add(item);
                    if (work.Id == selectedWorkId) item.IsSelected = true;
                }
            }
        }

        private void SetTimerData(object sender, System.ComponentModel.CancelEventArgs e)
        {
            databaseManager.SetDataSqlite(id, selectedWorkId, hours, minutes, seconds);
        }

        private void ResetData(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("آیا از ریست تایمر مطمئن هستید؟", "هشدار", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No, MessageBoxOptions.RtlReading) == MessageBoxResult.Yes)
            {
                databaseManager.ResetDataSqlite(id, selectedWorkId);
                ReadData();
            }
        }

        private void ChangeTimeCounter(object sender, SelectionChangedEventArgs e)
        {
            StopCounting(null,null);

            ComboBoxItem selectedItem = (ComboBoxItem)WorksDropdown.SelectedItem;
            if (selectedItem == null) return;
            string value = selectedItem.Content.ToString() ?? "";

            selectedWorkId = databaseManager.GetWorkIdSqlite(value);
            ReadData();
        }

        private void AddWork(object sender, RoutedEventArgs e)
        {
            AddWorkPage addWork = new AddWorkPage();
            addWork.Owner = this;
            addWork.ShowDialog();
            ReadWorksData();
        }

        private void DeleteWork(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("آیا از حذف این کار مطمئن هستید؟","هشدار",MessageBoxButton.YesNo,MessageBoxImage.Warning,MessageBoxResult.No,MessageBoxOptions.RtlReading) == MessageBoxResult.Yes)
            {
                databaseManager.RemoveWorkSqlite(selectedWorkId);
                ReadWorksData();
                WorksDropdown.SelectedIndex = 0;
                selectedWorkId = databaseManager.GetWorkIdSqlite(((ComboBoxItem)WorksDropdown.SelectedItem).Content.ToString() ?? "");
            }
        }
    }
}
