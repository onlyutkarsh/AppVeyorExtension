using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace AppVeyor.UI.Common
{
    public class DataContextSpy : Freezable // Enable ElementName and DataContext bindings
    {
        public DataContextSpy()
        {
            // This binding allows the spy to inherit a DataContext.
            BindingOperations.SetBinding(this, DataContextProperty, new Binding());

            this.IsSynchronizedWithCurrentItem = true;
        }

        /// <summary>
        /// Gets/sets whether the spy will return the CurrentItem of the 
        /// ICollectionView that wraps the data context, assuming it is
        /// a collection of some sort. If the data context is not a 
        /// collection, this property has no effect. 
        /// The default value is true.
        /// </summary>
        public bool IsSynchronizedWithCurrentItem { get; set; }

        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Borrow the DataContext dependency property from FrameworkElement.
        public static readonly DependencyProperty DataContextProperty =
            FrameworkElement.DataContextProperty.AddOwner(
            typeof(DataContextSpy),
            new PropertyMetadata(null, null, OnCoerceDataContext));

        static object OnCoerceDataContext(DependencyObject depObj, object value)
        {
            DataContextSpy spy = depObj as DataContextSpy;
            if (spy == null)
                return value;

            if (spy.IsSynchronizedWithCurrentItem)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(value);
                if (view != null)
                    return view.CurrentItem;
            }

            return value;
        }

        protected override Freezable CreateInstanceCore()
        {
            // We are required to override this abstract method.
            throw new NotImplementedException();
        }
    }
}
