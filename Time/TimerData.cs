namespace Time;
internal class TimerData
{
    public int Id { get; set; }
    public int WorkId { get; set; }
    public int Hours { get; set; }
    public int Minutes { get; set; }
    public int Seconds { get; set; }
    public bool IsClosed { get; set; }
}
internal class Work
{
    public Work(int id, string workName)
    {
        Id = id;
        WorkName = workName;
    }

    public int Id { get; set; }
    public string WorkName { get; set; }
}
