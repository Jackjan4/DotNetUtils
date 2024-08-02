using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

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



		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dbConnection"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		public static IEnumerable<T> ExecuteQuery<T>(DbConnection dbConnection, string query) {
			IEnumerable<T> result;

			using (dbConnection) {
				dbConnection.Open();
				result = dbConnection.Query<T>(query);

				dbConnection.Close();
			}
			return result;
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public static DataTable ExecuteQuery(DbConnection dbConnection, string query) {
			DataTable result = new DataTable();

			// Create DbCommand
			using (dbConnection)
			using (DbCommand dbCommand = dbConnection.CreateCommand()) {
				dbCommand.CommandText = query;

				// Open the connection
				dbConnection.Open();

				using (DbDataReader reader = dbCommand.ExecuteReader()) {
					result.Load(reader);

					// Close reader and connection
					reader.Close();
					dbConnection.Close();
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

			// Create DbCommand
#if NET8_0_OR_GREATER
			// C# Version: 

			await using (dbConnection) {
				await using (var dbCommand = dbConnection.CreateCommand()) {
					dbCommand.CommandText = query;

					// Open the connection
					await dbConnection.OpenAsync();

					await using (var reader = await dbCommand.ExecuteReaderAsync()) {
						result.Load(reader);

						// Closing the reader is not needed because .Dispose will already do that
						// Close connection
						await dbConnection.CloseAsync();
					}
				}
			}
#else
			// C# Version 7.3
			using (dbConnection) {
				using (var dbCommand = dbConnection.CreateCommand()) {
					dbCommand.CommandText = query;

					// Open the connection
					await dbConnection.OpenAsync();

					using (var reader = await dbCommand.ExecuteReaderAsync()) {
						result.Load(reader);

						// Closing the reader is not needed because .Dispose will already do that
						// Close connection
						dbConnection.Close();
					}
				}
			}
#endif
			return result;
		}
	}
}
