using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CenterTreeLevel
{
    internal class Program
    {

        string connectionString = ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString;

        static void Main(string[] args)
        {


            new Program().getModeerID();

        }

        List<int> moshrefs = new List<int>();

        List<int> modeers = new List<int>();



        public void getModeerID() // start
        {

            modeers.Add(41);
            modeers.Add(36);
            modeers.Add(37);
            modeers.Add(55);
            modeers.Add(56);
            modeers.Add(57);
            modeers.Add(1140);

            addCenterLevelModeer();


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (int modeer in modeers)
                {

                    SqlCommand cmd = new SqlCommand("select* from AccountRelationMapping where MagicParent = " + modeer, con);
                    cmd.CommandType = CommandType.Text;


                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Open();
                    }

                    var rdr = cmd.ExecuteReader();



                    while (rdr.Read())
                    {
                        int accountID = Convert.ToInt32(rdr["AccountID"]); // 45 47 619 1682; // moshrefs 
                        moshrefs.Add(accountID);

                        addCenterLevelMoshref(accountID);

                        handleMoshhref_start(accountID);

                    }

                    if (moshrefs.Count != 0) // check if modeer is found or not
                    {
                        getMoshrefChildrenModeer(moshrefs);

                    }
                    moshrefs.Clear();

                }

            }

        }

        void addCenterLevelMoshref(int moshrefID)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("updateCenterLevel", con);
                cmd.CommandType = CommandType.StoredProcedure;


                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }

                SqlParameter ParamterLevel = new SqlParameter();
                ParamterLevel.ParameterName = "@level";
                ParamterLevel.Value = 2;
                cmd.Parameters.Add(ParamterLevel);

                SqlParameter ParamterCenterID = new SqlParameter();
                ParamterCenterID.ParameterName = "@centerId";
                ParamterCenterID.Value = moshrefID;
                cmd.Parameters.Add(ParamterCenterID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



        void addCenterLevelModeer()
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();


                foreach (int modeer in modeers)
                {
                    SqlCommand cmd = new SqlCommand("updateCenterLevel", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Open();
                    }

                    SqlParameter ParamterLevel = new SqlParameter();
                    ParamterLevel.ParameterName = "@level";
                    ParamterLevel.Value = 1;
                    cmd.Parameters.Add(ParamterLevel);

                    SqlParameter ParamterCenterID = new SqlParameter();
                    ParamterCenterID.ParameterName = "@centerId";
                    ParamterCenterID.Value = modeer;
                    cmd.Parameters.Add(ParamterCenterID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        void addCenterLevelMandob(int mandobID)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("updateCenterLevel", con);
                cmd.CommandType = CommandType.StoredProcedure;


                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }

                SqlParameter ParamterLevel = new SqlParameter();
                ParamterLevel.ParameterName = "@level";
                ParamterLevel.Value = 3;
                cmd.Parameters.Add(ParamterLevel);

                SqlParameter ParamterCenterID = new SqlParameter();
                ParamterCenterID.ParameterName = "@centerId";
                ParamterCenterID.Value = mandobID;
                cmd.Parameters.Add(ParamterCenterID);

                con.Open();
                cmd.ExecuteNonQuery();
            }

        }


        void getMoshrefChildrenModeer(List<int> accountIDs)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                con.Open();

                foreach (int MoshrefAccID in accountIDs) // get all moshrfeen's  all manadeeb children
                {

                    string queryCenterID = "select* from AccountRelationMapping where MagicParent = " + MoshrefAccID;

                    SqlCommand cmd = new SqlCommand(queryCenterID, con);
                    cmd.CommandType = CommandType.Text;

                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Open();
                    }

                    var rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {

                        int mandobID = Convert.ToInt32(rdr["AccountID"]); // mandob                       
                        getPOS(mandobID);

                    }

                }
            }
        }

        void getPOS(int mandobID) // get children for all mandobs  
        {

            using (SqlConnection con = new SqlConnection(connectionString))

            {

                string queryCenterID = "select* from AccountRelationMapping where MagicParent = " + mandobID;

                SqlCommand cmd = new SqlCommand(queryCenterID, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                var rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    int posID = Convert.ToInt32(rdr["AccountID"]); // tager pos
                    addcenterLevelPOS(posID);

                }


            }
        }

        void addcenterLevelPOS(int posID)
        {

            using (SqlConnection con = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand("updateCenterLevel", con);
                cmd.CommandType = CommandType.StoredProcedure;


                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }

                SqlParameter ParamterLevel = new SqlParameter();
                ParamterLevel.ParameterName = "@level";
                ParamterLevel.Value = 4;
                cmd.Parameters.Add(ParamterLevel);

                SqlParameter ParamterCenterID = new SqlParameter();
                ParamterCenterID.ParameterName = "@centerId";
                ParamterCenterID.Value = posID;
                cmd.Parameters.Add(ParamterCenterID);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



        void handleMoshhref_start(int moshrefAccID)
        {


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string queryCenterID = "select* from AccountRelationMapping where ParentID = " + moshrefAccID;

                SqlCommand cmd = new SqlCommand(queryCenterID, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                var rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    int mandobID = Convert.ToInt32(rdr["AccountID"]);
                    addCenterLevelMandob(mandobID);
                    getPOS(mandobID); // moshref and modeer process 
                }

            }




        }











    }
}