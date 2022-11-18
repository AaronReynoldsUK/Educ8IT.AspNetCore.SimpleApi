// Copyright (c) Aaron Reynolds. All rights reserved. Licensed under the Apache License, Version 2.0.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educ8IT.AspNetCore.SimpleApi.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentityDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public IdentityDbContext() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected IdentityDbContext(DbContextOptions options)
            : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiUser> ApiUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiRole> ApiRoles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiClaim> ApiClaims { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiUserToken> ApiUserTokens { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiMfa> ApiMfas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiRoleClaimLink> ApiRoleClaimLinks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiUserClaimLink> ApiUserClaimLinks { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ApiUserRoleLink> ApiUserRoleLinks { get; set; }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
