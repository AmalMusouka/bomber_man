namespace bomber_man;
using Gtk;
using Gdk;
using GLib;



class MyWindow : Gtk.Window
{
    Game game = new Game();
    View view;

    bool on_timeout()
    {
        game.Tick(view.player1_animator.GetCurrentFrame());
        QueueDraw();
        return true;
    }
    public MyWindow() : base("BomberMan")
    {
        SetDefaultSize(13 * 19, 11 * 21);
        SetPosition(WindowPosition.CenterOnParent);
        view = new View(game);
        Add(view);
        
        AddEvents((int)EventMask.KeyPressMask | (int)EventMask.KeyReleaseMask);

        KeyPressEvent += view.OnKeyPressEvent;
        KeyReleaseEvent += view.OnKeyReleaseEvent;
        
        Timeout.Add(30, on_timeout);
        ShowAll();
    }
    protected override bool OnDeleteEvent(Event e)
    {
        Gtk.Application.Quit();
        return true;
    }
}