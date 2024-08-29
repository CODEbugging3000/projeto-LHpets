using System.Data.SqlClient;

namespace ATV2SENAI
{
    public class Banco
    {
        private List<Cliente> lista = new List<Cliente>();
        public List<Cliente> GetLista(){
            return lista;
        }
        public string GetListaString(){
            string enviar = "<!DOCTYPE html>\n <html>\n<head><meta charset='utf-8'/>\n"+
            "<title>Cadastro de cliente</title>\n"+
            "<style>table{border-spacing: 0; border-collapse: collapse;}th, td{border: 1px solid black; padding: 0 10px;} tbody tr:nth-child(even){background: #ccc;}</style>\n"+
            "</head>\n<body>\n"+
            "<H1>Lista de cliente</H1>\n"+
            "<table>\n"+
            "<thead>\n"+
            "<tr>\n"+
            "<th>CPF/CNPJ</th>\n"+
            "<th>Nome</th>\n"+
            "<th>Endereço</th>\n"+
            "<th>RG/IE</th>\n"+
            "<th>Tipo</th>\n"+
            "<th>Valor</th>\n"+
            "<th>Valor Imposto</th>\n"+
            "<th>Total</th>\n"+
            "</thead>\n"+
            "<tbody>\n";
            foreach (Cliente cli in GetLista())
            {
                enviar += "<tr>"+
                $"<td>{cli.cpf_cnpj}</td>"+
                $"<td>{cli.nome}</td>"+
                $"<td>{cli.endereco}</td>"+
                $"<td>{cli.rg_ie}</td>"+
                $"<td>{cli.tipo}</td>"+
                $"<td>{cli.valor.ToString()}</td>"+
                $"<td>{cli.valor_imposto.ToString()}</td>"+
                $"<td>{cli.total.ToString()}</td>";
            }
            enviar += "</tbody>\n</table>\n</body>\n</html>";
            return enviar;
        }

        public Banco(){
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                    "Integrated Security=true;"+
                    "Server=localhost;"+
                    "DataBase=vendas;"+
                    "Trusted_Connection=true;" //se não for autenticação do windows é false
                    // "User Id=sa;Password=12345;"+
                    // "Server=localhost\\SQLEXPRESS2;"+
                    // "Database=vendas;"+
                );
                using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
                {
                    string sql = "SELECT * FROM tblclientes";
                    using (SqlCommand cmd = new SqlCommand(sql, conn)){
                        conn.Open();
                        using (SqlDataReader tabela = cmd.ExecuteReader()){
                            while(tabela.Read()){
                                lista.Add(new Cliente(){
                                    cpf_cnpj = tabela["cpf_cnpj"].ToString(),
                                    nome = tabela["nome"].ToString(),
                                    endereco = tabela["endereco"].ToString(),
                                    rg_ie = tabela["rg_ie"].ToString(),
                                    tipo = tabela["tipo"].ToString(),
                                    valor = (float)Convert.ToDecimal(tabela["valor"]),
                                    valor_imposto = (float)Convert.ToDecimal(tabela["valor_imposto"]),
                                    total = (float)Convert.ToDecimal(tabela["total"]),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}