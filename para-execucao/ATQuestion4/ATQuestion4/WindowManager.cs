using System.Collections.Generic;
using System.Linq;

public class WindowManager
{
    private List<Window> _windows;  // Lista de janelas
    private Window _activeWindow;   // Janela ativa

    public WindowManager()
    {
        _windows = new List<Window>();
    }

    // Cria uma nova janela (CreateWindow)
    public void CreateWindow(string id, string title, int x, int y, int width, int height)
    {
        var newWindow = new Window(id, title, x, y, width, height);
        _windows.Add(newWindow);
        BringToFront(newWindow);  // Traz a nova janela para frente
    }

    // Fecha uma janela (DestroyWindow)
    public void DestroyWindow(string id)
    {
        var window = GetWindowById(id);
        if (window != null)
        {
            window.Close();
            _windows.Remove(window);

            // Se a janela fechada era a ativa, remover referência
            if (_activeWindow == window)
            {
                _activeWindow = null;
            }
        }
    }

    // Move uma janela (MoveWindow)
    public void MoveWindow(string id, int x, int y)
    {
        var window = GetWindowById(id);
        window?.Move(x, y);
    }

    // Redimensiona uma janela (ResizeWindow)
    public void ResizeWindow(string id, int width, int height)
    {
        var window = GetWindowById(id);
        window?.Resize(width, height);
    }

    // Altera o estado de uma janela (SetState)
    public void SetState(string id, string state)
    {
        var window = GetWindowById(id);
        window?.SetState(state);
    }

    // Traz uma janela para frente (BringToFront)
    public void BringToFront(Window window)
    {
        if (window.Exists && !window.IsMinimized)
        {
            int newZOrder = _windows.Max(w => w.ZOrder) + 1;
            window.BringToFront(newZOrder);
        }
    }

    // Define uma janela como ativa
    public void SetActiveWindow(string id)
    {
        var window = GetWindowById(id);
        if (window != null && window.Exists)
        {
            if (_activeWindow != null)
            {
                _activeWindow.SetActive(false);  // Desativa a janela anterior
            }

            window.SetActive(true);  // Ativa a nova janela
            _activeWindow = window;  // Atualiza a referência da janela ativa
            BringToFront(window);    // Traz a janela ativa para frente
        }
    }

    // Obtém uma janela por ID
    private Window GetWindowById(string id)
    {
        return _windows.FirstOrDefault(w => w.Id == id && w.Exists);
    }

    // Exibe o estado atual de todas as janelas
    public void DisplayWindowsState()
    {
        foreach (var window in _windows)
        {
            Console.WriteLine(window.ToString());
        }
    }
}
