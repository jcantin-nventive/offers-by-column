using System.Collections;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OffersByColumns
{
	/// <summary>
	/// Displays offers in columns with a footer
	/// </summary>
	public partial class GroupByColumnList : Control
	{
		private const string ItemsPanelName = "PART_ItemsPanel";
		private GroupByColumnPanel _itemsPanel;

		public GroupByColumnList()
		{
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ItemWidth = Windows.UI.Xaml.Window.Current.Bounds.Width - PeekingWidth;
			_itemsPanel = GetTemplateChild(ItemsPanelName) as GroupByColumnPanel;
			AddItemsToPanel();
		}
		
		public int PeekingWidth
		{
			get { return (int)GetValue(PeekingWidthProperty); }
			set { SetValue(PeekingWidthProperty, value); }
		}
		
		public static readonly DependencyProperty PeekingWidthProperty =
			DependencyProperty.Register("PeekingWidth", typeof(int), typeof(GroupByColumnList), new PropertyMetadata(100, PeekingWidthChanged));

		private static void PeekingWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is GroupByColumnList list)
			{
				list.ItemWidth = Windows.UI.Xaml.Window.Current.Bounds.Width - list.PeekingWidth;
			}
		}
		
		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}
		
		public static readonly DependencyProperty ItemWidthProperty =
			DependencyProperty.Register("ItemWidth", typeof(double), typeof(GroupByColumnList), new PropertyMetadata(0));
		
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GroupByColumnList), new PropertyMetadata(null, ItemsSourceChanged));
		
		public Style ItemContainerStyle
		{
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}
		
		public static readonly DependencyProperty ItemContainerStyleProperty =
			DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(GroupByColumnList), new PropertyMetadata(null));
		
		public Brush SeparatorBrush
		{
			get { return (Brush)GetValue(SeparatorBrushProperty); }
			set { SetValue(SeparatorBrushProperty, value); }
		}
		
		public static readonly DependencyProperty SeparatorBrushProperty =
			DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(GroupByColumnList), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
		
		public double ColumnSpacing
		{
			get { return (double)GetValue(ColumnSpacingProperty); }
			set { SetValue(ColumnSpacingProperty, value); }
		}
		
		public static readonly DependencyProperty ColumnSpacingProperty =
			DependencyProperty.Register("ColumnSpacing", typeof(double), typeof(GroupByColumnList), new PropertyMetadata(0));

		private static void ItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is GroupByColumnList list
				&& list._itemsPanel != null)
			{
				list.AddItemsToPanel();
			}
		}

		private void AddItemsToPanel()
		{
			int itemNo = 0;
			var children = new List<UIElement>();
			foreach(var item in ItemsSource ?? new UIElement[0])
			{
				// If it's the first item in the column, add the top border
				if (itemNo % _itemsPanel.NbRows == 0)
				{
					children.Add(GetNewSeparator());
				}

				children.Add(new Button
				{
					Style = ItemContainerStyle,
					Content = item
				});

				// Add separator after the item
				children.Add(GetNewSeparator());

				itemNo++;
			}

			_itemsPanel.Children.Clear();
			foreach(var child in children)
			{
				_itemsPanel.Children.Add(child);
			}
		}

		private Border GetNewSeparator()
		{
			return new Border
			{
				Height = 1,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Background = SeparatorBrush,
			};
		}
	}
}
