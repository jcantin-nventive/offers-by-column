using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OffersByColumns
{
	public class MyViewModel
	{
		public IEnumerable<string> CategoriesWithOffers => Enumerable.Range(0, 15).Select(nb => $"Very long item description #{nb}");
	}
}
