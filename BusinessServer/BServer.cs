using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataServer;
using DLL;

namespace BusinessServer
{
    internal class BServer : BServerInterface
    {
        private DServerInterface foob;

        public BServer()
        {
            ChannelFactory<DServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        //player
        public Player addUserAccountInfo(string username)
        {
            return foob.addUserAccountInfo(username);
        }

        public List<string> getAllPlayer(string roomName)
        {
            return foob.getAllPlayer(roomName);
        }

        public Player getUserAccountInfo(string userName)
        {
            return foob.getUserAccountInfo(userName);
        }

        public bool IfLoggedIn(string userName)
        {
            return foob.IfLoggedIn(userName);
        }

        public bool IsUserNameExist(string userName)
        {
            return foob.IsUserNameExist(userName);
        }
        public void LeaveRoom(string name, string roomName)
        {
            foob.LeaveRoom(name, roomName);
        }

        public void login(string userName)
        {
            foob.login(userName);
        }

        public void logout(string userName)
        {
            foob.logout(userName);
        }

        //server
        public bool addJoinedServer(string userName, string roomName)
        {
            return foob.addJoinedServer(userName, roomName);
        }
        public void CreateRoom(string name, string roomName)
        {
            foob.CreateRoom(name, roomName);
        }
        public List<string> GetAllRoom()
        {
            return foob.GetAllRoom();
        }

        public List<string> getJoinedServer(string userName)
        {
            return foob.getJoinedServer(userName);
        }

        public GlobalMessage getServer(string roomName)
        {
            return foob.getServer(roomName);
        }

        public int getServerID(string roomName)
        {
            return foob.getServerID(roomName);
        }

        public bool isRoomExist(string roomName)
        {
            return foob.isRoomExist(roomName);
        }


        //chat
        public void addGlobalMessage(string message, string roomName, bool isFile)
        {
            foob.addGlobalMessage(message, roomName, isFile);
        }

        public void addPM(string initName, string destName, string message, bool isFile)
        {
            foob.addPm(initName, destName, message, isFile);
        }

        public List<int> globalFile(string roomName)
        {
            return foob.globalFile(roomName);
        }
        public List<string> getGlobalMessage(string roomName)
        {
            return foob.getGlobalMessage(roomName);
        }

        public List<string> getPm(string initName, string destName)
        {
            return foob.getPM(initName, destName);
        }

        public List<int> pmFile(String username, string contactName)
        {
            return foob.pmFile(username, contactName);
        }
    }
}