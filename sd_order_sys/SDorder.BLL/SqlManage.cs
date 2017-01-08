using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace SDorder.BLL
{
    public static class SqlManage
    {
        public static DataSet Query(string sql, Dictionary<string, object> sqlparams)
        {
            MySqlCommand sqlcom = new MySqlCommand();
            sqlcom.CommandText = sql;
            MySqlParameter[] param = new MySqlParameter[sqlparams.Keys.Count];
            int num = 0;
            foreach (string key in sqlparams.Keys)
            {
                param[num] = new MySqlParameter(key, sqlparams[key]);
                num++;
            }
            return SDorder.DAL.MySqlHelper.GetDataSet(SDorder.DAL.MySqlHelper.connectionStringManager, sql, param);
        }
    }
}
