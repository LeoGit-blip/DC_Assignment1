using DLL;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace DataServer
{
    [ServiceContract]
    public interface DServerInterface
    {
        //player
        [OperationContract]
        bool IsUserNameExist(string UserNme); //check if one room have 2 same name

        [OperationContract]
        void login(String userName); //player login

        [OperationContract]
        void logout(String userName);//player logout

        [OperationContract]
        bool IfLoggedIn(String userName);//if player login

        [OperationContract]
        Player addUserAccountInfo(string username);//add new user

        [OperationContract]
        Player getUserAccountInfo(string userName);//get user info

        [OperationContract]
        List<string> getAllPlayer(string roomName);//get all players list

        //rooom
        [OperationContract]
        void CreateRoom(string name, string roomName);//create room

        [OperationContract]
        void LeaveRoom(string name, string roomName);//leave room

        [OperationContract]
        bool isRoomExist(string roomName);//check if room exist

        [OperationContract]
        List<string> getJoinedServer(string userName);//join server

        [OperationContract]
        List<string> GetAllRoom(); //display all room name

        [OperationContract]
        GlobalMessage getServer(string roomName);//get server

        [OperationContract]
        bool addJoinedServer(string userName, string roomName);//check if joined server

        [OperationContract]
        int getServerID(string roomName);//get server id

        //chat
        [OperationContract]
        List<int> globalFile(String roomName);//get file for global message

        [OperationContract]
        void addGlobalMessage(String message, String roomName, bool file);//add global message

        [OperationContract]
        List<string> getGlobalMessage(String roomName);//get global message

        [OperationContract]
        List<string> getPM(string initName, string destName);//get player pm info

        [OperationContract]
        void addPm(string initName, string destName, string message, bool isFile);//add pm message

        [OperationContract]
        List<int> pmFile(string initName, string destName);//get pm file
    }
}