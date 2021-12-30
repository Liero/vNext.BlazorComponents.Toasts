# Toasts
A javascript free, unopinionated Toasts Notifications library for Blazor.

Key features:

 - Unlimited amount of toast styles (a.k.a. severity levels)
 - Unlimited amount of icons
 - Show toasts at multiple positions
 - No CSS required when using with bootstrap
 - Highly customizable and extensible
 - Update displayed toast on the fly
 - Wrap the service with your own API to best fit needs of your app
 - Easy to test

![Build & Test Main](https://github.com/Liero/vNext.BlazorComponents.Toasts/workflows/Build%20&%20Test%20Main/badge.svg)

![Nuget](https://img.shields.io/nuget/v/vNext.BlazorComponents.Toasts.svg)

## Installing

You can install from Nuget using the following command:

`Install-Package vNext.BlazorComponents.Toasts`

Or via the Visual Studio package manger.

## Getting started

Place following in MainLayout.Razor or App.razor:

```razor
@using vNext.BlazorComponents.Toasts.Components
<ToastsHost />
```

Register the service in Program.cs

```csharp
using vNext.BlazorComponents.Toasts;

...

builder.Services.AddToasts(); //use overload to configure defaults
```

or in Startup.cs

void ConfigureServices(IServiceCollection services)
{
	services.AddToasts(); 
	...
}

## Basic usage

By default, ToastHost uses boostrap classes, see https://getbootstrap.com/docs/5.1/components/toasts/.
You need to reference Bootstrap 5 or newer.

```razor
@code {
    [Inject] IToastService Toasts { get; set; } = default!;

	void Open()
    {
        var toast = Toasts.Show(new()
        {
            Message = "Click me!",
            Title = "Hello world",
            Icon = "oi oi-warning text-warning",
            Position = "bottom-0 right-0", //use existing boostrap classes 
            Timeout = TimeSpan.Zero, 
            Css = "border-warning",
            ShowCloseButton = false;
        });

        toast.Closed += e =>
        {
            //add some custom logic when toast is closed
        };

        bool clicked = false;
        toast.Click += e =>
        {
            if (!clicked) 
            {
                clicked = true;
                e.Options.Message = "Done! You can now close me!";
                e.Options.Css = "border-success";
                e.Update(); //update existing toast
            }
            else 
            {
                e.Close(); //close programatically
            }
        };
    }
}
```

## Recommended use

BlazorComponents.Toasts offers endless posibilities of styles and templates, but 
you likely want to show just couple of predefined toast styles in your app.

Define your own API that best suits needs of your app.

You can either extend the interface using extensions methods, or you can create your own service that wraps `IToastService`

```csharp
public enum ToastSeverity
{
    Default,
    Info,
    Success,
    Warning,
    Error,
}

public static class ToastServiceExtensions
{
    public static ToastReference ShowInfo(this IToastService toastService, string message, string? title = null)
        => Show(toastService, ToastSeverity.Info, message, title);
    public static ToastReference ShowSuccess(this IToastService toastService, string message, string? title = null)
        => Show(toastService, ToastSeverity.Success, message, title);
    public static ToastReference ShowWarning(this IToastService toastService, string message, string? title = null)
        => Show(toastService, ToastSeverity.Warning, message, title);
    public static ToastReference ShowError(this IToastService toastService, string message, string? title = null)
        => Show(toastService, ToastSeverity.Error, message, title);

    public static ToastReference Show(this IToastService toastService, ToastSeverity severity, string message, string? title = null, TimeSpan? timeout = null)
    {
        return Show(toastService, severity, model =>
        {
            model.Message = message;
            model.Title = title;
            if (timeout.HasValue)
            {
                model.Timeout = timeout.Value;
            }
        });            
    }

    public static ToastReference Show(this IToastService toastService, ToastSeverity severity, Action<ToastOptions> configure)
    {
        var options = new ToastOptions();
        SetSeverity(toastService, options, severity);

        configure(options);
        return toastService.Show(options);
    }

    public static void SetSeverity(this IToastService _, ToastOptions options, ToastSeverity severity)
    {
        string themeColor = severity switch
        {
            ToastSeverity.Error => "danger", //boostrap specific
            _ => severity.ToString().ToLower()
        };

        options.Icon = severity switch
        {
            ToastSeverity.Success => "bi bi-hand-thumbs-up",
            ToastSeverity.Warning => "bi bi-exclamation-triangle-fill",
            ToastSeverity.Error => "bi bi-bug",
            ToastSeverity.Info => "bi bi-info-circle-fill",
            _ => "", //no icon for default
        };
        options.Css = $"toast-{themeColor}";
    }
}
```

More restrictive alternative using facade or adapter pattern:
```
public interface IMyToastService
{
    public ToastReference Show(MyToastOptions options);

    public ToastReference ShowInfo(string message, string? title = null)
            => Show(new MyToastOptions { Severity = ToastSeverity.Info, Message = message, Title = title });
    public ToastReference ShowSuccess(string message, string? title = null)
        => Show(new MyToastOptions { Severity = ToastSeverity.Success, Message = message, Title = title });
    public ToastReference ShowWarning(string message, string? title = null)
        => Show(new MyToastOptions { Severity = ToastSeverity.Warning, Message = message, Title = title });
    public ToastReference ShowError(string message, string? title = null)
        => Show(new MyToastOptions { Severity = ToastSeverity.Error, Message = message, Title = title });

    public ToastReference ShowExceptionNotification(Exception ex, string? title = null)
            => Show(new MyToastOptions 
            { 
                Severity = ToastSeverity.Error, //severity is new property.
                Title = title,
                Body = ExceptionToastTemplate.RenderFragment()
            });

     public ToastReference ShowProgress(ProgressToastOptions options)
            => Show(options);
}

public class MyToastService : IMyToastService
{
    private readonly IToastService _toasts;

    public MyToastService(IToastService toasts)
    {
        _toasts = toasts;
    }

    public ToastReference Show(MyToastOptions options)
    {
        return _toasts.Show(options.GetRawOptions());
    }
}
```