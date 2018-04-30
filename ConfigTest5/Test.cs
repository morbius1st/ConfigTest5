using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ConfigTest5
{
	// just a test routine
	public class Test
	{
		public static List<Orders> dataSource = new List<Orders>(5);

		public void GetTypeInfo()
		{
			Orders o = new Orders {customer = new Customer[] {new Customer()}};

			dataSource.Add(o);

			PropertyInfo info1 = dataSource[0].GetType().GetProperty("customer");

			Array c = (Array) info1.GetValue(dataSource[0], null);
			
			Type info2 = c.GetValue(0).GetType();

			PropertyInfo info3 = info2.GetProperty("OtherAddress");

			Debug.WriteLine("type| " + info3.PropertyType.Name);
		}
	}

	// sample orders class
	[Serializable]
	public class Orders
	{
		public long OrderID { get; set; }
		public string CustomerID { get; set; }
		public int EmployeeID { get; set; }
		public double Freight { get; set; }
		public string ShipCountry { get; set; }
		public string ShipCity { get; set; }
		public Customer[] customer { get; set; }
	}

	// sample customer class
	public class Customer
	{
		public string OtherAddress { get; set; }
		public int CustNum { get; set; }

	}

}