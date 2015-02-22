using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EFOptimization.DataModel;

namespace EFOptimization.Compare
{
	static class EntityGenerator
	{
		public static List<Order> Generate(int count)
		{
			var entities = new List<Order>(count);

			var random = new Random();

			for (int i = 0; i < count; i++)
			{
				entities.Add(new Order(random.Next()));
			}

			return entities;
		}
	}
}
