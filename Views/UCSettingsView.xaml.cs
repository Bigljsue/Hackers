using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_HackersList.Views
{
    /// <summary>
    /// Логика взаимодействия для UCSettingsView.xaml
    /// </summary>
    public partial class UCSettingsView : UserControl
    {
        public UCSettingsView()
        {
            FontSize = WPF_HackersList.Properties.Settings.Default.FontSize;
            InitializeComponent();
        }
    }
}
