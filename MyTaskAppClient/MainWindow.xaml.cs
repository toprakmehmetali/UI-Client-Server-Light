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
using client;
using client.Config;
using client.Models;
using MyTaskAppServer.Enums;

namespace MyTaskAppClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private DataTransferObject dataTransferObject = new DataTransferObject();
        public MainWindow()
        {
            InitializeComponent();
            Config.LoadConfigJson();
            Client.Connect();
            UIManager.UIManagerConfiguration(this);
        }
        private void BtnLight_Click(object sender, RoutedEventArgs e)
        {
            UIManager.SetLightStatus((!UIManager.lightStatus).ToString());
            UIManager.SendLightStatus();
        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            BtnRandom.Content = random.Next(0, 1000).ToString();
            UIManager.SendText();
        }
    }
}
