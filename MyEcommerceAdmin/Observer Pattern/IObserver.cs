using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.Observer_Pattern
{
    // Observer Interface
    public interface IObserver
    {
        void Update(Order order);
    }

}