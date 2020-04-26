using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Collections.Generic;

namespace Dresses.DAL
{
	public class MySqlDB
	{

		private static readonly string Server = "localhost";
		private static readonly string Database = "myApi";
		private static readonly string UserName = "kylmz";
		private static readonly string Password = "3413k";

		public MySqlConnection mysqlConnection;
		public MySqlDB()
		{
			mysqlConnection = new MySqlConnection("Server=" + Server + ";Database=" + Database + ";Uid=" + UserName + ";Pwd='" + Password + "';SslMode=none;port=3306");

		}

		public void Open()
		{
			if (mysqlConnection.State == System.Data.ConnectionState.Closed)
			{
				try
				{
					mysqlConnection.Open();

				}
				catch (Exception ex)
				{

					throw new Exception("database connection is unsuccess");
				}

			}
		}

		public DataTable GetList(string query,Dictionary<string,string> parameters = null)
		{
			try
			{
				Open();
				DataTable dataTable = new DataTable();
				MySqlCommand _cmd = new MySqlCommand
				{
					Connection = mysqlConnection,
					CommandText = query
				};
				if (parameters != null)
				{
					_cmd.Prepare();
					foreach (var item in parameters)
					{
						_cmd.Parameters.AddWithValue(item.Key, item.Value);
					}
				}
				_cmd.ExecuteNonQuery();

				MySqlDataAdapter _adapter = new MySqlDataAdapter(_cmd);
				_adapter.Fill(dataTable);

				MySqlCommandBuilder _cb = new MySqlCommandBuilder(_adapter);

				/*dgv.DataSource = _dataTable;
				dgv.DataMember = _dataTable.TableName;
				dgv.AutoResizeColumns();*/
				Close();
				return dataTable;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				Close();
			}

		}


		public int GoSql(MySqlCommand command)
		{
			try
			{
				return command.ExecuteNonQuery();
				
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				Close();
			}
		}

		public void Close()
		{
			mysqlConnection.Close();
		}
		
	}
}