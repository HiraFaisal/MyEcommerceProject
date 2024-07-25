using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.Observer_Pattern
{
    // Concrete Subject
    public class OrderSubject : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Order order)
        {
            foreach (var observer in _observers)
            {
                observer.Update(order);
            }
        }
    }

}