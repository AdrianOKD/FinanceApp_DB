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
        var sql = SqlQueries.GetUserSql;
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", loggedInUser);
        if (loggedInUser == null)
        {
            return null;
        }

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
                Username = reader.GetString(2),
                Password = reader.GetString(3),
                Balance = reader.GetDouble(4),
            };
        }
        catch
        {
            System.Console.WriteLine("Unable to find account");
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
                Username = reader.GetString(2),
                Password = reader.GetString(3),
                Balance = reader.GetDouble(4),
            };

            loggedInUser = user.Id;
            return user;
        }
        catch
        {
            System.Console.WriteLine("Could not find account");
            return null;
        }
    }

    public void Logout()
    {
        loggedInUser = null;
    }

    public User RegisterUser(string username, string password)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Password = password,
            Balance = 0.00,
        };

        var sql = SqlQueries.CreateUserSql;
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@name", user.Username);
        cmd.Parameters.AddWithValue("@password", user.Password);
        cmd.Parameters.AddWithValue("@balance", user.Balance);

        cmd.ExecuteNonQuery();

        return user;
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
        cmd.Parameters.AddWithValue("@id", currentUser.Id);
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
