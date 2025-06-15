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
                    new ImageSurface( "sprites/bomb_1.png"),
                    new ImageSurface( "sprites/bomb_2.png"),
                    new ImageSurface( "sprites/bomb_3.png")
                ]
            },
            {
                "explosion", 
                [
                    new ImageSurface( "sprites/explosion_1.png"),
                    new ImageSurface( "sprites/explosion_2.png"),
                    new ImageSurface( "sprites/explosion_3.png"),
                    new ImageSurface( "sprites/explosion_4.png")
                ]
            },
            {
                "horizontal_explosion",
                [
                    new ImageSurface( "sprites/horizontal_explosion_1.png"),
                    new ImageSurface( "sprites/horizontal_explosion_2.png"),
                    new ImageSurface( "sprites/horizontal_explosion_3.png"),
                    new ImageSurface( "sprites/horizontal_explosion_4.png")
                ]
            },
            {
                "horizontal_explosion_center",
                [
                    new ImageSurface( "sprites/explosion/horizontal_explosion_1_center_1.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_2_center_1.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_3_center_1.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_4_center_1.png")
                ]
            },
            {
                "horizontal_explosion_left",
                [
                    new ImageSurface( "sprites/explosion/horizontal_explosion_1_left.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_2_left.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_3_left.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_4_left.png")
                ]
            },
            {
                "horizontal_explosion_right",
                [
                    new ImageSurface( "sprites/explosion/horizontal_explosion_1_right.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_2_right.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_3_right.png"),
                    new ImageSurface( "sprites/explosion/horizontal_explosion_4_right.png")
                ]
            },
            {
                "vertical_explosion",
                [
                    new ImageSurface( "sprites/vertical_explosion_1.png"),
                    new ImageSurface( "sprites/vertical_explosion_2.png"),
                    new ImageSurface( "sprites/vertical_explosion_3.png"),
                    new ImageSurface( "sprites/vertical_explosion_4.png")
                ]
            },
            {
            
                "vertical_explosion_center",
            [
                new ImageSurface( "sprites/explosion/vertical_explosion_1_center_1.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_2_center_1.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_3_center_1.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_4_center_1.png")
            ]
            
            },
            {
                "vertical_explosion_up",
            [
                new ImageSurface( "sprites/explosion/vertical_explosion_1_up.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_2_up.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_3_up.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_4_up.png")
            ]
            
            },
            {
                "vertical_explosion_down",
            [
                new ImageSurface( "sprites/explosion/vertical_explosion_1_down.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_2_down.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_3_down.png"),
                new ImageSurface( "sprites/explosion/vertical_explosion_4_down.png")
                ]
            }

        };
    }
    
    /// <summary>
    /// Check if the explosion ended to stop the animation
    /// </summary>
    public bool ExplosionEnded()
    {
        return current_animation == "explosion" && current_frame == animations[current_animation].Length - 1 && frame_counter == 0;
    }
    
    /// <summary>
    /// Setting the frames
    /// </summary>
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

    /// <summary>
    /// Returning the current frame
    /// </summary>
    public ImageSurface GetCurrentFrame()
    {
        return animations[current_animation][current_frame];
    }
    
    /// <summary>
    /// Returning the specific explosion fram as there are different types of explosions
    /// </summary>
    public ImageSurface GetExplosionFrame(string name)
    {
        if (animations.ContainsKey(name))
        {
            return animations[name][current_frame];
        }

        return null;
    }
    
    /// <summary>
    /// Updating the animation
    /// </summary>
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