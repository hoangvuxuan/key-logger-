using game_caro.all_class;
using game_caro.all_class.network;

namespace game_caro
{
    public partial class Form1 : Form
    {

        Chess_board_manager cbm;
        Socket_manager socket_Manager;

        public Form1()
        {
            InitializeComponent();
            progressBar1.Step = Const.countdown_step;
            progressBar1.Maximum = Const.countdown_max;
            timer1.Interval = Const.countdown_interval;
            socket_Manager = new Socket_manager();


            cbm = new Chess_board_manager(panel1 as Panel, textBox1, pictureBox2);
            cbm.draw_chess();
            cbm.Player_click += Cbm_Player_click;
            cbm.Game_done += Cbm_Game_done;
        }

        private void end_game()
        {
            panel1.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            timer1.Stop();
            MessageBox.Show("end game");
        }

        private void quit()
        {
            if (MessageBox.Show("ban co muon thoat khong", "thong bao", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void Cbm_Game_done(object? sender, EventArgs e)
        {

            end_game();
        }

        private void Cbm_Player_click(object? sender, EventArgs e)
        {
            timer1.Start();
            progressBar1.Value = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            if (progressBar1.Value >= progressBar1.Maximum)
            {

                end_game();

            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            timer1.Stop();
            progressBar1.Value = 0;
            undoToolStripMenuItem.Enabled = true;
            cbm.draw_chess();

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cbm.undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quit();
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("ban co muon thoat khong", "thong bao", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }

        }

        public void listen()
        {
            string data = (string)socket_Manager.receive();
            MessageBox.Show(data);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            socket_Manager.IP = textBox2.Text;
            if(!socket_Manager.connect_server())
            {
                socket_Manager.create_verver();
                socket_Manager.send("say something");
                Thread thread = new Thread(() =>
                {
                    while(true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            listen();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    
                });
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                 
                Thread thread = new Thread(() =>
                {
                    listen();
                });
                thread.IsBackground = true;
                thread.Start();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.Text = socket_Manager.get_local_IPV4(System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211);
            if(string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = socket_Manager.get_local_IPV4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet);
            }
        }
    }
}
