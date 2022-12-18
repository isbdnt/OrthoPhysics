
namespace OrthoPhysics
{
    public class BodyLinkedList
    {
        public Body current;
        public BodyLinkedList next;

        public BodyLinkedList(Body current, BodyLinkedList next)
        {
            this.current = current;
            this.next = next;
        }
    }
}
