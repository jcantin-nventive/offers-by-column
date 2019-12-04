using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OffersByColumns
{
	public partial class GroupByColumnPanel : Panel
	{
		public GroupByColumnPanel()
		{
		}

		public int NbRows { get; set; } = 3;
		
		private double _itemWidth = 100;

		public IEnumerable<UIElement> UiElementChildren => Children.Cast<UIElement>();

		protected override Size MeasureOverride(Size availableSize)
		{
			GetItemWidthFromParent();

			var maxHeightByRow = new Dictionary<int, double>();
			int childNo = 0;
			int nbColumns = 0;
			foreach(var child in UiElementChildren)
			{
				child.Measure(new Size(width: _itemWidth, height: double.MaxValue));
				maxHeightByRow[childNo % NbRows] = child.DesiredSize.Height;

				if (childNo % NbRows == 0)
				{
					nbColumns++;
				}

				childNo++;
			}

			return new Size(
				width: _itemWidth * nbColumns,
				height: maxHeightByRow.Values.Select(height => height).Sum()
			);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			// TODO do we need to handle padding/margin?
			int childNo = 0;
			// Assuming each row has the same height. Is this safe? If not, how to proceed? Layout only the first row, then only the second, etc.?
			var rowHeight = UiElementChildren.FirstOrDefault()?.DesiredSize.Height;

			foreach (var child in UiElementChildren)
			{
				var columnNo = childNo / NbRows;
				var rowNo = childNo % NbRows;
				child.Arrange(
					new Rect(
						new Point(
							x: _itemWidth * columnNo,
							y: rowHeight.Value * rowNo),
						child.DesiredSize));

				childNo++;
			}

			return finalSize;
		}

		private void GetItemWidthFromParent()
		{
			FrameworkElement feParent = this;
			while (feParent != null)
			{
				feParent = VisualTreeHelper.GetParent(feParent) as FrameworkElement;
				if (feParent is OffersGroupedList list)
				{
					_itemWidth = list.ItemWidth;
				}
			}
		}
	}
}
