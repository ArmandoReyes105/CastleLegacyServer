using System.Collections.Generic;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract(CallbackContract = typeof(ISessionCallback))]
    public interface ISession
    {
        [OperationContract]
        void SavePlayerSession(string name);

        [OperationContract]
        void DisconectSession(string name); 

        [OperationContract]
        void UpdateSession(string name);

        [OperationContract]
        bool IsFriendActive(string username);
    }

    [ServiceContract]
    public interface ISessionCallback
    {
        [OperationContract]
        void NotifyConnection(string name);

        [OperationContract]
        void NotifyDisconnection(string name); 

    }
}
