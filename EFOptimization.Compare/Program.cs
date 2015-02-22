using System;

namespace EFOptimization.Compare
{
	class Program
	{
		static void Main(string[] args)
		{
			//int[] entityCounts = {1000, 10000, 100000};
			int[] entityCounts = { 100 };

			Console.WriteLine("Insert comparison:");
			Console.WriteLine();

			foreach (int count in entityCounts)
			{
				Console.WriteLine("Inserting {0} records...", count);

				Insert.ByAdd(count);
				Insert.ByAddWithNoDetect(count);
				Insert.ByRecreateContext(count);
				Insert.ByRange(count);
				Insert.BySqlBulkCopy(count);

				Console.WriteLine();
			}

			Console.WriteLine("Update comparison:");
			Console.WriteLine();

			foreach (int count in entityCounts)
			{
				Console.WriteLine("Updating {0} records...", count);	

				Update.BySaveChanges(count);
				Update.ByMerge(count);

				Console.WriteLine();
			}

			Console.WriteLine("All tests done!");
			Console.ReadLine();
		}
	}
}
