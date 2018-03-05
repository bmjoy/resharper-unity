﻿using JetBrains.Application.Settings;
using JetBrains.Application.Settings.Extentions;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Modules;
using JetBrains.ReSharper.UnitTestFramework;
using JetBrains.ReSharper.UnitTestFramework.DotNetCore;
using JetBrains.ReSharper.UnitTestFramework.Elements;
using JetBrains.ReSharper.UnitTestFramework.Strategy;
using JetBrains.ReSharper.UnitTestProvider.nUnit.v30;

namespace JetBrains.ReSharper.Plugins.Unity.Rider.UnitTesting
{
    [SolutionComponent]
    public class UnityNUnitServiceProvider : NUnitServiceProvider
    {
        private readonly UnityEditorProtocol myUnityEditorProtocol;
        private readonly RunViaUnityEditorStrategy myUnityEditorStrategy;

        public UnityNUnitServiceProvider(ISolution solution, IPsiModules psiModules, ISymbolCache symbolCache,
            IUnitTestElementIdFactory idFactory, IUnitTestElementManager elementManager, NUnitTestProvider provider,
            ISettingsStore settingsStore, ISettingsOptimization settingsOptimization, ISettingsCache settingsCache,
            UnitTestingCachingService cachingService, IDotNetCoreSdkResolver dotNetCoreSdkResolver, 
            UnityEditorProtocol unityEditorProtocol, IUnitTestResultManager unitTestResultManager)
            : base(solution, psiModules, symbolCache, idFactory, elementManager, provider, settingsStore,
                settingsOptimization, settingsCache, cachingService, dotNetCoreSdkResolver)
        {
            myUnityEditorProtocol = unityEditorProtocol;
            myUnityEditorStrategy = new RunViaUnityEditorStrategy(solution, myUnityEditorProtocol.UnityModel.Value, unitTestResultManager, myUnityEditorProtocol);
        }

        public override IUnitTestRunStrategy GetRunStrategy(IUnitTestElement element)
        {
            if (myUnityEditorProtocol.UnityModel.Value == null)
                return base.GetRunStrategy(element);

            return myUnityEditorStrategy;
        }
    }
}