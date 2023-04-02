using Jobsafe.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Jobsafe
{
    /// <summary>
    /// Lógica interna para FormularioModal.xaml
    /// </summary>
    public partial class FormularioModal : Window
    {
        public delegate void OnConfirm(Vaga vaga);
        public OnConfirm? onConfirm = null;
        private readonly FormularioInfo _formularioInfo;
        public FormularioModal(FormularioInfo info)
        {
            _formularioInfo = info;
            InitializeComponent();
            Title = _formularioInfo.Title;

            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        public void SetOnConfirm(OnConfirm onConfirm)
        {
            this.onConfirm = onConfirm;
        }
        

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
           
            
            if(onConfirm  != null)
            {
                if(InputCodigo.Text != "" && InputTitulo.Text != "" && InputLink.Text != "")
                {
                    var vaga = new Vaga()
                    {
                        Name = InputTitulo.Text,
                        Cod = InputCodigo.Text,
                        Link = InputLink.Text
                    };
                    DialogResult = true;
                    onConfirm.Invoke(vaga);
                }
                else
                {
                    var formularioInfor = new FormularioInfo();
                    formularioInfor.Title = "Menssagem";
                    formularioInfor.Subtitle = "Campos vazios";
                    formularioInfor.Description = "Você precisa preencher todos os campos.";
                    var modal = new AcaoModal(formularioInfor);
                    modal.Owner = this;
                    modal.ShowDialog();
                }
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Fecha a janela modal sem realizar a ação
            DialogResult = false;
        }

        private void InputCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void SetOnEditing(Vaga? vagaToUpdate)
        {
            
            InputCodigo.Text = vagaToUpdate.Cod;
            InputTitulo.Text = vagaToUpdate.Name;
            InputLink.Text = vagaToUpdate.Link;
            
        }
    }
}
