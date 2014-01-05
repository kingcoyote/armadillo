using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace SRPG.Scene.Battle
{
    public class FlatQueuedCommandControlRenderer : IFlatControlRenderer<QueuedCommandControl>
    {
        public void Render(QueuedCommandControl control, IFlatGuiGraphics graphics)
        {
            var controlBounds = control.GetAbsoluteBounds();

            // Draw the ability name
            graphics.DrawString("button.normal", controlBounds, control.Command.Ability.Name);
            
            var avatarBounds = new RectangleF(controlBounds.X, controlBounds.Y, controlBounds.Width, controlBounds.Height);
            avatarBounds.Height -= 10;
            avatarBounds.Width = avatarBounds.Height;
            avatarBounds.X += 5;
            avatarBounds.Y += 5;
            graphics.DrawElement("avatar.icon." + control.Command.Character.Avatar.Icon, avatarBounds);
            var abilityBounds = new RectangleF(controlBounds.X, controlBounds.Y, controlBounds.Width, controlBounds.Height);
            abilityBounds.Height -= 10;
            abilityBounds.Width = abilityBounds.Height;
            abilityBounds.X += controlBounds.Width - 5 - abilityBounds.Width;
            abilityBounds.Y += 5;
            graphics.DrawElement("radialcontrol.images." + control.Command.Ability.ImageName + ".normal", abilityBounds);
        }
    }
}
