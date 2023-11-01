using Oracle.ManagedDataAccess.Client;

namespace SK.Models
{
    public class Account : DbConnection
    {
        
        public string Password{ get; set; }
        public string Name { get; set; }

        public static Account GetUser(string login)
        {
            var Account = new Account();
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select * from login where login = :val1";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Account @getUser()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", login);
                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Account @getUser(): No data found");
                        oracleConn.Close();
                        return Account;
                    }

                    while (dataReader.Read())
                    {
                        Account.Name = (dataReader["NAZWA"].ToString());
                        Account.Password = (dataReader["PASSWORD_HASH"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Account @getUser() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @getUser(): Success");
            return Account;
        }
    }
}
