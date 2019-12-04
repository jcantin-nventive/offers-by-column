using Windows.UI.Xaml;
#if WINDOWS_UWP
// Deriving from ListBox, not Selector, on Windows because Selector has a private ctor
using SelectorControl = Windows.UI.Xaml.Controls.ListBox;
#else
// Deriving from Selector, not ListBox on Uno because it is not implemented
using SelectorControl = Windows.UI.Xaml.Controls.Primitives.Selector;
#endif

namespace OffersByColumns
{
	/// <summary>
	/// Displays offers in columns with a footer
	/// </summary>
	public class OffersGroupedList : SelectorControl
	{
		public OffersGroupedList()
		{
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ItemWidth = Window.Current.Bounds.Width - PeekingWidth;
		}

		/// <summary>
		/// Determines the default value of ItemWidth
		/// </summary>
		public int PeekingWidth { get; set; } = 100;
		
		public double ItemWidth
		{
			get { return (double)GetValue(ItemWidthProperty); }
			set { SetValue(ItemWidthProperty, value); }
		}
		
		public static readonly DependencyProperty ItemWidthProperty =
			DependencyProperty.Register("ItemWidth", typeof(double), typeof(OffersGroupedList), new PropertyMetadata(0));
	}
}
