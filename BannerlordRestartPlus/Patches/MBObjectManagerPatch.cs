using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

using HarmonyLib;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

using Debugger = System.Diagnostics.Debugger;

namespace BannerlordRestartPlus.Patches
{
    [HarmonyPatch(typeof(MBObjectManager))]
    public static class MBObjectManagerPatch
    {
        //[HarmonyPrefix]
        //[HarmonyPatch(nameof(LoadXML))]

        //public static bool PreLoadXML(ref MBObjectManager __instance, string id, bool isDevelopment, string gameType, bool skipXmlFilterForEditor = false)
        //{
        //    try
        //    {
        //        if (id == "NPCCharacters")
        //        {
        //            if (Debugger.IsAttached)
        //            {
        //                Debugger.Break();
        //            }
        //        }

        //        bool flag = skipXmlFilterForEditor | isDevelopment;
        //        XmlDocument mergedXmlForManaged = MBObjectManager.GetMergedXmlForManaged(id, false, flag, gameType);
        //        try
        //        {
        //            __instance.LoadXml(mergedXmlForManaged, isDevelopment);
        //        }
        //        catch (Exception exception)
        //        {
        //        }
        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //        return true;
        //    }
        //}

        //[HarmonyPostfix]
        //[HarmonyPatch(nameof(LoadXML))]

        //public static void PostLoadXML(ref BasicCharacterObject __instance, string id, bool isDevelopment, string gameType, bool skipXmlFilterForEditor = false)
        //{
        //    try
        //    {
        //        if (id == "NPCCharacters")
        //        {
        //            if (Debugger.IsAttached)
        //            {
        //                Debugger.Break();
        //            }
        //        }
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //        return;
        //    }
        //}

        //[HarmonyFinalizer]
        //[HarmonyPatch(nameof(LoadXML))]
        //public static Exception? LoadXML(ref Exception? __exception)
        //{
        //    if (__exception != null)
        //    {
        //        var e = __exception;
        //        TaleWorlds.Library.Debug.PrintError(e.Message, e.StackTrace);
        //        Debug.WriteDebugLineOnScreen(e.ToString());
        //        Debug.SetCrashReportCustomString(e.Message);
        //        Debug.SetCrashReportCustomStack(e.StackTrace);
        //    }
        //    return null;
        //}

        static FieldInfo ObjectTypeRecords = AccessTools.Field(typeof(MBObjectManager), "ObjectTypeRecords");
        public static void UpdateRegistration<T>(this MBObjectManager manager, T obj, string newId) where T: MBObjectBase
        {
            try
            {
                // Ensure registered
                manager.RegisterObject(obj);
            } catch(Exception e)
            {
                Debug.PrintWarning(e.ToString());
            }

            IEnumerator? enumerator;
            Type type = typeof(T);
            if (!type.IsSealed)
            {
                enumerator = (ObjectTypeRecords.GetValue(manager) as IList)?.GetEnumerator();
                try
                {
                    while (enumerator?.MoveNext() ?? false)
                    {
                        // Current is TaleWorlds.ObjectSystem.MBObjectManager.IObjectTypeRecord
                        object current = enumerator.Current;
                        var ObjectClass = current.GetType().DeclaredProperty("TaleWorlds.ObjectSystem.MBObjectManager.IObjectTypeRecord.ObjectClass").GetValue(current) as Type;
                        if (!type.IsAssignableFrom(ObjectClass))
                        {
                            continue;
                        }

                        var _registeredObjects = current.GetType().Field("_registeredObjects").GetValue(current) as IDictionary;
                        var _registeredObjectsWithGuid = current.GetType().Field("_registeredObjectsWithGuid").GetValue(current) as IDictionary;
                        var RegisteredObjectsList = current.GetType().Property("RegisteredObjectsList").GetValue(current) as IList;

                        //var GetNewId = current.GetType().Method("GetNewId");
                        //var newGuid = GetNewId.Invoke(current, new object[0]);

                        if ((_registeredObjects?.Contains(obj.StringId) ?? false) && (object) _registeredObjects[obj.StringId] == (object) obj)
                        {
                            _registeredObjects.Remove(obj.StringId);
                            _registeredObjects[newId] = obj;
                        }
                        if ((_registeredObjectsWithGuid?.Contains(obj.Id) ?? false) && (object) _registeredObjectsWithGuid[obj.Id] == (object) obj)
                        {
                            //_registeredObjectsWithGuid.Remove(obj.Id);
                            //obj.Id = (TaleWorlds.ObjectSystem.MBGUID) newGuid;
                            //_registeredObjectsWithGuid.Add(obj.Id, obj);
                        }
                        else
                        {
                            _registeredObjectsWithGuid?.Add(obj.Id, obj);
                        }
                        if (RegisteredObjectsList?.Contains(obj) ?? false)
                        {
                            //RegisteredObjectsList.Remove(obj);
                            //RegisteredObjectsList.Add(obj);
                        }
                        else
                        {
                            RegisteredObjectsList?.Add(obj);
                        }

                        obj.StringId = newId;
                        break;
                    }
                }
                finally
                {
                    (enumerator as IDisposable)?.Dispose();
                }
            }
            else
            {
                enumerator = (ObjectTypeRecords.GetValue(manager) as IList)?.GetEnumerator();
                try
                {
                    while (enumerator?.MoveNext() ?? false)
                    {
                        // Current is TaleWorlds.ObjectSystem.MBObjectManager.IObjectTypeRecord
                        object current = enumerator.Current;
                        var ObjectClass = current.GetType().DeclaredProperty("TaleWorlds.ObjectSystem.MBObjectManager.IObjectTypeRecord.ObjectClass").GetValue(current) as Type;
                        if (ObjectClass != type)
                        {
                            continue;
                        }

                        var _registeredObjects = current.GetType().Field("_registeredObjects").GetValue(current) as IDictionary;
                        var _registeredObjectsWithGuid = current.GetType().Field("_registeredObjectsWithGuid").GetValue(current) as IDictionary;
                        var RegisteredObjectsList = current.GetType().Property("RegisteredObjectsList").GetValue(current) as IList;

                        //var GetNewId = current.GetType().Method("GetNewId");
                        //var newGuid = GetNewId.Invoke(current, new object[0]);

                        if ((_registeredObjects?.Contains(obj.StringId) ?? false) && (object) _registeredObjects[obj.StringId] == (object) obj)
                        {
                            _registeredObjects.Remove(obj.StringId);
                            _registeredObjects[newId] = obj;
                        }
                        if ((_registeredObjectsWithGuid?.Contains(obj.Id) ?? false) && (object) _registeredObjectsWithGuid[obj.Id] == (object) obj)
                        {
                            //_registeredObjectsWithGuid.Remove(obj.Id);
                            //obj.Id = (TaleWorlds.ObjectSystem.MBGUID) newGuid;
                            //_registeredObjectsWithGuid.Add(obj.Id, obj);
                        }
                        else
                        {
                            _registeredObjectsWithGuid?.Add(obj.Id, obj);
                        }
                        if (RegisteredObjectsList?.Contains(obj) ?? false)
                        {
                            //RegisteredObjectsList.Remove(obj);
                            //RegisteredObjectsList.Add(obj);
                        }
                        else
                        {
                            RegisteredObjectsList?.Add(obj);
                        }

                        obj.StringId = newId;
                        break;
                    }
                }
                finally
                {
                    (enumerator as IDisposable)?.Dispose();
                }
            }

            obj.StringId = newId;
        }
    }
}