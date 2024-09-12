using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL;

namespace DataServer
{
    internal class DataServer : DServerInterface
    {
        private Database foob = Database.Intance;

        //user
        public Player addUserAccountInfo(string username) //create new player
        {
            return foob.addPlayer(username);
        }
        public List<string> getAllPlayer(string roomName)//return all player
        {
            return foob.getParticipants(roomName);
        }

        public Player getUserAccountInfo(string userName) //get user info
        {
            return foob.getPlayerInfo(userName);
        }

        public bool IfLoggedIn(string userName) //check if user login
        {
            return foob.isLoggedIn(userName);
        }

        public bool IsUserNameExist(string userName)// check if name duplicate
        {
            return foob.ifNameExist(userName);
        }
        public void LeaveRoom(string name, string roomName) // leave room
        {
            foob.leave(name, roomName);
        }

        public void login(string userName) //login
        {
            foob.setLogIn(userName);
        }

        public void logout(string userName)//logout
        {
            foob.setLogOut(userName);
        }


        //server
        public void CreateRoom(string name, string roomName)//create server/room
        {
            foob.addNewChatServer(name, roomName);
        }
        public bool addJoinedServer(string userName, string roomName)//check someone is joined room 
        {
            return foob.addJoinedServer(userName, roomName);
        }

        public List<string> getAllRoom(string roomName) //get all server
        {
            return foob.getRoom(roomName);
        }

        public List<string> getJoinedServer(string userName) // return chat room
        {
            return foob.getChatRooms(userName);
        }

        public GlobalMessage getServer(string roomName)// get room
        {
            return foob.getRoomInfo(roomName);
        }

        public int getServerID(string roomName) //get server id
        {
            return foob.getServerID(roomName);
        }

        public bool isRoomExist(string roomName)//check if room exist 
        {
            return foob.checkRoomNameFound(roomName);
        }

        //chat
        public void addGlobalMessage(string message, string roomName, bool isFile) //add chat to message
        {
            foob.addGlobalMessage(message, roomName, isFile);
        }

        public void addPm(string initName, string destName, string message, bool isFile) // pm to other player
        {
            foob.addPM(initName, destName, message, isFile);
        }
        public List<int> globalFile(string roomName)//get file for global message
        {
            return foob.globalFile(roomName);
        }

        public List<string> getGlobalMessage(string roomName) //get message for global message
        {
            return foob.getGlobalMessage(roomName);
        }

        public List<string> getPM(string initName, string destName) //get message for pm
        {
            return foob.getPM(initName, destName);
        }
        public List<int> pmFile(String username, string contactName)//get file for pm
        {
            return foob.pmFile(username, contactName);
        }
    }
}
