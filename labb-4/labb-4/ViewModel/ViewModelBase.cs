using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Fått mycket hjälp att implementera MVVM pattern från dessa källor: https://www.youtube.com/watch?v=DhSMx_MVgiQ
// Denna klass utgör basen för andra ViewModel-klasser. Föräldraklass.

namespace labb_4.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
