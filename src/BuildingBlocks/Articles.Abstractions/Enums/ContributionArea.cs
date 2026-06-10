namespace Articles.Abstractions.Enums;

public enum ContributionArea
{
    //mandatory
    OriginalDraft = 1,
    Revision,

    //optional
    Analysis,
    Investigation,
    Visualisation
}

public static class ContributionAreaCategories
{
    public static HashSet<ContributionArea> MandatoryAreas =
    [
        ContributionArea.OriginalDraft,
        ContributionArea.Revision
    ];

    public static HashSet<ContributionArea> OptionalAreas =
    [
        ContributionArea.Analysis,
        ContributionArea.Investigation,
        ContributionArea.Visualisation
    ];
}