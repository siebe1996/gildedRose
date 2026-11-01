using Domain.SeedWork;

namespace Domain.Items;

public class Item : Entity<Guid>, IAggregateRoot
{
    private readonly ItemKind _kind;

    public Item(string name, int sellIn, int quality) : base(Guid.NewGuid()) // Generate a new Guid for the item, normally done by DB
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
        SellIn = sellIn;
        Quality = quality;
        _kind = GetKind(name);
    }

    public string Name { get; }
    public int SellIn { get; private set; }

    public int Quality
    {
        get;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Quality), "Quality cannot be negative.");
            }
            field = value;
        }
    }

    public void UpdateQuality()
    {
        switch (_kind)
        {
            case ItemKind.Normal:
                UpdateNormal();
                break;

            case ItemKind.GoodWine:
                UpdateGoodWine();
                break;

            case ItemKind.Backstage:
                UpdateBackstage();
                break;

            case ItemKind.Sulfuras:
                UpdateSulfuras();
                break;

            case ItemKind.Conjured:
                UpdateConjured();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateNormal()
    {
        DecreaseQuality();
        SellIn--;

        if (SellIn < 0)
        {
            DecreaseQuality();
        }
    }

    private void UpdateGoodWine()
    {
        IncreaseQuality();
        SellIn--;

        if (SellIn < 0)
        {
            IncreaseQuality();
        }
    }

    private void UpdateBackstage()
    {
        var increases = SellIn switch
        {
            < 6 => 3,
            < 11 => 2,
            _ => 1
        };

        for (var i = 0; i < increases; i++)
        {
            IncreaseQuality();
        }

        SellIn--;

        if (SellIn < 0)
        {
            Quality = 0;
        }
    }

    // ReSharper disable once MemberCanBeMadeStatic.Local
    private void UpdateSulfuras()
    {
        // Legendary item: never changes quality or sell-in.
    }

    private void UpdateConjured()
    {
        DecreaseQuality();
        DecreaseQuality();
        SellIn--;

        // ReSharper disable once InvertIf
        if (SellIn < 0)
        {
            DecreaseQuality();
            DecreaseQuality();
        }
    }


    private void IncreaseQuality()
    {
        if (Quality < 50)
        {
            Quality++;
        }
    }

    private void DecreaseQuality()
    {
        if (Quality > 0)
        {
            Quality--;
        }
    }



    private static ItemKind GetKind(string name) => name switch
    {
        "Good Wine" => ItemKind.GoodWine,
        "Backstage passes for Re:factor" or "Backstage passes for HAXX" => ItemKind.Backstage,
        "B-DAWG Keychain" => ItemKind.Sulfuras,
        "Conjured" or "Duplicate Code" or "Long Methods" or "Ugly Variable Names" => ItemKind.Conjured,
        _ => ItemKind.Normal
    };
}