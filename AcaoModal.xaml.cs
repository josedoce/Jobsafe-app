using Jobsafe.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Lógica interna para AcaoModal.xaml
    /// </summary>
    public partial class AcaoModal : Window
    {
   
        public delegate void OnConfirm();
        private OnConfirm? onConfirm = null;
        private FormularioInfo _formularioInfo;
        public AcaoModal(FormularioInfo info)
        {
            _formularioInfo = info;
            InitializeComponent();
            Title = _formularioInfo.Title;
            Subtitle.Content = _formularioInfo.Subtitle;
            Description.Text = _formularioInfo.Description;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        public void SetOnConfirm(OnConfirm onConfirm)
        {
            this.onConfirm = onConfirm;
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            // Realiza a ação desejada quando o botão Salvar é clicado
            // Por exemplo, salvar os dados do formulário em um banco de dados

            // Fecha a janela modal quando a ação é concluída
            DialogResult = true;
            if (onConfirm != null)
            {
                onConfirm.Invoke();
            }
            
           
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Fecha a janela modal sem realizar a ação
            DialogResult = false;
        }
    }
}
