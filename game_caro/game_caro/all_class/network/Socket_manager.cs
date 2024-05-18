using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
 

namespace game_caro.all_class.network
{
    internal class Socket_manager
    {
        //for client
        Socket client;
        public bool connect_server()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), Convert.ToInt32(port));
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                client.Connect(ipe);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
 
        }
        //end

        //for server
        Socket server;
        public void create_verver()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), Convert.ToInt32(port));
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ipe);
            server.Listen(10);

            Thread accep = new Thread(() =>
                {
                    client = server.Accept();
                }           
            );
            accep.IsBackground = true;
            accep.Start();

            

        }

        //end

        //for both
        public string IP = "127.0.0.1";
        public string port = "2402";
        public bool is_server = true;

        /*lay ipv4 cua card mang dang dung*/
        public string get_local_IPV4(NetworkInterfaceType type)
        {
            string output = "";
            foreach(NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach(UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if(ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }

            return output;
        }

        public byte[] SerializeData(object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(jsonData);
        }

        public object DeserializeData(byte[] data)
        {
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject(jsonData);
        }

        public bool send(object data)
        {
            byte[] sendData = SerializeData(data);

            return send_data(client, sendData);

        }

        public object receive()
        {
            byte[] receiveData = new byte[2048];
            bool is_ok = receive_data(client, receiveData);
            return DeserializeData(receiveData);
        }

        private bool send_data(Socket socket, byte[] data)
        {
            return socket.Send(data) == 1? true: false;
        }

        private bool receive_data(Socket socket, byte[] data)
        {
            return socket.Receive(data) == 1 ? true : false;
        }
        //end
    }
}
