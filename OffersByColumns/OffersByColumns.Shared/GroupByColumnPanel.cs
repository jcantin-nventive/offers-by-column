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
		
		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}
		
		public static readonly DependencyProperty ItemWidthProperty =
			DependencyProperty.Register("ItemWidth", typeof(double), typeof(GroupByColumnPanel), new PropertyMetadata(100));

		public Style ItemContainerStyle
		{
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}

		public static readonly DependencyProperty ItemContainerStyleProperty =
			DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(GroupByColumnPanel), new PropertyMetadata(null));

		public double ColumnSpacing
		{
			get { return (double)GetValue(ColumnSpacingProperty); }
			set { SetValue(ColumnSpacingProperty, value); }
		}

		public static readonly DependencyProperty ColumnSpacingProperty =
			DependencyProperty.Register("ColumnSpacing", typeof(double), typeof(GroupByColumnPanel), new PropertyMetadata(0));

		public IEnumerable<UIElement> UiElementChildren => Children.Cast<UIElement>();

		protected override Size MeasureOverride(Size availableSize)
		{
			var maxHeightByRow = new Dictionary<int, double>();
			int childNo = 0;
			int nbColumns = 0;
			int rowNo = 0;
			foreach(var child in UiElementChildren)
			{
				var isSeperator = child is Border;

				if (isSeperator)
				{
					child.Measure(new Size(width: ItemWidth, height: 1));
				}
				else
				{
					child.Measure(new Size(width: ItemWidth, height: double.MaxValue));
					maxHeightByRow[rowNo] = child.DesiredSize.Height;

					if (rowNo == 0)
					{
						nbColumns++;
					}

					childNo++;
					rowNo = childNo % NbRows;
				}
			}

			return new Size(
				width: GetTotalWidthWithColumns(nbColumns),
				height: maxHeightByRow.Values.Select(height => height).Sum() + NbRows + 1
			);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int childNo = 0;
			int nbSeperators = 0;
			// Assuming each row has the same height
			var rowHeight = finalSize.Height / NbRows;

			foreach (var child in UiElementChildren)
			{
				var columnNo = childNo / NbRows;
				var rowNo = childNo % NbRows;

				if (rowNo == 0)
				{
					nbSeperators = 0;
				}

				var isSeperator = child is Border;

				if (isSeperator)
				{
					child.Arrange(
						new Rect(
							new Point(
								x: GetPrecedingWidthWithColumns(columnNo),
								y: rowHeight * rowNo + nbSeperators
							),
							new Size(
								width: ItemWidth,
								height: 1
							)
						)
					);
					nbSeperators++;
				}
				else
				{
					child.Arrange(
						new Rect(
							new Point(
								x: GetPrecedingWidthWithColumns(columnNo),
								y: rowHeight * rowNo + nbSeperators
							),
							new Size
							(
								width: ItemWidth,
								height: rowHeight
							)));

					childNo++;
				}
			}

			return finalSize;
		}

		/// <summary>
		/// Gets the total width filled, for a given number of columns
		/// </summary>
		private double GetTotalWidthWithColumns(int nbColumns)
		{
			// The last column has no spacing after it so we substract 1
			var nbColumnsForSpacing = Math.Max(0, nbColumns - 1);

			return ItemWidth * nbColumns + (ColumnSpacing * nbColumnsForSpacing);
		}

		/// <summary>
		/// Gets the width to the left, for a given number of columns.
		/// </summary>
		private double GetPrecedingWidthWithColumns(int nbColumns)
		{
			return ItemWidth * nbColumns + (ColumnSpacing * nbColumns);
		}
	}
}
