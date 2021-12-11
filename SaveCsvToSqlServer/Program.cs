using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SaveCsvToSqlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var lineNumber = 0;
                byte[] result;

                using (SqlConnection connection = new SqlConnection("SQLCONNECTION"))
                {
                    connection.Open();
                    string filename = @"FilePath";
                    using (FileStream SourceStream = File.Open(filename, FileMode.Open))
                    {
                        result = new byte[SourceStream.Length];
                        SourceStream.Read(result, 0, (int)SourceStream.Length);
                    }
                    var csv = System.Text.Encoding.ASCII.GetString(result);

                    char[] delims = new[] { '\r', '\n' };
                    string[] strings = csv.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                    var skipList = strings.Skip(1).ToList().Select(x => x.Split(','));

                    var cmd = new SqlCommand();
                    foreach (var values in skipList)
                    {
                        var sql = $"INSERT SQL";
                        cmd.CommandText = sql;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Connection = connection;
                        cmd.ExecuteNonQuery();
                    }

                    lineNumber++;
                    connection.Close();
                }

                Console.WriteLine("Importado");
                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Ocorreu um erro.");
                Console.ReadLine();
            }
        }
    }
}
