using BusinessServer;
using Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace DC_Assignment
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public delegate void UpdateJoinedRoomListDelegate();
    public delegate void UpdatePublicChatDelegate();
    public partial class Lobby : Window
    {
        private BServerInterface foob;
        EnterMainWindow enterMainWindow;
        private Thread serverListenerThread;
        private string username;
        private string message;
        private string roomName;
        //private string currPmName; //current private message name
        private bool allChat;
        private bool pmChat;
        private UpdateJoinedRoomListDelegate updateJoinedRoomListDelegate;
        private UpdatePublicChatDelegate updatePublicChatDelegate;

        public Lobby(BServerInterface inFoob, string inRoomName, string inUsername, EnterMainWindow inEnterMainWindow)
        {
            InitializeComponent();
            foob = inFoob;
            username = inUsername;
            enterMainWindow = inEnterMainWindow;
            roomName = inRoomName;
            allChat = true;
            pmChat = false;

            Public_Chat_Grid.Visibility = Visibility.Visible;
            Private_Chat_Grid.Visibility = Visibility.Collapsed;

            updateJoinedRoomListDelegate += UpdateJoinedRoomList;
            updatePublicChatDelegate += UpdatePublicChatRoom;
            serverListenerThread = new Thread(ListenToServer);
            serverListenerThread.Start();
        }

        private void UpdateJoinedRoomList()
        {
            Dispatcher.Invoke(() =>
            {
                Joined_Users_List.Items.Clear();
                List<string> users = foob.getAllPlayer(roomName);
                foreach (string user in users)
                {
                    Joined_Users_List.Items.Add(user);
                }
            });
        }

        private void UpdatePublicChatRoom()
        {
            Dispatcher.Invoke(()=>
            {
                Public_Chat_Message_Box.Document.Blocks.Clear();
                List<string> messages = foob.getGlobalMessage(roomName);
                foreach (string message in messages)
                {
                    Public_Chat_Message_Box.AppendText(message);
                    Public_Chat_Message_Box.AppendText(Environment.NewLine);
                }
            });
        }

        private void ListenToServer()
        {
            while (true)
            {
                Thread.Sleep(5000);
                updateJoinedRoomListDelegate?.Invoke();
                updatePublicChatDelegate?.Invoke();
            }
        }
        private void PublicChat_Button_Click(object sender, RoutedEventArgs e)
        {
            allChat = true;
            pmChat = false;
            Public_Chat_Grid.Visibility = Visibility.Visible;
            Private_Chat_Grid.Visibility = Visibility.Collapsed;
        }

        private void PrivateChat_Button_Click(object sender, RoutedEventArgs e)
        {
            allChat = false;
            pmChat = true;
            Public_Chat_Grid.Visibility = Visibility.Collapsed;
            Private_Chat_Grid.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(message);
            if (allChat)
            {
                if(message.Equals("") || Chat_Message_TextBox.Text.Equals(""))
                {
                    MessageBox.Show("Please enter a message.");
                }
                else
                {
                    string nMessage = username + ":" + message;
                    foob.addGlobalMessage(username + ": ", roomName, false);
                    Public_Chat_Message_Box.AppendText(nMessage);
                    Public_Chat_Message_Box.AppendText(Environment.NewLine);
                }
            }
            else if(pmChat)
            {
                if (message.Equals("") || Chat_Message_TextBox.Text.Equals(""))
                {
                    MessageBox.Show("Please enter a message.");
                }
            }
        }

        private void Chat_Message_TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            message = Chat_Message_TextBox.Text;
        }
    }
}
