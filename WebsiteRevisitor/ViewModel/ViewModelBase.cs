using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WebsiteRevisitor.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase() { }

        public virtual string DisplayName { get; protected set; }

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Raised when a property on this object has a new value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged; // Good practice to store handler to prevent other thread from overwriting.
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion InotifyPropertyChanged Members

        #region IDisposable
        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform clean-up logic,
        /// such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

#if DEBUG
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected
        /// </summary>
        ~ViewModelBase()
        {
            string msg = $"{this.GetType().Name} ({this.DisplayName}) ({this.GetHashCode()}) Finalized";
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif
        #endregion IDisposable
    }
}
