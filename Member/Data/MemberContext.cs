using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Member.Models;

namespace Member.Data
{
    public class MemberContext : DbContext
    {
        public MemberContext (DbContextOptions<MemberContext> options)
            : base(options)
        {
        }

        public DbSet<Member.Models.MembersModel> MembersModel { get; set; } = default!;
    }
}
