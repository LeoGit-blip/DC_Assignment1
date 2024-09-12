using DLL;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace BusinessServer
{
    [ServiceContract]
    public interface BServerInterface
    {

        //player
        [OperationContract]
        void LeaveRoom(string name, string roomName);//leave room

        [OperationContract]
        bool IsUserNameExist(string UserNme); //check if one room have 2 same name

        [OperationContract]
        void login(String userName); //player login

        [OperationContract]
        void logout(String userName);//player logout

        [OperationContract]
        bool IfLoggedIn(String userName);//if player login

        [OperationContract]
        List<string> getAllPlayer(string roomName); // return all player

        [OperationContract]
        Player addUserAccountInfo(string username); //add new user

        [OperationContract]
        Player getUserAccountInfo(string userName); //get user info



        //server
        [OperationContract]
        void CreateRoom(string name, string roomName);//create room
        [OperationContract]
        int getServerID(string roomName);//get server id

        [OperationContract]
        bool isRoomExist(string roomName);//check if room exist

        [OperationContract]
        List<string> getJoinedServer(string userName); // get joined server

        [OperationContract]
        List<string> getAllRoom(string roomName); //display all room name

        [OperationContract]
        GlobalMessage getServer(string roomName);// get room

        [OperationContract]
        bool addJoinedServer(string userName, string roomName);//add joined server



        //chat
        [OperationContract]
        void addGlobalMessage(String message, String roomName, bool file); // add message

        [OperationContract]
        List<string> getGlobalMessage(String roomName);// get message

        [OperationContract]
        List<int> globalFile(String roomName); // get file

        [OperationContract]
        void addPM(string initName, string destName, string message, bool isFile); //add pm message

        [OperationContract]
        List<string> getPm(string initName, string destName);//get pm message

        [OperationContract]
        List<int> pmFile(string initName, string destName);//get pm file
    }
}