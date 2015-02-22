using System;

namespace EFOptimization.DataModel
{
	public partial class Order
	{
		public Order()
		{
			
		}

		public Order(int number)
		{
			Date = DateTime.Now;
			Number = number;
		}
	}
}
