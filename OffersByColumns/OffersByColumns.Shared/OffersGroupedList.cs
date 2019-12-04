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
	public partial class OffersGroupedList : SelectorControl
	{
		public OffersGroupedList()
		{
		}

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			ItemWidth = Window.Current.Bounds.Width - PeekingWidth;
		}
		
		public int PeekingWidth
		{
			get { return (int)GetValue(PeekingWidthProperty); }
			set { SetValue(PeekingWidthProperty, value); }
		}
		
		public static readonly DependencyProperty PeekingWidthProperty =
			DependencyProperty.Register("PeekingWidth", typeof(int), typeof(OffersGroupedList), new PropertyMetadata(100, PeekingWidthChanged));

		private static void PeekingWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is OffersGroupedList list)
			{
				list.ItemWidth = Window.Current.Bounds.Width - list.PeekingWidth;
			}
		}
		
		public double ItemWidth { get; private set; }
	}
}
