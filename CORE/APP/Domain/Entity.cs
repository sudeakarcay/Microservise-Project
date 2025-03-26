namespace CORE.APP.Domain
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        protected Entity(int id)
        {
            Id = id;
        }

        protected Entity()
        {
        }
    }
}