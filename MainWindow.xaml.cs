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
using System.IO;

namespace JPGviewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<string> styles = new List<string> { "light", "dark" };
            styleBox.SelectionChanged += ThemeChange;
            styleBox.ItemsSource = styles;
            styleBox.SelectedItem = "dark";
        }
        private void ThemeChange(object sender, SelectionChangedEventArgs e)
        {
            string style = styleBox.SelectedItem as string;
            var uri = new Uri(style + ".xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Image";
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "Image files (*.jpg)|*.jpg";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                image.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.RelativeOrAbsolute));
            }
        }

        private void openPathButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayFiles(pathTextbox.Text);
        }
        private string currentLocation = "";
        private void DisplayFiles(string filePath)
        {
            try
            {
                string[] filesList = Directory.GetFiles(filePath, "*.jpg");
                fileList.Children.Clear();
                currentLocation = filePath;
                for (int i = 0; i < filesList.Length - 1; i++)
                {
                    bool isHidden = (File.GetAttributes(filesList[i]) & FileAttributes.Hidden) == FileAttributes.Hidden;

                    if (!isHidden)
                    {
                        int startOfName = filesList[i].LastIndexOf("\\");
                        string fileName = filesList[i].Substring(startOfName + 1, filesList[i].Length - (startOfName + 1));

                        Button newButton = new Button();
                        newButton.Content = fileName;
                        newButton.Name = "name" + i;
                        newButton.HorizontalAlignment = HorizontalAlignment.Stretch;
                        newButton.VerticalAlignment = VerticalAlignment.Top;
                        fileList.Children.Add(newButton);
                        newButton.Click += new RoutedEventHandler(newButton_Click);
                    }
                }
            }
            catch(ArgumentException)
            {
                
            }
            catch(DirectoryNotFoundException)
            {

            }
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            char lastChar = currentLocation[currentLocation.Length - 1];
            char BS = char.Parse(@"\");
            string filePath = "";
            if (lastChar != BS)
            {
                filePath = currentLocation + @"\" + btn.Content;
            }
            else if (lastChar == BS)
            {
                filePath = currentLocation + btn.Content;
            }
            image.Source = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}