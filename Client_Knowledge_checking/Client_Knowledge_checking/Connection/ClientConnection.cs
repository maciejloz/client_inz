﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Client_Knowledge_checking.Test;

namespace Client_Knowledge_checking.Connection
{
    /// <summary>
    /// Klasa, ktora obsluguje polaczenie klienta z serwerem, wykorzystuje wzorzec Singleton
    /// </summary>
    public sealed class ClientConnection
    {
        public IPAddress serverIpAddress;
        public int portNumber;
        public string clientName;
        public static Dictionary<string, string> TypeOfReceivedServerMessage;
        private static ClientConnection single_oInstance = null;
        private NetworkStream networkStream;
        private TcpClient tcpClient;
        private TestFile testFile;

        public static ClientConnection Instance
        {
            get
            {
                if (single_oInstance == null)
                    single_oInstance = new ClientConnection();
                return single_oInstance;
            }
        }

        internal void InitializeInstance(string client_name, IPAddress ip_address, int port_number)
        {
            clientName = client_name;
            serverIpAddress = ip_address;
            portNumber = port_number;
        }

        ~ClientConnection()
        {
            if (networkStream != null)
                networkStream.Dispose();

            if (tcpClient != null && tcpClient.Connected)
            {
                tcpClient.Close();          
            }
        }

        public static void InitializeDictionary()
        {
            if (TypeOfReceivedServerMessage == null)
            {
                TypeOfReceivedServerMessage = new Dictionary<string, string>();
                TypeOfReceivedServerMessage.Add("Response To Logging", "All OK");
                TypeOfReceivedServerMessage.Add("ResponseToSendingReport", "Report is Ok");
                TypeOfReceivedServerMessage.Add("GettingTest", "Test was sent");
            }
        }

        internal bool Connect()
        {
            bool returnedValue;
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(serverIpAddress, portNumber);
                networkStream = tcpClient.GetStream();
                Byte[] sendBytes = null;
                sendBytes = Encoding.UTF8.GetBytes(clientName);
                networkStream.Write(sendBytes, 0, sendBytes.Length);
                networkStream.Flush();
                GetServerResponse(TypeOfReceivedServerMessage["Response To Logging"]);
                returnedValue = true;
            }
            catch(SocketException)
            {
                MessageBox.Show("Wystąpił błąd podczas nawiązywania połączenia");
                returnedValue = false;
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Z jakichś względów wysłany do serwera bufor jest pusty, spróbuj ponownie");
                returnedValue = false;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Z jakichś względów wysłany do serwera bufor jest zbyt duży, spróbuj ponownie");
                returnedValue = false;
            }
            catch (IOException)
            {
                MessageBox.Show("Wybrane gniazdo zostało wcześniej zamknięte. Poinformuj nauczyciela o zaistniałej sytuacji.");
                returnedValue = false;
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Problem z odczytem danych z sieci. Ponownie spróbuj nawiązać połączenie");
                returnedValue = false;
            }
            return returnedValue;
        }

        public void GetServerResponse(string desireFeedback)
        {
            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;

            try
            {
                networkStream = tcpClient.GetStream();
            
                if (networkStream.CanRead)
                {
                    do
                    {
                        numberOfBytesRead = networkStream.Read(myReadBuffer, 0, myReadBuffer.Length);// myReadBuffer.Length);
                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                    } while (networkStream.DataAvailable);
                    networkStream.Flush();

                    if (!(myCompleteMessage.ToString() == desireFeedback))
                        MessageBox.Show("Problem z odebraniem wszystkich danych z serwera");
                }
                else
                {
                    MessageBox.Show("Zaistniały problemy z komunikacją z klientem");
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Bufor wysłany przez serwer jest pusty");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Serwer wysłał za duży bufor danych");
            }
            catch (IOException)
            {
                MessageBox.Show("Wybrane gniazdo zostało wcześniej zakmnięte");
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Problem z odczytem danych z sieci");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        internal Task WaitForTest()
        {
            Task task = Task.Run(() =>
            {
                while (networkStream.DataAvailable == false) ;
            });
            return task;
        }

        internal Task WaitForConfirmation()
        {
            Task task = Task.Run(() =>
            {
                while (networkStream.DataAvailable == false) ;
            });
            return task;
        }

        internal TestFile GetTestFromServer(string clientName)
        {
            Int64 catchedBytes = 0;
            int countOfBytes;
            var buffer = new byte[1024 * 8];

            try
            {
                networkStream.Read(buffer, 0, 8);
                Int64 numberOfBytes = BitConverter.ToInt64(buffer, 0);
                testFile = new TestFile(clientName);
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(testFile.Dispose);//(testFile.Dispose);
                Directory.CreateDirectory(testFile.unzippedTestPath);
                using (var fileWithTest = File.Create(testFile.zippedTestPath))
                {
                    while (catchedBytes < numberOfBytes && (countOfBytes = networkStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fileWithTest.Write(buffer, 0, countOfBytes);
                        catchedBytes += countOfBytes;
                    }
                    networkStream.Flush();
                    testFile.fileWithTest = fileWithTest;
                }

                MessageBoxResult result = MessageBox.Show("Test został pomyślnie ściągnięty z Serwera", "Rozpocznij Test", MessageBoxButton.OKCancel);
                if (!(result == MessageBoxResult.OK))
                {
                    testFile.Dispose(null, null);
                    testFile = null;
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Bufor wysłany przez serwer jest pusty");
                testFile.Dispose(null, null);
                testFile = null;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Serwer wysłał za duży bufor danych");
                testFile.Dispose(null, null);
                testFile = null;
            }
            catch (IOException)
            {
                MessageBox.Show("Wybrane gniazdo zostało wcześniej zakmnięte lub wystąpił problem podczas zapisywania pliku z testem na dysku twardym");
                testFile.Dispose(null, null);
                testFile = null;
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Problem z odczytem danych z sieci");
                testFile.Dispose(null, null);
                testFile = null;
            }
            return testFile;
        }

        internal Task SendHtmlReport()
        {
            Task task = Task.Run(() =>
            {
                using (var fileIO = File.OpenRead(Report.ReportGenerator.Instance.reportPath))
                {
                    try
                    {
                        Instance.networkStream.Write(BitConverter.GetBytes(fileIO.Length), 0, 8);
                        var buffer = new byte[1024 * 8];
                        int count;

                        while ((count = fileIO.Read(buffer, 0, buffer.Length)) > 0)
                            Instance.networkStream.Write(buffer, 0, count);

                        Instance.networkStream.Flush();
                    }
                    catch (ArgumentNullException)
                    {
                        MessageBox.Show("Z niewiadomych przyczyn bufor, w którym powinien się znajdować generowany raport, jest pusty");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Wysyłany raport jest zbyt duży");
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Wybrane gniazdo po stronie klienta zostało wcześniej zakmnięte");
                    }
                    catch (ObjectDisposedException)
                    {
                        MessageBox.Show("Problem z odczytem danych z sieci");
                    }
                }
            });
            return task;
        }
    }
}
