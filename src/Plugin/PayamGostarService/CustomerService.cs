using System;
using Septa.PayamGostar.ServiceLayer.ServiceType.Api;

namespace Septa.PgNopIntegration.Plugin.PayamGostarService
{
    /// <summary>
    /// CustomerService channel
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// Returns true when a customer(user) exists in Payam Gostar customers(users)
        /// </summary>
        /// <param name="username">Username</param>
        /// /// <param name="email">Email</param>
        /// <returns>bool</returns>
        public bool IsRegistered(string username, string email)
        {
            return new bool();
        }
    }
}
