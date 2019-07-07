using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using PeterO.Cbor;

namespace AuthServerUI
{
    public class MyDatabase
    {
        public SqlConnection Connection { get; private set; }

        public bool OpenMyDatabase()
        {
            try {
                Connection = new SqlConnection(@"server=.\SQLExpress; Database=ACE; Integrated Security=true");
                Connection.Open();
            }
            catch (SqlException e) {
                return false;
            }

            return true;
        }

        private void InitAndLoadDatabase()
        {
            CBORObject authConfig = null;
            string fileName = "as.json";
            FileStream fs = new FileStream(fileName, FileMode.Open);
            using (TextReader reader = new StreamReader(fs)) {
                string json = reader.ReadToEnd();
                authConfig = CBORObject.FromJSONString(json);
            }

            SqlCommand command = Connection.CreateCommand();
            SqlTransaction transaction = Connection.BeginTransaction("Creation");
            command.Transaction = transaction;

            try {
                command.CommandText = "CREATE DATABASE AuthServer";
                command.CommandType = CommandType.Text;
                if (command.ExecuteNonQuery() == -1) throw new Exception("Failed to create database");

                // Build the entity table 

                command.CommandText = "CREATE TABLE EntityTable (EntityNo int NOT NULL IDENTITY PRIMARY KEY, Name nvarchar(255), Comment nvarchar(255), Type char(10), DefaultAudience char(10), DefaultScope varbinary(255), Six char(10), Profiles nvarchar(255), Eight char(10), Attributes nvarchar(255));";
                if (command.ExecuteNonQuery() == -1) throw new Exception("Failed to create entity table");

                foreach (CBORObject clientKey in authConfig["ClientKeys"].Values) {
                    StringBuilder sb = new StringBuilder("INSERT INTO EntityTable (" +
                                                         "Name, Comment" +
                                                         ") VALUES (");
                }

                // Build the key table

                command.CommandText = "CREATE TABLE KeyTable (KeyNo int NOT NULL IDENTITY PRIMARY KEY, KeyName nvarchar(255), KeyId varbinary(255), ASConnection nvarchar(10), Comment nvarchar(255), KeyValue varbinary(4000), PIV_Last int DEFAULT 0, HitTest_Last int DEFAULT 0, EntityID int, KeyDefaultAudience nvarchar(255), KeyDefaultScope varbinary(255));";
                if (command.ExecuteNonQuery() == -1) throw new Exception("Failed to create KeyTable");


                // Fill in the Key Table

                foreach (CBORObject clientKey in authConfig["ClientKeys"].Values) {
                    StringBuilder sb = new StringBuilder("INSERT INTO KeyTable (" +
                                                         "KeyValue, Comment" +
                                                         ") VALUES (");

                    //  Key Value
                    sb.AppendFormat("\"{0}\", ", clientKey["Key"].AsString());
                    sb.AppendFormat("\"{0}\", ", clientKey["comment"].AsString());

                    sb.Append(");");
                    command.CommandText = sb.ToString();
                }

                //  Build the Resource Table
                command.CommandText =
                    "CREATE TABLE ResourceTable (ResourceNo int NOT NULL IDENTITY PRIMARY KEY, Audience nvarchar(255), Rules varbinary(255), Comment varchar(255));";
                if (command.ExecuteNonQuery() == -1) throw new Exception("Failed to create ResourceTable");

                //  Build the Resource/Entity Map Table
                command.CommandText =
                    "CREATE TABLE ResourceMap (RecordNo int NOT NULL IDENTITY PRIMARY KEY, EntityId int, ResourceId int);";
                if (command.ExecuteNonQuery() == -1) throw new Exception("Failed to create ResourceMap");

            }
            catch (Exception ex) {
                transaction.Rollback();
                throw;
            }
        }
    }
}
