using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jobsafe.Modelo;
using Jobsafe.Service;

namespace Jobsafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
       
        private Vaga? actualVagaSelected = null;
        private VagaService vagaService;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            vagaService = new VagaService();
            ListaVagas.ItemsSource = vagaService.Vagas();
        }

        

        private void Inserir_Click(object sender, RoutedEventArgs e)
        {
            var modal = new FormularioModal(new FormularioInfo());
            modal.SetOnConfirm((Vaga vaga) =>
            {
                vagaService.Create(vaga, (Vaga vaga) =>
                {
                    LoadList();
                    MessageBox.Show(vaga.Name+" foi criada.");
                });
                
            });
            modal.Owner = this;
            modal.ShowDialog();
        }

        private void LoadList()
        {
            ListaVagas.ItemsSource = null;
            ListaVagas.ItemsSource = vagaService.Vagas();
        }
        
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if(actualVagaSelected != null) {
                var formularioInfor = new FormularioInfo();
                formularioInfor.Title = "Editar";
                var modal = new FormularioModal(formularioInfor);
                modal.SetOnEditing(actualVagaSelected);
                modal.SetOnConfirm((Vaga newVaga) =>
                {
                    newVaga.Id = actualVagaSelected.Id;
                    vagaService.Update(newVaga, (Vaga vaga) =>
                    {
                        LoadList();
                        MessageBox.Show(this, vaga.Name + " foi editada com sucesso.");
                    });
                });
                modal.Owner = this;
                modal.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "Não é possivel editar sem antes selecionar uma vaga.");
            }
            
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if(actualVagaSelected == null)
            {
                MessageBox.Show("Selecione uma vaga para excluir.");
                return;
            }
            var formularioInfor = new FormularioInfo();
            formularioInfor.Title = "Excluir";
            formularioInfor.Subtitle = "Deseja mesmo excluir ?";
            formularioInfor.Description = "Ao confirmar esta ação, você excluirá um registro.";
            var modal = new AcaoModal(formularioInfor);
            modal.SetOnConfirm(()=>{
                vagaService.Delete(actualVagaSelected, () =>
                {
                    LoadList();
                    MessageBox.Show("foi excluido.");
                    actualVagaSelected = null;
                });
  
            });
            modal.Owner = this;
            modal.ShowDialog();
        }



        private void ListaVagas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                actualVagaSelected = ListaVagas.SelectedItem as Vaga;
                
                
                /*
                    throw new Exception("Abra a conexão com a base de dados.");
                if (InputNome.Text == "" || InputNumero.Text == "")
                    throw new Exception("Preencha os dois campos.");
                if (vagaToUpdate == null)
                    throw new Exception("Selecione uma linha.");
                SQLiteCommand cmd = new();
                cmd.Connection = sqlite;
                //campos <id, nome, fone>
                cmd.CommandText = "UPDATE contatos SET nome = @nome, fone = @fone WHERE id LIKE @id";
                cmd.Parameters.AddWithValue("@id", vagaToUpdate.Id);
                cmd.Parameters.AddWithValue("@nome", InputNome.Text);
                cmd.Parameters.AddWithValue("@fone", InputNumero.Text);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Logador.Text = "Contato editado.";
                LoadList();
                ClearFields();
                */
            
            
        }

        private void ListaVagas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ListaVagas.UnselectAllCells();
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            SearchModal searchModal = new SearchModal();

            searchModal.SetOnSearched((List<Vaga> vagas) =>
            {
                ListaVagas.ItemsSource = null;
                ListaVagas.ItemsSource = vagas;
            });

            searchModal.Owner = this;
            searchModal.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            searchModal.ShowDialog();
        }
    }
}
