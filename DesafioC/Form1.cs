using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace Desafio
{
    public partial class Form1 : Form
    {
        object objData; //obj com valor do JSON para ser usado após baixado

        public Form1()
        {
            InitializeComponent();
            EscritorLog("LOG");
        }
        
        //função para criar e escrever log
        public void EscritorLog(string info)
        {            
            StreamWriter sw = new StreamWriter("..\\..\\LOG.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + ": " +info);
            sw.Close();
        }

        private void btnBaixar_Click(object sender, EventArgs e)
        {
            try //conecta na WEB API
            {
                var requisicaoWeb = WebRequest.CreateHttp("https://jsonplaceholder.typicode.com/users");
                requisicaoWeb.Method = "GET";

                using (var resposta = requisicaoWeb.GetResponse())
                {
                    var streamDados = resposta.GetResponseStream();
                    StreamReader reader = new StreamReader(streamDados);
                    object objResponse = reader.ReadToEnd();

                    EscritorLog("WEB API conectado com sucesso!");
                    objData = objResponse;
                    streamDados.Close();
                    resposta.Close();                    
                }  

                //mostra o JSON no txtBox
                var dadosJson = JsonConvert.DeserializeObject<dynamic>(objData.ToString());
                txtBoxJSON.Text = dadosJson.ToString();
                EscritorLog("JSON Carregado.");
            }
            catch (Exception) //caso não consiga conectar na WEB API
            {
                MessageBox.Show("FALHA!! Não foi possível conectar com o WEB API!");
                EscritorLog("Falha na conexão com o WEB API");
                txtBoxJSON.Text = "FALHA!";
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (objData == null) //se obj do JSON estiver vazio é porque não foi baixado ainda
            {
                txtBoxJSON.Text = "FALHA!";
                MessageBox.Show("É preciso baixar os dados primeiro");
                EscritorLog("É preciso baixar os dados primeiro");
            }
            else
            {
                List<Pessoa> pJSON = JsonConvert.DeserializeObject<List<Pessoa>>(objData.ToString());

                SqlCommand cmd = new SqlCommand();
                Conexao conexao = new Conexao();

                try //conecta no banco
                {
                    //abre conexao                    
                    cmd.Connection = conexao.conectar();
                    EscritorLog("Conexão com  banco aberta.");

                    //comando sql
                    cmd.CommandText = "INSERT INTO Pessoa(id, nome, username, street, suite, city, zipcode, lat, lng, telefone, website, cnome, catchphrase, bs) " +
                                     "VALUES (@id, @nome, @username, @street, @suite, @city, @zipcode, @lat, @lng, @telefone, @website, @cnome, @catchphrase, @bs)";

                    foreach (Pessoa pessoa in pJSON) //para cada pessoa no JSON
                    {
                        if (pessoa.address.suite.Contains("Suite")) //caso possua Suite no parametro suite
                        {
                            //Adiciona o valor da pessoa nos parametros do comando
                            cmd.Parameters.AddWithValue("@id", pessoa.id);
                            cmd.Parameters.AddWithValue("@nome", pessoa.name);
                            cmd.Parameters.AddWithValue("@username", pessoa.username);
                            cmd.Parameters.AddWithValue("@street", pessoa.address.street);
                            cmd.Parameters.AddWithValue("@suite", pessoa.address.suite);
                            cmd.Parameters.AddWithValue("@city", pessoa.address.city);
                            cmd.Parameters.AddWithValue("@zipcode", pessoa.address.zipcode);
                            cmd.Parameters.AddWithValue("@lat", pessoa.address.geo.lat);
                            cmd.Parameters.AddWithValue("@lng", pessoa.address.geo.lng);
                            cmd.Parameters.AddWithValue("@telefone", pessoa.phone);
                            cmd.Parameters.AddWithValue("@website", pessoa.website);
                            cmd.Parameters.AddWithValue("@cnome", pessoa.company.name);
                            cmd.Parameters.AddWithValue("@catchphrase", pessoa.company.catchPhrase);
                            cmd.Parameters.AddWithValue("@bs", pessoa.company.bs);

                            //executa o comando com os parametros que foram adicionados
                            cmd.ExecuteNonQuery();
                            //limpa os parametros para que possa usar os proximos dados
                            cmd.Parameters.Clear();
                        }
                    }

                    //fecha a conexao
                    cmd.Connection = conexao.desconectar();
                    MessageBox.Show("Salvo com sucesso!!");
                    EscritorLog("Salvo no banco com sucesso!");
                    EscritorLog("Conexão com banco encerrada.");
                }
                catch (Exception) //caso não consiga conectar no banco
                {
                    MessageBox.Show("Falha!! Não foi possível conectar no banco!!");
                    EscritorLog("Falha ao conectar no banco...");
                }
            }
        }

        private void txtBoxJSON_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class Pessoa
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public string website { get; set; }
        public Company company { get; set; }
    }


    public class Address
    {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public Geo geo { get; set; }
    }

    public class Company
    {
        public string name { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }
    }

    public class Geo
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

}