using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Web;
using Dresses.DAL;
using Dresses.Models;
using MySql.Data.MySqlClient;

namespace Dresses.Repo
{

	public class DataRepo
	{
		MySqlDB mysql = new MySqlDB();

		internal DressModel GetDress(string dressID)
		{
			string qry = "select * from dresses where id=@DRESSID";

			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("@DRESSID", dressID);
			DataTable dt = mysql.GetList(qry, parameters);
			if (dt.Rows.Count > 0)
			{
				return new DressModel
				{
					DresID = dt.Rows[0].Field<Int32>("id"),
					Title =dt.Rows[0].Field<string>("title"),
					Color = dt.Rows[0].Field<string>("color"),
					Price = dt.Rows[0].Field<Int32>("price"),
					Quantity = dt.Rows[0].Field<Int32>("quantity"),
					Size = dt.Rows[0].Field<string>("size")
				};
			}
			else
			{
				return null;
			}

		}

		internal LoginModel CheckLogin(LoginModel loginInfo)
		{
			string qry = "select * from Users where Username=@USERNAME and Password=@PASSWORD";

			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("@USERNAME", loginInfo.Username);
			parameters.Add("@PASSWORD", loginInfo.Password);
			DataTable dt = mysql.GetList(qry, parameters);
			if (dt.Rows.Count > 0)	
			{
				return new LoginModel
				{
					Username = dt.Rows[0].Field<string>("Username"),
					Password = dt.Rows[0].Field<string>("Password"),
					Token = dt.Rows[0].Field<string>("token"),
					ExpireTime = dt.Rows[0].Field<DateTime>("exipre_timestamp")
				};
			}
			else
			{
				return null;
			}

		}

		internal string CheckToken(string token)
		{
			string qry = "select Username from Users where token=@TOKEN ";

			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("@TOKEN", token);
			DataTable dt = mysql.GetList(qry, parameters);

			return dt.Rows.Count>0 ? dt.Rows[0]["Username"].ToString() : "";

		}
		internal void updateNewToken(LoginModel login)
		{

			try
			{
				mysql.Open();
				MySqlCommand cmd = new MySqlCommand();
				cmd.Connection = mysql.mysqlConnection;
				string qry = "";
				qry += "update myApi.Users ";
				qry += " set token=@TOKEN ";
				qry += ", exipre_timestamp=@EXPIRETIME ";
	
				qry += "where Username=@USERNAME and ";
				qry += " Password=@PASSWORD; ";
				cmd.CommandText = qry;
				cmd.Prepare();

				cmd.Parameters.AddWithValue("@TOKEN", login.Token);
				cmd.Parameters.AddWithValue("@EXPIRETIME", DateTime.Now.AddDays(1));
				cmd.Parameters.AddWithValue("@USERNAME", login.Username);
				cmd.Parameters.AddWithValue("@PASSWORD", login.Password);


				mysql.GoSql(cmd);

			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);

			}
			finally
			{
				mysql.Close();
			}
		}

		public List<DressModel> GetDresses()
		{
			List<DressModel> blogList = new List<DressModel>();
			string qry = "select * from dresses ";

			
			DataTable dataTable = mysql.GetList(qry);
			foreach (DataRow item in dataTable.Rows)
			{
				blogList.Add(new DressModel
				{

					DresID = item.Field<Int32>("id"),
					Title = item.Field<string>("title"),
					Color = item.Field<string>("color"),
					Price = item.Field<Int32>("price"),
					Quantity = item.Field<Int32>("quantity"),
					Size = item.Field<string>("size"),

				}); 

			}
			return blogList;
		}

		public int updateDress(DressModel dress)
		{

			try
			{
			mysql.Open();
			MySqlCommand cmd = new MySqlCommand();
			cmd.Connection = mysql.mysqlConnection;
			string qry = "";
			qry +="update myApi.dresses ";
			qry += " set title=@title ";
			qry += ", color=@color "; 
			qry += ", price=@price "; 
			qry += ", quantity=@quantity "; 
			qry += ", size=@size ";
			qry += "where id=@DRESSID; ";
			cmd.CommandText = qry;
			cmd.Prepare();

			cmd.Parameters.AddWithValue("@title", dress.Title);
			cmd.Parameters.AddWithValue("@color", dress.Color);
			cmd.Parameters.AddWithValue("@price", dress.Price);
			cmd.Parameters.AddWithValue("@quantity", dress.Quantity);
			cmd.Parameters.AddWithValue("@size", dress.Size);
			cmd.Parameters.AddWithValue("@DRESSID", dress.DresID);
			mysql.GoSql(cmd);

			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
				
			}
			finally
			{
				mysql.Close();
			}


			return 1;
		}



		public int InsertDress(DressModel dress)
		{

			MySqlCommand cmd = new MySqlCommand();
			mysql.Open();
			cmd.Connection = mysql.mysqlConnection;
			string qry = "";
			qry += "INSERT INTO dresses ";
			qry += "(title,color,price,quantity,size) ";
			qry += " VALUES (@title,@color,@price,@quantity,@size)";

			cmd.CommandText = qry;
			cmd.Prepare();


			cmd.Parameters.AddWithValue("@title", dress.Title);
			cmd.Parameters.AddWithValue("@color", dress.Color);
			cmd.Parameters.AddWithValue("@price", dress.Price);
			cmd.Parameters.AddWithValue("@quantity", dress.Quantity);
			cmd.Parameters.AddWithValue("@size", dress.Size);
			return mysql.GoSql(cmd);

		}

		public int deleteDress(string dressID)
		{
			try
			{

				MySqlCommand cmd = new MySqlCommand();
				mysql.Open();
				cmd.Connection = mysql.mysqlConnection;
				cmd.CommandText = "delete from dresses where id=@DRESSID;";
				cmd.Prepare();

				cmd.Parameters.AddWithValue("@DRESSID", dressID);
				return mysql.GoSql(cmd);

			}
			finally
			{
				mysql.Close();

			}		

		}
	}
}