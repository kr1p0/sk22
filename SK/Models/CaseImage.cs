using System.Drawing;

namespace SK.Models
{
    public class CaseImage
    {
        public string ImageName { get; set; }
        public string ImageId { get; set; }
        public byte [] ImageBytes { get; set; }  


        public byte[] ImageToByte(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }

        public Image ByteToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }


        /*
        public void SaveImage(byte[] imagen)
        {
            var success = false;
            var numberOfAttempts = 6;

            var sqlQuery = "INSERT INTO F4_Blob( img )VALUES (:val1 )";

            var oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("--->@Blob() @SaveImage()  Connected status: " + oracleConn.State +
                       $" [Approaches remaining: {numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", imagen);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        oracleConn.Close();
                        Console.WriteLine("--->@Blob() @SaveImage() Error: " + ex.Message);
                        return;
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            Console.WriteLine("--->@Blob() @SaveImage(): Success");
            oracleConn.Close();

        }


        public Byte[] LoadImage()
        {
            Byte[] buffer = null;
            Image img = null;
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = "select img from F4_Blob Where id = 15 ";

            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("--->@Blob() @LoadImage()  Connected status: " + oracleConn.State +
                        $" [Approaches remaining: {numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("--->@Blob() @LoadImage():  No data found");
                        return null;
                    }

                    while (dataReader.Read())
                    {
                        buffer = (Byte[])(dataReader.GetOracleBlob(0)).Value;
                        //buffer = (Byte[])(dataReader.GetOracleBlob(1)).Value;
                        //var content = new String(Encoding.UTF8.GetChars(buffer));
                        //img = ByteToImage(buffer);
                    }

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        oracleConn.Close();
                        Console.WriteLine("--->@Blob() @LoadImage(): Error:  " + ex.Message);
                        return null;
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            Console.WriteLine("--->@Blob() @LoadImage(): Success");
            oracleConn.Close();
            return buffer;

        }
        */
    }
}
