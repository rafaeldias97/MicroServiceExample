using System;

namespace AccountService.Domain.EventBus
{
    public interface IBaseEventBus<T> where T : class
    {
        T Publisher(T entity);
        void Subscribe(Action<string> cb);
    }
}
