//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Xml;

//using HarmonyLib;

//using Helpers;

//using TaleWorlds.CampaignSystem;
//using TaleWorlds.CampaignSystem.CharacterDevelopment;
//using TaleWorlds.CampaignSystem.Extensions;
//using TaleWorlds.CampaignSystem.Settlements;
//using TaleWorlds.Core;
//using TaleWorlds.Library;
//using TaleWorlds.Localization;
//using TaleWorlds.ObjectSystem;

//using Debugger = System.Diagnostics.Debugger;

//namespace BannerlordRestartPlus.Patches
//{
//    [HarmonyPatch(typeof(MBObjectBase))]
//    public static class MBObjectBasePatch
//    {
//        [HarmonyReversePatch]
//        [HarmonyPatch(typeof(MBObjectBase), nameof(MBObjectBase.Deserialize))]
//        [MethodImpl(MethodImplOptions.NoInlining)]
//        public static void Deserialize(MBObjectBase instance, MBObjectManager objectManager, XmlNode node)
//        {
//            Console.WriteLine($"MBObjectBasePatch.Deserialize({instance}, {objectManager}, {node})");
//        }
//    }
//    [HarmonyPatch(typeof(BasicCharacterObject))]
//    public static class BasicCharacterObjectPatch
//    {

//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(InitializeHeroBasicCharacterOnAfterLoad))]

//        public static bool PreInitializeHeroBasicCharacterOnAfterLoad(ref BasicCharacterObject __instance, BasicCharacterObject originCharacter)
//        {
//            try
//            {
//                var player = __instance.StringId?.ToLower()?.Contains("player") ?? false;
//                var main = __instance.StringId?.ToLower()?.Contains("main") ?? false;
//                if (player || main)
//                {
//                    if (Debugger.IsAttached)
//                    {
//                        Debugger.Break();
//                    }
//                }
//                //__instance.IsSoldier = originCharacter.IsSoldier;
//                //__instance._isBasicHero = originCharacter._isBasicHero;
//                //__instance.DefaultCharacterSkills = originCharacter.DefaultCharacterSkills;
//                //__instance.HairTags = originCharacter.HairTags;
//                //__instance.BeardTags = originCharacter.BeardTags;
//                //__instance.TattooTags = originCharacter.TattooTags;
//                //__instance.BodyPropertyRange = originCharacter.BodyPropertyRange;
//                //__instance.IsFemale = originCharacter.IsFemale;
//                //__instance.Race = originCharacter.Race;
//                //__instance.Culture = originCharacter.Culture;
//                //__instance.DefaultFormationGroup = originCharacter.DefaultFormationGroup;
//                //__instance.DefaultFormationClass = originCharacter.DefaultFormationClass;
//                //__instance.FormationPositionPreference = originCharacter.FormationPositionPreference;
//                //__instance._equipmentRoster = originCharacter._equipmentRoster;
//                //__instance.KnockbackResistance = originCharacter.KnockbackResistance;
//                //__instance.KnockdownResistance = originCharacter.KnockdownResistance;
//                //__instance.DismountResistance = originCharacter.DismountResistance;
//                return true;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }

//        [HarmonyPostfix]
//        [HarmonyPatch(nameof(InitializeHeroBasicCharacterOnAfterLoad))]

