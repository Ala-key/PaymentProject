using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class PaymentConfigurations : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.UserId);
    }
}