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
        /// <summary>
        /// 记录操作类：增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlparams"></param>
        /// <returns></returns>
        public static bool OpRecord(string sql, Dictionary<string, object> sqlparams)
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
            int result = SDorder.DAL.MySqlHelper.ExecuteNonQuery(SDorder.DAL.MySqlHelper.connectionStringManager, CommandType.Text,
                sql, param);
            if (result > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 查询是否存在记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlparams"></param>
        /// <returns></returns>
        public static object Exists(string sql, Dictionary<string, object> sqlparams)
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
            return SDorder.DAL.MySqlHelper.ExecuteScalar(SDorder.DAL.MySqlHelper.connectionStringManager, CommandType.Text,
                sql, param);
        }
    }
}
