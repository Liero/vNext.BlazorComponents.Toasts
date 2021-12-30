namespace vNext.BlazorComponents.Toasts;

public class ToastReference
{
    private PeriodicTimer? _timer;
    private bool _isRunning;
    DateTime? _timeOfFirstStart;

    public event Action<ToastReference>? Refreshing;
    public event Action<ToastReference>? Closed;
    public event Action<ToastReference>? Click;
    public ToastReference(ToastOptions options)
    {
        Options = options;
    }
    public ToastOptions Options { get; set; }
    public TimeSpan Elapsed => _timeOfFirstStart == null ? TimeSpan.Zero
        : (DateTime.UtcNow - _timeOfFirstStart.Value);

    public void Close()
    {
        Pause();
        Closed?.Invoke(this);
    }

    public void Update()
    {
        Refreshing?.Invoke(this);
    }

    internal void OnClick() => Click?.Invoke(this);


    internal void Pause()
    {
        _isRunning = false;
        _timer?.Dispose();
    }


    internal async void Start()
    {
        if (_isRunning)
        {
            Pause();
        }
        _timeOfFirstStart ??= DateTime.UtcNow;
        if (Options.Timeout != default)
        {
            _isRunning = true;
            var timeout = Options.Timeout - Elapsed;
            if (timeout < Options.ExtendedTimeout)
            {
                timeout = Options.ExtendedTimeout;
            }
            _timer = new PeriodicTimer(timeout);
            await _timer.WaitForNextTickAsync();
            if (_isRunning)
            {
                Close();
            }
        }
    }
}
