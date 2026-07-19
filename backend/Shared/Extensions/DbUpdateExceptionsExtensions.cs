using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
//using Npgsql;
//using Microsoft.Data.Sqlite;

namespace Shared.Extensions
{
    public static class DbUpdateExceptionExtensions
    {
        public static bool IsUniqueConstraintViolation(this DbUpdateException exception)
        {
            if (exception.InnerException is null)
                return false;

            return exception.InnerException switch
            {
                // PostgresException pgEx => pgEx.SqlState == PostgresErrorCodes.UniqueViolation,

                SqlException sqlEx => sqlEx.Number is 2601 or 2627,

                // SqliteException sqliteEx => sqliteEx.SqliteErrorCode == 19 &&
                //                             (sqliteEx.ExtendedSqliteErrorCode is 2067 or 1555 ||
                //                              sqliteEx.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)),

                _ => false
            };
        }

        public static bool TryGetUniqueConstraintViolation(
            this DbUpdateException exception,
            out string? constraintName)
        {
            constraintName = null;

            if (exception.InnerException is null)
                return false;

            switch (exception.InnerException)
            {
                //case PostgresException pgEx when pgEx.SqlState == PostgresErrorCodes.UniqueViolation:
                //    constraintName = pgEx.ConstraintName; // Наприклад: "IX_Users_Email"
                //    return true;

                case SqlException sqlEx when sqlEx.Number is 2601 or 2627:
                    var sqlMatch = Regex.Match(sqlEx.Message, @"(?:index|constraint)\s+['""](?<name>[^'""]+)['""]", RegexOptions.IgnoreCase);
                    constraintName = sqlMatch.Success ? sqlMatch.Groups["name"].Value : "UnknownConstraint";
                    return true;

                //case SqliteException sqliteEx when sqliteEx.SqliteErrorCode == 19 &&
                //                                  (sqliteEx.ExtendedSqliteErrorCode is 2067 or 1555 ||
                //                                   sqliteEx.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)):
                //    var sqliteMatch = Regex.Match(sqliteEx.Message, @"failed:\s*(?<name>.+)", RegexOptions.IgnoreCase);
                //    constraintName = sqliteMatch.Success ? sqliteMatch.Groups["name"].Value.Trim() : "UnknownConstraint";
                //    return true;

                default:
                    return false;
            }
        }

        public static string CleanConstraintName(string? constraintName)
        {
            if (string.IsNullOrWhiteSpace(constraintName)) return "Unknown";

            if (constraintName.Contains('.'))
                constraintName = constraintName.Split('.').Last();

            var parts = constraintName.Split('_');
            return parts.Length > 1 ? parts.Last() : constraintName;
        }
    }
}
