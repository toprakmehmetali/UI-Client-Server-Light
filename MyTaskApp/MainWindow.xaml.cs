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
using MyTaskAppServer.Enums;
using MyTaskAppServer.Models;
using server;
using server.Config;
using server.Models;


namespace MyTaskApp
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
            Server.SetServerSettings();
            Server.SetEmptyArrayClients();
            Server.InitializeServerData();
            Server.StartServer();
            Server.ListenServer();
            UIManager.UIManagerConfiguration(this);

        }

        private void BtnRandom_Click(object sender, RoutedEventArgs e)
        {
            TbRandom.Text = random.Next(0, 1000).ToString();
        }

        private void BtnLight_Click(object sender, RoutedEventArgs e)
        {
            UIManager.SetLightStatus((!UIManager.LightStatus).ToString());
           
        }

        private void TbRandom_TextChanged(object sender, TextChangedEventArgs e)
        {
            UIManager.randomText = TbRandom.Text;
            UIManager.SendText();
        }

    }
}
