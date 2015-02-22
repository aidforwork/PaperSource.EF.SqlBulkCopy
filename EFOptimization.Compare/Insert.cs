using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

using EFOptimization.DataModel;

namespace EFOptimization.Compare
{
	public static class Insert
	{
		public static void ByAdd(int count)
		{
			DatabaseOps.ClearPrimaryTable();

			//Создаем нужное количество объектов для добавления в контекст
			var entities = EntityGenerator.Generate(count);

			using (var context = new EntityDataModel())
			{
				var stopwatch = new Stopwatch();
				
				stopwatch.Start();

				entities.ForEach(entity => context.Orders.Add(entity));

				context.SaveChanges();

				stopwatch.Stop();

				Console.WriteLine("Add({0}) = {1} s", count, stopwatch.Elapsed.TotalSeconds);
			}
		}

		public static void ByAddWithNoDetect(int count)
		{
			DatabaseOps.ClearPrimaryTable();

			var entities = EntityGenerator.Generate(count);

			using (var context = new EntityDataModel())
			{
				var stopwatch = new Stopwatch();

				stopwatch.Start();

				context.Configuration.AutoDetectChangesEnabled = false;
				
				entities.ForEach(entity => context.Orders.Add(entity));
				
				context.Configuration.AutoDetectChangesEnabled = true;

				context.SaveChanges();

				stopwatch.Stop();

				Console.WriteLine("NoDetect({0}) = {1} s", count, stopwatch.Elapsed.TotalSeconds);
			}
		}

		public static void ByRecreateContext(int count)
		{
			DatabaseOps.ClearPrimaryTable();

			var entities = EntityGenerator.Generate(count);

			var stopwatch = new Stopwatch();

			stopwatch.Start();

			foreach (var entity in entities)
			{
				using (var context = new EntityDataModel())
				{
					context.Orders.Add(entity);
					context.SaveChanges();
				}
			}

			stopwatch.Stop();

			Console.WriteLine("RecreateContext({0}) = {1} s", count, stopwatch.Elapsed.TotalSeconds);
		}
		
		public static void ByRange(int count)
		{
			DatabaseOps.ClearPrimaryTable();

			var entities = EntityGenerator.Generate(count);

			using (var context = new EntityDataModel())
			{
				var stopwatch = new Stopwatch();

				stopwatch.Start();

				context.Orders.AddRange(entities);
				context.SaveChanges();

				stopwatch.Stop();

				Console.WriteLine("Range({0}) = {1} s", count, stopwatch.Elapsed.TotalSeconds);
			}
		}

		public static void BySqlBulkCopy(int count, bool output = true)
		{
			DatabaseOps.ClearPrimaryTable();

			var entities = EntityGenerator.Generate(count);

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			BulkCopy(entities, "Order");

			stopwatch.Stop();

			if (output) Console.WriteLine("BulkCopy({0}) = {1} s", entities.Count, stopwatch.Elapsed.TotalSeconds);
		}

		public static List<Order> BulkCopy(List<Order> entities, string table = "Order")
		{
			var context = new EntityDataModel();

			string connectionString = context.Database.Connection.ConnectionString;

			using (IDataReader reader = entities.GetDataReader())
			using (SqlConnection connection = new SqlConnection(connectionString))
			using (SqlBulkCopy bcp = new SqlBulkCopy(connection))
			{
				connection.Open();

				bcp.DestinationTableName = string.Format("[{0}]", table);

				bcp.ColumnMappings.Add("Id", "Id");
				bcp.ColumnMappings.Add("Date", "Date");
				bcp.ColumnMappings.Add("Number", "Number");
				bcp.ColumnMappings.Add("Text", "Text");

				bcp.WriteToServer(reader);
			}

			return entities;
		}
	}
}
