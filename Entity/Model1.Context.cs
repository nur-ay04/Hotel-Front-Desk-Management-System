﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Projem_Otel.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class oteldbEntities2 : DbContext
    {
        public oteldbEntities2()
            : base("name=oteldbEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Odalar> Odalar { get; set; }
        public virtual DbSet<OdaTurleri> OdaTurleri { get; set; }
        public virtual DbSet<Sehirler> Sehirler { get; set; }
        public virtual DbSet<View_1> View_1 { get; set; }
        public virtual DbSet<Durum> Durum { get; set; }
        public virtual DbSet<Musteriler> Musteriler { get; set; }
    }
}
