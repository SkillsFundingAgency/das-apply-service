using System.Data;

namespace SFA.DAS.ApplyService.Infrastructure.Database
{
    public interface IDbConnectionHelper
    {
        /// <summary>
        /// Gets an authorised connection string into the RoATP Service database.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Caller is responsible for disposing the connection.</remarks>
        /// <example>
        /// <code>
        /// using(var connection = IDbConnectionHelper.GetDatabaseConnection())
        /// {
        ///     Console.WriteLine(connection.State);
        /// }
        /// </code>
        /// </example>
        IDbConnection GetDatabaseConnection();
    }
}
