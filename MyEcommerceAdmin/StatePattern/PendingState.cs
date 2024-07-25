using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyEcommerceAdmin.StatePattern
{
    public class PendingState : IOrderStatusState
    {
        private readonly string _connectionString;

        public PendingState(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Approve(int orderId)
        {
            string approveQuery = @"
                UPDATE OrderStatus
                SET Status = 'Approved'
                WHERE OrderID = @OrderID;
            ";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(approveQuery, connection);
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Cancel(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Start a transaction to ensure atomicity
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string deleteOrderStatusQuery = @"
                        DELETE FROM OrderStatus 
                        WHERE OrderID = @OrderID;
                    ";

                    SqlCommand deleteOrderStatusCommand = new SqlCommand(deleteOrderStatusQuery, connection, transaction);
                    deleteOrderStatusCommand.Parameters.AddWithValue("@OrderID", orderId);
                    deleteOrderStatusCommand.ExecuteNonQuery();
                    // Delete from OrderDetails
                    string deleteOrderDetailsQuery = @"
                        DELETE FROM OrderDetails 
                        WHERE OrderID = @OrderID;
                    ";

                    SqlCommand deleteOrderDetailsCommand = new SqlCommand(deleteOrderDetailsQuery, connection, transaction);
                    deleteOrderDetailsCommand.Parameters.AddWithValue("@OrderID", orderId);
                    deleteOrderDetailsCommand.ExecuteNonQuery();

                    // Delete from Orders
                    string deleteOrdersQuery = @"
                        DELETE FROM Orders 
                        WHERE OrderID = @OrderID;
                    ";

                    SqlCommand deleteOrdersCommand = new SqlCommand(deleteOrdersQuery, connection, transaction);
                    deleteOrdersCommand.Parameters.AddWithValue("@OrderID", orderId);
                    deleteOrdersCommand.ExecuteNonQuery();

                    // Delete from OrderStatus


                    // Commit transaction if all commands succeed
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback transaction if there's an exception
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}