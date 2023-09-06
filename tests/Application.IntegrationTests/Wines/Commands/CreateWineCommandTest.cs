using FluentAssertions;
using Microsoft.Extensions.DependencyInjection.Wines.Commands.CreateWine;
using NUnit.Framework;
using wineo.Domain.Entities;

namespace wineo.Application.IntegrationTests.Wines.Commands;

using static Testing;


public class CreateWineCommandTest : BaseTestFixture
{
    [Test]
    public async Task ShouldCreateWine()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateWineCommand
        {
           Country = "France",
           Description = "A wine",
           Type = WineType.White,
           Name = "Chateau wineo",
           Region = "MyRegion",
           Year = 2005
        };

        var itemId = await SendAsync(command);

        var item = await FindAsync<Wine>(itemId);

        item.Should().NotBeNull();
        item.Country.Should().Be(command.Country);
        item.Region.Should().Be(command.Region);
        item.Year.Should().Be(command.Year);
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}