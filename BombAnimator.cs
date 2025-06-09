namespace bomber_man;
using Cairo;

public class BombAnimator
{
    private Dictionary<string, ImageSurface[]> animations;
    private int current_frame = 0;
    private int frame_counter = 0;
    public int frame_speed = 8; // ticks per switch
    private string sprite_dir;
    private string current_animation = "bomb";
    
    public BombAnimator()
    {
        sprite_dir = System.IO.Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName, "sprites");        
        Console.WriteLine("Loading idle_down sprite from: " + sprite_dir);
        animations = new Dictionary<string, ImageSurface[]>
        {
            {
                "bomb", 
                [
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "bomb_1.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "bomb_2.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "bomb_3.png"))
                ]
            },
            {
                "explosion", 
                [
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "explosion_1.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "explosion_2.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "explosion_3.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "explosion_4.png"))
                ]
            },
            {
                "horizontal_explosion",
                [
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "horizontal_explosion_1.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "horizontal_explosion_2.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "horizontal_explosion_3.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "horizontal_explosion_4.png"))
                ]
            },
            {
                "vertical_explosion",
                [
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "vertical_explosion_1.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "vertical_explosion_2.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "vertical_explosion_3.png")),
                    new ImageSurface(System.IO.Path.Combine(sprite_dir, "vertical_explosion_4.png"))
                ]
            }

        };
    }

    public bool ExplosionBegin()
    {
        return current_animation == "explosion" && current_frame == 0;
    }
    public bool ExplosionEnded()
    {
        return current_animation == "explosion" && current_frame == animations[current_animation].Length - 1 && frame_counter == 0;
    }
    public void SetAnimation(string animation)
    {
        if (current_animation != animation)
        {
            current_animation = animation;
            current_frame = 0;
            frame_counter = 0;
        }
        frame_speed = animation == "explosion" ? 4 : 8;
    }

    public ImageSurface GetCurrentFrame()
    {
        return animations[current_animation][current_frame];
    }
    
    public void UpdateAnimation()
    {
        frame_counter++;
        if (frame_counter >= frame_speed)
        {
            frame_counter = 0;
            current_frame++;
            if (current_frame >= animations[current_animation].Length)
            {
                if (current_animation == "bomb")
                {
                    current_frame = 0;
                }
                else
                {
                    current_frame = animations[current_animation].Length - 1;
                }
            }
        }
    }
}