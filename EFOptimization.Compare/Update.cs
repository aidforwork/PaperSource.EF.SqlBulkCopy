using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

using EFOptimization.DataModel;

namespace EFOptimization.Compare
{
	public static class Update
	{
		public static void BySaveChanges(int count)
		{
			DatabaseOps.ClearPrimaryTable();
			Insert.BySqlBulkCopy(count, false);

			using (var context = new EntityDataModel())
			{
				var entities = context.Orders.ToList();

				entities.ForEach(entity => entity.Date = DateTime.Now.AddYears(666));

				var stopwatch = new Stopwatch();
				stopwatch.Start();

				context.SaveChanges();
				
				stopwatch.Stop();

				Console.WriteLine("SaveChanges({0}) = {1} s", count, stopwatch.Elapsed.TotalSeconds);
			}
		}

		public static void ByMerge(int count)
		{
			DatabaseOps.ClearPrimaryTable();
			DatabaseOps.CreateTempTable();

			Insert.BySqlBulkCopy(count, false);

			using (var context = new EntityDataModel())
			{
				var entities = context.Orders.ToList();

				entities.ForEach(entity => entity.Date = DateTime.Now.AddYears(666));

				var stopwatch = new Stopwatch();
				stopwatch.Start();

				Insert.BulkCopy(entities, "Order_TEMP");
				
				DatabaseOps.Merge();

				stopwatch.Stop();

				Console.WriteLine("MERGE({0}) = {1} s", count, stopwatch.Elapsed.TotalSeconds);
			}

			DatabaseOps.DropTempTable();
		}
	}
}
