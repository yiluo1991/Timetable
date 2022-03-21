using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.DbContext
{
    public class RunTimeFactory : IDesignTimeDbContextFactory<TimeTableDbContext>
    {
        public TimeTableDbContext CreateDbContext(string[] args)
        {
            return new TimeTableDbContext("Server = 127.0.0.1; Port = 5432; User Id = postgres; Password = 1234; Database = Timetable");
        }
    }
}
