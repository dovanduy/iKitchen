using System;
using System.Collections.Generic;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using SunTzu.Core.Cache;
using SunTzu.Utility;

namespace SunTzu.Core.AOP
{
    public class Container
    {
        public const string CONTAINER = "container";
        private static readonly ILog logger = LogManager.GetLogger(typeof (Container));

        private IUnityContainer container;

        public Container()
        {
            container = new UnityContainer();
            
            container.AddNewExtension<Interception>();
            //container.AddNewExtension<GlobalResolveLifeTimeExtension>();

            container.RegisterInstance(typeof (Container), this);
        }

        public void EnableGlobalStopWatch(params string[] assemblyNames)
        {
            //container.AddNewExtension<GlobalInterceptExtension>();

            //container.Configure<GlobalInterceptExtension>()
            //    .SetTypeMatchingRule(new AssemblyTypeMatchingRule(assemblyNames))
            //    .AddNewPolicy("stopwatch")
            //    .AddMatchingRule(new AssemblyNamesMatchingRule(assemblyNames))
            //    .AddCallHandler<StopWatchCallHandler>();
        }

        public void RegisterInstance(Type type, object obj)
        {
            container.RegisterInstance(type, obj);
        }

        public void RegisterInstance(Type type, string name, object obj)
        {
            container.RegisterInstance(type, name, obj);
        }

        public void RegisterType(Type interfaceClazz, Type implementationClazz)
        {
            container.RegisterType(interfaceClazz, implementationClazz);
        }

        public void RegisterType(Type interfaceClazz, Type implementationClazz, string name)
        {
            container.RegisterType(interfaceClazz, implementationClazz, name);
        }

        public void RegisterType(Type interfaceClazz, Type implementationClazz, string name, LifetimeManager lifetimeManager)
        {
            container.RegisterType(interfaceClazz, implementationClazz, name, lifetimeManager);
        }

        public void RegisterType(Type interfaceClazz, Type implementationClazz, LifetimeManager lifetimeManager)
        {
            container.RegisterType(interfaceClazz, implementationClazz, lifetimeManager);
        }

        public void RegisterType(Type type, LifetimeManager lifetimeManager)
        {
            container.RegisterType(type, lifetimeManager);
        }

        public void AddInterceptor<T>()
        {
            container.Configure<Interception>()
                .SetInterceptorFor<T>(new VirtualMethodInterceptor());
        }

        public void AddInterceptor(Type type)
        {
            container.Configure<Interception>().SetInterceptorFor(type, new VirtualMethodInterceptor());
        }

        public void BuildUp(object obj)
        {
            StopWatch stopWatch = new StopWatch();
            container.BuildUp(obj.GetType(), obj);
            stopWatch.Stop();
            logger.Debug("BuildUp object, type=" + obj.GetType() + ", ElapsedTime=" + stopWatch.ElapsedMs());
        }

        public T GetInstance<T>()
        {
            return container.Resolve<T>();
        }

        public object GetInstance(Type type)
        {
            return container.Resolve(type);
        }

        public T GetInstance<T>(string name)
        {
            return container.Resolve<T>(name);
        }

        public List<T> GetInstances<T>()
        {
            return new List<T>(container.ResolveAll<T>());            
        }

        public void Dispose()
        {
            container.Dispose();
            container = null;
        }
    }
}