using System.Linq;
using Microsoft.Xna.Framework;

namespace Torch
{
    public abstract class Layer : Object
    {
        public int ZIndex;
        public Scene Scene { get; private set; }
        public readonly GameComponentCollection Components;

        protected Layer(Scene scene, Object parent) : base(scene.Game, parent)
        {
            Scene = scene;
            Components = new GameComponentCollection();
        }

        // draw
        public override void Update(GameTime gametime)
        {
            foreach (var component in (from IUpdateable c in Components orderby c.UpdateOrder select c).ToArray())
            {
                component.Update(gametime);
            }
        }
        // update
        public override void Draw(GameTime gametime)
        {
            foreach (var component in (from IDrawable c in Components orderby c.DrawOrder select c).ToArray())
            {
                component.Draw(gametime);
            }
        }
    }
}
