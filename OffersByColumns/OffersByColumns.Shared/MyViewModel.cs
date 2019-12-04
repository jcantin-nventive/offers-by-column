using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OffersByColumns
{
	public class MyViewModel
	{
		public IEnumerable<IEnumerable<string>> CategoriesWithOffers => Enumerable.Range(0, 5).Select(idx => Enumerable.Range(idx * 5, 5).Select(nb => $"Item #{nb}"));
	}
}
