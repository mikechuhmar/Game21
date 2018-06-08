using GameClient.ServiceGame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Threading;
using System.Windows;



namespace GameClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceGame.IServiceGameCallback
    {
        bool isConnected = false;
        ServiceGameClient client;
        int ID;
        int countCard = 0;
        bool CheckConnectIsBegin = false;
        Thread thread;
        public class card
        {
            public string Достоинство { get; set; }
            public string Масть { get; set; }
            
        }
        //List<card> cards = new List<card>();
        ObservableCollection<card> cards = new ObservableCollection<card>();
        int secDT(DateTime a)
        {
            return ((a.Day * 24 + a.Hour) * 60 + a.Minute) * 60 + a.Second;
        }
        void Nothing()
        {

        }
        void CheckConnect()
        {
            if (!CheckConnectIsBegin)
            {
                
                CheckConnectIsBegin = true;
                Thread.Sleep(20000);
                client = new ServiceGameClient(new InstanceContext(this));
                try
                {
                    client.CheckConnectClientToHost();
                }
                catch
                {
                    client.Close();
                    this.Close();
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        void ReconnectingPrepare()
        {
            
            client = null;
            tbUserName.IsEnabled = true;
            bConnDisconn.Content = "Подключиться";
            isConnected = false;
            bReqCard.IsEnabled = false;
            bStopRequest.IsEnabled = false;
            bPrepare.IsEnabled = false;
            List<string> empty = new List<string>();
            lbPlayerList.ItemsSource = empty;
            lbPlayerList.Items.Refresh();
        }
        void ConnectUser()
        {
            if (!isConnected)
            {
                try
                {

                    client = new ServiceGameClient(new InstanceContext(this));
                    
                    
                    if (client.JoinPossible())
                    {

                        ID = client.Connect(tbUserName.Text);
                        tbUserName.IsEnabled = false;
                        bConnDisconn.Content = "Отключиться";
                        isConnected = true;
                        bPrepare.IsEnabled = true;
                    }

                }
                catch
                {

                }
            }
        }

        void DisconnectUser()
        {
            
            try
            {

                client.CheckConnectClientToHost();
                thread = new Thread(Nothing);
                client.Disconnect(ID);
                ReconnectingPrepare();
            }
            catch
            {
                if (!CheckConnectIsBegin)
                {
                    thread = new Thread(CheckConnect);
                    thread.Start();
                }                    

            }
                
            

        }
        public void MsgCallback(string msg)
        {
            lbInfo.Items.Add(msg);
            
            lbInfo.ScrollIntoView(lbInfo.Items[lbInfo.Items.Count - 1]);
        }

        private void bConnDisconn_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
                bPrepare.IsEnabled = false;
            }
            else
            {
                ConnectUser();
                
            }
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                DisconnectUser();
            }
            catch
            {

            }
        }
        
        private void bPrepare_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                client.CheckConnectClientToHost();
                thread = new Thread(Nothing);
                client.Prepare(ID);
                bPrepare.IsEnabled = false;

                
            }
            catch
            {
                if (!CheckConnectIsBegin)
                {
                    thread = new Thread(CheckConnect);
                    thread.Start();
                }
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //dgUserCard.ItemsSource.
            bPrepare.IsEnabled = false;
            bReqCard.IsEnabled = false;
            bStopRequest.IsEnabled = false;
        }

        public void NewCurrentUserCallback()
        {
            bReqCard.IsEnabled = true;
            bStopRequest.IsEnabled = true;
            
        }

        public void NewCardCallback(string dignity, string suit, int points)
        {
            
            cards.Add(new card() { Достоинство = dignity, Масть = suit});

            dgUserCard.ItemsSource = cards;
            dgUserCard.Items.Refresh();
            lPoints.Content = "Сумма Ваших очков: " + points;
            countCard++;
        }

        private void bReqCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                client.CheckConnectClientToHost();
                thread = new Thread(Nothing);
                RequestCardUser();
            }
            catch
            {
                if (!CheckConnectIsBegin)
                {
                    thread = new Thread(CheckConnect);
                    thread.Start();
                }

            }
           
        }

        void RequestCardUser()
        {
            if (client.IsCurrent(ID))
            {
                client.RequestCard(ID);
                if (countCard == 6)
                {
                    bReqCard.IsEnabled = false;
                    client.StopRequestCard(ID);
                    bStopRequest.IsEnabled = false;
                }
            }
            else
            {
                bStopRequest.IsEnabled = false;
                bReqCard.IsEnabled = false;
            }
        }            
 
        private void bStopRequest_Click(object sender, RoutedEventArgs e)
        {           
            
            try
            {

                client.CheckConnectClientToHost();
                thread = new Thread(Nothing);
                StopRequestCardUser();
            }
            catch
            {
                if (!CheckConnectIsBegin)
                {
                    thread = new Thread(CheckConnect);
                    thread.Start();
                }
            }
        }
        void StopRequestCardUser()
        {
            if (client.IsCurrent(ID))
            {
                bReqCard.IsEnabled = false;

                client.StopRequestCard(ID);
                bStopRequest.IsEnabled = false;
            }

            else
            {
                bStopRequest.IsEnabled = false;
                bReqCard.IsEnabled = false;
            }
        }       

        public void GameFinishCallback()
        {
            cards = new ObservableCollection<card>();
            dgUserCard.ItemsSource = cards;
            dgUserCard.Items.Refresh();
            
        }

        public void NewGameCallback()
        {
            countCard = 0;
            bPrepare.IsEnabled = true;
            lPoints.Content = "";
            
        }

       

        public void AmountCoinsCallback(int coins)
        {
            lUserCoins.Content = "Количество Ваших монет: " + coins;
        }      



        public void PlayerListCallback(string[] playerList)
        {
            lbPlayerList.ItemsSource = playerList;
            lbPlayerList.Items.Refresh();
        }

        public int Pong()
        {
 
            return 1;
        }

        public void CheckConnectCallback()
        {
            
        }
    }
}
