namespace bomber_man;
using Cairo;

public class BombAnimator
{
    private Dictionary<string, ImageSurface[]> animations;
    private int current_frame = 0;
    private int frame_counter = 0;
    public int frame_speed = 8; // ticks per switch

    private string current_animation = "bomb";
    
    public BombAnimator()
    {
        animations = new Dictionary<string, ImageSurface[]>
        {
            {
                "bomb", 
                [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_3.png")
                ]
            },
            {
                "explosion", 
                [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_3.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_4.png")
                ]
            },
            {
                "horizontal_explosion",
                [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/horizontal_explosion_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/horizontal_explosion_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/horizontal_explosion_3.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/horizontal_explosion_4.png")
                ]
            },
            {
                "vertical_explosion",
                [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/vertical_explosion_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/vertical_explosion_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/vertical_explosion_3.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/vertical_explosion_4.png"),
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

        if (animation == "explosion")
        {
            frame_speed = 4;
        }
        else
        {
            frame_speed = 8;
        }
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