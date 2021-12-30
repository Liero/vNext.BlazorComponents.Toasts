using Microsoft.AspNetCore.Components;

namespace vNext.BlazorComponents.Toasts;

public class ToastOptions
{
    /// <summary>
    /// Either <see cref="Message"/> or <see cref="Body"/> is required
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// overrides default Title. <see cref="ToastService.ToastDefaults"/>
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// overrides default position. <see cref="ToastService.ToastDefaults"/>
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// overrides default Css. <see cref="ToastService.ToastDefaults"/>
    /// </summary>
    public string? Css { get; set; }

    /// <summary>
    /// overrides default Icon. <see cref="ToastService.ToastDefaults"/>
    /// Use empty string hide icon
    /// </summary>
    public string? Icon { get; set; }


    public bool? ShowCloseButton { get; set; }

    /// <summary>
    /// Template for toasts body - excluding Icon and Close button
    /// </summary>
    public RenderFragment<ToastReference>? Body { get; set; }

    /// <summary>
    /// Template for toasts body - excluding Close button.
    /// </summary>
    public RenderFragment<ToastReference>? Header { get; set; }

    /// <summary>
    /// Template for entire toast including <see cref="Header"/>, <see cref="Body"/>, <see cref="Icon"/> and Close button.
    /// </summary>
    public RenderFragment<ToastReference>? Template { get; set; }

    public TimeSpan Timeout { get; set; } = TimeSpan.MinValue;
    public TimeSpan ExtendedTimeout { get; set; } = TimeSpan.MinValue;


    internal void CopyDefaults(ToastOptions defaults)
    {
        Position ??= defaults.Position;
        Css ??= defaults.Css;
        Title ??= defaults.Title;
        Body ??= defaults.Body;
        Header ??= defaults.Header;
        Template ??= defaults.Template;
        ShowCloseButton ??= defaults.ShowCloseButton;
        if (Timeout == TimeSpan.MinValue) Timeout = defaults.Timeout;
        if (ExtendedTimeout == TimeSpan.MinValue) ExtendedTimeout = defaults.ExtendedTimeout;
    }
}
    