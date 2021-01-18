using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

namespace Desafio
{
    public class Conexao
    {
        SqlConnection con = new SqlConnection();

        public Conexao()
        {
            StreamReader sr = new StreamReader("..\\..\\DesafioC.ini");

            //con.ConnectionString = @"Data Source=DESKTOP-KMMITMG\SQLEXPRESS;Initial Catalog=DESAFIO;Integrated Security=True";
            con.ConnectionString = @sr.ReadToEnd();
        }
        public SqlConnection conectar()
        {
            con.Open();
            return con;
        }

        public SqlConnection desconectar()
        {
            con.Close();
            return con;
        }
    }
}
