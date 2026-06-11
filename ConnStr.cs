using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace JUSA_Election_MS
{
    class ConnStr
    {
        public String ConnString = "Server=SAABARIN; database=JUSA_Election; Integrated security=true";
        public SqlDataAdapter DataAdap;
        public SqlConnection SConn;
        public SqlCommand SCommand;
        public SqlDataReader SDataReader;
        public DataTable Dtable;
        public DataSet Dset;



    }

}