//        public static void PostInitializeHeroBasicCharacterOnAfterLoad(ref BasicCharacterObject __instance, BasicCharacterObject originCharacter)
//        {
//            try
//            {
//                var player = __instance.StringId?.ToLower()?.Contains("player") ?? false;
//                var main = __instance.StringId?.ToLower()?.Contains("main") ?? false;
//                if (player || main)
//                {
//                    if (Debugger.IsAttached)
//                    {
//                        Debugger.Break();
//                    }
//                }
//                //__instance.IsSoldier = originCharacter.IsSoldier;
//                //__instance._isBasicHero = originCharacter._isBasicHero;
//                //__instance.DefaultCharacterSkills = originCharacter.DefaultCharacterSkills;
//                //__instance.HairTags = originCharacter.HairTags;
//                //__instance.BeardTags = originCharacter.BeardTags;
//                //__instance.TattooTags = originCharacter.TattooTags;
//                //__instance.BodyPropertyRange = originCharacter.BodyPropertyRange;
//                //__instance.IsFemale = originCharacter.IsFemale;
//                //__instance.Race = originCharacter.Race;
//                //__instance.Culture = originCharacter.Culture;
//                //__instance.DefaultFormationGroup = originCharacter.DefaultFormationGroup;
//                //__instance.DefaultFormationClass = originCharacter.DefaultFormationClass;
//                //__instance.FormationPositionPreference = originCharacter.FormationPositionPreference;
//                //__instance._equipmentRoster = originCharacter._equipmentRoster;
//                //__instance.KnockbackResistance = originCharacter.KnockbackResistance;
//                //__instance.KnockdownResistance = originCharacter.KnockdownResistance;
//                //__instance.DismountResistance = originCharacter.DismountResistance;
//                return;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return;
//            }
//        }

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(InitializeHeroBasicCharacterOnAfterLoad))]
//        public static Exception? InitializeHeroBasicCharacterOnAfterLoad(ref Exception? __exception)
//        {
//            if (__exception != null)
//            {
//                var e = __exception;
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//            }
//            return null;
//        }

//        [HarmonyPrefix]
//        [HarmonyPatch(nameof(Deserialize))]

