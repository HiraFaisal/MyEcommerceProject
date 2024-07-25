using MyEcommerceAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace MyEcommerceAdmin.Observer_Pattern
{
    // Concrete Observer
    public class EmailNotificationObserver : IObserver
    {
        public void Update(Order order)
        {
            SendEmailNotification(order);
        }

        private void SendEmailNotification(Order order)
        {
            // Extract customer email
            var customerEmail = order.ShippingDetail.Email;

            // Create the email message
            var fromAddress = new MailAddress("islam23022005@gmail.com", "Islam");
            var toAddress = new MailAddress(customerEmail);
            const string subject = "Order Placed Successfully";
            var body = "Your order has been successfully placed.\n\n";
            body += "Your Order ID is : " + order.OrderID + "\n";
            body += "Order Date: " + order.OrderDate + "\n";
            body += "Customer Name: " + order.ShippingDetail.FirstName + " " + order.ShippingDetail.LastName + "\n";
            body += "Customer Email: " + order.ShippingDetail.Email + "\n";
            body += "Customer Mobile: " + order.ShippingDetail.Mobile + "\n";
            body += "Customer Address: " + order.ShippingDetail.Address + ", " +
                    order.ShippingDetail.City + ", " + order.ShippingDetail.PostCode + "\n";
            body += "\nIf above details are not correct or have any mistake email us at: islam23022005@gmail.com\n";

            // Create the SMTP client and send the email
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("islam23022005@gmail.com", "stdqawbqeemeugpa")
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    smtpClient.Send(message);
                }
                catch (SmtpException ex)
                {
                    // Log or handle the exception
                    Console.WriteLine("SMTP Exception: " + ex.Message);
                }
            }
        }
    }
}

