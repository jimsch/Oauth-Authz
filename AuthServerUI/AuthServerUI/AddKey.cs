using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AuthServer;
using PeterO.Cbor;

namespace AuthServerUI
{
    public partial class AddKey : Form
    {
        private readonly MyDatabase _database;
        private readonly int _entityNo;
        private readonly int _keyNo;

        public AddKey(MyDatabase database, int entityNo, int keyNo = -1)
        {
            InitializeComponent();
            _entityNo = entityNo;
            _keyNo = keyNo;
            _database = database;
        }


        private void addKey_Load(object sender, EventArgs e)
        {
            SqlDataReader reader = null;
            if (_keyNo != -1) {
                try {
                    SqlCommand cmd = new SqlCommand($"SELECT * FROM KeyTable WHERE KeyNo={_keyNo}",
                        _database.Connection);
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!reader.HasRows) {
                        MessageBox.Show("Key not found");
                        reader.Close();
                        return;
                    }

                    tbKeyName.Text = reader.GetString(reader.GetOrdinal("KeyName"));
                    cbAsConnection.Text = reader.GetString(reader.GetOrdinal("ASConnection"));
                    tbComment.Text = reader.GetString(reader.GetOrdinal("Comment"));
                    tbKeyValue.Text = CBORObject.DecodeFromBytes(reader.GetSqlBinary(reader.GetOrdinal("KeyValue")).Value).ToString();
                    int ord = reader.GetOrdinal("KeyDefaultAudience");
                    if (!reader.IsDBNull(ord)) tbDefaultAudience.Text = reader.GetString(ord);

                    ord = reader.GetOrdinal("KeyDefaultScope");
                    if (!reader.IsDBNull(ord)) tbDefaultScope.Text = reader.GetString(ord);
                }
                catch (Exception except) {
                    MessageBox.Show("Exception: " + except);
                }

                reader?.Close();

            }

        }

        public void AddToDatabase(MyDatabase database, int entityId)
        {
            SqlTransaction transaction = database.Connection.BeginTransaction();

            try {
                CBORObject key = CBORDiagnostics.Parse(tbKeyValue.Text);

                StringBuilder blder = new StringBuilder("INSERT INTO KeyTable(KeyName,ASConnection, EntityID");
                StringBuilder vals = new StringBuilder($") VALUES('{tbKeyName.Text}', '{cbAsConnection.Text}', {entityId}");
                if (key.ContainsKey(CBORObject.FromObject(2))) {
                    blder.Append(", KeyID");
                    vals.Append(", ");
                    vals.Append(ToHex(key[CBORObject.FromObject(2)].GetByteString()));
                }

                blder.Append(", KeyValue");
                vals.Append(", ");
                vals.Append(ToHex(key.EncodeToBytes()));

                if (tbComment.Text != "") {
                    blder.Append(", Comment");
                    vals.Append(", '");
                    vals.Append(tbComment.Text.Replace("'", "''"));
                    vals.Append("'");
                }
                if (tbDefaultAudience.Text != "") {
                    blder.Append(", KeyDefaultAudience");
                    vals.Append(", '");
                    vals.Append(tbDefaultAudience.Text);
                    vals.Append("' ");
                }

                if (tbDefaultScope.Text != "") {
                    blder.Append(", KeyDefaultScope");
                    vals.Append(", '");
                    vals.Append(tbDefaultScope.Text);
                    vals.Append("' ");
                }

                vals.Append(");");
                blder.Append(vals);
                String txt = blder.ToString();

                SqlCommand cmd = new SqlCommand(txt, database.Connection);
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch {
                transaction.Rollback();
            }
        }

        public void UpdateDatabase()
        {
            SqlTransaction transaction = _database.Connection.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand() {
                    Connection = _database.Connection,
                    Transaction = transaction
                };

                StringBuilder builder = new StringBuilder("UPDATE KeyTable SET ");
                string addComma = "";

                if (tbKeyValue.Modified) {
                    CBORObject key = CBORDiagnostics.Parse(tbKeyValue.Text);

                    builder.Append("KeyValue=@KeyValue");
                    addComma = ", ";
                    cmd.Parameters.Add("KeyValue", SqlDbType.Binary).Value = key.EncodeToBytes();

                    if (key.ContainsKey(CBORObject.FromObject(2))) {

                        builder.Append(", KeyID=@KeyID");
                        cmd.Parameters.Add("KeyID", SqlDbType.Binary).Value = key[CBORObject.FromObject(2)].GetByteString();
                    }
                }

                if (tbKeyName.Modified) {
                    builder.Append(addComma);
                    builder.Append("KeyName=@KeyName");
                    cmd.Parameters.Add("KeyName", SqlDbType.Text).Value = tbKeyName.Text;
                    addComma = ", ";
                }

                if (true) { // Look for a change test.
                    builder.Append(addComma);
                    builder.Append("ASConnection=@ASConnection");
                    cmd.Parameters.Add("ASConnection", SqlDbType.Text).Value = cbAsConnection.Text;
                    addComma = ", ";
                }

                if (tbComment.Modified) {
                    builder.Append(addComma);
                    builder.Append("Comment=@Comment");
                    cmd.Parameters.Add("Comment", SqlDbType.Text).Value = tbComment.Text;
                    addComma = ", ";
                }

                if (tbDefaultAudience.Modified)
                {
                    builder.Append(addComma);
                    builder.Append("KeyDefaultAudience=@KeyDefaultAudience");
                    cmd.Parameters.Add("KeyDefaultAudience", SqlDbType.Text).Value = tbDefaultAudience.Text;
                    addComma = ", ";
                }

                if (tbDefaultScope.Modified)
                {
                    builder.Append(addComma);
                    builder.Append("KeyDefaultScope=@KeyDefaultScope");
                    cmd.Parameters.Add("KeyDefaultScope", SqlDbType.Text).Value = tbDefaultScope.Text;
                    addComma = ", ";
                }

                builder.Append($" WHERE KeyNo={_keyNo};");

                if (addComma != "") {
                    cmd.CommandText = builder.ToString();
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }

        static private string ToHex(byte[] rgb)
        {
            return "0x" + BitConverter.ToString(rgb).Replace("-", "");
        }

    }
}
