namespace bomber_man;
using Cairo;

class PlayerAnimator
{
    private Dictionary<string, ImageSurface[]> animations;
    private int current_frame = 0;
    private int frame_counter = 0;
    private int frame_speed = 6; // ticks per switch

    public string current_animation = "idle_down";

    public PlayerAnimator()
    {
        animations = new Dictionary<string, ImageSurface[]>
        {
            {
                "idle_down", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png")
                ]
            },
            {
                "idle_left", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_left.png")
                ]
            },
            {
                "idle_right", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_right.png")
                ]
            },
            {
                "idle_up", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_up.png")
                ]
            },
            {
                "move_down", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_down1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_down2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_down.png"),
                ]
            },
            {
                "move_right", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_right1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_right.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_right2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_right.png"),
                ]
            },
            {
                "move_left", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_left1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_left.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_left2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_left.png"),
                ]   
            },
            {
                "move_up", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_up1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_up.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_walk_up2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_idle_up.png"),
                ]   
            },
            {
                "death", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_3.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_4.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_5.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_6.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomberman_die_7.png"),
                ]
            }
        };

    }

    public bool PlayerDeath()
    {
        return current_animation == "death" && current_frame== 0;
    }

    public bool PlayerDeathEnd()
    {
        return current_animation == "death" && current_frame == animations[current_animation].Length - 1 && frame_counter == 0;
    }

    public void SetAnimation(string animation)
    {
        if (current_animation != animation)
        {
            current_animation = animation;
            current_frame = 0;
            frame_counter = 0;
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
            current_frame = (current_frame + 1) % animations[current_animation].Length;
        }
    }
}