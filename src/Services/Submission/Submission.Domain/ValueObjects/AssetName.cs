namespace Submission.Domain.ValueObjects;

public class AssetName: StringValueObject
{
    private AssetName(string value) => Value = value;

    public static AssetName FromAssetType(AssetType assetType) => new AssetName(assetType.ToString());
}
