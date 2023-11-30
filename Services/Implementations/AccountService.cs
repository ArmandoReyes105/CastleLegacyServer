using Database.Model;
using Services.DTO;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System;

namespace Services.Implementations
{
    public partial class AccountService : IAccount
    {

        public int AddAccountWithProfile(Account account, Profile profile)
        {
            int band = 0;

            using (var context = new CastleLegacyEntities1())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {   
                    context.Account.Add(account);
                    context.SaveChanges();

                    profile.Id_Account = account.Id_Account;

                    context.Profile.Add(profile);
                    context.SaveChanges();

                    dbContextTransaction.Commit();
                    band = 1;
                }
            }

            return band;
        }

        public int AddFriend(Friend friend)
        {
            int result = 0; 

            using (var db = new CastleLegacyEntities1())
            {
                db.Friend.Add(friend);
                result = db.SaveChanges(); 
                Console.WriteLine(result);
            }

            return result; 
        }

        public int DeleteFriend(int accountId, int friendId)
        {
            int result = 0;
            using (var db = new CastleLegacyEntities1())
            {
                var friend = db.Friend.FirstOrDefault(d => d.Id_Account == accountId && d.Id_AccountFriend == friendId); 
                db.Friend.Remove(friend);
                result = db.SaveChanges(); 
                Console.WriteLine(result);
            }
            return result; 
        }

        public AccountData GetAccountByUsername(string username)
        {

            using (CastleLegacyEntities1 db = new CastleLegacyEntities1())
            {

                AccountData accountData = null;
                var profile = db.Profile.Where(d => d.Username == username).FirstOrDefault();

                if(profile != null)
                {
                    var account = db.Account.Where(d => d.Id_Account == profile.Id_Account).FirstOrDefault();
                    accountData = new AccountData(); 

                    accountData.Id_Account = account.Id_Account;
                    accountData.Password = account.Password;
                    accountData.Email = account.Email;
                    
                }

                return accountData;

            }

        }

        public ProfileData GetProfileByUsername(string username)
        {

            ProfileData profileData;

            using (CastleLegacyEntities1 db = new CastleLegacyEntities1())
            {
                var profile = db.Profile.Where(d => d.Username == username).FirstOrDefault(); 

                profileData = new ProfileData();
                profileData.Id_Profile = profile.Id_Profile;
                profileData.Username = profile.Username;
                profileData.ProfileImage = (int)profile.ProfileImage; 
                profileData.Victories = (int)profile.Victories;
                profileData.Losses = (int)profile.Losses;
                profileData.Id_Account = (int)profile.Id_Account; 

            }

            return profileData; 
        }

        public List<ProfileData> GetAccountFriends(int accoundId)
        {
            List<ProfileData> friends = new List<ProfileData>();

            using (CastleLegacyEntities1 db = new CastleLegacyEntities1())
            {
                var friendsId = db.Friend.Where(d => d.Id_Account == accoundId); 

                foreach (var friendId in friendsId)
                {
                    var profile = db.Profile.Where(d => d.Id_Profile == friendId.Id_AccountFriend).FirstOrDefault();

                    ProfileData friendData = new ProfileData();
                    friendData.Id_Profile = profile.Id_Profile; 
                    friendData.Username = profile.Username;
                    friendData.ProfileImage = (int)profile.ProfileImage;
                    friendData.Victories = (int)profile.Victories;
                    friendData.Losses = (int)profile.Losses; 
                    friendData.Id_Account = (int)profile.Id_Account;

                    friends.Add(friendData); 
                }
            }

            return friends; 
        }

        public List<ProfileData> SearchProfiles(string username)
        {
            List<ProfileData> users = new List<ProfileData>();

            using (CastleLegacyEntities1 db = new CastleLegacyEntities1())
            {
                var profiles = db.Profile.Where(d => d.Username.Contains(username)).ToList();

                foreach (var profile in profiles)
                {
                    
                    ProfileData friendData = new ProfileData();
                    friendData.Id_Profile = profile.Id_Profile;
                    friendData.Username = profile.Username;
                    friendData.ProfileImage = (int)profile.ProfileImage;
                    friendData.Victories = (int)profile.Victories;
                    friendData.Losses = (int)profile.Losses;
                    friendData.Id_Account = (int)profile.Id_Account;

                    users.Add(friendData);
                }
            }

            return users;
        }

    }

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.PerSession)]
    public partial class AccountService : ISession
    {

        private static Dictionary<string, ISessionCallback> currentUsers = new Dictionary<string, ISessionCallback>();

        public void SavePlayerSession(string name)
        {

            ISessionCallback callback = OperationContext.Current.GetCallbackChannel<ISessionCallback>();
            currentUsers.Add(name, callback);

            foreach (var user in new List<string>(currentUsers.Keys))
            {
                if(user != name)
                {
                    try
                    {
                        currentUsers[user].NotifyConnection(name);
                    }
                    catch (ObjectDisposedException ex)
                    {
                        Console.WriteLine($"Error al notificar a {user}: {ex.Message}");
                    }  
                    
                }
            }

        }

        public void DisconectSession(string name)
        {
            currentUsers.Remove(name);
            foreach (var user in new List<string>(currentUsers.Keys))
            {
                try
                {
                    currentUsers[user].NotifyDisconnection(name);
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine($"Error al notificar a {user}: {ex.Message}");
                }
            }
        }

        public void UpdateSession(string name)
        {
            ISessionCallback callback = OperationContext.Current.GetCallbackChannel<ISessionCallback>(); 
            currentUsers[name] = callback;
        }

        public bool IsFriendActive(string username)
        {
            return currentUsers.ContainsKey(username); 
        }
    }
}
