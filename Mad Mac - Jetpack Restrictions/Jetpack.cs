using System.Collections.Generic;
using Sandbox.Definitions;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Mad_Mac_Jetpack_Restrictions
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    class Jetpack : MySessionComponentBase
    {
        private Dictionary<string, MyObjectBuilder_ThrustDefinition> _dataCache = new Dictionary<string, MyObjectBuilder_ThrustDefinition>();

        public override void LoadData()
        {
            var defs = MyDefinitionManager.Static.GetAllDefinitions();
            foreach (var def in defs)
            {
                var c = def as MyCharacterDefinition;
                if (c?.Jetpack == null)
                    continue;

                _dataCache[c.Id.SubtypeName] = c.Jetpack.ThrustProperties.Clone() as MyObjectBuilder_ThrustDefinition;

                MyLog.Default.WriteLineAndConsole($"JetPack: Updating {c.Id.SubtypeName}");
                c.Jetpack.ThrustProperties.ForceMagnitude = 160;
                c.Jetpack.ThrustProperties.ConsumptionFactorPerG = 100;
                c.Jetpack.ThrustProperties.SlowdownFactor = 3;
                c.Jetpack.ThrustProperties.EffectivenessAtMinInfluence = (float)12.0;
            }
        }

        protected override void UnloadData()
        {
            var defs = MyDefinitionManager.Static.GetAllDefinitions();
            foreach (var def in defs)
            {
                var c = def as MyCharacterDefinition;
                if (c?.Jetpack == null)
                    continue;

                MyObjectBuilder_ThrustDefinition cache;
                if (!_dataCache.TryGetValue(c.Id.SubtypeName, out cache) || cache == null)
                    continue;

                MyLog.Default.WriteLineAndConsole($"JetPack: Restoring {c.Id.SubtypeName} to default");
                c.Jetpack.ThrustProperties.ForceMagnitude = cache.ForceMagnitude;
                c.Jetpack.ThrustProperties.ConsumptionFactorPerG = cache.ConsumptionFactorPerG;
                c.Jetpack.ThrustProperties.SlowdownFactor = cache.SlowdownFactor;
                c.Jetpack.ThrustProperties.EffectivenessAtMinInfluence = cache.EffectivenessAtMaxInfluence;
            }
        }
    }
}