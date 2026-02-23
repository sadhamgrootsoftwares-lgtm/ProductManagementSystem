using ProductManagement.BlazorUI.Services;

namespace ProductManagement.BlazorUI.Services { 

    public class ToastService
    {
        public event Action<string, bool>? OnShow;

        public void Show(string message, bool success)
        {
            OnShow?.Invoke(message, success);
        }
    }
}