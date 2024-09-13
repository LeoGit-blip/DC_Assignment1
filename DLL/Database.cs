using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace DLL
{
    [DataContract]
    public class Database
    {
        [DataMember]
        public int playerID = 1;

        [DataMember]
        public int serverID = 1000;

        [DataMember]
        private List<Player> players;

        [DataMember]
        private List<GlobalMessage> roomChat;

        [DataMember]
        private List<PrivateMessage> pm;

        private List<string> allServers;

        public static Database Intance { get; } = new Database();

        static Database() { }

        private Database()
        {
            players = new List<Player>();
            roomChat = new List<GlobalMessage>();
            pm = new List<PrivateMessage>();
            allServers = new List<string>();
        }

        [DataMember]
        public List<string> AllServers
        {
            get { return allServers; }
            set { allServers = value; }
        }

        //player
        public Player addPlayer(string userName) //add player
        {
            Player newPlayer = new Player();
            newPlayer.userName = userName;
            newPlayer.playerID = playerID;
            players.Add(newPlayer);
            playerID++;
            return newPlayer;
        }

        public void setLogIn(string userName) //login
        {
            Player player = getPlayerInfo(userName);
            if (player != null)
            {
                player.LoggedIn = true;
            }
        }

        public void setLogOut(string userName)  //logout
        {
            Player player = getPlayerInfo(userName);
            if (player != null)
            {
                player.LoggedIn = false;
            }
        }

        public bool isLoggedIn(string userName) //check if the user login or not
        {
            Player temp = null;
            foreach (Player player in players)
            {
                if (player.userName.Equals(userName))
                {
                    temp = player;
                }
            }
            return temp.LoggedIn;
        }

        public int getPlayerID(string userName) //get player id
        {
            Player temp = null;
            foreach (Player player in players)
            {
                if (player.userName.Equals(userName))
                {
                    temp = player;
                }
            }
            return temp.playerID;
        }

        public bool ifNameExist(string userName) //check if duplicate name exist
        {
            bool found = false;
            foreach (Player player in players)
            {
                if (player.userName.Equals(userName))
                {
                    found = true;
                }
            }
            return found;
        }

        public void leave(string userName, string roomName)//leave room
        {
            Player tempPlayer = getPlayerInfo(userName);
            var room = getRoomInfo(roomName);
            if (tempPlayer != null && room != null)
            {
                room.AllPlayers.Remove(tempPlayer.userName);
                addMessages($"System: {tempPlayer.userName} has left the room.", room);
                updataRoomInfo(room);
            }
        }

        public Player getPlayerInfo(string userName)//get player info
        {
            Player temp = null;
            foreach (Player player in players)
            {
                if (player.userName.Equals(userName))
                {
                    temp = player;
                }
            }
            return temp;
        }

        public void updateUserAccountInfo(Player currentUser)//update user account
        {
            List<Player> temp = players;
            for (int i = 0; i < players.Count; i++)
            {
                if (temp[i].playerID.Equals(currentUser.playerID))
                {
                    players.RemoveAt(i);
                }
            }
            players.Add(currentUser);
        }

        private bool checkJoined(string username, GlobalMessage room)//check if player inside room or not
        {
            bool joined = false;
            foreach (string user in room.AllPlayers)
            {
                if (user.Equals(username))
                {
                    joined = true;
                }
            }
            return joined;
        }

        public List<string> getParticipants(string roomName)//get all players
        {
            GlobalMessage room = getRoomInfo(roomName);
            return room.AllPlayers;
        }

        //server
        public List<string> getRoom(string userName) //return list of player in the room
        {
            Player temp = null;
            foreach (Player player in players)
            {
                if (player.userName.Equals(userName))
                {
                    temp = player;
                }
            }
            return temp.Room;
        }

        public void addNewChatServer(string userName, string roomName)//add new room
        {
            GlobalMessage newRoom = new GlobalMessage();
            newRoom.RoomName = roomName;
            newRoom.RoomID = serverID;

            Player host = getPlayerInfo(userName);
            List<string> newList = new List<string>();
            newList.Add(userName);
            newRoom.AllPlayers = newList;
            addMessages($"System: {host.userName} has joined the room.", newRoom);
            roomChat.Add(newRoom);
            allServers.Add(roomName);

            List<string> newRoomList = host.Room;
            newRoomList.Add(roomName);
            host.Room = newRoomList;

            serverID++;
        }

        public int getServerID(string roomName)//get id for server
        {
            GlobalMessage temp = null;
            foreach (GlobalMessage room in roomChat)
            {
                if (room.RoomName.Equals(roomName))
                {
                    temp = room;
                }
            }
            return temp.RoomID;
        }
        public bool checkRoomNameFound(string roomName)
        {
            bool found = false;

            foreach (String room in allServers)
            {
                if (room.Equals(roomName))
                {
                    found = true;
                }
            }
            return found;
        }

        public GlobalMessage getRoomInfo(String roomName)//get information of room
        {
            GlobalMessage temp = null;
            foreach (GlobalMessage room in roomChat)
            {
                if (room.RoomName.Equals(roomName))
                {
                    temp = room;
                }
            }
            return temp;
        }
        public void updataRoomInfo(GlobalMessage currentRoom) //update information inside room
        {
            List<GlobalMessage> temp = roomChat;
            for (int i = 0; i < allServers.Count; i++)
            {
                if (temp[i].RoomName.Equals(currentRoom.RoomName))
                {
                    allServers.RemoveAt(i);
                    roomChat.RemoveAt(i);
                }
            }
            roomChat.Add(currentRoom);
            allServers.Add(currentRoom.RoomName);
        }

        //chat
        private void addMessages(string message, GlobalMessage room) // check to add message
        {
            List<string> messageList = room.Messages;
            messageList.Add(message);
            room.Messages = messageList;
            Console.WriteLine(message);
        }
        public void addGlobalMessage(string message, string roomName, bool file)
        {
            GlobalMessage room = getRoomInfo(roomName);
            addMessages(message, room);
            if (file)
            {
                int locNo = room.Messages.Count - 1;
                List<int> temp = room.File;
                temp.Add(locNo);
            }
            updataRoomInfo(room);
        }
        public List<string> getGlobalMessage(String roomName)//get global message
        {
            GlobalMessage room = getRoomInfo(roomName);
            return room.Messages;
        }

        public bool addJoinedServer(string userName, string roomName)//add joined server
        {
            bool joined = false;
            Player player = getPlayerInfo(userName);
            GlobalMessage room = getRoomInfo(roomName);
            if (checkJoined(userName, room))
            {
                MessageBox.Show("The player " + userName + "has already joined in this server.");
            }
            else 
            {
                List<string> newList = room.AllPlayers;
                newList.Add(userName);
                room.AllPlayers = newList;
                addMessages("System: " + userName + " has joined the chat.", room);
                joined = true;
            }
            List<string> newroomList = player.Room;
            newroomList.Add(roomName);
            player.Room = newroomList;
            updateUserAccountInfo(player);
            //updataRoomInfo(room);
            return joined;
        }


        public List<string> getChatRooms(string userName)//return list of room
        {
            Player temp = null;
            foreach (Player player in players)
            {
                if (player.userName.Equals(userName))
                {
                    temp = player;
                }
            }
            return temp.Room;
        }
        private void updatePMList(PrivateMessage updated)
        {
            List<PrivateMessage> newList = pm;
            for (int i = 0; i < pm.Count; i++)
            {
                if (pm[i].InitPlayer.Equals(updated.InitPlayer))
                {
                    if (pm[i].DestPlayer.Equals(updated.DestPlayer))
                    {
                        newList.RemoveAt(i);
                    }
                }
                else if (pm[i].InitPlayer.Equals(updated.DestPlayer))
                {
                    if (pm[i].DestPlayer.Equals(updated.InitPlayer))
                    {
                        newList.RemoveAt(i);
                    }
                }
                else if (pm[i].DestPlayer.Equals(updated.InitPlayer))
                {
                    if (pm[i].InitPlayer.Equals(updated.DestPlayer))
                    {
                        newList.RemoveAt(i);
                    }
                }
                else if (pm[i].DestPlayer.Equals(updated.DestPlayer))
                {
                    if (pm[i].InitPlayer.Equals(updated.InitPlayer))
                    {
                        newList.RemoveAt(i);
                    }
                }
            }
            pm = newList;
            pm.Add(updated);
        }
        public void addPM(string username, string contactName, string message, bool isFile)
        {
            PrivateMessage conversation = null; //the conversation
            List<string> messageList;
            foreach (PrivateMessage pm in pm)
            {
                if (pm.InitPlayer.Equals(username))
                {
                    if (pm.DestPlayer.Equals(contactName))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.InitPlayer.Equals(contactName))
                {
                    if (pm.DestPlayer.Equals(username))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.DestPlayer.Equals(contactName))
                {
                    if (pm.InitPlayer.Equals(username))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.DestPlayer.Equals(username))
                {
                    if (pm.InitPlayer.Equals(contactName))
                    {
                        conversation = pm;
                    }
                }
            }
            if (conversation == null) //the conversation havent exist before
            {
                conversation = new PrivateMessage();
                conversation.InitPlayer = username;
                conversation.DestPlayer = contactName;
                messageList = new List<string>();
                messageList.Add(message);
                pm.Add(conversation);
            }
            else //conversation between two parties already exist
            {
                messageList = conversation.Messages;
                messageList.Add(message);
                updatePMList(conversation);
            }
            if (isFile)
            {
                int locNo = conversation.Messages.Count - 1;
                List<int> temp = conversation.File;
                temp.Add(locNo);
                conversation.File = temp;
            }
            conversation.Messages = messageList;
        }
        public List<string> getPM(string username, string contactName)
        {
            List<string> messageList;
            PrivateMessage conversation = null;
            foreach (PrivateMessage pm in pm)
            {
                if (pm.InitPlayer.Equals(username))
                {
                    if (pm.DestPlayer.Equals(contactName))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.InitPlayer.Equals(contactName))
                {
                    if (pm.DestPlayer.Equals(username))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.DestPlayer.Equals(contactName))
                {
                    if (pm.InitPlayer.Equals(username))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.DestPlayer.Equals(username))
                {
                    if (pm.InitPlayer.Equals(contactName))
                    {
                        conversation = pm;
                    }
                }
            }
            if (conversation != null)
            {
                messageList = conversation.Messages;
            }
            else
            {
                messageList = new List<string>();
            }
            return messageList;
        }

        public List<int> globalFile(string roomName)
        {
            GlobalMessage room = getRoomInfo(roomName);
            return room.File;
        }

        public List<int> pmFile(String username, string contactName)
        {
            List<int> fileLoc;
            PrivateMessage conversation = null;
            foreach (PrivateMessage pm in pm)
            {
                if (pm.InitPlayer.Equals(username))
                {
                    if (pm.DestPlayer.Equals(contactName))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.InitPlayer.Equals(contactName))
                {
                    if (pm.DestPlayer.Equals(username))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.InitPlayer.Equals(contactName))
                {
                    if (pm.DestPlayer.Equals(username))
                    {
                        conversation = pm;
                    }
                }
                else if (pm.DestPlayer.Equals(username))
                {
                    if (pm.InitPlayer.Equals(contactName))
                    {
                        conversation = pm;
                    }
                }
            }
            if (conversation == null)
            {
                fileLoc = new List<int>();
            }
            else
            {
                fileLoc = conversation.File;
            }
            return fileLoc;
        }
    }
}


