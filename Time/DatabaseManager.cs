using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Time;
internal class DatabaseManager
{
    public TimerData ReadData(int selectedWork)
    {
        int id = 0;
        int workId = selectedWork;
        int h = 0;
        int m = 0;
        int s = 0;

        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = $"SELECT TOP 1 * FROM dbo.TimerData WHERE IsClosed = 0 AND WorkId = {selectedWork}";

        var command = new SqlCommand(query,connection);

        SqlDataReader reader = command.ExecuteReader();

        if (reader.HasRows)
            while (reader.Read())
            {
                id = (int)reader.GetValue(0);
                h = (int)reader.GetValue(1);
                m = (int)reader.GetValue(2);
                s = (int)reader.GetValue(3);
                workId = (int)reader.GetValue(5);
            }
        else
            CreateNewRecord(selectedWork);

        connection.Close();

        return new TimerData() {
            Id = id,
            Hours = h,
            Minutes = m,
            Seconds = s,
            IsClosed = false,
            WorkId = workId
        };
    }
    public List<Work> ReadWorks()
    {
        List<Work> works = new();
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = "SELECT * FROM dbo.Work";

        var command = new SqlCommand(query, connection);

        SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            works.Add(new((int)reader.GetValue(0), reader.GetString(1)));
        }

        connection.Close();

        return works;
    }
    public void SetData(int id, int selectedWorkId, int h, int m, int s)
    {
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = @$"  UPDATE TimerData 
                            SET  Hours = {h}
                                ,Minutes = {m}
                                ,Seconds = {s}
                            WHERE Id = {id} AND IsClosed = 0 AND WorkId = {selectedWorkId};";

        var command = new SqlCommand(query,connection);

        SqlDataAdapter adapter = new();
        adapter.UpdateCommand = command;
        adapter.UpdateCommand.ExecuteNonQuery();

        command.Dispose();

        connection.Close();
    }
    public void ResetData(int id,int selectedWorkId)
    {
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = @$"  UPDATE TimerData 
                            SET  IsClosed = TRUE
                            WHERE Id = {id} AND WorkId = {selectedWorkId}";

        var command = new SqlCommand(query,connection);

        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.UpdateCommand = command;
        adapter.UpdateCommand.ExecuteNonQuery();

        command.Dispose();

        connection.Close();
    }

    private void CreateNewRecord(int selectedWorkId)
    {
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = @$"  INSERT INTO TimerData 
                            (Hours, Minutes, Seconds, IsClosed, WorkId)
                            VALUES 
                            (0, 0, 0, 0,{selectedWorkId});";

        var command = new SqlCommand(query, connection);

        SqlDataAdapter adapter = new();
        adapter.InsertCommand = command;
        adapter.InsertCommand.ExecuteNonQuery();

        command.Dispose();

        connection.Close();
    }

    internal void AddWork(string text)
    {
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = @$"  INSERT INTO Work 
                            (WorkName)
                            VALUES 
                            (N'{text}');";

        var command = new SqlCommand(query, connection);

        SqlDataAdapter adapter = new();
        adapter.InsertCommand = command;
        adapter.InsertCommand.ExecuteNonQuery();

        command.Dispose();

        connection.Close();
    }

    internal int GetWorkId(string workName)
    {
       
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = $@"SELECT * FROM TimerDB.dbo.Work WHERE WorkName LIKE N'%{workName}%'";

        var command = new SqlCommand(query, connection);

        SqlDataReader reader = command.ExecuteReader();

        int workId = 0;
        while (reader.Read())
        {
            workId = (int)reader.GetValue(0);
        }

        connection.Close();

        return workId;
    }

    internal void RemoveWork(int selectedWorkId)
    {
        using var connection = new SqlConnection(@"Data Source=.;Initial Catalog=TimerDB;TrustServerCertificate=True;Trusted_Connection=True;");
        connection.Open();

        string query = $@"DELETE FROM dbo.Work WHERE Id = ${selectedWorkId}";

        var command = new SqlCommand(query, connection);

        SqlDataAdapter adapter = new();
        adapter.DeleteCommand = command;
        adapter.DeleteCommand.ExecuteNonQuery();

        command.Dispose();

        connection.Close();
    }
}
