using Gdk;
using Gtk;
using Cairo;
using Color = Cairo.Color;

class Area : DrawingArea
{
    Color backgroundColor = new Color(173, 173, 173);
    
}
class MyWindow : Gtk.Window
{
    public MyWindow() : base("BomberMan")
    {
        Resize(256, 224);
        Add(new Area());
    }
    protected override bool OnDeleteEvent(Event e)
    {
        Application.Quit();
        return true;
    }
}

class Start
{
    static void Main()
    {
        Application.Init();
        MyWindow win = new MyWindow();
        win.ShowAll();
        Application.Run();
    }
}