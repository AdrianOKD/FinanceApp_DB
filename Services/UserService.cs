namespace EgenInlämning;

using Npgsql;

public class UserService : IUserService
{
    private readonly NpgsqlConnection connection;
    private Guid? loggedInUser = null;

    public UserService(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public User? GetLoggedInUser()
    {
        if (loggedInUser == null)
        {
            System.Console.WriteLine("No user currently logged in");
            return null;
        }

        var sql = SqlQueries.GetUserSql;
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@user_id", loggedInUser);

        try
        {
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Balance = reader.GetDouble(3),
            };
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Unable to find account{ex.Message}");
            return null;
        }
    }

    public User? Login(string username, string password)
    {
        var sql = SqlQueries.LoginSql;
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        try
        {
            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                System.Console.WriteLine("Invalid username or password");
                return null;
            }

            var user = new User
            {
                Id = reader.GetGuid(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Balance = reader.GetDouble(3),
            };

            loggedInUser = user.Id;
            System.Console.WriteLine($"Logged in with ID: {loggedInUser}");
            return user;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Could not find account: {ex.Message}");
            return null;
        }
    }

    public void Logout()
    {
        if (loggedInUser == null)
        {
            throw new InvalidOperationException("No user is currently logged in");
        }

        loggedInUser = null;
    }

    public User? RegisterUser(string username, string password)
    {
        using (var dbTransaction = connection.BeginTransaction())
        {
            try
            {
                var checkSql = SqlQueries.CheckUserNameSql;
                using (var checkCmd = new NpgsqlCommand(checkSql, this.connection, dbTransaction))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    var count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        Console.WriteLine(
                            "Username already exists. Please choose a different username."
                        );
                        dbTransaction.Rollback();
                        return null;
                    }
                }
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = username,
                    Password = password,
                    Balance = 0.00,
                };

                var sql = SqlQueries.CreateUserSql;
                using (var cmd = new NpgsqlCommand(sql, this.connection, dbTransaction))
                {
                    cmd.Parameters.AddWithValue("@user_id", user.Id);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@balance", user.Balance);

                    cmd.ExecuteNonQuery();
                }
                dbTransaction.Commit();
                Console.WriteLine($"Successfully registered user: {username}");
                return user;
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                Console.WriteLine($"Failed to register user: {ex.Message}");
                return null;
            }
        }
    }

    public void RemoveUser(string username, string password)
    {
        var currentUser = GetLoggedInUser();
        if (currentUser == null)
        {
            System.Console.WriteLine("You have to log in to remove account");
            return;
        }
        var sql = SqlQueries.DeleteUserSql;
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@user_id", currentUser.Id);
        try
        {
            cmd.ExecuteNonQuery();
            loggedInUser = null;
            System.Console.WriteLine("Account removed.");
        }
        catch
        {
            System.Console.WriteLine("Failed to remove account.");
        }
    }
}
