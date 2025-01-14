namespace EgenInlämning;

using Npgsql;

public class UserService : IUserService
{
    private NpgsqlConnection connection;
    private Guid? loggedInUser = null;

    public UserService(NpgsqlConnection connection)
    {
        this.connection = connection;
    }

    public User? GetLoggedInUser()
    {
        if (loggedInUser == null)
        {
            return null;
        }

        var sql = SqlQueries.GetUserSql;
        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("@id", loggedInUser);
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
                Name = reader.GetString(2),
                Password = reader.GetString(3),
                Balance = reader.GetDouble(4)
            };
        }
        catch { }
    }

    public User? Login(string username, string password)
    {
        var sql = SqlQueries.LoginSql;
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var user = new User
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(2),
            Password = reader.GetString(3),
        };

        loggedInUser = user.Id;

        return user;
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
            Name = username,
            Password = password,
            Balance = 0.00,
        };

        var sql = SqlQueries.RegUserSql;
        using var cmd = new NpgsqlCommand(sql, this.connection);
        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@name", user.Name);
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
        var sql = SqlQueries.RemoveUserSql;
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
