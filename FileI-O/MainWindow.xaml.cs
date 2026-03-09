using System.IO;
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

namespace FileI_O
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        //text ergen in krijgen
        private void readFileButon_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder result = ReadEmailFile(@"C:\Users\Trist\Desktop\Graduaat\C-Advanced\FileI-O\FileI-O\Temp-folder\TextFile1.txt");

        }

        //streamreader
        StringBuilder ReadEmailFile(string filepath)
        {
            StringBuilder sb = new StringBuilder();

            using (StreamReader reader = new StreamReader(filepath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }

            return sb;
        }


        private void readDialogButon_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string , string> contacts = new Dictionary<string , string>();
            resultTextBox.Text = contacts.ToString();
        }

        private void readDictionaryButon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void writeButon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButon_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}