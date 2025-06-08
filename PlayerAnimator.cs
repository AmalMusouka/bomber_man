namespace bomber_man;
using Cairo;

class PlayerAnimator
{
    private Dictionary<string, ImageSurface[]> animations;
    private int currentFrame = 0;
    private int frameCounter = 0;
    private int frameSpeed = 6; // ticks per switch

    private string currentAnimation = "idle_down";

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
            }
        };

    }

    public void SetAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            currentFrame = 0;
            frameCounter = 0;
        }
    }

    public ImageSurface GetCurrentFrame()
    {
        var frames = animations[currentAnimation];
        if (frames.Length == 1)
        {
            return frames[0];
        }
        
        frameCounter++;

        if (frameCounter >= frameSpeed)
        {
            frameCounter = 0;
            currentFrame = (currentFrame + 1) % frames.Length;
        }
        return frames[currentFrame];
    }
}