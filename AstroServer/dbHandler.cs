using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace AstroServer
{
    internal class dbHandler
    {
        private Entity db;
        public Entity Database
        { 
            get { return db; }
            set { db = value; }
        }

        public dbHandler()
        {
            db = new Entity();
        }

        public string CreateUser(string username, string password, string email, string profilePic, DateTime joinDate, string role)
        {
            if (role != "admin" || role != "user")
                return "Invalid role. Only user either 'admin' or 'user'.";

            SqlConnection conn = new SqlConnection(@"Server=.\ProjectModels;Database=Astro_DB;Trusted_connection=True;");
            
            SqlDataAdapter adapter = new SqlDataAdapter("select * from dbo.tbl_Users", conn);
            new SqlCommandBuilder(adapter);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "tbl_Users");

            DataTable dataTable = ds.Tables[0].GetChanges(DataRowState.Added) ?? new DataTable();
            DataRow row = ds.Tables[0].NewRow();
            row["id"] = (dataTable.Rows.Count + 1);

            // search this
            //db.tbl_Users.Add;
            //db.tbl_Users.Append;

            return "User " + username + " created.";
        }

        public bool ValidateCredentials(string username, string password)
        {
            using(db)
                foreach(var user in db.tbl_Users)
                    if (username == user.Username && password == user.Password)
                        return true;
                    else if (username == user.Username && password != user.Password)
                        return false;
            return false;
        }
    }
}
