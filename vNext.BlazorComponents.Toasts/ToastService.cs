namespace vNext.BlazorComponents.Toasts;


public class ToastService : IToastService
{
    private List<ToastReference> _activeToasts = new();

    public event Action<ToastReference>? Showing;

    public ToastService()
    {
        ToastDefaults = new()
        {
            Position = "bottom-0 end-0",
            ShowCloseButton = true,
            Timeout = TimeSpan.FromSeconds(5),
            ExtendedTimeout = TimeSpan.FromSeconds(1),
        };
        ToastDefaults.Header = toast => r => r.AddContent(0, toast.Options.Title);
        ToastDefaults.Body = toast => r => r.AddContent(0, toast.Options.Message);
    }

    public IReadOnlyCollection<ToastReference> ActiveToasts => _activeToasts;

    public ToastOptions ToastDefaults { get; set; }

    public ToastReference Show(ToastOptions toastModel)
    {
        ToastReference toast = new ToastReference(toastModel);
        toast.Closed += Toast_Closed;
        toastModel.CopyDefaults(ToastDefaults);

        _activeToasts.Add(toast);
        Showing?.Invoke(toast);
        toast.Start();

        return toast;

        void Toast_Closed(ToastReference args)
        {
            _activeToasts.Remove(args);
            toast.Closed -= Toast_Closed;
        }
    }

    internal void Pause()
    {
        foreach (var toast in _activeToasts)
        {
            toast.Pause();
        }
    }
    internal void Resume()
    {
        foreach (var toast in _activeToasts)
        {
            toast.Start();
        }
    }
}
