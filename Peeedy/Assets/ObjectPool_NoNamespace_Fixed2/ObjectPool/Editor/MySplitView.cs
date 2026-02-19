using UnityEngine.UIElements;

/// <summary>
/// Custom split view for the PoolManagerEditor UXML.
/// Uses [UxmlElement] (newer Unity) and also provides UxmlFactory (older Unity).
/// </summary>
[UxmlElement]
public partial class MySplitView : TwoPaneSplitView
{
    // Back-compat for Unity versions that still rely on UxmlFactory/UxmlTraits.
    public new class UxmlFactory : UxmlFactory<MySplitView, UxmlTraits> { }
}
