
using System.Collections.Generic;
using System.ServiceModel;

namespace WCF_Game21
{
    //Интерфейс для запросов
    [ServiceContract(CallbackContract = typeof(IServerGameCallback))]
    public interface IServiceGame
    {
        
        [OperationContract]
        int Connect(string name);

        [OperationContract(IsOneWay = true)]
        void Disconnect(int id);
        [OperationContract]
        void SendMessage(string msg, int id);
        [OperationContract(IsOneWay = true)]
        void Prepare(int id);
        [OperationContract(IsOneWay = true)]
        void RequestCard(int id);
        [OperationContract(IsOneWay = true)]
        void StopRequestCard(int id);
        [OperationContract]
        bool IsCurrent(int id);
        [OperationContract]
        void Ping(int id);
        [OperationContract]
        bool IsConnect(int id);
        [OperationContract]
        bool JoinPossible();
        [OperationContract]
        void CheckConnectClientToHost();
        

    }
    //Интерфейс для обратных вызовов
    public interface IServerGameCallback
    {
        
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg);
        [OperationContract(IsOneWay = true)]
        void NewCurrentUserCallback();
        [OperationContract(IsOneWay = true)]
        void NewCardCallback(string dignity, string suit, int points);
        [OperationContract(IsOneWay = true)]
        void GameFinishCallback();
        [OperationContract(IsOneWay = true)]
        void NewGameCallback();
        [OperationContract(IsOneWay = true)]
        void AmountCoinsCallback(int coins);
        [OperationContract(IsOneWay = true)]
        void PlayerListCallback(List<string> playerList);
        [OperationContract]
        int Pong();
        [OperationContract(IsOneWay = true)]
        void CheckConnectCallback();
    }
}
