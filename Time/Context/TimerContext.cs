using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time.Context;

internal class TimerContext : DbContext
{
    //public TimerContext(DbContextOptions<TimerContext> options) : base(options){}

    public DbSet<TimerData> Timers { get; set; }
    public DbSet<Work> Works { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Data Source = Data.db");

        base.OnConfiguring(optionsBuilder);
    }
}
