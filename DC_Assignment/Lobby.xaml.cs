using BusinessServer;
using Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace DC_Assignment
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public delegate void UpdateJoinedRoomListDelegate();
    public delegate void UpdatePublicChatDelegate();
    public delegate void UpdatePrivateChatDelegate();
    public partial class Lobby : Window
    {
        private BServerInterface foob;
        EnterMainWindow enterMainWindow;
        private Thread serverListenerThread;
        private string username;
        private string message;
        private string roomName;
        private string currPmName; //current private message name
        private bool allChat;
        private bool pmChat;
        private UpdateJoinedRoomListDelegate updateJoinedRoomListDelegate;
        private UpdatePublicChatDelegate updatePublicChatDelegate;
        private UpdatePrivateChatDelegate updatePrivateChatDelegate;

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
            updatePrivateChatDelegate += UpdatePrivateChatRoom;
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
                int i = 0;
                bool isFile = false;
                List<int> files = foob.globalFile(roomName);
                foreach (string message in messages)
                {
                    foreach (int loc in files)
                    {
                        if(loc == i)
                        {
                            isFile = true;
                        }
                    }
                    if(isFile)
                    {
                        loadFileServer(message);
                    }
                    else
                    {
                        Public_Chat_Message_Box.AppendText(message);
                        Public_Chat_Message_Box.AppendText(Environment.NewLine);
                    }
                    i++;
                    isFile = false;
                }
            });
        }

        private void UpdatePrivateChatRoom()
        {
            Dispatcher.Invoke(() => 
            { 
                Private_Chat_Message_Box.Document.Blocks.Clear();
                refreshPrivateMessage();
            });
        }

        private void ListenToServer()
        {
            while (true)
            {
                Thread.Sleep(5000);
                updateJoinedRoomListDelegate?.Invoke();
                updatePublicChatDelegate?.Invoke();
                updatePrivateChatDelegate?.Invoke();
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
            refreshPrivateMessage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (allChat)
            {
                if(message.Equals("") || Chat_Message_TextBox.Text.Equals(""))
                {
                    MessageBox.Show("Please enter a message.");
                }
                else
                {
                    string nMessage = username + ":" + message;
                    foob.addGlobalMessage(nMessage, roomName, false);
                    Public_Chat_Message_Box.AppendText(nMessage);
                    Public_Chat_Message_Box.AppendText(Environment.NewLine);
                    Chat_Message_TextBox.Clear();
                }
            }
            else if(pmChat)
            {
                if(currPmName != null)
                {
                    if (message.Equals("") || Chat_Message_TextBox.Text.Equals(""))
                    {
                        MessageBox.Show("Please enter a message.");
                    }
                    else
                    {
                        string nMessage = username + ":" + message;
                        foob.addPM(username, currPmName, nMessage, false);
                        Private_Chat_Message_Box.AppendText(nMessage);
                        Private_Chat_Message_Box.AppendText(Environment.NewLine);
                        Chat_Message_TextBox.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a contact.");
                }
            }
        }

        private void Chat_Message_TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            message = Chat_Message_TextBox.Text;
        }

        private void Joined_Users_List_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(Joined_Users_List.SelectedItem != null)
            {
                currPmName = Joined_Users_List.SelectedItem.ToString();
                refreshPrivateMessage();
            }
            else
            {
                MessageBox.Show("currPmName is null");
            }
        }

        private void refreshPrivateMessage()
        {
            Private_Chat_Message_Box.Document.Blocks.Clear();
            List<string> message = foob.getPm(username, currPmName);
            int i = 0;
            bool isFile = false;
            List<int> files = foob.pmFile(username, currPmName);
            foreach(string s in message)
            {
                foreach(int s2 in files)
                {
                    if (s2==i)
                    {
                        isFile = true;
                    }
                }
                if (isFile)
                {
                    loadFileServer(s);
                }
                else
                {
                    Private_Chat_Message_Box.AppendText(s);
                    Private_Chat_Message_Box.AppendText(Environment.NewLine);
                }
                i++;
                isFile = false;
            }
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            refreshPrivateMessage();
        }

        private void Add_Document_Button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            string filepath = "";
            if(response == true)
            {
                if(allChat)
                {
                    filepath = openFileDialog.FileName;
                    foob.addGlobalMessage(username + ": ", roomName, false);
                    foob.addGlobalMessage(filepath, roomName, true);
                    Public_Chat_Message_Box.AppendText(username + ": ");
                    loadFileServer(filepath);
                }
                else if(pmChat)
                {
                    filepath = openFileDialog.FileName;
                    foob.addPM(username,currPmName, username + ": ",false);
                    foob.addPM(username, currPmName, filepath,true);
                    Private_Chat_Message_Box.AppendText(username + ": ");
                    loadFileServer(filepath);
                }
            }
            else
            {
                MessageBox.Show("No file selected.");
            }
        }

        private void loadFileServer(String filepath)
        {
            string filename = System.IO.Path.GetFileName(filepath);
            Hyperlink hyperlink = new Hyperlink(new Run(filename));
            hyperlink.IsEnabled = true;
            hyperlink.NavigateUri = new Uri(filepath);

            hyperlink.RequestNavigate += Hyperlink_RequestNavigate;

            Paragraph paragraph = new Paragraph(hyperlink);
            paragraph.IsEnabled = true;

            if(allChat)
            {
                Public_Chat_Message_Box.Document.Blocks.Add(paragraph);
                Public_Chat_Message_Box.IsDocumentEnabled = true;
                Public_Chat_Message_Box.IsReadOnly = true;
                Public_Chat_Message_Box.AppendText(Environment.NewLine);
            }
            else if(pmChat)
            {
                Private_Chat_Message_Box.Document.Blocks.Add(paragraph);
                Private_Chat_Message_Box.IsDocumentEnabled = true;
                Private_Chat_Message_Box.IsReadOnly = true;
                Private_Chat_Message_Box.AppendText(Environment.NewLine);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                if (sender is Hyperlink hyperlink)
                {
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = hyperlink.NavigateUri.ToString();
                    process.Start();
                }
            }
            catch (Exception) { throw; }
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            foob.LeaveRoom(username, roomName);
            Joined_Users_List.Items.Clear();
            MessageBox.Show(username + "left the room");
            enterMainWindow.Show();
            this.Close();
        }
    }
}
