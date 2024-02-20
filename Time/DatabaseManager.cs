using System.Collections.Generic;
using System.Linq;
using Time.Context;

namespace Time;
internal class DatabaseManager
{
    private TimerContext context;
    public TimerContext Context
    {
        get 
        {
            if (context == null)
            {
                context = new TimerContext();
                context.Database.EnsureCreated();
            }

            return context;
        }
    }

    /*public TimerData ReadData(int selectedWork)
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
    }*/
    public TimerData ReadDataSqlite(int selectedWork)
    {
        var timer = Context.Timers.FirstOrDefault(t => !t.IsClosed && t.WorkId == selectedWork);
        if (timer == null)
            timer = CreateNewRecordSqlite(selectedWork);

        return timer;
    }
    /*public List<Work> ReadWorks()
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
    }*/
    public List<Work> ReadWorksSqlite()
    {
        return Context.Works.ToList();
    }

    /*public void SetData(int id, int selectedWorkId, int h, int m, int s)
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
    }*/
    public void SetDataSqlite(int id, int selectedWorkId, int h, int m, int s)
    {
        var timerData = Context.Timers.FirstOrDefault(t => t.Id == id && !t.IsClosed && t.WorkId == selectedWorkId);
        if (timerData == null) return;

        timerData.Hours = h;
        timerData.Minutes = m;
        timerData.Seconds = s;

        Context.Update(timerData);
        Context.SaveChanges();
    }
    /*public void ResetData(int id,int selectedWorkId)
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
    }*/
    public void ResetDataSqlite(int id, int selectedWorkId)
    {
        var timerData = Context.Timers.FirstOrDefault(t => t.Id == id && !t.IsClosed && t.WorkId == selectedWorkId);
        if (timerData == null) return;

        timerData.IsClosed = true;

        Context.Update(timerData);
        Context.SaveChanges();
    }

    /*private void CreateNewRecord(int selectedWorkId)
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
    }*/
    private TimerData CreateNewRecordSqlite(int selectedWorkId)
    {
        var timerData = new TimerData()
        {
            Hours = 0,
            Minutes = 0,
            Seconds = 0,
            WorkId = selectedWorkId,
            IsClosed = false,
        };

        Context.Timers.Add(timerData);
        Context.SaveChanges();
        return timerData;
    }

    /*internal void AddWork(string text)
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
    }*/
    internal void AddWorkSqlite(string text)
    {
        Context.Works.Add(new Work(text));
        Context.SaveChanges();
    }

    /*internal int GetWorkId(string workName)
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
    }*/
    internal int GetWorkIdSqlite(string workName)
    {
       
        var work = Context.Works.FirstOrDefault(w=>w.WorkName.Contains(workName));

        return work?.Id ?? -1;
    }

    /*internal void RemoveWork(int selectedWorkId)
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
    }*/
    internal void RemoveWorkSqlite(int selectedWorkId)
    {
        var work = Context.Works.FirstOrDefault(w=>w.Id == selectedWorkId);
        if (work == null) return;
        Context.Works.Remove(work);
        Context.SaveChanges();
    }
}
