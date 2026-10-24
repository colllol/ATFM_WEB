using System;
using prjApplication.SLOTS;

internal static class SlotComparisonRulesTests
{
    private static int checks;
    private static void Expect(bool expected, bool hasSlot, bool hasPermission, string scenario)
    {
        bool actual = SlotComparisonRules.IsKq3Mismatch(hasSlot, hasPermission);
        if (actual != expected) throw new InvalidOperationException("KQ3 rule failed: " + scenario);
        checks++;
    }
    private static int Main()
    {
        Expect(true, false, false, "absent from both sources is KQ3");
        Expect(false, true, false, "present in SLOT only is not KQ3");
        Expect(false, false, true, "present in permission only is not KQ3");
        Expect(false, true, true, "present in both sources is not KQ3");
        Console.WriteLine("PASS " + checks + " KQ3 rule checks");
        return 0;
    }
}
