using Jobsafe.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Jobsafe.Service
{
    public class VagaService
    {
        public delegate void OnCreate(Vaga vaga);
        private SQLiteConnection? sqlite = null;
        private string baseDados;
        private string strConection;
        private string dirPath;
       
        public VagaService() {
            
            baseDados = Path.Combine(Directory.GetCurrentDirectory(), @".\db\dbsqlite.db");
            strConection = @"Data Source = " + baseDados + "; Version = 3";
            dirPath = baseDados.Replace(@"\dbsqlite.db", "");

            CreateDatabaseIfNotExists();
        }


        public void Create(Vaga vaga, OnCreate onCreate)
        {
            try
            {
                OpenConnetion();
                VerifyFields(vaga);
               
                SQLiteCommand cmd = new();
                cmd.Connection = sqlite;
                //campos <id, nome, fone>
                cmd.CommandText = "INSERT INTO vagas (name, code, link) VALUES(@name, @code, @link)";
                //cmd.Parameters.AddWithValue("@id", contato.Id);
                cmd.Parameters.AddWithValue("@name", vaga.Name);
                cmd.Parameters.AddWithValue("@code", vaga.Cod);
                cmd.Parameters.AddWithValue("@link", vaga.Link);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                onCreate.Invoke(vaga);
            }
            catch(Exception ex)
            {
                ShowDialog(ex.Message);
                return;
            }
            finally
            {
                CloseConnetion();
            }
        }

        public List<Vaga> Vagas()
        {
            try
            {
                OpenConnetion();
                
                //campos <id, nome, fone>
                string query = "SELECT * FROM vagas";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query,sqlite);
                DataTable dados = new DataTable();
                adapter.Fill(dados);
                List<Vaga> listVaga = new();

                foreach (DataRow dr in dados.Rows)
                {
                    listVaga.Add(new Vaga() { 
                        Id = int.Parse(dr["id"].ToString()), 
                        Name = dr["name"].ToString(), 
                        Cod = dr["code"].ToString(), 
                        Link = dr["link"].ToString() 
                    });
                }

                adapter.Dispose();
                return listVaga;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
                return new();
            }
            finally
            {
                CloseConnetion();
            }
        }

        

        public void Update(Vaga vaga, Action<Vaga> value)
        {
            try
            {
                OpenConnetion();
                VerifyFields(vaga);
                SQLiteCommand cmd = new();
                cmd.Connection = sqlite;
                cmd.CommandText = "UPDATE vagas SET name = @name, code = @code, link = @link WHERE id LIKE @id";
                cmd.Parameters.AddWithValue("@id", vaga.Id);
                cmd.Parameters.AddWithValue("@name", vaga.Name);
                cmd.Parameters.AddWithValue("@code", vaga.Cod);
                cmd.Parameters.AddWithValue("@link", vaga.Link);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                value.Invoke(vaga);
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
            }
            finally
            {
                CloseConnetion();
            }
        }

        public void Delete(Vaga? vaga, Action value)
        {
            try
            {
                OpenConnetion();
                VerifyFields(vaga);
                SQLiteCommand cmd = new();
                cmd.Connection = sqlite;
                cmd.CommandText = "DELETE FROM vagas WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", vaga.Id);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                value.Invoke();
            }catch(Exception ex)
            {
                ShowDialog(ex.Message);
            }
            finally
            {
                CloseConnetion();
            }
        }

        public async Task<List<Vaga>> FindBy(string filterBy, string criteria)
        {
            try
            {
                if (criteria == "")
                    throw new Exception("Preencha o campo.");
                OpenConnetion();
                //campos <id, nome, fone>
                string query = $"SELECT * FROM vagas WHERE {filterBy} LIKE '{criteria}%'";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, sqlite);
                DataTable dados = new DataTable();
                adapter.Fill(dados);

                List<Vaga> listVaga = new();

                foreach (DataRow dr in dados.Rows)
                {
                    listVaga.Add(new Vaga()
                    {
                        Id = int.Parse(dr["id"].ToString()),
                        Name = dr["name"].ToString(),
                        Cod = dr["code"].ToString(),
                        Link = dr["link"].ToString()
                    });
                }
                
                adapter.Dispose();
                await Task.Delay(1000);
                return listVaga;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
                return new();
            }
            finally
            {
                CloseConnetion();
            }
        }

        private void CreateDatabaseIfNotExists()
        {
            VerifyFiles(dirPath, baseDados);
            SQLiteConnection connection = new SQLiteConnection(strConection);
            try
            {
                connection.Open();
                SQLiteCommand cmd = new()
                {
                    Connection = connection,
                    CommandText = "CREATE TABLE IF NOT EXISTS vagas (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, code VARCHAR(15), link TEXT)"
                };

                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
                return;
            }
            finally
            {
                connection.Close();
            }
        }

        private void OpenConnetion()
        {

            try
            {
                VerifyFiles(dirPath, baseDados);
                sqlite = new SQLiteConnection(strConection);
                sqlite.Open();
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
            }

        }

        private void CloseConnetion()
        {
            try
            {
                if (sqlite != null)
                {
                    sqlite.Close();
                }
            }
            catch (Exception ex)
            {
                ShowDialog(ex.Message);
            }
        }

        private void ShowDialog(string message)
        {
            MessageBox.Show(message);
        }

        private void VerifyFields(Vaga vaga)
        {
            if (vaga.Cod == "" || vaga.Name == "" || vaga.Link == "")
                throw new Exception("Preencha os todos os campos.");
        }

        private void VerifyFiles(string directoryPath, string databaseFilepath)
        {
            if (!Directory.Exists(databaseFilepath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!File.Exists(databaseFilepath))
            {
                SQLiteConnection.CreateFile(databaseFilepath);
            }
        }
    }
}
