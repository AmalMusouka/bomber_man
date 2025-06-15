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
    
    /// <summary>
    /// Sets the game window to the center and creates the window with a given size
    /// </summary>
    public MyWindow() : base("BomberMan")
    {
        SetDefaultSize(GameConfig.TILE_WIDTH * 11, GameConfig.TILE_HEIGHT * 13);
        SetPosition(WindowPosition.Center);
        view = new View(game);

        Alignment alignment = new Alignment(0.5f, 0.5f, 0, 0);
        alignment.Add(view);
        
        Add(alignment);
        
        AddEvents((int)EventMask.KeyPressMask | (int)EventMask.KeyReleaseMask);

        KeyPressEvent += view.OnKeyPressEvent;
        KeyReleaseEvent += view.OnKeyReleaseEvent;
        
        Timeout.Add(30, on_timeout);
        ShowAll();
    }
    
    /// <summary>
    /// Setting the background to a light grey
    /// </summary>
    protected override bool OnDrawn(Cairo.Context cr)
    {
        cr.SetSourceRGB(0.68, 0.68, 0.68); // RGB(173, 173, 173) light grey
        cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);
        cr.Fill();

        return base.OnDrawn(cr); // Continue normal drawing
    }
    
    protected override bool OnDeleteEvent(Event e)
    {
        Gtk.Application.Quit();
        return true;
    }
}