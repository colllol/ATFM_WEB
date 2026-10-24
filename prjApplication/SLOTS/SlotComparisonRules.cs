namespace prjApplication.SLOTS
{
    internal static class SlotComparisonRules
    {
        internal static bool IsKq3Mismatch(bool hasSlot, bool hasPermission)
        {
            return !hasSlot && !hasPermission;
        }
    }
}
