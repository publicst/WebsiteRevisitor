using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteRevisitor.ViewModel
{
    public class VMProp<T> : ViewModelBase
    {
        private T _value;
        public T Value
        {
            get => _value;
            set { _value = value; RaisePropertyChanged(); }
        }
    }
}
