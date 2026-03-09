using Microsoft.Win32;
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
        // Dictionary als class-level variabele zodat meerdere knoppen er toegang toe hebben
        // Key = naam, Value = e-mailadres
        Dictionary<string, string> contacts = new Dictionary<string, string>();

        //Pad naar jouw Temp
        private string emailPath = @"C:\Users\Trist\Desktop\Graduaat\C-Advanced\FileI-O\FileI-O\Temp\email.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        // ===== METHODE: ReadEmailFile =====
        // Leest een bestand regel per regel in een StringBuilder
        // Bevat foutafhandeling voor bestand-niet-gevonden en onverwachte fouten
        StringBuilder ReadEmailFile(string filepath)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Het bestand werd niet gevonden!\nControleer het pad: " + filepath,
                    "Bestand niet gevonden", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er deed zich een onverwachte fout voor:\n" + ex.Message,
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return sb;
        }

        // ===== KNOP: Inlezen =====
        private void readFileButon_Click(object sender, RoutedEventArgs e)
        {
            // ✅ Gebruikt het pad naar jouw Temp-folder
            StringBuilder result = ReadEmailFile(emailPath);
            resultTextBox.Text = result.ToString();
        }

        // ===== KNOP: Inlezen/Dialoogvenster =====
        private void readDialogButon_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Tekstbestanden (*.txt)|*.txt|Alle bestanden (*.*)|*.*";

            // ✅ Standaard map instellen op jouw Temp-folder
            openFileDialog.InitialDirectory = @"C:\Users\Trist\Desktop\Graduaat\C-Advanced\FileI-O\FileI-O\Temp";

            if (openFileDialog.ShowDialog() == true)
            {
                StringBuilder result = ReadEmailFile(openFileDialog.FileName);
                resultTextBox.Text = result.ToString();
            }
        }

        private void readDictionaryButon_Click(object sender, RoutedEventArgs e)
        {
            contacts.Clear();

            try
            {
                string[] lines = File.ReadAllLines(emailPath);

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Splits op komma in 2 delen: "Achternaam Voornaam" en "email"
                    string[] parts = line.Split(',');

                    if (parts.Length >= 2)
                    {
                        // ✅ Verwijder aanhalingstekens
                        string naam = parts[0].Trim().Trim('"');
                        string email = parts[1].Trim().Trim('"');

                        if (!contacts.ContainsKey(naam))
                        {
                            contacts.Add(naam, email);
                        }
                    }
                }

                // Mooi uitgelijnde output
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, string> contact in contacts)
                {
                    sb.AppendLine($"{contact.Key.PadRight(25)}: {contact.Value}");
                }

                resultTextBox.Text = sb.ToString();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Het bestand email.txt werd niet gevonden!",
                    "Bestand niet gevonden", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Onverwachte fout:\n" + ex.Message,
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void writeButon_Click(object sender, RoutedEventArgs e)
        {
            if (contacts.Count == 0)
            {
                MessageBox.Show("Er zijn geen contacten ingelezen!\nKlik eerst op 'Inlezen/Dictionary'.",
                    "Geen data", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Schrijft Adressen.txt naar Temp
                string adressenPath = @"C:\Users\Trist\Desktop\Graduaat\C-Advanced\FileI-O\FileI-O\Temp\Adressen.txt";

                using (StreamWriter writer = new StreamWriter(adressenPath))
                {
                    foreach (KeyValuePair<string, string> contact in contacts)
                    {
                        writer.WriteLine(contact.Value);
                    }
                }

                MessageBox.Show("Adressen zijn weggeschreven naar:\n" + adressenPath,
                    "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het wegschrijven:\n" + ex.Message,
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addButon_Click(object sender, RoutedEventArgs e)
        {
            string naam = nameTextBox.Text;
            string email = emailTextBox.Text;

            if (string.IsNullOrWhiteSpace(naam) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Vul zowel een naam als een e-mailadres in!",
                    "Invoer onvolledig", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "email";
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.Filter = "Tekstbestanden (*.txt)|*.txt";

            saveFileDialog.InitialDirectory = @"C:\Users\Trist\Desktop\Graduaat\C-Advanced\FileI-O\FileI-O\Temp";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, append: true))
                    {
                        writer.WriteLine($"{naam} {email}");
                    }

                    MessageBox.Show("Contact succesvol toegevoegd!",
                        "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij het toevoegen:\n" + ex.Message,
                        "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}