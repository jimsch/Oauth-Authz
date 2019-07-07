using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AuthServer;

namespace AuthServerUI
{
    public partial class AddEntity : Form
    {
        public AddEntity()
        {
            InitializeComponent();
        }

        private void Label7_Click(object sender, EventArgs e)
        {
            
        }

        public void InsertInDatabase(MyDatabase database)
        {
            SqlTransaction transaction = database.Connection.BeginTransaction("Creation");
            try {
                SqlCommand command = database.Connection.CreateCommand();
                command.Transaction = transaction;

                StringBuilder fields = new StringBuilder("INSERT INTO EntityTable(Name, Comment, Type, Profiles");
                StringBuilder data = new StringBuilder(") VALUES (@Name, @Comment, @Type, @Profiles");

                command.Parameters.AddWithValue("@Name", nameTB.Text);
                command.Parameters.AddWithValue("@Comment", commentTB.Text);
                command.Parameters.AddWithValue("@Type", GetTypeCode());
                command.Parameters.AddWithValue("@Profiles", GetProfiles());

                if (tbAudience.Text != "") {
                    fields.Append(", DefaultAudience");
                    data.Append(", @Audience");
                    command.Parameters.Add("@Audience", SqlDbType.Text).Value = tbAudience.Text;
                }

                if (tbScope.Text != "") {
                    fields.Append(", DefaultScope");
                    data.Append(", @Scope");
                    command.Parameters.Add("@Scope", SqlDbType.Binary).Value =
                        CBORDiagnostics.Parse(tbScope.Text).EncodeToBytes();
                }

                if (tbAttributes.Text != "") {
                    fields.Append(", Attributes");
                    data.Append(", @Attributes");
                    command.Parameters.Add("@Attributes", SqlDbType.Binary).Value =
                        CBORDiagnostics.Parse(tbAttributes.Text).EncodeToBytes();
                }

                fields.Append(data);
                fields.Append(");");

                command.CommandText = fields.ToString();
                command.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (SqlException e) {
                MessageBox.Show("Exception occured trying to fill Entity Table\n" + e.Message);
                transaction.Rollback();
            }
            catch {
                transaction.Rollback();
            }
        }

        private string GetTypeCode()
        {
            switch (typeCB.Text)
            {
            case "Client":
                return "C";
            case "Resource Server":
                return "RS";
            case "Authorization Server":
                return "AS";
            default:
                throw new Exception("Unknown value");
            }
        }

        private string GetProfiles()
        {
            string ret = "";
            foreach (var item in checkedListBox1.CheckedItems)
            {
                switch (item)
                {
                case "DTLS":
                case "OSCORE":
                    ret += " " + item;
                    break;

                default:
                    throw new Exception("Unknown profile value");
                }

            }

            return ret.Substring(1);
        }
    }
}
