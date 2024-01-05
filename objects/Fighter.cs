
using MonoGame.Aseprite.Sprites;
using Microsoft.Xna.Framework;


namespace gbFighter.objects;
public class Fighter
{

    private Vector2 _position = new Vector2(10, 10);
    private int speed = 300;

    private SpriteSheet _spriteSheet;

    public Fighter(SpriteSheet _spriteSheet, Vector2 _position)
    {
        this._spriteSheet = _spriteSheet;
        this._position = _position;
    }


}
