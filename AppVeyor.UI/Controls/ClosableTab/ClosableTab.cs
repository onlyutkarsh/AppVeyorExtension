using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AppVeyor.UI.ViewModel;
using Microsoft.VisualStudio.Shell;

namespace AppVeyor.UI.Controls.ClosableTab
{
    public class ClosableTab : TabItem
    {
        private Label _labelTabTitle;
        private Button _btnClose;


        // Constructor
        public ClosableTab()
        {
            // Create an instance of the usercontrol
            ClosableHeader closableTabHeader = new ClosableHeader();

            // Assign the usercontrol to the tab header
            Header = closableTabHeader;

            // Attach to the ClosableHeader events (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter += new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave += new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click += new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
            _labelTabTitle = ((ClosableHeader)Header).label_TabTitle;
            _btnClose = ((ClosableHeader)Header).button_close;

            //closableTabHeader.MouseEnter += (sender, args) =>
            //{
            //    _labelTabTitle.SetResourceReference(ForegroundProperty,
            //        VsBrushes.CommandBarTextActiveKey);
            //};
            //closableTabHeader.label_TabTitle.MouseLeave += (sender, args) =>
            //{
            //    _labelTabTitle.SetResourceReference(ForegroundProperty,
            //        VsBrushes.CommandBarTextActiveKey);
            //};



        }



        /// <summary>
        /// Property - Set the Title of the Tab
        /// </summary>
        public string Title
        {
            get { return ((ClosableHeader)Header).label_TabTitle.Content as string; }
            set
            {
                ((ClosableHeader)Header).label_TabTitle.Content = value;
            }
        }


        //
        // - - - Overrides  - - -
        //


        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            _btnClose.Foreground = Brushes.White;
            _labelTabTitle.Foreground = Brushes.White;

            //((ClosableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            _btnClose.SetResourceReference(ForegroundProperty, VsBrushes.CommandBarTextActiveKey);
            _labelTabTitle.SetResourceReference(ForegroundProperty, VsBrushes.CommandBarTextActiveKey);

            //((ClosableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            //((ClosableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            //_labelTabTitle.FontWeight = FontWeights.Normal;

        }





        //
        // - - - Event Handlers  - - -
        //


        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsSelected)
            {
                _btnClose.Foreground = Brushes.White;
                return;
            }
            _btnClose.SetResourceReference(ForegroundProperty, VsBrushes.CommandBarTextActiveKey);
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            //_btnClose.Foreground = Brushes.Black;
            if (IsSelected)
            {
                _btnClose.Foreground = Brushes.White;
                return;
            }
            _btnClose.SetResourceReference(ForegroundProperty, VsBrushes.CommandBarTextActiveKey);
        }


        // Button Close Click - Remove the Tab - (or raise an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
            //((TabControl)this.Parent).Items.Remove(this);
            AppVeyorWindowViewModel.Instance.TabClosed(this);
        }


        // Label SizeChanged - When the Size of the Label changes (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _btnClose.Margin =
                new Thickness(_labelTabTitle.ActualWidth + 5, 3, 1, 0);
        }

    }
}
