using Castle.DynamicProxy;
using Core.IoC;
using Core.Utilities.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Core.Aspects.Performance
{
    public class PerformanceAspect:MethodInterception
    {
        private int _interval; //Kaç saniye sonra kontrol edilmesi gerektiğini gösteren değer.
        private Stopwatch _stopwatch;

        public PerformanceAspect()
        {
            _interval = 3;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();

        }
        public PerformanceAspect(int interval)
        {
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
            _interval = interval;
        }
        
        //İşlem başlamadan önce stopwatch çalıştırılır.

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }
        protected override void OnAfter(IInvocation invocation)
        {
            _stopwatch.Stop();
            double totalSeconds = _stopwatch.Elapsed.TotalSeconds;
            //Toplam süre belirtilen süreyi geçtiyse
            if (totalSeconds > _interval)
            {
                //Mail Kodları
                //Database Kodları

                Debug.WriteLine($"Performans Raporu: {invocation.Method.DeclaringType.FullName}" +
                    $".{invocation.Method.Name} ==> {totalSeconds}");
            }
            _stopwatch.Reset();
        }
    }
}
