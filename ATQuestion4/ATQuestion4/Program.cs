using System;

public class Program
{
    public static void Main(string[] args)
    {
        WindowManager windowManager = new WindowManager();

        // Criar janelas (CreateWindow)
        windowManager.CreateWindow("1", "Home", 100, 100, 400, 300);
        windowManager.CreateWindow("2", "Settings", 200, 150, 500, 350);
        windowManager.CreateWindow("3", "Profile", 300, 250, 600, 400);

        // Definir uma janela como ativa (SetActiveWindow)
        windowManager.SetActiveWindow("1");

        // Mover uma janela (MoveWindow)
        windowManager.MoveWindow("2", 250, 200);

        // Redimensionar uma janela (ResizeWindow)
        windowManager.ResizeWindow("3", 700, 500);

        // Minimizar uma janela (SetState)
        windowManager.SetState("2", "Minimized");

        // Traz a janela ativa para frente (BringToFront)
        windowManager.SetActiveWindow("3");

        // Fechar uma janela (DestroyWindow)
        windowManager.DestroyWindow("1");

        // Exibir o estado atual de todas as janelas
        windowManager.DisplayWindowsState();
    }
}
