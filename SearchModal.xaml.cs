using Jobsafe.Modelo;
using Jobsafe.Service;
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
    /// Lógica interna para SearchModal.xaml
    /// </summary>
    public partial class SearchModal : Window
    {
        public delegate void OnSearch(List<Vaga> vagas);
        private OnSearch? onSearch;
        private string filterBy = "cod";
        private VagaService vagaService;
      
        public SearchModal()
        {    
            InitializeComponent();
            vagaService = new VagaService();

        }
        public void SetOnSearched(OnSearch onSearch)
        {
            this.onSearch = onSearch;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            ProgressSearch.Visibility = Visibility.Visible;
            InputSearch.Visibility = Visibility.Collapsed;
            List<Vaga> listVagas = await vagaService.FindBy(filterBy, InputSearch.Text);
            ProgressSearch.Visibility = Visibility.Collapsed;
            InputSearch.Visibility = Visibility.Visible;
            if (listVagas.Count != 0)
            {
                if(onSearch != null)
                {
                    onSearch.Invoke(listVagas);
                }
                SearchMessage.Foreground = Brushes.Green;
                SearchMessage.Content = $"{listVagas.Count} resultados foram encontrados.";
            }
            else
            {
                SearchMessage.Foreground = Brushes.Red;
                SearchMessage.Content = "Nenhum resultado foi encontrado.";
                await Task.Delay(10000);
                SearchMessage.Content = "";
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                string filterBy = "";
                switch (radioButton.Content)
                {
                    case "ID":
                        filterBy = "id";
                        break;
                    case "NOME":
                        filterBy = "name";
                        break;
                    case "CÓDIGO":
                        filterBy = "code";
                        break;
                    case "LINK":
                        filterBy = "link";
                        break;
                    default:
                        filterBy = "code";
                        break;
                }
                this.filterBy = filterBy;
            }
        } 
        
        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender is ComboBox comboBox) && (comboBox.SelectedItem is not null) && (comboBox.SelectedItem is ComboBoxItem item))
            {
                string filterBy = "";
                switch (item.Tag)
                {
                    case "ID":
                        filterBy = "id";
                        break;
                    case "NOME":
                        filterBy = "name";
                        break;
                    case "CÓDIGO":
                        filterBy = "code";
                        break;
                    case "LINK":
                        filterBy = "link";
                        break;
                    default:
                        filterBy = "code";
                        break;
                }

                this.filterBy = filterBy;
            }
        }
    }
}
