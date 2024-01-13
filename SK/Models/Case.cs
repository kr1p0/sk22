using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SK.Models
{
    public class Case : DbConnection
    {
        public class StatusType
        {
            public static readonly string Inactive = "inactive";
            public static readonly string Highlighted = "highlighted";
        }

        public string CaseId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UnigueNumber { get; set; }
        public string Description { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTimeOnly { get; set; }
        public string EndTimeOnly { get; set; }
        public string Status { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Sn { get; set; }
        public string Notification { get; set; }
        public string CompletedDate { get; set; }
        public string Highlight { get; set; }



        public static List<Case> GetCaseList(string condition = "", string search = "", string sort = "", string rowFrom = "0", string rowsCount = "100")
        {
            var resultLi = new List<Case>();
            var success = false;
            var numberOfAttempts = 6;

            var whereClause = condition != "" || search != "" ? "where" : "";
            var andClause = condition != "" && search != "" ? "and" : "";

            string sqlsqlQuery = $"select * from case {whereClause} {condition} {andClause} {search} {sort}  OFFSET {rowFrom} ROWS FETCH NEXT {rowsCount} ROWS ONLY ";  //FETCH FIRST 10 ROWS ONLY // OFFSET 10 ROWS FETCH NEXT 3 ROWS ONLY

            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetCase()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetCaseList(): No data found");
                        oracleConn.Close();
                        return resultLi;
                    }

                    while (dataReader.Read())
                    {
                        var tempObj = new Case();
                        tempObj.CaseId = (dataReader["case_id"].ToString());
                        tempObj.FirstName = (dataReader["first_name"].ToString());
                        tempObj.LastName = (dataReader["last_name"].ToString());
                        tempObj.UnigueNumber = (dataReader["unique_number"].ToString());
                        tempObj.Description = (dataReader["description"].ToString());
                        tempObj.Telephone = (dataReader["telephone"].ToString());
                        tempObj.Email = (dataReader["email"].ToString());
                        tempObj.StartDate = (dataReader["start_date"].ToString());
                        tempObj.EndDate = (dataReader["end_date"].ToString());
                        tempObj.Status = (dataReader["status"].ToString());
                        tempObj.Model = (dataReader["model_name"].ToString());
                        tempObj.Manufacturer = (dataReader["manufacturer"].ToString());
                        tempObj.Notification = (dataReader["powiadomienie"].ToString());
                        tempObj.CompletedDate = (dataReader["completed_date"].ToString());
                        tempObj.Highlight = (dataReader["highlight"].ToString());
                        resultLi.Add(tempObj);
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetCaseList() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetCase(): Success");
            return resultLi;
        }


        public static Case GetSingleCase(string id)
        {
            var CaseObj = new Case();
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select * from case where case_id = :val1";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetSingleCase()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", id);
                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetSingleCase(): No data found");
                        oracleConn.Close();
                        return CaseObj;
                    }

                    while (dataReader.Read())
                    {
                        CaseObj.CaseId = (dataReader["case_id"].ToString());
                        CaseObj.FirstName = (dataReader["first_name"].ToString());
                        CaseObj.LastName = (dataReader["last_name"].ToString());
                        CaseObj.UnigueNumber = (dataReader["unique_number"].ToString());
                        CaseObj.Description = (dataReader["description"].ToString());
                        CaseObj.Telephone = (dataReader["telephone"].ToString());
                        CaseObj.Email = (dataReader["email"].ToString());
                        CaseObj.StartDate = (dataReader["start_date"].ToString());
                        CaseObj.EndDate = (dataReader["end_date"].ToString());
                        CaseObj.Status = (dataReader["status"].ToString());
                        CaseObj.Model = (dataReader["model_name"].ToString());
                        CaseObj.Manufacturer = (dataReader["manufacturer"].ToString());
                        CaseObj.Notification = (dataReader["powiadomienie"].ToString());
                        CaseObj.CompletedDate = (dataReader["completed_date"].ToString());
                        CaseObj.Highlight = (dataReader["highlight"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetSingleCase() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetSingleCase(): Success");
            return CaseObj;
        }

        public static string GetNumberOfRows(string condition = "", string search = "")
        {
            var whereClause = condition != "" || search != "" ? "where" : "";
            var andClause = condition != "" && search != "" ? "and" : "";

            string totalRow = "0";
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select count(*) from case {whereClause} {condition} {andClause} {search}";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetNumberOfRows()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetNumberOfRows(): No data found");
                        oracleConn.Close();
                        return totalRow;
                    }

                    while (dataReader.Read())
                    {
                        totalRow = dataReader[0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetNumberOfRows() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetNumberOfRows(): Success");
            return totalRow;
        }



        public static List<Case> SearchCase(string searchData)
        {
            var resultLi = new List<Case>();
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"SELECT * FROM case WHERE  (LOWER(first_name) LIKE '%{searchData}%' OR" +
                $"  LOWER(last_name) LIKE '%{searchData}%'  OR  LOWER(unique_number) LIKE '%{searchData}%' OR" +
                $"  LOWER(telephone) LIKE '%{searchData}%' OR  LOWER(email) LIKE '%{searchData}%' OR" +
                $"  LOWER(manufacturer) LIKE '%{searchData}%' OR  LOWER(sn) LIKE '%{searchData}%')";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @SearchCase()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @SearchCase(): No data found");
                        oracleConn.Close();
                        return resultLi;
                    }

                    while (dataReader.Read())
                    {
                        var tempObj = new Case();
                        tempObj.CaseId = (dataReader["case_id"].ToString());
                        tempObj.FirstName = (dataReader["first_name"].ToString());
                        tempObj.LastName = (dataReader["last_name"].ToString());
                        tempObj.UnigueNumber = (dataReader["unique_number"].ToString());
                        tempObj.Description = (dataReader["description"].ToString());
                        tempObj.Telephone = (dataReader["telephone"].ToString());
                        tempObj.Email = (dataReader["email"].ToString());
                        tempObj.StartDate = (dataReader["start_date"].ToString());
                        tempObj.EndDate = (dataReader["end_date"].ToString());
                        tempObj.Status = (dataReader["status"].ToString());
                        tempObj.Model = (dataReader["model_name"].ToString());
                        tempObj.Manufacturer = (dataReader["manufacturer"].ToString());
                        tempObj.Notification = (dataReader["powiadomienie"].ToString());
                        tempObj.CompletedDate = (dataReader["completed_date"].ToString());
                        tempObj.Highlight = (dataReader["highlight"].ToString());
                        resultLi.Add(tempObj);
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @SearchCase() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @SearchCase(): Success");
            return resultLi;
        }


        public static bool InsertCase(Case obj, List<CaseImage>? imgData = null)
        {
            var success = false;
            var numberOfAttempts = 6;
            var returnedID = "";

            var sqlQuery = "INSERT INTO case(first_name, last_name, unique_number, telephone , email , start_date , end_date, description, manufacturer, model_name, powiadomienie ) " +
                "VALUES (:val1 , :val2, :val3, :val4, :val5, TO_TIMESTAMP(:val6, 'YYYY-MM-DD HH24:MI:SS') , TO_TIMESTAMP(:val7 , 'YYYY-MM-DD HH24:MI:SS') , :val8, :val9, :val10, :val11 ) " +
                "RETURNING case_id INTO :returnInsertedRowId ";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @InsertCase()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", obj.FirstName);
                    cmd.Parameters.Add("val2", obj.LastName);
                    cmd.Parameters.Add("val3", obj.UnigueNumber);
                    cmd.Parameters.Add("val4", obj.Telephone);
                    cmd.Parameters.Add("val5", obj.Email);
                    cmd.Parameters.Add("val6", obj.StartDate);
                    cmd.Parameters.Add("val7", obj.EndDate);
                    cmd.Parameters.Add("val8", obj.Description);
                    cmd.Parameters.Add("val9", obj.Manufacturer);
                    cmd.Parameters.Add("val10", obj.Model);
                    cmd.Parameters.Add("val11", obj.Notification);
                    cmd.Parameters.Add(":returnInsertedRowId", OracleDbType.Decimal, ParameterDirection.Output);

                    cmd.ExecuteNonQuery();

                    returnedID = (cmd.Parameters[":returnInsertedRowId"].Value).ToString();

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @InsertCase() Error: " + ex.Message);
                        oracleConn.Close();
                        return false;
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            foreach (var image in imgData)
            {
                InsertImage(returnedID, image.ImageName, image.ImageBytes);
            }

            /*
            foreach(var image in imageNames)
            {
                InsertImageName(returnedID, image);
            }
             */

            Console.WriteLine("@Case @InsertCase(): Success");
            oracleConn.Close();
            return true;
        }


        public static bool UpdateCase(Case obj, List<CaseImage>? imgData = null)
        {
            var success = false;
            var numberOfAttempts = 6;
            var returnedID = "";

            var sqlQuery = "UPDATE case SET first_name = :val1, last_name = :val2 , unique_number = :val3," +
                " telephone = :val4 , email = :val5 , start_date = TO_TIMESTAMP(:val6, 'YYYY-MM-DD HH24:MI:SS') ," +
                " end_date = TO_TIMESTAMP(:val7 , 'YYYY-MM-DD HH24:MI:SS'), description = :val8, model_name = :val9, " +
                " manufacturer = :val10, powiadomienie  = :val11  where case_id = :val12";



            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @UpdateCase()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", obj.FirstName);
                    cmd.Parameters.Add("val2", obj.LastName);
                    cmd.Parameters.Add("val3", obj.UnigueNumber);
                    cmd.Parameters.Add("val4", obj.Telephone);
                    cmd.Parameters.Add("val5", obj.Email);
                    cmd.Parameters.Add("val6", obj.StartDate);
                    cmd.Parameters.Add("val7", obj.EndDate);
                    cmd.Parameters.Add("val8", obj.Description);
                    cmd.Parameters.Add("val9", obj.Model);
                    cmd.Parameters.Add("val10", obj.Manufacturer);
                    cmd.Parameters.Add("val11", obj.Notification);
                    cmd.Parameters.Add("val12", obj.CaseId);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @UpdateCase() Error: " + ex.Message);
                        oracleConn.Close();
                        return false;
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            foreach (var image in imgData)
            {
                InsertImage(obj.CaseId, image.ImageName, image.ImageBytes);
            }

            /*
             foreach (var image in imageNames)
            {
                InsertImageName(obj.CaseId, image);
            }
             */

            Console.WriteLine("@Case @UpdateCase(): Success");
            oracleConn.Close();
            return true;
        }



        public static string InsertImage(string caseId, string imageName, byte[] imageBytes)
        {
            var success = false;
            var numberOfAttempts = 6;

            var sqlQuery = "INSERT INTO image (case_id, image_name, imageBlob) values (:val1 , :val2 , :val3)";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @InsertImageName()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", caseId);
                    cmd.Parameters.Add("val2", imageName);
                    cmd.Parameters.Add("val3", imageBytes);

                    cmd.ExecuteNonQuery();


                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @InsertImageName() Error: " + ex.Message);
                        oracleConn.Close();
                        return "(🗙) Wystąpił błąd";
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }




            Console.WriteLine("@Case @InsertImageName(): Success");
            oracleConn.Close();
            return "(✓) Powodzenie";
        }

        public static string UpdateCaseStatus(string caseId, string newStatus , string completedDate)
        {
            var success = false;
            var numberOfAttempts = 6;


            var sqlQuery = "update case set status = :val1 , completed_date = TO_TIMESTAMP(:val2 , 'YYYY-MM-DD HH24:MI') where case_id= :val3";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @UpdateCaseStatus()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", newStatus);
                    cmd.Parameters.Add("val2", completedDate);
                    cmd.Parameters.Add("val3", caseId);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @UpdateCaseStatus() Error: " + ex.Message);
                        oracleConn.Close();
                        return "(🗙) Wystąpił błąd";
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            Console.WriteLine("@Case @UpdateCaseStatus(): Success");
            oracleConn.Close();
            return "(✓) Powodzenie";
        }

        public static string UpdateSingleColumn(string caseId, string column, string value)
        {
            var success = false;
            var numberOfAttempts = 6;


            var sqlQuery = $"update case set {column} = :val1 where case_id= :val2";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @UpdateSingleColumn()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", value);
                    cmd.Parameters.Add("val2", caseId);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @UpdateSingleColumn() Error: " + ex.Message);
                        oracleConn.Close();
                        return "(🗙) Wystąpił błąd";
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            Console.WriteLine("@Case @UpdateSingleColumn(): Success");
            oracleConn.Close();
            return "(✓) Powodzenie";
        }


        public static string UpdateHighlight(string caseId, string newDescription)
        {
            var success = false;
            var numberOfAttempts = 6;


            var sqlQuery = "update case set description = :val1 where case_id= :val2";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @UpdateCaseDescription()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", newDescription);
                    cmd.Parameters.Add("val2", caseId);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @UpdateCaseDescription() Error: " + ex.Message);
                        oracleConn.Close();
                        return "(🗙) Wystąpił błąd";
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            Console.WriteLine("@Case @UpdateCaseDescription(): Success");
            oracleConn.Close();
            return "(✓) Powodzenie";
        }

        public static bool RemoveCase(string caseId)
        {
            RemoveImageByCaseId(caseId);

            var success = false;
            var numberOfAttempts = 6;
            var sqlQuery = "delete from case where case_id = :val1";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @RemoveCase()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);

                    cmd.Parameters.Add("val1", caseId);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @RemoveCase() Error: " + ex.Message);
                        oracleConn.Close();
                        return false;
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            Console.WriteLine("@Case @RemoveCase(): Success");
            oracleConn.Close();
            return true;
        }


        public static bool CheckIfUnique(int number)
        {
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select * from case where unique_number = :val1";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @CheckIfUnique()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", number);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @CheckIfUnique(): No data found");
                        oracleConn.Close();
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetCase()() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            return false;
        }

        public static List<CaseImage> GetImages(string id)
        {
            var resultLi = new List<CaseImage>();
            var resultLiImageNamesOnly = new List<string>();
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select * from image where case_id = :val1 ";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetImages()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", id);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetImages(): No data found");
                        oracleConn.Close();
                        return resultLi;
                    }

                    while (dataReader.Read())
                    {
                        var tempObj = new CaseImage();
                        tempObj.ImageId = (dataReader["image_id"].ToString());
                        tempObj.ImageName = (dataReader["image_name"].ToString());
                        tempObj.ImageBytes = (Byte[])(dataReader.GetOracleBlob(3)).Value; // 0-index
                        resultLi.Add(tempObj);
                        resultLiImageNamesOnly.Add(dataReader["image_name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetImages() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetImages(): Success");
            return resultLi;
        }


        public static (List<CaseImage> resultLi, List<string> resultLiImageNamesOnly) GetImageNames(string id)
        {
            var resultLi = new List<CaseImage>();
            var resultLiImageNamesOnly = new List<string>();
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select image_name, image_id  from image where case_id = :val1 ";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetImageNames()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", id);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetImageNames(): No data found");
                        oracleConn.Close();
                        return (resultLi, resultLiImageNamesOnly);
                    }

                    while (dataReader.Read())
                    {
                        var tempObj = new CaseImage();
                        tempObj.ImageId = (dataReader["image_id"].ToString());
                        tempObj.ImageName = (dataReader["image_name"].ToString());
                        resultLi.Add(tempObj);
                        resultLiImageNamesOnly.Add(dataReader["image_name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetImageNames() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetImageNames(): Success");
            return (resultLi, resultLiImageNamesOnly);
        }


        public static string GetImageNameByImgId(string id)
        {
            string result = "";
            var success = false;
            var numberOfAttempts = 6;
            string sqlsqlQuery = $"select image_name from image where image_id = :val1 ";
            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetImageNameByImgId()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);
                    cmd.Parameters.Add("val1", id);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetImageNameByImgId(): No data found");
                        oracleConn.Close();
                        return result;
                    }

                    while (dataReader.Read())
                    {
                        result = (dataReader["image_name"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetImageNameByImgId() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetImageNameByImgId(): Success");
            return result;
        }


        public static string RemoveImage(List<string> imageIdList)
        {
            var success = false;
            var numberOfAttempts = 6;


            var sqlQuery = "delete from image where image_id = :val1";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @RemoveImage()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");

                    foreach (var imageId in imageIdList)
                    {
                        OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);
                        cmd.Parameters.Add("val1", imageId);
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @RemoveImage() Error: " + ex.Message);
                        oracleConn.Close();
                        return "(🗙) Wystąpił błąd";
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            Console.WriteLine("@Case @RemoveImage(): Success");
            oracleConn.Close();
            return "(✓) Powodzenie";
        }

        public static string RemoveImageByCaseId(string caseId)
        {
            var success = false;
            var numberOfAttempts = 6;


            var sqlQuery = "delete from image where case_id = :val1";

            var oracleConn = new OracleConnection(ConnString);

            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @RemoveImageByCaseId()  Connected status: " + oracleConn.State +
                            $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlQuery, oracleConn);

                    cmd.Parameters.Add("val1", caseId);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                    {
                        Console.WriteLine("@Case @RemoveImageByCaseId() Error: " + ex.Message);
                        oracleConn.Close();
                        return "(🗙) Wystąpił błąd";
                    }
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }

            Console.WriteLine("@Case @RemoveImageByCaseId(): Success");
            oracleConn.Close();
            return "(✓) Powodzenie";
        }




        public static List<Case> GetReminders()
        {
            var resultLi = new List<Case>();
            var success = false;
            var numberOfAttempts = 6;
            string dateNow = Convert.ToDateTime(CentralEuTimeZone.Get()).ToString("yyyy-MM-dd HH:mm:ss");


            string sqlsqlQuery = $"select * from case where end_date > TO_TIMESTAMP( '{dateNow}' , 'YYYY/MM/DD HH24:MI:SS') AND powiadomienie = 'y'";
            Console.WriteLine(sqlsqlQuery);

            OracleConnection oracleConn = new OracleConnection(ConnString);
            while (!success && numberOfAttempts > 1)
            {
                try
                {
                    success = true;
                    oracleConn.Open();
                    Console.WriteLine("@Case @GetCase()  Connected status: " + oracleConn.State +
                       $" [Approach: {7 - numberOfAttempts}]");
                    OracleCommand cmd = new OracleCommand(sqlsqlQuery, oracleConn);

                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (!dataReader.HasRows)
                    {
                        Console.WriteLine("@Case @GetCaseList(): No data found");
                        oracleConn.Close();
                        return resultLi;
                    }

                    while (dataReader.Read())
                    {
                        var tempObj = new Case();
                        tempObj.CaseId = (dataReader["case_id"].ToString());
                        tempObj.EndDate = (dataReader["end_date"].ToString());
                        resultLi.Add(tempObj);
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    numberOfAttempts--;

                    if (numberOfAttempts == 1)
                        Console.WriteLine("@Case @GetCaseList() Error: " + ex.Message);
                    else
                        Thread.Sleep(10000 / numberOfAttempts);
                    oracleConn.Close();
                }
            }
            oracleConn.Close();
            Console.WriteLine("@Case @GetCase(): Success");
            return resultLi;
        }
    }
}
