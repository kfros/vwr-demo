using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vwr.Domain.Entities;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> o) : base(o) { }
    public DbSet<EventEntity> Events => Set<EventEntity>();
    public DbSet<ReceiptEntity> Receipts => Set<ReceiptEntity>();
}