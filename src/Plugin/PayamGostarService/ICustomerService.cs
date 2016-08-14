using System;
using System.ServiceModel;

namespace Septa.PayamGostar.ServiceLayer.ServiceType.Api
{
    /// <summary>
    /// Service contract for Payam Gostar service which handles customers(users) functionalities from and to nop commerce
    /// </summary>
    [ServiceContract]
    public interface ICustomerService
    {
        /// <summary>
        /// Returns true when a customer(user) exists in Payam Gostar customers(users)
        /// </summary>
        /// <param name="username">Username</param>
        /// /// <param name="email">Email</param>
        /// <returns>bool</returns>
        [OperationContract]
        bool IsRegistered(string username, string email);
    }
}
