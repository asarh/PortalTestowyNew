using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalR.API.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Sqlite;


namespace PortalR.API.Data
{
  public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){ }
        public DbSet<Value> Id { get; set; }
        public DbSet<Value>  Values { get; set; }

     }
}
