using Microsoft.Extensions.DependencyInjection;

namespace vNext.BlazorComponents.Toasts;

public static class ToastsServiceCollectionExtensions
{
    /// <summary>
    /// Adds <see cref="IToastService"/> as scoped serice
    /// </summary>
    /// <param name="options">usually used to configure defaults</param>
    public static IServiceCollection AddToasts(this IServiceCollection services, Action<ToastService>? options = null)
    {
        return services.AddScoped<IToastService, ToastService>(sp =>
        {
            var service = new ToastService();
            options?.Invoke(service); // configure options
            return service;
        });
    }
}

