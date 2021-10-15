using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalR.API.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace PortalR.API.Data
{
  public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){ }
        public DbSet<Value>  Values { get; set; }
    }
public class Test
{ }

}
