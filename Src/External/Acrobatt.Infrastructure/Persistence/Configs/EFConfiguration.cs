using Acrobatt.Domain.Accounts;
using Acrobatt.Domain.Accounts.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acrobatt.Infrastructure.Persistence.Configs;

internal class EfConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(a => a.Id).HasColumnName("account_id");
        builder.HasKey(a => a.Id).HasName("pk_account_id");
        
        builder.Property(a => a.Email).HasColumnName("email");
        builder.HasIndex(a => a.Email).IsUnique().HasDatabaseName("email");

        builder.Property(a => a.FirstName).IsRequired().HasColumnName("firstName");
        
        builder.Property(a => a.LastName).IsRequired().HasColumnName("lastName");
        
        builder.Property(a => a.Pseudo).IsRequired().HasColumnName("pseudo");
        
        builder.Property(a => a.Password).IsRequired().HasColumnName("password")
            .HasConversion(password 
                => password.Value, password => new Password(password));

        builder.OwnsMany(a => a.GameConfigurations, gameConfigBuilder =>
        {
            gameConfigBuilder.Property(a => a.Id).HasColumnName("game_configuration_id");
            gameConfigBuilder.HasKey(c => c.Id).HasName("pk_game_configuration_id");
            gameConfigBuilder.Property(c => c.GameName).IsRequired().HasColumnName("game_name");
            gameConfigBuilder.Property(c => c.Duration).IsRequired().HasColumnName("duration");
            gameConfigBuilder.Property(c => c.IsPrivate).IsRequired().HasColumnName("is_private");
            gameConfigBuilder.Property(c => c.MaxPlayers).IsRequired().HasColumnName("max_players");
            gameConfigBuilder.Property(c => c.GameMode).IsRequired().HasColumnName("gameMode");
            gameConfigBuilder.Property(c => c.MaxFlags).IsRequired().HasColumnName("max_flags");
            gameConfigBuilder.OwnsMany(c => c.Teams, teamBuilder =>
            {
                teamBuilder.Property(t => t.Id).HasColumnName("team_id");
                teamBuilder.HasKey(t => t.Id).HasName("pk_team_id");
                teamBuilder.Property(t => t.Color).HasColumnName("color");
                teamBuilder.Property(t => t.NbPlayer).HasColumnName("nb_player");
                teamBuilder.Property(t => t.Name).HasColumnName("name");
                
                teamBuilder.ToTable("teams");
            });
            
            gameConfigBuilder.OwnsOne(a => a.Map, mapBuilder =>
            {
                mapBuilder.HasKey(m => m.Id).HasName("pk_map_id");
                mapBuilder.Property(m => m.Id).HasColumnName("map_id").ValueGeneratedOnAdd();
                mapBuilder.HasIndex(m => m.MapName).HasDatabaseName("map_name").IsUnique();
                mapBuilder.Property(m => m.MapName).HasColumnName("map_name").IsRequired();
                mapBuilder.Property(m => m.IsPublic).HasColumnName("is_public").HasDefaultValue(false);
                mapBuilder.Property(m => m.MapCenter).HasColumnName("map_center")
                    .HasConversion(position => position.ToString(), position => Position.FromString(position));

                mapBuilder.ToTable("maps");
            });
            gameConfigBuilder.ToTable("game_configurations");
        });

        builder.ToTable("accounts");
    }
}