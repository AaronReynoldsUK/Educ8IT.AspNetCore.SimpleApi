// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext() { }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options) { }

        public virtual DbSet<ApiUser> ApiUsers { get; set; }
        public virtual DbSet<ApiRole> ApiRoles { get; set; }
        public virtual DbSet<ApiClaim> ApiClaims { get; set; }
        public virtual DbSet<ApiUserToken> ApiUserTokens { get; set; }

        public virtual DbSet<ApiRoleClaimLink> ApiRoleClaimLinks { get; set; }
        public virtual DbSet<ApiUserClaimLink> ApiUserClaimLinks { get; set; }
        public virtual DbSet<ApiUserRoleLink> ApiUserRoleLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApiUserClaimLink>(entity =>
            {
                entity.HasKey(row =>
                    new
                    {
                        row.UserId,
                        row.ClaimId
                    });
                entity.ToTable("ApiUserClaims");
            });

            modelBuilder.Entity<ApiUserRoleLink>(entity =>
            {
                entity.HasKey(row =>
                    new
                    {
                        row.UserId,
                        row.RoleId
                    });
                entity.ToTable("ApiUserRoles");
            });

            modelBuilder.Entity<ApiRoleClaimLink>(entity =>
            {
                entity.HasKey(row =>
                    new
                    {
                        row.RoleId,
                        row.ClaimId
                    });
                entity.ToTable("ApiRoleClaims");
            });
        }
    }
}
