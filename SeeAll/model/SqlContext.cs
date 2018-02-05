using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeAll.model
{
    class SqlContext : DbContext
    {
        public SqlContext()
            : base("DbConnection")
        { }

        public DbSet<Model_dateTime> model_dateTime { get; set; }
        public DbSet<Model_CycleDateTime> model_CycleDateTime { get; set; }
    }
}
