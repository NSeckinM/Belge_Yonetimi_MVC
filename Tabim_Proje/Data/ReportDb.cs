using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Tabim_Proje.Data
{
    public class ReportDb
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BelgeYonetimiDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public DataTable GetRecord()
        {
            SqlCommand com = new SqlCommand("select Id, UserName,UserLastName,Explanation,ConsiderationStatus,ResultOfConsideration from UserRequests", con);
            SqlDataAdapter da = new SqlDataAdapter(com);

            DataTable dt = new DataTable();

            da.Fill(dt);

            return dt;

        }


    }
}
