using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace WCF_Game21
{
    //Класс реализации сервиса IServiceGame
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceGame : IServiceGame
    {
        List<User> usersInGame = null;
        CardDeck cardDeck = new CardDeck(52);
        int nextId = 1;
        int currentUserId;
        bool isGameStart = false;
        Thread threadWait;
        List<int> discID = new List<int>();
        bool stopThreadWait = true;
        bool startThreadWait = false;
        void Nothing()
        {

        }

        int secDT(DateTime a)
        {
            return ((a.Day * 24 + a.Hour) * 60 + a.Minute) * 60 + a.Second;
        }

        List<string> playerList
        {
            get
            {
                List<string> list = new List<string>();
                list.Add("Количество человек в игре: " + usersInGame.Count + " из 6");
                foreach (var item in usersInGame)
                {
                    list.Add(item.Name);

                }
                return list;
            }
        }

        void SendPlayerList()
        {
            foreach (var item in usersInGame)
            {
                item.operationContext.GetCallbackChannel<IServerGameCallback>().PlayerListCallback(playerList);
            }
        }

        public int Connect(string name)
        {
            if (usersInGame == null)
                usersInGame = new List<User>();
            User user = new User()
            {
                ID = nextId,
                Name = name,
                operationContext = OperationContext.Current,                
            };
            nextId++;            
            SendMessage(": " + user.Name + " подключился к игре!", 0);
            usersInGame.Add(user);
            Thread thread = new Thread(SendPlayerList);
            thread.Start();
            return user.ID;
        }
        

        public void Disconnect(int id)
        {
            var user = usersInGame.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                if (user.ID == currentUserId)
                    StopRequestCard(user.ID);
                usersInGame.Remove(user);
                SendMessage(": " + user.Name + " покинул игру!", 0);
                SendPlayerList();
            }
        }

        public bool IsCurrent(int id)
        {
            if (id == currentUserId)
                return true;
            else
                return false;

        }   
        
        public void Prepare(int id)
        {
            Thread.Sleep(500);
            var user = usersInGame.FirstOrDefault(i => i.ID == id);
            user.IsPrepare = true;
            SendMessage(": " + user.Name + " готов к игре!", 0);
            if(usersInGame.Count >= 2)
            {
                if (usersInGame.All(x => x.IsPrepare == true))
                {
                    SendMessage(": Все игроки готовы к игре!", 0);
                    //SendMessage(": LOL", 0);
                    currentUserId = usersInGame.FirstOrDefault().ID;
                    cardDeck.Mix();
                    if (!isGameStart)
                    {
                        
                        isGameStart = true;
                        SendMessage(": Игра началась!", 0);
                        foreach(var item in usersInGame)
                        {
                            item.operationContext.GetCallbackChannel<IServerGameCallback>().AmountCoinsCallback(item.Coins);
                        }
                        user = usersInGame.FirstOrDefault(i => i.ID == currentUserId);
                        SendMessage(": Очередь игрока " + user.Name, 0);
                        user.operationContext.GetCallbackChannel<IServerGameCallback>().NewCurrentUserCallback();
                        startThreadWait = true;
                        stopThreadWait = false;
                        RequestCard(currentUserId);
                        
                    }
                    
                }
            }
        }
        void waitingPlayerStep()
        {
            //if (isGameStart)
            {
                DateTime start = DateTime.Now;
                var user = usersInGame.FirstOrDefault(i => i.ID == currentUserId);
                while (secDT(DateTime.Now) - secDT(start) < 20 && !stopThreadWait)
                {

                }
                if (!stopThreadWait)
                    SendMessage(": Время хода " + user.Name + " закончилось", 0);
                StopRequestCard(currentUserId);
                
            }
        }
        void WaitThread()
        {
            if (isGameStart)
            {
                Thread thread = new Thread(waitingPlayerStep);
                thread.Start();
            }
        }
        public void RequestCard(int id)
        {
            if (startThreadWait)
            {              

                startThreadWait = false;
            }
            else
            {
                stopThreadWait = true;

                stopThreadWait = false;
            }
            threadWait = new Thread(waitingPlayerStep);
            var user = usersInGame.FirstOrDefault(i => i.ID == id);
            //var card = cardDeck.Cards.Last();
            var card = cardDeck.Cards.Last(x => cardDeck.Cards.IndexOf(x) < cardDeck.Cards.IndexOf(cardDeck.Cards.Last()));
            user.cards.Add(card);
            cardDeck.Cards.Remove(card);
            SendMessage(": " + user.Name + " получил карту", 0);
            user.operationContext.GetCallbackChannel<IServerGameCallback>().NewCardCallback(card.dignity.ToString(), card.suit.ToString(), user.amountPoints());
            
            threadWait.Start();
        }
        
        public void SendMessage(string msg, int id)
        {
            foreach (var item in usersInGame)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = usersInGame.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }
                answer += msg;
                item.operationContext.GetCallbackChannel<IServerGameCallback>().MsgCallback(answer);
            }
        }
        
        void ForcedDisconnect(int id)
        {
            var user = usersInGame.FirstOrDefault(i => i.ID == id);
            SendMessage(": " + user.Name + " был отключён из-за проблем с сетевым подключением", 0);
            usersInGame.Remove(user);
        }
        void CheckConnectClients()
        {
            foreach (var item in usersInGame)
            {
                try
                {
                    item.operationContext.GetCallbackChannel<IServerGameCallback>().CheckConnectCallback();
                    item.InGame = true;
                }
                catch
                {
                    item.InGame = false;
                }
            }
            foreach (var item in usersInGame)
            {
                if (!item.InGame)
                    ForcedDisconnect(item.ID);
            }
            foreach (var item in usersInGame)
            {
                item.operationContext.GetCallbackChannel<IServerGameCallback>().PlayerListCallback(playerList);
            }

        }
       

        public void StopRequestCard(int id)
        {
            //stopThreadWait = true;

            
            //stopThreadWait = false;
            var user = usersInGame.FirstOrDefault(i => i.ID == id);
            if (user != usersInGame.Last())
            {
                
                currentUserId = usersInGame[usersInGame.IndexOf(user) + 1].ID;
                user = usersInGame.FirstOrDefault(i => i.ID == currentUserId);
                SendMessage(": Очередь игрока " + user.Name, 0);
                user.operationContext.GetCallbackChannel<IServerGameCallback>().NewCurrentUserCallback();
                RequestCard(currentUserId);
                

            }
            else
            {
                stopThreadWait = true;
                startThreadWait = false;
                isGameStart = false;
                foreach (var item in usersInGame)
                {
                    item.IsPrepare = false;
                }                
                SendMessage(": Игра окончена ", 0);
                currentUserId = 0;
                foreach (User item in usersInGame)
                {
                    SendMessage(": " + item.Name + " имеет " + item.amountPoints() + " очков", 0);
                }
                int winPoints = usersInGame.Min(x => Math.Abs(x.amountPoints() - 21));
              
                List<User> winners = (from u in usersInGame
                                     where Math.Abs(u.amountPoints() - 21) == winPoints
                                     select u).ToList();
                foreach(User item in winners)
                {
                    SendMessage(": " + item.Name + " победил", 0);

                }
                
                
                foreach (User item in usersInGame)
                {
                    item.Coins = winners.Contains(item) ? item.Coins + 50 : item.Coins - 50;                  
                  
                }

                CheckConnectClients();
                foreach (User item in usersInGame)
                {
                    item.operationContext.GetCallbackChannel<IServerGameCallback>().AmountCoinsCallback(item.Coins);
                    
                    item.cards = new List<Card>();
                    item.operationContext.GetCallbackChannel<IServerGameCallback>().GameFinishCallback();
                }
                foreach (User item in usersInGame)
                {
      
                    item.operationContext.GetCallbackChannel<IServerGameCallback>().NewGameCallback();
                }
                cardDeck = new CardDeck(52);
                
                //threadWait.Join();
                //threadWait = null;

            }
            
        }

        public void Ping(int id)
        {
            try
            {

            }
            catch
            {
                //SendMessage(": " + user.Name + " был отключён из-за проблем с соединением", 0);
                //discID.Add(user.ID);
            }
        }

        public bool IsConnect(int id)
        {
            return usersInGame.Exists(x => x.ID == id);
        }
        public bool JoinPossible()
        {
            if (usersInGame == null || usersInGame.Count < 6)
                return true;
            else return false;
        }

        public void CheckConnectClientToHost()
        {
           
        }
    }
}
