﻿using System;
using System.Collections.Generic;
using JetBrains.Application;
using JetBrains.Application.Settings;
using JetBrains.Application.Settings.Implementation;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Scope;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.Settings;
using JetBrains.Util;

namespace JetBrains.ReSharper.Plugins.Unity.Feature.Services.LiveTemplates
{
    // Defines settings for the Unity QuickList, or we don't get a QuickList at all
    // Note that the QuickList can be empty, but it's still required
    [ShellComponent]
    public class UnityQuickListDefaultSettings : HaveDefaultSettings
    {
        private readonly ILogger myLogger;
        private readonly ISettingsSchema mySettingsSchema;
        private readonly IMainScopePoint myProjectMainPoint;
        private readonly IMainScopePoint myFilesMainPoint;

        public UnityQuickListDefaultSettings(ILogger logger, ISettingsSchema settingsSchema,
                                             UnityProjectScopeCategoryUIProvider projectScopeProvider,
                                             UnityScopeCategoryUIProvider filesScopeProvider)
            : base(logger, settingsSchema)
        {
            myLogger = logger;
            mySettingsSchema = settingsSchema;
            myProjectMainPoint = projectScopeProvider.MainPoint;
            myFilesMainPoint = filesScopeProvider.MainPoint;
        }

        public override void InitDefaultSettings(ISettingsStorageMountPoint mountPoint)
        {
            InitialiseQuickList(mountPoint, myProjectMainPoint);
            InitialiseQuickList(mountPoint, myFilesMainPoint);

            // TODO: Not sure if this would be better handled in a .dotSettings file
            AddToQuickList(mountPoint, myProjectMainPoint, "MonoBehaviour", 1, "5ff5ac38-7207-4256-91ae-b5436552db13");
            AddToQuickList(mountPoint, myProjectMainPoint, "PlayModeTest", 2, "0bcdbc13-d26e-4512-9750-fb930f532e88");
            AddToQuickList(mountPoint, myProjectMainPoint, "EditModeTest", 3, "7b7fa2c7-0ee5-4d4f-bb1f-ddbeacdbfc94");
            AddToQuickList(mountPoint, myProjectMainPoint, "StandardSurfaceShader", 4, "4b8178a2-8110-4068-a788-43b8227564e5");
            AddToQuickList(mountPoint, myProjectMainPoint, "UnlitShader", 5, "fdbd3ad2-8db2-466a-a934-4ce25cb40564");
            AddToQuickList(mountPoint, myProjectMainPoint, "ImageEffectShader", 6, "7b10542b-0a61-4bd8-ba91-e5bad4d39f5b");
        }

        private void InitialiseQuickList(ISettingsStorageMountPoint mountPoint, IMainScopePoint quickList)
        {
            var settings = new QuickListSettings {Name = quickList.QuickListTitle};
            SetIndexedKey(mountPoint, settings, new GuidIndex(quickList.QuickListUID));
        }

        private void AddToQuickList(ISettingsStorageMountPoint mountPoint, IMainScopePoint quickList, string name, int position, string guid)
        {
            var quickListKey = mySettingsSchema.GetIndexedKey<QuickListSettings>();
            var entryKey = mySettingsSchema.GetIndexedKey<EntrySettings>();
            var dictionary = new Dictionary<SettingsKey, object>
            {
                {quickListKey, new GuidIndex(quickList.QuickListUID)},
                {entryKey, new GuidIndex(new Guid(guid))}
            };

            if (!ScalarSettingsStoreAccess.IsIndexedKeyDefined(mountPoint, entryKey, dictionary, null, myLogger))
                ScalarSettingsStoreAccess.CreateIndexedKey(mountPoint, entryKey, dictionary, null, myLogger);
            SetValue(mountPoint, (EntrySettings e) => e.EntryName, name, dictionary);
            SetValue(mountPoint, (EntrySettings e) => e.Position, position, dictionary);
        }

        public override string Name => "Unity QuickList settings";
    }
}