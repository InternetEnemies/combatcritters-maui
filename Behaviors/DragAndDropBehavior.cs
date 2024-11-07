/*
This behavior will attach the drag and drop gesture to a frame and expose
DragStarting and DragCompleted events. 
*/
using Microsoft.Maui.Controls;

namespace Combat_Critters_2._0.Behaviors
{
    public class DragAndDropBehavior : Behavior<Frame>
    {
        public event EventHandler<DragStartingEventArgs>? DragStarting;
        public event EventHandler<DropEventArgs>? DropCompleted;

        protected override void OnAttachedTo(Frame bindable)
        {
            base.OnAttachedTo(bindable);
            var dragGesture = new DragGestureRecognizer();
            dragGesture.DragStarting += (s, e) => DragStarting?.Invoke(s, e);
            bindable.GestureRecognizers.Add(dragGesture);

            var dropGesture = new DropGestureRecognizer();
            dropGesture.Drop += (s, e) => DropCompleted?.Invoke(s, e);
            bindable.GestureRecognizers.Add(dropGesture);
        }

        protected override void OnDetachingFrom(Frame bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.GestureRecognizers.Clear();
        }
    }
}