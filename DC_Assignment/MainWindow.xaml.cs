using BusinessServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private string username = "";
        private BServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<BServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8200/BusinessService";
            foobFactory = new ChannelFactory<BServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!foob.IsUserNameExist(username))
            {
                foob.addUserAccountInfo(username);
                foob.login(username);
                MessageBox.Show("Registration Success. Log in as "+username+".");
                EnterMainWindow enterMainWindow = new EnterMainWindow(foob, username, this);
                enterMainWindow.Show();
                this.Hide();
            }
            else
            {
                foob.getUserAccountInfo(username);
                if(foob.IfLoggedIn(username))
                {
                    MessageBox.Show("The user is logged in");
                }
                else
                {
                    foob.login(username);
                    MessageBox.Show("Registration Success. Log in as " + username + ".");
                    EnterMainWindow enterMainWindow = new EnterMainWindow(foob, username, this);
                    enterMainWindow.Show();
                    this.Hide();
                }
            }
        }

        private void Add_Client_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            username = UsernameBox.Text;
        }
    }
}
