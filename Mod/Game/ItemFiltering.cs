using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppItemFiltering;
using MelonLoader;

namespace Mod.Game
{
    internal class ItemFiltering
    {
        // You have to manually call this method from a bug with Il2cppSystem.Nullable<T>1 in the generated code

        //[CallerCount(2)]
        //[CachedScanResults(RefRangeStart = 682653, RefRangeEnd = 682655, XrefRangeStart = 682645, XrefRangeEnd = 682653, MetadataInitTokenRva = 72755696L, MetadataInitFlagRva = 95657858L)]
        //public unsafe Rule.RuleOutcome Match(ItemDataUnpacked itemData, [Out] Il2CppSystem.Nullable<int> color, [Out] Il2CppSystem.Nullable<bool> emphasize)
        //{
        //    //IL_0093: Expected native int or pointer, but got O
        //    //IL_00a9: Expected native int or pointer, but got O
        //    IL2CPP.Il2CppObjectBaseToPtrNotNull(this);
        //    System.IntPtr* ptr = stackalloc System.IntPtr[3];
        //    *ptr = IL2CPP.Il2CppObjectBaseToPtr(itemData);
        //    byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
        //    nint num2 = 0;
        //    *(nint**)num = &num2;
        //    byte* num3 = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
        //    nint num4 = 0;
        //    *(nint**)num3 = &num4;
        //    System.IntPtr exc;
        //    System.IntPtr obj = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Match_Public_RuleOutcome_ItemDataUnpacked_Nullable_1_Int32_Nullable_1_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull(this), (void**)ptr, ref exc);
        //    Il2CppException.RaiseExceptionIfNecessary(exc);
        //    nint num5 = num2;
        //    *(Il2CppSystem.Nullable<T>*)(nint)color = ((num5 == 0) ? null : new Il2CppSystem.Nullable<T>(num5));
        //    nint num6 = num4;
        //    *(Il2CppSystem.Nullable<T>*)(nint)emphasize = ((num6 == 0) ? null : new Il2CppSystem.Nullable<T>(num6));
        //    return *(Rule.RuleOutcome*)IL2CPP.il2cpp_object_unbox(obj);
        //}
        public unsafe static Rule.RuleOutcome Match(ItemDataUnpacked itemData, Il2CppSystem.Nullable<int>? color, Il2CppSystem.Nullable<bool>? emphasize)
        {
            // Get the current item filter
            ItemFilter itemFilter = ItemFilterManager.Instance.Filter; // TODO: Consider expanding this to support alternative filters in the future. 

            if (itemFilter == null)
            {
                MelonLogger.Error("ItemFiltering.Match: itemFilter is null");
                return Rule.RuleOutcome.SHOW;
            }

            System.IntPtr* ptr = stackalloc System.IntPtr[3];
            *ptr = IL2CPP.Il2CppObjectBaseToPtr(itemData);
            byte* num = (byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)));
            nint num2 = 0;
            *(nint**)num = &num2;
            byte* num3 = (byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)));
            nint num4 = 0;
            *(nint**)num3 = &num4;

            IntPtr exc = IntPtr.Zero;

            var itemFilterType = typeof(ItemFilter).GetField("NativeMethodInfoPtr_Match_Public_RuleOutcome_ItemDataUnpacked_Nullable_1_Int32_Nullable_1_Boolean_0", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            if (itemFilterType == null)
            {
                MelonLogger.Error("ItemFiltering.Match: itemFilterType is null");
                return Rule.RuleOutcome.SHOW;
            }
            System.Reflection.FieldInfo fieldInfo = itemFilterType;

            if (fieldInfo == null)
            {
                MelonLogger.Error("ItemFiltering.Match: fieldInfo is null");
                return Rule.RuleOutcome.SHOW;
            }

            IntPtr? matchMethodNullable = fieldInfo.GetValue(itemFilter) as IntPtr?;
            if (matchMethodNullable == null || matchMethodNullable == IntPtr.Zero)
            {
                MelonLogger.Error("ItemFiltering.Match: matchMethod is null");
                return Rule.RuleOutcome.SHOW;
            }

            IntPtr matchMethod = matchMethodNullable.Value;

            IntPtr obj = IL2CPP.il2cpp_runtime_invoke(matchMethod, IL2CPP.Il2CppObjectBaseToPtrNotNull(itemFilter), (void**)ptr, ref exc);

            if (obj == IntPtr.Zero)
            {
                MelonLogger.Error("ItemFiltering.Match: obj is null");
                return Rule.RuleOutcome.SHOW;
            }

            nint num5 = num2;

            if (num5 == 0)
            {
                color = null;
            }
            else
            {
                color = new Il2CppSystem.Nullable<int>((int)num5); // Explicit cast to int to avoid CS8600
            }

            nint num6 = num4;

            if (num6 == 0)
            {
                emphasize = null;
            }
            else
            {
                emphasize = new Il2CppSystem.Nullable<bool>(num6 != 0); // Explicit conversion to bool to avoid CS8600
            }

            return *(Rule.RuleOutcome*)IL2CPP.il2cpp_object_unbox(obj);
        }
    }
}
