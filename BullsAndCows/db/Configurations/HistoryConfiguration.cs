using BullsAndCows.db.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BullsAndCows.db.Configurations
{
    public class HistoryConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            builder.HasKey(o => o.Id);
            //делаем автоинкремент для первичного ключа
            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            //связь
            builder
                .HasOne(h => h.Player)
                .WithMany(p => p.History)
                .HasForeignKey(h => h.Player_id);
        }
    }
}
