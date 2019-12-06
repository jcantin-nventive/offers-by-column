using System.Collections;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System;
#if __IOS__
using Foundation;
#endif
#if __ANDROID__
using Android.Runtime;
#endif

namespace OffersByColumns
{
	/// <summary>
	/// Displays offers in columns with a footer
	/// </summary>
#if __IOS__
	[Register("ColumnList")]
#endif
	public partial class ColumnList : Control
	{
#if __ANDROID__
		//
		// Summary:
		//     Native constructor, do not use explicitly.
		//
		// Remarks:
		//     Used by the Xamarin Runtime to materialize native objects that may have been
		//     collected in the managed world.
		public ColumnList(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public ColumnList()
		{
		}
#endif
#if __IOS__
		/// <summary>
		/// Native constructor, do not use explicitly.
		/// </summary>
		/// <remarks>Used by the Xamarin Runtime to materialize native 
		/// objects that may have been collected in the managed world.
		/// </remarks>
		public ColumnList(IntPtr handle) : base(handle) { }

		public ColumnList()
		{
		}
#endif

		private const string ItemsPanelName = "PART_ItemsPanel";
		private ColumnPanel _itemsPanel;

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_itemsPanel = GetTemplateChild(ItemsPanelName) as ColumnPanel;
			AddItemsToPanel();
		}

		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}

		public static readonly DependencyProperty ItemWidthProperty =
			DependencyProperty.Register("ItemWidth", typeof(double), typeof(ColumnList), new PropertyMetadata((double)313));

		public double ItemHeight
		{
			get { return (double)GetValue(ItemHeightProperty); }
			set { SetValue(ItemHeightProperty, value); }
		}
		
		public static readonly DependencyProperty ItemHeightProperty =
			DependencyProperty.Register("ItemHeight", typeof(double), typeof(ColumnList), new PropertyMetadata((double)100));
		
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ColumnList), new PropertyMetadata(null, ItemsSourceChanged));

		public Style ItemContainerStyle
		{
			get { return (Style)GetValue(ItemContainerStyleProperty); }
			set { SetValue(ItemContainerStyleProperty, value); }
		}

		public static readonly DependencyProperty ItemContainerStyleProperty =
			DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ColumnList), new PropertyMetadata(null));

		public Brush SeparatorBrush
		{
			get { return (Brush)GetValue(SeparatorBrushProperty); }
			set { SetValue(SeparatorBrushProperty, value); }
		}

		public static readonly DependencyProperty SeparatorBrushProperty =
			DependencyProperty.Register("SeparatorBrush", typeof(Brush), typeof(ColumnList), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

		public double ColumnSpacing
		{
			get { return (double)GetValue(ColumnSpacingProperty); }
			set { SetValue(ColumnSpacingProperty, value); }
		}

		public static readonly DependencyProperty ColumnSpacingProperty =
			DependencyProperty.Register("ColumnSpacing", typeof(double), typeof(ColumnList), new PropertyMetadata(0));

		private static void ItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is ColumnList list
				&& list._itemsPanel != null)
			{
				list.AddItemsToPanel();
			}
		}

		private void AddItemsToPanel()
		{
			int itemNo = 0;
			var children = new List<UIElement>();
			foreach (var item in ItemsSource ?? new UIElement[0])
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
			foreach (var child in children)
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
