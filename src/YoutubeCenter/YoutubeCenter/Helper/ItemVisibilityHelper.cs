using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.Helper
{
    public static class ItemVisibilityHelper
    {
        public static readonly DependencyProperty ItemVisibilityProperty =
            DependencyProperty.RegisterAttached("ItemVisibility",
                typeof(ICommand),
                typeof(ItemVisibilityHelper),
                new FrameworkPropertyMetadata(null, OnScrollViewerPropertyChanged));

        public static ICommand GetItemVisibility(DependencyObject ob) => (ICommand)ob.GetValue(ItemVisibilityProperty);

        public static void SetItemVisibility(DependencyObject ob, ICommand value) => ob.SetValue(ItemVisibilityProperty, value);

        private static ParallelOptions GetParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 20 };

        private static void OnScrollViewerPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as ScrollViewer).Loaded += ScrollViewerItemVisibilityHelper_Loaded;
        }

        private static void ScrollViewerItemVisibilityHelper_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as ScrollViewer).Loaded -= ScrollViewerItemVisibilityHelper_Loaded;

            (sender as ScrollViewer).Unloaded += ScrollViewerItemVisibilityHelper_Unloaded;
            (sender as ScrollViewer).ScrollChanged += ScrollViewerItemVisibilityHelper_ScrollChanged;
        }

        private static void ScrollViewerItemVisibilityHelper_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var itemsPresenter = ((sender as ScrollViewer).Content as ItemsPresenter);
            var itemsControl = (itemsPresenter.TemplatedParent as ItemsControl);

            if (itemsControl == null)
                return;

            foreach (var video in itemsControl.Items.Cast<Video>())
            {
                var itemContainer = (FrameworkElement)itemsControl.ItemContainerGenerator.ContainerFromItem(video);
                video.IsVisible = IsFullyOrPartiallyVisible(itemContainer, itemsControl);
                //Console.WriteLine($"{video.Title}: {video.IsVisible}");
            }
        }

        private static void ScrollViewerItemVisibilityHelper_Unloaded(object sender, RoutedEventArgs e)
        {
            (sender as ScrollViewer).Unloaded -= ScrollViewerItemVisibilityHelper_Unloaded;
            (sender as ScrollViewer).ScrollChanged -= ScrollViewerItemVisibilityHelper_ScrollChanged;
        }

        //private static void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        //{
        //    var scrollViewer = (ScrollViewer)sender;
        //    if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight)
        //    {
        //        var command = GetScrollToBottom(sender as ScrollViewer);
        //        if (command == null || !command.CanExecute(null))
        //            return;

        //        command.Execute(null);
        //    }
        //}

        public static bool IsFullyOrPartiallyVisible(FrameworkElement child, FrameworkElement scrollViewer)
        {
            var childTransform = child.TransformToAncestor(scrollViewer);
            var childRectangle = childTransform.TransformBounds(
                                      new Rect(new Point(0, 0), child.RenderSize));
            var ownerRectangle = new Rect(new Point(0, 0), scrollViewer.RenderSize);
            return ownerRectangle.IntersectsWith(childRectangle);
        }
    }
}
