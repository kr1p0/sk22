using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Models
{
    public class DbConnection
    {
        public static string ConnString { get; } = "User ID=ADMIN; Password=#B1taSm1etana2; Data Source=sk_high";

        
        //public string ConnString { get; } = "DATA SOURCE=localhost:1521/XEPDB1;;" +
        //"PERSIST SECURITY INFO=True;USER ID=admin; password=opov027; Pooling = True; ";

        //public static string ConnStringStatic { get; } = "DATA SOURCE=localhost:1521/XEPDB1;;" +
        // "PERSIST SECURITY INFO=True;USER ID=admin; password=opov027; Pooling = True; ";
    }
}
