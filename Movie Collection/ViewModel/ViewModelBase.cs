using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Threading;

namespace Movie_Collection.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public Dispatcher Dispatcher {get;set;}


        #region Отображаемое имя
        public virtual string DisplayName { get; protected set; }
        #endregion

        #region Помощники по отладке

        /// <summary>
        /// Предупреждает разработчика, если у этого объекта нет открытого свойства с указанным именем. Этот метод не существует в сборке выпуска.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Убедитесь, что имя свойства соответствует реальному общедоступному свойству экземпляра этого объекта.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Неверное имя свойства: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Возвращает, было ли выброшено исключение или если Debug.Fail () используется, когда недопустимое имя свойства передается методу VerifyPropertyName. 
        /// Значением по умолчанию является false, но подклассы, используемые модульные тесты, могут переопределить метод получения этого свойства, чтобы вернуть значение true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;//Генерация события
            if(handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
            
        }
    }
}
