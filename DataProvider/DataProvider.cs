using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace HR.DataProvider
{
    /// <summary>
    /// DataProvider Interface
    /// </summary>
    interface IDataProvider
    {

        DataSet FetchData(string procName, SqlParameter[] sqlParams);
        DataTable ExecuteReader(string procName, SqlParameter[] sqlParams);
        DataTable ExecuteReaderText(string viewName);
        int ExecuteCrud(string procName, SqlParameter[] sqlParams);

    }

    /// <summary>
    /// Data Provider class 
    /// </summary>
    public class DataProvider : IDataProvider
    {
        private const int sqlConnectionException = -20002;
        public DataProvider()
        {

        }
        #region Members

        readonly string _sqlConnStr;
        private SqlConnection _sqlconn;
        public SqlConnection SqlConn
        {
            get { return _sqlconn ?? (_sqlconn = new SqlConnection(_sqlConnStr)); }
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// FetchData
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataSet FetchData(string procName, SqlParameter[] sqlParams)
        {
            DataSet dsFetch;
            try
            {

                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();
                using (var sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = SqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = procName;

                    if (sqlParams != null)
                    {
                        sqlCmd.Parameters.AddRange(sqlParams);
                    }

                    using (var sqlAdaptr = new SqlDataAdapter(sqlCmd))
                    {
                        dsFetch = new DataSet();
                        sqlAdaptr.Fill(dsFetch);
                    }
                }

            }

            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
            }

            return dsFetch;
        }
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataTable ExecuteReader(string procName, SqlParameter[] sqlParams)
        {
            DataTable dataTable;
            try
            {

                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();
                using (var sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = SqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = procName;
                    if (sqlParams != null)
                    {
                        sqlCmd.Parameters.AddRange(sqlParams);
                    }

                    using (var reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        dataTable = new DataTable();
                        dataTable.Load(reader);
                    }

                }

            }
            catch (SqlException exp)
            {

                throw exp;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// ExecuteReaderText
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public DataTable ExecuteReaderText(string viewName)
        {
            DataTable dataTable;
            try
            {

                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();
                using (var sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = SqlConn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = viewName;

                    using (var reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        dataTable = new DataTable();
                        dataTable.Load(reader);
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
            }

            return dataTable;
        }

        /// <summary>
        /// ExecuteCrud
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public int ExecuteCrud(string procName, SqlParameter[] sqlParams)
        {
            int recordStatus = ExecuteNonQuery(procName, sqlParams);
            return recordStatus;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(string procName, SqlParameter[] sqlParams)
        {
            int result;
            try
            {
                if (SqlConn.State == ConnectionState.Closed)
                    SqlConn.Open();

                using (var sqlCmd = new SqlCommand())
                {

                    sqlCmd.Connection = SqlConn;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = procName;

                    if (sqlParams != null)
                    {
                        sqlCmd.Parameters.AddRange(sqlParams);
                    }


                    result = sqlCmd.ExecuteNonQuery();
                    //result = Convert.ToInt32(sqlCmd.Parameters["IsError"].Value);
                    //if (result == -1)
                    //result = 1;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (SqlConn.State == ConnectionState.Open)
                    SqlConn.Close();
            }

            return result;
        }

        #endregion Methods

        #region Constructor

        public DataProvider(string connString)
        {
            _sqlConnStr = connString;
        }

        #endregion
    }
}
