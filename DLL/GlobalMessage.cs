using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DLL
{
    [DataContract]
    public class GlobalMessage
    {
        private string roomName;
        private int roomID;
        private List<string> allPlayers = new List<string> { };
        private List<string> messages = new List<string> { };
        private List<int> file = new List<int>();

        [DataMember]
        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }

        [DataMember]
        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }

        [DataMember]
        public List<string> AllPlayers
        {
            get { return allPlayers; }
            set { allPlayers = value; }
        }

        [DataMember]
        public List<string> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        [DataMember]
        public List<int> File
        {
            get { return file; }
            set { file = value; }
        }
    }
}
