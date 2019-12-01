using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace XmlSerializerApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Game> games;
        string filename = "";

        public MainWindow()
        {
            InitializeComponent();

            games = new List<Game>();

            dataGrid.ItemsSource = games;
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".xml";
            fileDialog.Filter = "XML Files (*.xml)|*.xml";

            bool? result = fileDialog.ShowDialog();
            filename = fileDialog.FileName;

            if (result == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Game>));

                using (Stream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    games = (List<Game>)serializer.Deserialize(stream);
                }
            }

            RefreshData();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            games.Add(new Game());

            RefreshData();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (filename == "")
                return;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Game>));

            using (Stream stream = new FileStream(filename, FileMode.Create))
            {
                serializer.Serialize(stream, games);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            Game gameDel = dataGrid.SelectedItem as Game;

            if (gameDel == null)
                return;

            games.Remove(gameDel);

            RefreshData();
        }

        private void RefreshData()
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = games;
        }
    }
}
