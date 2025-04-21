using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppItemFiltering;
using MelonLoader;
using System.Reflection;

namespace Mod.Game
{
    internal class ItemFiltering
    {
        // You have to manually call this method from a bug with Il2cppSystem.Nullable<T>1 in the generated code
        //[CallerCount(2)]
        //[CachedScanResults(RefRangeStart = 755433, RefRangeEnd = 755435, XrefRangeStart = 755420, XrefRangeEnd = 755433, MetadataInitTokenRva = 0, MetadataInitFlagRva = 0)]
        //public unsafe Rule.RuleOutcome Match(ItemDataUnpacked itemData, out Il2CppSystem.Nullable<int> color,
        //      out Il2CppSystem.Nullable<bool> emphasize, out int matchingRuleNumber)
        //{
        //    IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)this);
        //    System.IntPtr* numPtr1 = stackalloc System.IntPtr[4];
        //    numPtr1[0] = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)itemData);
        //    System.IntPtr num1 = (System.IntPtr)numPtr1 + checked(new System.IntPtr(1) * sizeof(System.IntPtr));
        //    System.IntPtr zero1 = System.IntPtr.Zero;
        //    System.IntPtr* numPtr2 = &zero1;
        //    *(System.IntPtr*)num1 = (System.IntPtr)numPtr2;
        //    System.IntPtr num2 = (System.IntPtr)numPtr1 + checked(new System.IntPtr(2) * sizeof(System.IntPtr));
        //    System.IntPtr zero2 = System.IntPtr.Zero;
        //    System.IntPtr* numPtr3 = &zero2;
        //    *(System.IntPtr*)num2 = (System.IntPtr)numPtr3;
        //    *(System.IntPtr*)((System.IntPtr)numPtr1 + checked(new System.IntPtr(3) * sizeof(System.IntPtr))) = (System.IntPtr)ref matchingRuleNumber;
        //    System.IntPtr num3;
        //    System.IntPtr num4 = IL2CPP.il2cpp_runtime_invoke(ItemFilter.NativeMethodInfoPtr_Match_Public_RuleOutcome_ItemDataUnpacked_byref_Nullable_1_Int32_byref_Nullable_1_Boolean_byref_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)this), (void**)numPtr1, ref num3);
        //    Il2CppException.RaiseExceptionIfNecessary(num3);
        //    ref Il2CppSystem.Nullable<int> local1 = ref color;
        //    System.IntPtr pointer1 = zero1;
        //    Il2CppSystem.Nullable<int> nullable1 = pointer1 == System.IntPtr.Zero ? (Il2CppSystem.Nullable<int>)null : new Il2CppSystem.Nullable<int>(pointer1);
        //    local1 = nullable1;
        //    ref Il2CppSystem.Nullable<bool> local2 = ref emphasize;
        //    System.IntPtr pointer2 = zero2;
        //    Il2CppSystem.Nullable<bool> nullable2 = pointer2 == System.IntPtr.Zero ? (Il2CppSystem.Nullable<bool>)null : new Il2CppSystem.Nullable<bool>(pointer2);
        //    local2 = nullable2;
        //    return *(Rule.RuleOutcome*)IL2CPP.il2cpp_object_unbox(num4);
        //}

        public unsafe static Rule.RuleOutcome Match(ItemDataUnpacked itemData, Il2CppSystem.Nullable<int>? color, Il2CppSystem.Nullable<bool>? emphasize, int matchingRuleNumber)
        {
            color = null;
            emphasize = null;
            matchingRuleNumber = 0;

            var itemFilter = ItemFilterManager.Instance.Filter;
            if (itemFilter == null)
            {
                MelonLogger.Error("ItemFiltering.Match: itemFilter is null");
                return Rule.RuleOutcome.SHOW;
            }

            IntPtr* ptr = stackalloc IntPtr[4];
            ptr[0] = IL2CPP.Il2CppObjectBaseToPtr(itemData);

            nint colorNative = 0;
            nint emphasizeNative = 0;
            int ruleNum = 0;

            ptr[1] = (IntPtr)(&colorNative);
            ptr[2] = (IntPtr)(&emphasizeNative);
            ptr[3] = (IntPtr)(&ruleNum);

            IntPtr exc = IntPtr.Zero;

            var methodField = typeof(ItemFilter)
                .GetField("NativeMethodInfoPtr_Match_Public_RuleOutcome_ItemDataUnpacked_byref_Nullable_1_Int32_byref_Nullable_1_Boolean_byref_Int32_0",
                    BindingFlags.NonPublic | BindingFlags.Static);

            if (methodField?.GetValue(itemFilter) is not IntPtr matchMethod || matchMethod == IntPtr.Zero)
            {
                MelonLogger.Error("ItemFiltering.Match: native method pointer not found");
                return Rule.RuleOutcome.SHOW;
            }

            IntPtr result = IL2CPP.il2cpp_runtime_invoke(matchMethod, IL2CPP.Il2CppObjectBaseToPtrNotNull(itemFilter), (void**)ptr, ref exc);
            if (exc != IntPtr.Zero)
            {
                Il2CppException.RaiseExceptionIfNecessary(exc);
                return Rule.RuleOutcome.SHOW;
            }

            if (colorNative != 0)
                color = new Il2CppSystem.Nullable<int>((int)colorNative);

            if (emphasizeNative != 0)
                emphasize = new Il2CppSystem.Nullable<bool>(emphasizeNative != 0);

            matchingRuleNumber = ruleNum;

            return *(Rule.RuleOutcome*)IL2CPP.il2cpp_object_unbox(result);
        }
    }
}