//        public static bool PreDeserialize(ref BasicCharacterObject __instance, MBObjectManager objectManager, XmlNode node)
//        {
//            try
//            {
//                var player = __instance.StringId?.ToLower()?.Contains("player") ?? false;
//                var main = __instance.StringId?.ToLower()?.Contains("main") ?? false;
//                if (player || main)
//                {
//                    if (Debugger.IsAttached)
//                    {
//                        Debugger.Break();
//                    }
//                }
//                //float num;
//                //MBObjectBasePatch.Deserialize((__instance as MBObjectBase), objectManager, node);
//                //XmlAttribute itemOf = node.Attributes["name"];
//                //if (itemOf != null)
//                //{
//                //    __instance.SetName(new TextObject(itemOf.Value, null));
//                //}
//                //this.HairTags = "";
//                //this.BeardTags = "";
//                //this.TattooTags = "";
//                //this.Race = 0;
//                //XmlAttribute xmlAttribute = node.Attributes["race"];
//                //if (xmlAttribute != null)
//                //{
//                //    this.Race = FaceGen.GetRaceOrDefault(xmlAttribute.Value);
//                //}
//                //XmlNode xmlNodes = node.Attributes["occupation"];
//                //if (xmlNodes != null)
//                //{
//                //    this.IsSoldier = xmlNodes.InnerText.IndexOf("soldier", StringComparison.OrdinalIgnoreCase) >= 0;
//                //}
//                //this._isBasicHero = XmlHelper.ReadBool(node, "is_hero");
//                //this.FaceMeshCache = XmlHelper.ReadBool(node, "face_mesh_cache");
//                //this.IsObsolete = XmlHelper.ReadBool(node, "is_obsolete");
//                //MBCharacterSkills mBCharacterSkill = objectManager.ReadObjectReferenceFromXml("skill_template", typeof(MBCharacterSkills), node) as MBCharacterSkills;
//                //if (mBCharacterSkill == null)
//                //{
//                //    this.DefaultCharacterSkills = MBObjectManager.Instance.CreateObject<MBCharacterSkills>(base.StringId);
//                //}
//                //else
//                //{
//                //    this.DefaultCharacterSkills = mBCharacterSkill;
//                //}
//                //BodyProperties bodyProperty = new BodyProperties();
//                //BodyProperties bodyProperty1 = new BodyProperties();
//                //foreach (XmlNode childNode in node.ChildNodes)
//                //{
//                //    if (childNode.Name == "Skills" || childNode.Name == "skills")
//                //    {
//                //        if (mBCharacterSkill != null)
//                //        {
//                //            continue;
//                //        }
//                //        this.DefaultCharacterSkills.Init(objectManager, childNode);
//                //    }
//                //    else if (childNode.Name == "Equipments" || childNode.Name == "equipments")
//                //    {
//                //        List<XmlNode> xmlNodes1 = new List<XmlNode>();
//                //        foreach (XmlNode childNode1 in childNode.ChildNodes)
//                //        {
//                //            if (childNode1.Name != "equipment")
//                //            {
//                //                continue;
//                //            }
//                //            xmlNodes1.Add(childNode1);
//                //        }
//                //        foreach (XmlNode childNode2 in childNode.ChildNodes)
//                //        {
//                //            if (childNode2.Name == "EquipmentRoster" || childNode2.Name == "equipmentRoster")
//                //            {
//                //                if (this._equipmentRoster == null)
//                //                {
//                //                    this._equipmentRoster = MBObjectManager.Instance.CreateObject<MBEquipmentRoster>(base.StringId);
//                //                }
//                //                this._equipmentRoster.Init(objectManager, childNode2);
//                //            }
//                //            else
//                //            {
//                //                if (!(childNode2.Name == "EquipmentSet") && !(childNode2.Name == "equipmentSet"))
//                //                {
//                //                    continue;
//                //                }
//                //                string innerText = childNode2.Attributes["id"].InnerText;
//                //                bool flag = (childNode2.Attributes["civilian"] == null ? false : Boolean.Parse(childNode2.Attributes["civilian"].InnerText));
//                //                if (this._equipmentRoster == null)
//                //                {
//                //                    this._equipmentRoster = MBObjectManager.Instance.CreateObject<MBEquipmentRoster>(base.StringId);
//                //                }
//                //                this._equipmentRoster.AddEquipmentRoster(MBObjectManager.Instance.GetObject<MBEquipmentRoster>(innerText), flag);
//                //            }
//                //        }
//                //        if (xmlNodes1.Count <= 0)
//                //        {
//                //            continue;
//                //        }
//                //        this._equipmentRoster.AddOverridenEquipments(objectManager, xmlNodes1);
//                //    }
//                //    else if (childNode.Name != "face")
//                //    {
//                //        if (!(childNode.Name == "Resistances") && !(childNode.Name == "resistances"))
//                //        {
//                //            continue;
//                //        }
//                //        this.KnockbackResistance = XmlHelper.ReadFloat(childNode, "knockback", 25f) * 0.01f;
//                //        this.KnockbackResistance = MBMath.ClampFloat(this.KnockbackResistance, 0f, 1f);
//                //        this.KnockdownResistance = XmlHelper.ReadFloat(childNode, "knockdown", 50f) * 0.01f;
//                //        this.KnockdownResistance = MBMath.ClampFloat(this.KnockdownResistance, 0f, 1f);
//                //        this.DismountResistance = XmlHelper.ReadFloat(childNode, "dismount", 50f) * 0.01f;
//                //        this.DismountResistance = MBMath.ClampFloat(this.DismountResistance, 0f, 1f);
//                //    }
//                //    else
//                //    {
//                //        foreach (XmlNode xmlNodes2 in childNode.ChildNodes)
//                //        {
//                //            if (xmlNodes2.Name == "hair_tags")
//                //            {
//                //                foreach (XmlNode childNode3 in xmlNodes2.ChildNodes)
//                //                {
//                //                    this.HairTags = String.Concat(this.HairTags, childNode3.Attributes["name"].Value, ",");
//                //                }
//                //            }
//                //            else if (xmlNodes2.Name == "beard_tags")
//                //            {
//                //                foreach (XmlNode xmlNodes3 in xmlNodes2.ChildNodes)
//                //                {
//                //                    this.BeardTags = String.Concat(this.BeardTags, xmlNodes3.Attributes["name"].Value, ",");
//                //                }
//                //            }
//                //            else if (xmlNodes2.Name == "tattoo_tags")
//                //            {
//                //                foreach (XmlNode childNode4 in xmlNodes2.ChildNodes)
//                //                {
//                //                    this.TattooTags = String.Concat(this.TattooTags, childNode4.Attributes["name"].Value, ",");
//                //                }
//                //            }
//                //            else if (xmlNodes2.Name == "BodyProperties")
//                //            {
//                //                if (BodyProperties.FromXmlNode(xmlNodes2, out bodyProperty))
//                //                {
//                //                    continue;
//                //                }
//                //                Debug.FailedAssert("cannot read body properties", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCharacterObject.cs", "Deserialize", 428);
//                //            }
//                //            else if (xmlNodes2.Name != "BodyPropertiesMax")
//                //            {
//                //                if (xmlNodes2.Name != "face_key_template")
//                //                {
//                //                    continue;
//                //                }
//                //                this.BodyPropertyRange = objectManager.ReadObjectReferenceFromXml<MBBodyProperty>("value", xmlNodes2);
//                //            }
//                //            else
//                //            {
//                //                if (BodyProperties.FromXmlNode(xmlNodes2, out bodyProperty1))
//                //                {
//                //                    continue;
//                //                }
//                //                bodyProperty = bodyProperty1;
//                //                Debug.FailedAssert("cannot read max body properties", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCharacterObject.cs", "Deserialize", 437);
//                //            }
//                //        }
//                //    }
//                //}
//                //if (this.BodyPropertyRange == null)
//                //{
//                //    this.BodyPropertyRange = MBObjectManager.Instance.RegisterPresumedObject<MBBodyProperty>(new MBBodyProperty(base.StringId));
//                //    this.BodyPropertyRange.Init(bodyProperty, bodyProperty1);
//                //}
//                //this.IsFemale = false;
//                //this.DefaultFormationGroup = 0;
//                //XmlNode itemOf1 = node.Attributes["is_female"];
//                //if (itemOf1 != null)
//                //{
//                //    this.IsFemale = Convert.ToBoolean(itemOf1.InnerText);
//                //}
//                //this.Culture = objectManager.ReadObjectReferenceFromXml<BasicCultureObject>("culture", node);
//                //XmlNode itemOf2 = node.Attributes["age"];
//                //if (itemOf2 == null)
//                //{
//                //    BodyProperties bodyPropertyMax = this.BodyPropertyRange.BodyPropertyMax;
//                //    num = MathF.Max(20f, bodyPropertyMax.Age);
//                //}
//                //else
//                //{
//                //    num = (float) Convert.ToInt32(itemOf2.InnerText);
//                //}
//                //this.Age = num;
//                //XmlNode itemOf3 = node.Attributes["level"];
//                //this.Level = (itemOf3 != null ? Convert.ToInt32(itemOf3.InnerText) : 1);
//                //XmlNode itemOf4 = node.Attributes["default_group"];
//                //if (itemOf4 != null)
//                //{
//                //    this.DefaultFormationGroup = this.FetchDefaultFormationGroup(itemOf4.InnerText);
//                //}
//                //this.DefaultFormationClass = this.DefaultFormationGroup;
//                //this._isRanged = this.DefaultFormationClass.IsRanged();
//                //this._isMounted = this.DefaultFormationClass.IsMounted();
//                //XmlNode xmlNodes4 = node.Attributes["formation_position_preference"];
//                //this.FormationPositionPreference = (xmlNodes4 != null ? (FormationPositionPreference) Enum.Parse(typeof(FormationPositionPreference), xmlNodes4.InnerText) : FormationPositionPreference.Middle);
//                //XmlNode itemOf5 = node.Attributes["default_equipment_set"];
//                //if (itemOf5 != null)
//                //{
//                //    this._equipmentRoster.InitializeDefaultEquipment(itemOf5.Value);
//                //}
//                //MBEquipmentRoster mBEquipmentRoster = this._equipmentRoster;
//                //if (mBEquipmentRoster == null)
//                //{
//                //    return;
//                //}
//                //mBEquipmentRoster.OrderEquipments();
//                return true;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return true;
//            }
//        }

