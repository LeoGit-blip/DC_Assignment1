using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class PrivateMessage
    {
        private string initPlayer;
        private string destPlayer;
        private List<string> messages = new List<string>();
        private List<int> file = new List<int>();

        [DataMember]
        public string InitPlayer
        {
            get { return initPlayer; }
            set { initPlayer = value; }
        }

        [DataMember]
        public string DestPlayer
        {
            get { return destPlayer; }
            set { destPlayer = value; }
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