using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Client_Knowledge_checking
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public MainWindow mainWindow;
        public enum WindowStatus {toExit, notToExit};
        public WindowStatus logInWindowStatus;

        public LogInWindow()
        {
            InitializeComponent();
        }

        public void StartAgain()
        {
            //this.Activate();
            this.Show();
            logInWindowStatus = WindowStatus.notToExit;
            mainWindow.Close();
        }

        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip;
            int portNumber;
            string clientName;
            if (nameAndSurnameTextBox.Text != "" && nameAndSurnameTextBox.Text.Length > 7)
            {
                if (ipNumberTextBox.Text != "" && IPAddress.TryParse(ipNumberTextBox.Text, out ip))
                {
                    try
                    {
                        clientName = nameAndSurnameTextBox.Text;
                        portNumber = System.Convert.ToUInt16(portNumberTextBox.Text);  
                        mainWindow = new MainWindow();
                        if (mainWindow.Connect(clientName, ip, portNumber) == true)
                        {
                            mainWindow.Show();
                            mainWindow.GetLogInWindowInstance(this);
                            //this.Close();
                            this.Hide();
                            logInWindowStatus = WindowStatus.toExit;
                            mainWindow.WaitForTestWrapper();
                            //StartAgain();
                        }
                    }
                    catch (OverflowException ex)
                    {
                        MessageBox.Show( "Podaj poprawny nr portu, od 0 do 65 000");
                    }
                    catch (FormatException ex)
                    {
                        MessageBox.Show("Podaj nr portu w poprawnym formacie");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }
                else
                    MessageBox.Show("Podaj poprawne IP");
            }
            else
                MessageBox.Show("Podaj poprawne imię i nazwisko") ;          
        }

    }
}