//        [HarmonyPostfix]
//        [HarmonyPatch(nameof(Deserialize))]

//        public static void PostDeserialize(ref BasicCharacterObject __instance, MBObjectManager objectManager, XmlNode node)
//        {
//            try
//            {
//                var player = __instance.StringId?.ToLower()?.Contains("player") ?? false;
//                var main = __instance.StringId?.ToLower()?.Contains("main") ?? false;
//                if (player || main)
//                {
//                    if (Debugger.IsAttached)
//                    {
//                        Debugger.Break();
//                    }
//                }
//                //float num;
//                //MBObjectBasePatch.Deserialize((__instance as MBObjectBase), objectManager, node);
//                //XmlAttribute itemOf = node.Attributes["name"];
//                //if (itemOf != null)
//                //{
//                //    __instance.SetName(new TextObject(itemOf.Value, null));
//                //}
//                //this.HairTags = "";
//                //this.BeardTags = "";
//                //this.TattooTags = "";
//                //this.Race = 0;
//                //XmlAttribute xmlAttribute = node.Attributes["race"];
//                //if (xmlAttribute != null)
//                //{
//                //    this.Race = FaceGen.GetRaceOrDefault(xmlAttribute.Value);
//                //}
//                //XmlNode xmlNodes = node.Attributes["occupation"];
//                //if (xmlNodes != null)
//                //{
//                //    this.IsSoldier = xmlNodes.InnerText.IndexOf("soldier", StringComparison.OrdinalIgnoreCase) >= 0;
//                //}
//                //this._isBasicHero = XmlHelper.ReadBool(node, "is_hero");
//                //this.FaceMeshCache = XmlHelper.ReadBool(node, "face_mesh_cache");
//                //this.IsObsolete = XmlHelper.ReadBool(node, "is_obsolete");
//                //MBCharacterSkills mBCharacterSkill = objectManager.ReadObjectReferenceFromXml("skill_template", typeof(MBCharacterSkills), node) as MBCharacterSkills;
//                //if (mBCharacterSkill == null)
//                //{
//                //    this.DefaultCharacterSkills = MBObjectManager.Instance.CreateObject<MBCharacterSkills>(base.StringId);
//                //}
//                //else
//                //{
//                //    this.DefaultCharacterSkills = mBCharacterSkill;
//                //}
//                //BodyProperties bodyProperty = new BodyProperties();
//                //BodyProperties bodyProperty1 = new BodyProperties();
//                //foreach (XmlNode childNode in node.ChildNodes)
//                //{
//                //    if (childNode.Name == "Skills" || childNode.Name == "skills")
//                //    {
//                //        if (mBCharacterSkill != null)
//                //        {
//                //            continue;
//                //        }
//                //        this.DefaultCharacterSkills.Init(objectManager, childNode);
//                //    }
//                //    else if (childNode.Name == "Equipments" || childNode.Name == "equipments")
//                //    {
//                //        List<XmlNode> xmlNodes1 = new List<XmlNode>();
//                //        foreach (XmlNode childNode1 in childNode.ChildNodes)
//                //        {
//                //            if (childNode1.Name != "equipment")
//                //            {
//                //                continue;
//                //            }
//                //            xmlNodes1.Add(childNode1);
//                //        }
//                //        foreach (XmlNode childNode2 in childNode.ChildNodes)
//                //        {
//                //            if (childNode2.Name == "EquipmentRoster" || childNode2.Name == "equipmentRoster")
//                //            {
//                //                if (this._equipmentRoster == null)
//                //                {
//                //                    this._equipmentRoster = MBObjectManager.Instance.CreateObject<MBEquipmentRoster>(base.StringId);
//                //                }
//                //                this._equipmentRoster.Init(objectManager, childNode2);
//                //            }
//                //            else
//                //            {
//                //                if (!(childNode2.Name == "EquipmentSet") && !(childNode2.Name == "equipmentSet"))
//                //                {
//                //                    continue;
//                //                }
//                //                string innerText = childNode2.Attributes["id"].InnerText;
//                //                bool flag = (childNode2.Attributes["civilian"] == null ? false : Boolean.Parse(childNode2.Attributes["civilian"].InnerText));
//                //                if (this._equipmentRoster == null)
//                //                {
//                //                    this._equipmentRoster = MBObjectManager.Instance.CreateObject<MBEquipmentRoster>(base.StringId);
//                //                }
//                //                this._equipmentRoster.AddEquipmentRoster(MBObjectManager.Instance.GetObject<MBEquipmentRoster>(innerText), flag);
//                //            }
//                //        }
//                //        if (xmlNodes1.Count <= 0)
//                //        {
//                //            continue;
//                //        }
//                //        this._equipmentRoster.AddOverridenEquipments(objectManager, xmlNodes1);
//                //    }
//                //    else if (childNode.Name != "face")
//                //    {
//                //        if (!(childNode.Name == "Resistances") && !(childNode.Name == "resistances"))
//                //        {
//                //            continue;
//                //        }
//                //        this.KnockbackResistance = XmlHelper.ReadFloat(childNode, "knockback", 25f) * 0.01f;
//                //        this.KnockbackResistance = MBMath.ClampFloat(this.KnockbackResistance, 0f, 1f);
//                //        this.KnockdownResistance = XmlHelper.ReadFloat(childNode, "knockdown", 50f) * 0.01f;
//                //        this.KnockdownResistance = MBMath.ClampFloat(this.KnockdownResistance, 0f, 1f);
//                //        this.DismountResistance = XmlHelper.ReadFloat(childNode, "dismount", 50f) * 0.01f;
//                //        this.DismountResistance = MBMath.ClampFloat(this.DismountResistance, 0f, 1f);
//                //    }
//                //    else
//                //    {
//                //        foreach (XmlNode xmlNodes2 in childNode.ChildNodes)
//                //        {
//                //            if (xmlNodes2.Name == "hair_tags")
//                //            {
//                //                foreach (XmlNode childNode3 in xmlNodes2.ChildNodes)
//                //                {
//                //                    this.HairTags = String.Concat(this.HairTags, childNode3.Attributes["name"].Value, ",");
//                //                }
//                //            }
//                //            else if (xmlNodes2.Name == "beard_tags")
//                //            {
//                //                foreach (XmlNode xmlNodes3 in xmlNodes2.ChildNodes)
//                //                {
//                //                    this.BeardTags = String.Concat(this.BeardTags, xmlNodes3.Attributes["name"].Value, ",");
//                //                }
//                //            }
//                //            else if (xmlNodes2.Name == "tattoo_tags")
//                //            {
//                //                foreach (XmlNode childNode4 in xmlNodes2.ChildNodes)
//                //                {
//                //                    this.TattooTags = String.Concat(this.TattooTags, childNode4.Attributes["name"].Value, ",");
//                //                }
//                //            }
//                //            else if (xmlNodes2.Name == "BodyProperties")
//                //            {
//                //                if (BodyProperties.FromXmlNode(xmlNodes2, out bodyProperty))
//                //                {
//                //                    continue;
//                //                }
//                //                Debug.FailedAssert("cannot read body properties", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCharacterObject.cs", "Deserialize", 428);
//                //            }
//                //            else if (xmlNodes2.Name != "BodyPropertiesMax")
//                //            {
//                //                if (xmlNodes2.Name != "face_key_template")
//                //                {
//                //                    continue;
//                //                }
//                //                this.BodyPropertyRange = objectManager.ReadObjectReferenceFromXml<MBBodyProperty>("value", xmlNodes2);
//                //            }
//                //            else
//                //            {
//                //                if (BodyProperties.FromXmlNode(xmlNodes2, out bodyProperty1))
//                //                {
//                //                    continue;
//                //                }
//                //                bodyProperty = bodyProperty1;
//                //                Debug.FailedAssert("cannot read max body properties", "C:\\Develop\\MB3\\Source\\Bannerlord\\TaleWorlds.Core\\BasicCharacterObject.cs", "Deserialize", 437);
//                //            }
//                //        }
//                //    }
//                //}
//                //if (this.BodyPropertyRange == null)
//                //{
//                //    this.BodyPropertyRange = MBObjectManager.Instance.RegisterPresumedObject<MBBodyProperty>(new MBBodyProperty(base.StringId));
//                //    this.BodyPropertyRange.Init(bodyProperty, bodyProperty1);
//                //}
//                //this.IsFemale = false;
//                //this.DefaultFormationGroup = 0;
//                //XmlNode itemOf1 = node.Attributes["is_female"];
//                //if (itemOf1 != null)
//                //{
//                //    this.IsFemale = Convert.ToBoolean(itemOf1.InnerText);
//                //}
//                //this.Culture = objectManager.ReadObjectReferenceFromXml<BasicCultureObject>("culture", node);
//                //XmlNode itemOf2 = node.Attributes["age"];
//                //if (itemOf2 == null)
//                //{
//                //    BodyProperties bodyPropertyMax = this.BodyPropertyRange.BodyPropertyMax;
//                //    num = MathF.Max(20f, bodyPropertyMax.Age);
//                //}
//                //else
//                //{
//                //    num = (float) Convert.ToInt32(itemOf2.InnerText);
//                //}
//                //this.Age = num;
//                //XmlNode itemOf3 = node.Attributes["level"];
//                //this.Level = (itemOf3 != null ? Convert.ToInt32(itemOf3.InnerText) : 1);
//                //XmlNode itemOf4 = node.Attributes["default_group"];
//                //if (itemOf4 != null)
//                //{
//                //    this.DefaultFormationGroup = this.FetchDefaultFormationGroup(itemOf4.InnerText);
//                //}
//                //this.DefaultFormationClass = this.DefaultFormationGroup;
//                //this._isRanged = this.DefaultFormationClass.IsRanged();
//                //this._isMounted = this.DefaultFormationClass.IsMounted();
//                //XmlNode xmlNodes4 = node.Attributes["formation_position_preference"];
//                //this.FormationPositionPreference = (xmlNodes4 != null ? (FormationPositionPreference) Enum.Parse(typeof(FormationPositionPreference), xmlNodes4.InnerText) : FormationPositionPreference.Middle);
//                //XmlNode itemOf5 = node.Attributes["default_equipment_set"];
//                //if (itemOf5 != null)
//                //{
//                //    this._equipmentRoster.InitializeDefaultEquipment(itemOf5.Value);
//                //}
//                //MBEquipmentRoster mBEquipmentRoster = this._equipmentRoster;
//                //if (mBEquipmentRoster == null)
//                //{
//                //    return;
//                //}
//                //mBEquipmentRoster.OrderEquipments();
//                return;
//            }
//            catch (Exception e)
//            {
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//                return;
//            }
//        }

//        [HarmonyFinalizer]
//        [HarmonyPatch(nameof(Deserialize))]
//        public static Exception? Deserialize(ref Exception? __exception)
//        {
//            if (__exception != null)
//            {
//                var e = __exception;
//                TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
//                Debug.WriteDebugLineOnScreen(e.ToString());
//                Debug.SetCrashReportCustomString(e.Message);
//                Debug.SetCrashReportCustomStack(e.StackTrace);
//            }
//            return null;
//        }
//    }
//}