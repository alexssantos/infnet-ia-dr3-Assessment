public class Window
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    public bool Exists { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsMinimized { get; private set; }
    public int XPosition { get; private set; }
    public int YPosition { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int ZOrder { get; private set; }  // Profundidade (Z-index)

    // Construtor para criar uma janela
    public Window(string id, string title, int x, int y, int width, int height)
    {
        Id = id;
        Title = title;
        Exists = true;
        IsActive = false;
        IsMinimized = false;
        XPosition = x;
        YPosition = y;
        Width = width;
        Height = height;
    }

    // Altera a posição da janela
    public void Move(int x, int y)
    {
        if (Exists && !IsMinimized)
        {
            XPosition = x;
            YPosition = y;
        }
    }

    // Redimensiona a janela
    public void Resize(int width, int height)
    {
        if (Exists && !IsMinimized)
        {
            Width = width;
            Height = height;
        }
    }

    // Minimiza ou restaura a janela
    public void SetState(string state)
    {
        if (Exists)
        {
            if (state == "Minimized") IsMinimized = true;
            else if (state == "Displayed") IsMinimized = false;
        }
    }

    // Traz a janela para a frente
    public void BringToFront(int newZOrder)
    {
        if (Exists && !IsMinimized)
        {
            ZOrder = newZOrder;
        }
    }

    // Fecha (destroi) a janela
    public void Close()
    {
        Exists = false;
    }

    // Define se a janela está ativa
    public void SetActive(bool active)
    {
        if (Exists) IsActive = active;
    }

    // Exibir o estado da janela
    public override string ToString()
    {
        return $"Window{{Id='{Id}', Title='{Title}', Exists={Exists}, IsActive={IsActive}, IsMinimized={IsMinimized}, X={XPosition}, Y={YPosition}, Width={Width}, Height={Height}, ZOrder={ZOrder}}}";
    }
}
