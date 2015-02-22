using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

using EFOptimization.DataModel;

namespace EFOptimization.Compare
{
	static class DatabaseOps
	{
		static DatabaseOps()
		{
			using (var context = new EntityDataModel())
			{
				var connection = context.Database.Connection;

				Server server = new Server(new ServerConnection(new SqlConnection(connection.ConnectionString)));

				Database = server.Databases[connection.Database];
			}
		}

		private static Database Database { get; set; }

		private static Table TempTable
		{
			get
			{
				var database = Database;

				return database.Tables["Order_TEMP"];
			}
		}

		private static Table PrimaryTable
		{
			get
			{
				var database = Database;

				return database.Tables["Order"];
			}
		}

		public static void DropTempTable()
		{
			if (TempTable != null) TempTable.Drop();
		}

		public static void ClearTempTable()
		{
			if (TempTable != null) TempTable.TruncateData();
		}

		public static void ClearPrimaryTable()
		{
			if (PrimaryTable != null) PrimaryTable.TruncateData();
		}

		public static void Merge()
		{
			using (var context = new EntityDataModel())
			{
				context.Merge();
			}
		}

		public static void RunSqlScript(string script)
		{
			Database.ExecuteWithResults(script);
		}

		public static void CreateTempTable()
		{
			DropTempTable();

			ScriptingOptions options = new ScriptingOptions
			{
				Default = true, 
				NoIdentities = true, 
				DriAll = true
			};

			StringCollection script = PrimaryTable.Script(options);

			var sb = new StringBuilder();

			foreach (string str in script)
			{
				sb.Append(str);
				sb.Append(Environment.NewLine);
			}

			string tempScript = sb.ToString().Replace("Order", "Order_TEMP");

			RunSqlScript(tempScript);
		}
	}
}
