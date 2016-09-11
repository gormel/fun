namespace Fun1.Model
{
    public abstract class FieldOption
    {
        protected Field Field { get; }

        protected FieldOption(Field field)
        {
            Field = field;
        }

        public abstract int Cost { get; }

        public virtual bool Enabled => Cost <= Field.Score;

        public abstract string Name { get; }

        public abstract void Fire();
    }
}
