
namespace Torch.UserInterface
{
    public class RadialButtonControl : ImageButtonControl
    {
        public delegate void RadialButtonDelegate();

        public RadialButtonDelegate MouseClick = () => { };
        public RadialButtonDelegate MouseOut = () => { };
        public RadialButtonDelegate MouseOver = () => { };
        public RadialButtonDelegate MouseRelease = () => { };

        protected override void OnMouseEntered()
        {
            base.OnMouseEntered();
            MouseOver.Invoke();
        }

        protected override void OnMouseLeft()
        {
            base.OnMouseLeft();
            MouseOut.Invoke();
        }

        protected override void OnMousePressed(Nuclex.Input.MouseButtons button)
        {
            base.OnMousePressed(button);
            MouseClick.Invoke();
        }

        protected override void OnMouseReleased(Nuclex.Input.MouseButtons button)
        {
            base.OnMouseReleased(button);
            MouseRelease.Invoke();
        }
    }
}
