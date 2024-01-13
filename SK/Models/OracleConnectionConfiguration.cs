using Oracle.ManagedDataAccess.Client;

namespace SK.Models
{
 
    public class OracleConnectionConfiguration : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Set TnsAdmin value to directory location of tnsnames.ora and sqlnet.ora files
            OracleConfiguration.TnsAdmin = @"Wallet_SK";

            // Set WalletLocation value to directory location of the ADB wallet (i.e. cwallet.sso)
            OracleConfiguration.WalletLocation = @"Wallet_SK";

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //Cleanup logic here
            Console.WriteLine("OracleConnectionConfiguration service stop.");
        }
    }

}
