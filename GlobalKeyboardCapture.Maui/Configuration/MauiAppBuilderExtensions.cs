﻿using GlobalKeyboardCapture.Maui.Core.Interfaces;
using Microsoft.Maui.LifecycleEvents;

namespace GlobalKeyboardCapture.Maui.Configuration;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseKeyboardHandling(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events =>
        {
#if WINDOWS
            events.AddWindows(windows => windows
                .OnLaunched((application, args) =>
                {
                    if (Application.Current is null)
                        return;

                    var handler = Application.Current.Handler?.MauiContext?.Services.GetService<ILifecycleHandler>();
                    handler?.OnStart();
                }));
#elif ANDROID
            events.AddAndroid(android => android
                .OnResume(activity =>
                {
                    if (Application.Current is null)
                        return;
                    
                    var handler = Application.Current.Handler?.MauiContext?.Services.GetService<ILifecycleHandler>();
                    handler?.OnResume();
                })
                .OnCreate((activity, bundle) =>
                {
                    if (Application.Current is null)
                        return;
                    
                    var handler = Application.Current.Handler?.MauiContext?.Services.GetService<ILifecycleHandler>();
                    handler?.OnStart();
                }));
#endif
        });

        return builder;
    }
}
