using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_caro.all_class
{
    public class Chess_board_manager
    {
        private Panel panel;
        private List<Player> playerList;
        private int curent_player;
        private TextBox curent_player_name;
        private PictureBox curent_player_img;
        private List<List<Button>> matrix;
        private Stack<Point> play_timeline;

        public event EventHandler player_click;
        public event EventHandler Player_click
        {
            add
            {
                player_click += value;
            }

            remove
            {
                player_click -= value;
            }
        }

        private event EventHandler game_done;
        public event EventHandler Game_done
        {
            add
            {
                game_done += value;
            }

            remove
            {
                game_done -= value;
            }
        }


        public Chess_board_manager(Panel panel, TextBox tb, PictureBox pic)
        {
            this.panel = panel;
            this.curent_player_name = tb;
            this.curent_player_img = pic;

            this.playerList = new List<Player>() 
            { 
                new Player("play 1", Image.FromFile(Application.StartupPath + "\\picture\\2.jpg")),
                new Player("play 2", Image.FromFile(Application.StartupPath + "\\picture\\download.jpg")),
            };

            
            
        }

        public void draw_chess()
        {
            panel.Enabled = true;
            panel.Controls.Clear();
            curent_player = 0;
            this.play_timeline = new Stack<Point>();
            change_player();


            matrix = new List<List<Button>>();
            Button old = new Button() { Width = 0, Location = new Point(0, 0) };

            for (int i = 0; i < Const.chess_row; i++)
            {
                matrix.Add(new List<Button>());

                for (int j = 0; j < Const.chess_row; j++)
                {
                    Button bt = new Button()
                    {
                        Width = Const.chess_w,
                        Height = Const.chess_h,
                        Location = new Point(old.Location.X + old.Width, old.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString() + "_" + j.ToString(),
                    };

                    matrix[i].Add(bt);

                    bt.Click += Bt_Click;

                    old = bt;
                    panel.Controls.Add(bt);
                }

                old = new Button() { Location = new Point(0, old.Location.Y + Const.chess_h) };
                old.Width = 0;

            }
        }

        public bool undo()
        {
            if(play_timeline.Count == 0)
            {
                return false;
            }
            Point old_point = play_timeline.Pop();
            Button old_bt = matrix[old_point.X][old_point.Y];

            old_bt.BackgroundImage = null;
            curent_player = curent_player == 1 ? 0 : 1;
            change_player();
            return true;
        }

        private void Bt_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b.BackgroundImage != null)
            {
                return;
            }

            change_x_o(b);
            change_player();

            play_timeline.Push(get_point(b));


            if(player_click != null)
            {
                player_click (this, new EventArgs());
            }

            if(is_end(b))
            {
                end_game();
            }

        }
        private void end_game()
        {
            if(game_done != null)
            {
                game_done(this, new EventArgs());
            }
             
            
        }

        private bool is_end(Button button)
        {
  
            return is_end_hori(button)|| is_end_verti(button) || is_end_left(button) || is_end_right(button);

        }

        private Point get_point(Button button)
        {
            
            string s_point = button.Tag.ToString();

            string[] path = s_point.Split('_');
            int hori = Convert.ToInt32(path[0]);
            int verti = Convert.ToInt32(path[1]);
            Point point = new Point(hori, verti);

            
            return point;
        }
        private bool is_end_hori(Button button)
        {
            Point p = get_point(button);
            int count_left = 0;
            for(int i = p.Y; i>= 0; i--)
            {
                if (button.BackgroundImage == matrix[p.X][i].BackgroundImage)
                {
                    count_left++;
                }
                else
                    break;
            }

            int count_right = 0;
            for (int i = p.Y + 1; i < Const.chess_row; i++)
            {
                if (button.BackgroundImage == matrix[p.X][i].BackgroundImage)
                {
                    count_right++;
                }
                else
                    break;
            }

            return count_left + count_right == 5;
        }
        private bool is_end_verti(Button button)
        {
            Point p = get_point(button);
            int count_up = 0;
            for (int i = p.X; i >= 0; i--)
            {
                if (button.BackgroundImage == matrix[i][p.Y].BackgroundImage)
                {
                    count_up++;
                }
                else
                    break;
            }

            int count_down = 0;
            for (int i = p.X + 1; i < Const.chess_row; i++)
            {
                if (button.BackgroundImage == matrix[i][p.Y].BackgroundImage)
                {
                    count_up++;
                }
                else
                    break;
            }

            return count_up + count_down == 5;
        }
        private bool is_end_left(Button button)
        {

            Point p = get_point(button);
            int temp = 0;

            int count_up = 0;
            temp = 0;
            for (int i = p.X; i >= 0; i--)
            {
                if(p.Y - temp < 0)
                {
                    break;
                }

                if (button.BackgroundImage == matrix[i][p.Y - temp].BackgroundImage)
                {
                    count_up++;
                }
                else
                    break;

                temp++;
            }

            int count_down = 0;
            temp = 0;
            for (int i = p.X + 1; i < Const.chess_row; i++)
            {

                temp++;
                if(p.Y + temp >= Const.chess_row)
                {
                    break ;
                }

                if (button.BackgroundImage == matrix[i][p.Y + temp].BackgroundImage)
                {
                    count_down++;
                }
                else
                    break;

                
            }
            return count_up + count_down == 5;
        }
        private bool is_end_right(Button button)
        {
            Point p = get_point(button);
            int temp = 0;

            int count_up = 0;
            temp = 0;
            for (int i = p.X; i >= 0; i--)
            {
                if (p.Y + temp >= Const.chess_row)
                {
                    break;
                }

                if (button.BackgroundImage == matrix[i][p.Y + temp].BackgroundImage)
                {
                    count_up++;
                }
                else
                    break;

                temp++;
            }

            int count_down = 0;
            temp = 0;
            for (int i = p.X + 1; i < Const.chess_row; i++)
            {

                temp++;
                if (p.Y - temp < 0)
                {
                    break;
                }

                if (button.BackgroundImage == matrix[i][p.Y - temp].BackgroundImage)
                {
                    count_down++;
                }
                else
                    break;


            }
            return count_up + count_down == 5;
        }

        private void change_x_o(Button button)
        {
            button.BackgroundImage = playerList[curent_player].Image;
            curent_player = curent_player == 1 ? 0 : 1;

        }

        private void change_player()
        {

            curent_player_name.Text = playerList[curent_player].Name;
            curent_player_img.BackgroundImage = playerList[curent_player].Image;
        }

    }
}
