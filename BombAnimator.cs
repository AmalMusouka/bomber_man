namespace bomber_man;
using Cairo;

public class BombAnimator
{
    private Dictionary<string, ImageSurface[]> animations;
    private int currentFrame = 0;
    private int frameCounter = 0;
    public int frameSpeed = 8; // ticks per switch

    private string currentAnimation = "bomb";
    
    public BombAnimator()
    {
        animations = new Dictionary<string, ImageSurface[]>
        {
            {
                "bomb", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/bomb_3.png")

                ]
            },
            {
                "explosion", [
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_1.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_2.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_3.png"),
                    new ImageSurface("/home/amalmusouka/bomber_man/sprites/explosion_4.png")
                ]
            }

        };
    }

    public bool ExplosionEnded()
    {
        return currentAnimation == "explosion" && currentFrame == animations[currentAnimation].Length - 1 && frameCounter == 0;
    }
    public void SetAnimation(string animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            currentFrame = 0;
            frameCounter = 0;
        }

        if (animation == "explosion")
        {
            frameSpeed = 2;
        }
        else
        {
            frameSpeed = 8;
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