using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobsafe.Modelo
{
    public class FormularioInfo
    {
        private string _title = "Formulario Title";
        private string _subtitle = "Formulario Subtitle";
        private string _description = "Formulario Description";
        public FormularioInfo() { }
        public FormularioInfo(string title, string subtitle, string description)
        {
            _title = title;
            _subtitle = subtitle;
            _description = description;
        }

        public string Title { get { return _title; } set { _title = value; } }
        public string Subtitle { get { return _subtitle; } set { _subtitle = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        

    }
}
