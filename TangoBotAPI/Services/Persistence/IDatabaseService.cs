using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace TangoBotApi.Services.Persistence
{
    /// <summary>
    /// Provides functionalities for database operations.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Executes a query and returns the results.
        /// </summary>
        /// <typeparam name="T">The type of the results.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of results.</returns>
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object parameters = null);

        /// <summary>
        /// Executes a command and returns the number of affected rows.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <param name="parameters">The parameters for the command.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
        Task<int> ExecuteCommandAsync(string command, object parameters = null);

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the transaction object.</returns>
        Task<IDbTransaction> BeginTransactionAsync();

        /// <summary>
        /// Commits the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction to commit.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CommitTransactionAsync(IDbTransaction transaction);

        /// <summary>
        /// Rolls back the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction to roll back.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RollbackTransactionAsync(IDbTransaction transaction);
    }
}


