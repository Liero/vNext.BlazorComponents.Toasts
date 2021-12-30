namespace vNext.BlazorComponents.Toasts;

public interface IToastService
{
    IReadOnlyCollection<ToastReference> ActiveToasts { get; }

    ToastReference Show(ToastOptions options);
}
