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

namespace Time
{
    /// <summary>
    /// Interaction logic for AddWorkPage.xaml
    /// </summary>
    public partial class AddWorkPage : Window
    {
        public AddWorkPage()
        {
            InitializeComponent();
        }

        private void AddWork(object sender, RoutedEventArgs e)
        {
            DatabaseManager databaseManager = new DatabaseManager();
            databaseManager.AddWork(TextboxWorkName.Text);
            Close();
        }
    }
}
