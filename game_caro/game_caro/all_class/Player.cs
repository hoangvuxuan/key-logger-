using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_caro.all_class
{
    public class Player
    {
        private string _name;
        private Image _image;

        public Player(string name, Image image)
        {
            this._name = name;
            this._image = image;
        }

        public string Name  //ctrl r e
        { 
            get => _name; 
            set => _name = value;
        }
        public Image Image { get => _image; set => _image = value; }
    }
}
