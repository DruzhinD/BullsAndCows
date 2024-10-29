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
    internal class PlayerConfiguration : IEntityTypeConfiguration<PlayerEntity>
    {
        public void Configure(EntityTypeBuilder<PlayerEntity> builder)
        {
            builder.HasKey(o => o.Id);
            //делаем автоинкремент для первичного ключа
            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            //связь
            builder
                .HasMany(p => p.History)
                .WithOne(h => h.Player);

        }
    }
}
