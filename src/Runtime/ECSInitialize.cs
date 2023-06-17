using System;
using System.Linq;
using System.Reflection;
using ECSExtension.Patch;
using Unity.Entities;

namespace UniverseLib.Runtime
{
    public static class ECSInitialize
    {
        public enum ECSVersion
        {
            UNKNOWN,
            NOT_USED,
            V0_17,
            V0_51,
            V1_0
        }

        private static ECSVersion _currentECSVersion = ECSVersion.UNKNOWN;

        public static ECSVersion CurrentECSVersion
        {
            get
            {
                if (_currentECSVersion != ECSVersion.UNKNOWN)
                    return _currentECSVersion;

                try
                {
                    DetermineCurrentEcsVersion();
                }
                catch (Exception)
                {
                    _currentECSVersion = ECSVersion.NOT_USED;
                }
                
                if (_currentECSVersion != ECSVersion.UNKNOWN)
                    return _currentECSVersion;

                Universe.LogWarning("Failed to determine ECS version!");
                return _currentECSVersion;
            }
        }

        private static void DetermineCurrentEcsVersion()
        {
            var archetypeFlagsType = Assembly.GetAssembly(typeof(EntityManager)).GetType("Unity.Entities.ArchetypeFlags");
            var typeFlagsNames = Enum.GetNames(archetypeFlagsType);
            if (typeFlagsNames.Contains("HasHybridComponents"))
            {
                _currentECSVersion = ECSVersion.V0_17;
            }

            if (typeFlagsNames.Contains("HasWeakAssetRefs"))
            {
                _currentECSVersion = ECSVersion.V0_51;
            }
        }
        
        internal static void Init()
        {
            if (CurrentECSVersion == ECSVersion.NOT_USED) return;
            
            if (CurrentECSVersion == ECSVersion.V0_17)
            {
                Universe.Harmony.PatchAll(typeof(World_Init_Patch));
                Universe.Harmony.PatchAll(typeof(World_Dispose_Patch));
            }
            else
            {
                ECSHelper.StartListening();
            }
        }
    }
}