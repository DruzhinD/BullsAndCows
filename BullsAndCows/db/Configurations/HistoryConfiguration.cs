using BullsAndCows.db.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.db.Configurations
{
    public class HistoryConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            builder.HasKey(o => o.Id);

            //связь
            builder
                .HasOne(h => h.Player)
                .WithMany(p => p.History)
                .HasForeignKey(h => h.Player_id);
        }
    }
}
