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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Client_Knowledge_checking.Test;

namespace Client_Knowledge_checking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LogInWindow logInWindowInstance;
        /// <summary>
        /// Konstruktor klasy MainWindow. Inicjalizuje wszystkie kontrolki oraz dodaje metode do event handlera -
        /// jesli nastapi metoda Close() na tej klasie, wowczas wywolana zostanie metoda obslugujaca MainWindow_Closed.
        /// Na samym końcu inicjalizowany jest słownik w klasie ClientConnection
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
            Connection.ClientConnection.InitializeDictionary();
        }

        /// <summary>
        ///Metoda dla event handlera przechwytujacego zdarzenie zamkniecia okna MainWindow
        /// w zaleznosci od statusu okna z panelem logowania, wymusza badz nie, jego zamkniecie 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (logInWindowInstance.logInWindowStatus == LogInWindow.WindowStatus.toExit)
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() => logInWindowInstance.Close()));
        }

        internal void GetLogInWindowInstance(LogInWindow logInWindow)
        {
            logInWindowInstance = logInWindow;
        }

        internal bool Connect(string clientName, IPAddress ip, int portNumber)
        {
            bool Result = false;

            if (Utilities.MockedTestFile.isFileMocked)
                return true;
            else
            {
                Connection.ClientConnection.Instance.InitializeInstance(clientName, ip, portNumber);
                if (Connection.ClientConnection.Instance.Connect() == true)
                    return Result;
                else
                    return Result;
            }
        }

        internal async void WaitForTestWrapper()
        {
            TestFile testFile;

            if (Utilities.MockedTestFile.isFileMocked)
            {
                testFile = new TestFile();
                testFile.fileWithTest = Utilities.MockedTestFile.fileWithTest;
            }
            else
            {
                Func<TestFile> indicatorToGetTestFromServer = new Func<TestFile>(() => Connection.ClientConnection.Instance.GetTestFromServer());

                await Connection.ClientConnection.Instance.WaitForTest();
                testFile = await Task.Run<TestFile>(indicatorToGetTestFromServer);
            }
            //TestFile testFile = null;
            //await ClientConnection.Instance.GetTestFromServer();
            if (testFile != null)
                StartTest(testFile);
            else
                CancelTest();
        }

        private void StartTest(Test.TestFile testFile)
        {
            Test.Interpreter.StartInterpreting(testFile, this);
        }

        private void CancelTest()
        {
            logInWindowInstance.StartAgain();
        }

        public void ChangeContext(Question question)
        {
            if (!(DataContext == question))
                DataContext = question;
        }

        private void nextQuestion_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
