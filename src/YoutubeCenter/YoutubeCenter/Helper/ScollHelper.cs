using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YoutubeCenter.Helper
{
    public static class ScrollHelper
    {
        public static readonly DependencyProperty ScrollToBottomProperty =
            DependencyProperty.RegisterAttached("ScrollToBottom",
                typeof(ICommand),
                typeof(ScrollHelper),
                new FrameworkPropertyMetadata(null, OnScrollToBottomPropertyChanged));

        public static ICommand GetScrollToBottom(DependencyObject ob) => (ICommand)ob.GetValue(ScrollToBottomProperty);

        public static void SetScrollToBottom(DependencyObject ob, ICommand value) => ob.SetValue(ScrollToBottomProperty, value);

        private static void OnScrollToBottomPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as ScrollViewer).Loaded += OnScrollViewerLoaded;
        }

        private static void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            (sender as ScrollViewer).Loaded -= OnScrollViewerLoaded;

            (sender as ScrollViewer).Unloaded += OnScrollViewerUnloaded;
            (sender as ScrollViewer).ScrollChanged += OnScrollViewerScrollChanged;
        }

        private static void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
            {
                var command = GetScrollToBottom(sender as ScrollViewer);
                if (command == null || !command.CanExecute(null))
                    return;

                command.Execute(null);
            }
        }

        private static void OnScrollViewerUnloaded(object sender, RoutedEventArgs e)
        {
            (sender as ScrollViewer).Unloaded -= OnScrollViewerUnloaded;
            (sender as ScrollViewer).ScrollChanged -= OnScrollViewerScrollChanged;
        }
    }
}
