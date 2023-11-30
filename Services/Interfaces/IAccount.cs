using System.Collections.Generic;
using System.ServiceModel;
using Database.Model; 
using Services.DTO;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IAccount
    {
        [OperationContract]
        int AddAccountWithProfile(Account account, Profile profile);

        [OperationContract]
        int AddFriend(Friend friend);

        [OperationContract]
        int DeleteFriend(int accountId ,int friendId);

        [OperationContract]
        AccountData GetAccountByUsername(string username);

        [OperationContract]
        ProfileData GetProfileByUsername(string username);

        [OperationContract]
        List<ProfileData> GetAccountFriends(int accountId);

        [OperationContract]
        List<ProfileData> SearchProfiles(string username);


    }
}
