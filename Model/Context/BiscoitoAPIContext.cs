
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Context
{
    public class BiscoitoAPIContext : IdentityDbContext<BiscoitoAPIUser>
    {


        public BiscoitoAPIContext(DbContextOptions<BiscoitoAPIContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);




        }
    }
}
