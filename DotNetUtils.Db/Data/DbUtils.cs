using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;



namespace Roslan.DotNetUtils.Db.Data {



    public static class DbUtils {



        private const string SqlTestQuery = "SELECT 'OK' AS Feld";



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool TestConnection(DbConnection dbConnection) {
            // Execute test query
            DataTable result = null;

            result = ExecuteQuery(dbConnection, SqlTestQuery); // This can throw, user needs to handle errors

            if (result == null)
                return false;

            return result.Rows.Count > 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> TestConnectionAsync(DbConnection dbConnection) {
            // Execute test query
            DataTable result = null;

            result = await ExecuteQueryAsync(dbConnection, SqlTestQuery); // This can throw, user needs to handle errors

            if (result == null)
                return false;

            return result.Rows.Count > 0;
        }



        /// There was a static function ExecuteQuery<T> with return type IEnumerable<T> here which used dapper
        /// to map the result onto an object. That function is removed because it had no benefit over doing that manually with dapper.


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static DataTable ExecuteQuery(DbConnection dbConnection, string query) {
            var result = new DataTable();

            // We do not need to 'using' the connection. The caller must do that

            // Create DbCommand
            using (var dbCommand = dbConnection.CreateCommand()) {
                dbCommand.CommandText = query;

                // Open the connection
                dbConnection.Open();

                using (var reader = dbCommand.ExecuteReader(CommandBehavior.CloseConnection)) {

                    result.BeginLoadData();
                    result.Load(reader);
                    result.EndLoadData();

                    // Reader is closed by disposing it
                    // dbConnection is closed by caller by disposing it
                }
            }

            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<DataTable> ExecuteQueryAsync(DbConnection dbConnection, string query) {
            var result = new DataTable();

#if NET8_0_OR_GREATER
            // C# Version: > 7.3

            await using var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = query;

            // Open the connection
            await dbConnection.OpenAsync();

            await using var reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            result.BeginLoadData();
            result.Load(reader);
            result.EndLoadData();

            // Closing the reader is not needed because disposing it will already do that
            // Closing the connection is not needed because the caller will do that

#else
            // C# Version 7.3
            using (var dbCommand = dbConnection.CreateCommand()) {
                dbCommand.CommandText = query;

                // Open the connection
                await dbConnection.OpenAsync();

                using (var reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection)) {
                    result.Load(reader);

                    // Closing the reader is not needed because .Dispose will already do that
                    // Closing the connection is not needed because the caller will do that
                }
            }

#endif
            return result;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<DataTable> ExecuteStoredProcedureAsync(DbConnection dbConnection,
            string storedProcedureName, params DbParameter[] parameters) {
            var result = new DataTable();

#if NET8_0_OR_GREATER
            await using var dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = storedProcedureName;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddRange(parameters);

            await dbConnection.OpenAsync();

            await using var reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            result.BeginLoadData();
            result.Load(reader);
            result.EndLoadData();
#else
            // C# Version 7.3
            using (var dbCommand = dbConnection.CreateCommand()) {
                dbCommand.CommandText = storedProcedureName;
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddRange(parameters);

                await dbConnection.OpenAsync();

                using (var reader = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection)) {
                    result.Load(reader);
                }
            }
#endif

            return result;
        }
    }
}
