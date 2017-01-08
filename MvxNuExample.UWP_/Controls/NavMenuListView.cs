using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace MvxNuExample.UWP.Controls
{
    public class NavMenuListView : ListView
    {
        private SplitView splitViewHost;

        public NavMenuListView()
        {
            SelectionMode = ListViewSelectionMode.Single;
            IsItemClickEnabled = true;
            ItemClick += ItemClickedHandler;

            // Locate the hosting SplitView control
            Loaded += (s, a) =>
            {
                var parent = VisualTreeHelper.GetParent(this);
                while (parent != null && !(parent is SplitView))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent != null)
                {
                    splitViewHost = parent as SplitView;

                    splitViewHost.RegisterPropertyChangedCallback(SplitView.IsPaneOpenProperty, (sender, args) =>
                    {
                        OnPaneToggled();
                    });

                    // Call once to ensure we're in the correct state
                    OnPaneToggled();
                }
            };
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Remove the entrance animation on the item containers.
            for (int i = 0; i < ItemContainerTransitions.Count; i++)
            {
                if (ItemContainerTransitions[i] is EntranceThemeTransition)
                {
                    ItemContainerTransitions.RemoveAt(i);
                }
            }
        }

        public void SetSelectedItem(ListViewItem item)
        {
            int index = -1;
            if (item != null)
            {
                index = IndexFromContainer(item);
            }

            for (int i = 0; i < Items.Count; i++)
            {
                var lvi = (ListViewItem)ContainerFromIndex(i);
                if (i != index)
                {
                    lvi.IsSelected = false;
                }
                else if (i == index)
                {
                    lvi.IsSelected = true;
                }
            }
        }

        public event EventHandler<ListViewItem> ItemInvoked;

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            var focusedItem = FocusManager.GetFocusedElement();

            switch (e.Key)
            {
                case VirtualKey.Up:
                    TryMoveFocus(FocusNavigationDirection.Up);
                    e.Handled = true;
                    break;

                case VirtualKey.Down:
                    TryMoveFocus(FocusNavigationDirection.Down);
                    e.Handled = true;
                    break;

                case VirtualKey.Tab:
                    var shiftKeyState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift);
                    var shiftKeyDown = (shiftKeyState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

                    // If we're on the header item then this will be null and we'll still get the default behavior.
                    if (focusedItem is ListViewItem)
                    {
                        var currentItem = (ListViewItem)focusedItem;
                        bool onlastitem = currentItem != null && IndexFromContainer(currentItem) == Items.Count - 1;
                        bool onfirstitem = currentItem != null && IndexFromContainer(currentItem) == 0;

                        if (!shiftKeyDown)
                        {
                            if (onlastitem)
                            {
                                TryMoveFocus(FocusNavigationDirection.Next);
                            }
                            else
                            {
                                TryMoveFocus(FocusNavigationDirection.Down);
                            }
                        }
                        else // Shift + Tab
                        {
                            if (onfirstitem)
                            {
                                TryMoveFocus(FocusNavigationDirection.Previous);
                            }
                            else
                            {
                                TryMoveFocus(FocusNavigationDirection.Up);
                            }
                        }
                    }
                    else if (focusedItem is Control)
                    {
                        if (!shiftKeyDown)
                        {
                            TryMoveFocus(FocusNavigationDirection.Down);
                        }
                        else // Shift + Tab
                        {
                            TryMoveFocus(FocusNavigationDirection.Up);
                        }
                    }

                    e.Handled = true;
                    break;

                case VirtualKey.Space:
                case VirtualKey.Enter:
                    // Fire our event using the item with current keyboard focus
                    InvokeItem(focusedItem);
                    e.Handled = true;
                    break;

                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        private void TryMoveFocus(FocusNavigationDirection direction)
        {
            if (direction == FocusNavigationDirection.Next || direction == FocusNavigationDirection.Previous)
            {
                FocusManager.TryMoveFocus(direction);
            }
            else
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                }
            }
        }

        private void ItemClickedHandler(object sender, ItemClickEventArgs e)
        {
            // Triggered when the item is selected using something other than a keyboard
            var item = ContainerFromItem(e.ClickedItem);
            InvokeItem(item);
        }

        private void InvokeItem(object focusedItem)
        {
            SetSelectedItem(focusedItem as ListViewItem);
            ItemInvoked(this, focusedItem as ListViewItem);

            if (splitViewHost.IsPaneOpen && (
                splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay ||
                splitViewHost.DisplayMode == SplitViewDisplayMode.Overlay))
            {
                splitViewHost.IsPaneOpen = false;
                if (focusedItem is ListViewItem)
                {
                    ((ListViewItem)focusedItem).Focus(FocusState.Programmatic);
                }
            }
        }

        private void OnPaneToggled()
        {
            if (splitViewHost.IsPaneOpen)
            {
                ItemsPanelRoot.ClearValue(FrameworkElement.WidthProperty);
                ItemsPanelRoot.ClearValue(FrameworkElement.HorizontalAlignmentProperty);
            }
            else if (splitViewHost.DisplayMode == SplitViewDisplayMode.CompactInline ||
                splitViewHost.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                ItemsPanelRoot.SetValue(FrameworkElement.WidthProperty, this.splitViewHost.CompactPaneLength);
                ItemsPanelRoot.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            }
        }
    }
}
