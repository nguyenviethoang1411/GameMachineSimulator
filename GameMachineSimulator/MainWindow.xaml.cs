using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GameMachineSimulator
{
    public partial class MainWindow : Window
    {
        private readonly string filePath = @"C:\Temp\jackpot_messages.txt";

        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        }

        private void SendBet_Click(object sender, RoutedEventArgs e)
        {
            if (MachineIdBox.SelectedItem is not ComboBoxItem selectedItem)
            {
                MessageBox.Show("Please select a machine.");
                return;
            }

            string machineId = selectedItem.Content.ToString()!;
            string betInput = BetAmountBox.Text.Trim();

            if (!double.TryParse(betInput, out double amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid bet amount.");
                return;
            }

            string message = $"{machineId},{amount:F2}";

            try
            {
                File.AppendAllText(filePath, message + Environment.NewLine);

                // Update UI with success and log
                StatusText.Text = $"✅ Bet sent: {message}";

                BetLog.Items.Insert(0, $"🟢 {DateTime.Now:T} - Sent: {message}");
                if (BetLog.Items.Count > 10)
                    BetLog.Items.RemoveAt(10);

                BetAmountBox.Clear();
                BetAmountBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
