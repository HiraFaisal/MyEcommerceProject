using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.Observer_Pattern
{
    // Subject Interface
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(Order order);
    }

}