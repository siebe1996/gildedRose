using Domain.Items;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace Domain.UnitTests.Items;

[UseReporter(typeof(DiffReporter))]
public class ItemApprovalTests
{
    [Fact]
    public void ThirtyDays()
    {
        var items = new List<Item>
        {
            new("Ring of Cleansening Code", 10, 20),
            new("Good Wine", 2, 0),
            new("Elixir of the SOLID", 5, 7),
            new("B-DAWG Keychain", 0, 80),
            new("B-DAWG Keychain", -1, 80),
            new("Backstage passes for Re:factor", 15, 20),
            new("Backstage passes for Re:factor", 10, 49),
            new("Backstage passes for HAXX", 5, 49),
            new("Duplicate Code", 3, 6),
            new("Long Methods", 3, 6),
            new("Ugly Variable Names", 3, 6)
        };

        var output = new StringBuilder();

        for (var day = 0; day < 31; day++)
        {
            output.AppendLine("-------- day " + day + " --------");
            output.AppendLine("name, sellIn, quality");

            foreach (var item in items)
            {
                output.AppendLine(item.Name + ", " + item.SellIn + ", " + item.Quality);
            }

            output.AppendLine();

            foreach (var item in items)
            {
                item.UpdateQuality();
            }
        }

        Approvals.Verify(output.ToString());
    }
}