using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobsafe.Modelo
{
    public class Vaga
    {
        private int _id;
        private string _name;
        private string _cod;
        private string _link;
        public Vaga() { }
        public Vaga(int id, string name, string cod, string link)
        {
            _id = id;
            _name = name;
            _cod = cod;
            _link = link;
        }
        
        public int Id { get { return _id; } set { _id = value; }  }
        public string Name { get { return _name; } set { _name = value; } }
        public string Cod { get { return _cod; } set { _cod = value; } }
        public string Link { get { return _link; } set { _link = value; } }

    }
}
