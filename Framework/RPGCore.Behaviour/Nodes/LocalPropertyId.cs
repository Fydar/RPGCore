using System;

namespace RPGCore.Behaviour
{
    public struct LocalPropertyId
    {
        private LocalId Target;
        private string Property;

        public LocalPropertyId(string id)
        {
            string[] components = id.Split('.');

            Target = new LocalId(components[0]);
            Property = components[1];
        }

        public override string ToString()
        {
            return $"{Target}.{Property}";
        }

        public override int GetHashCode()
        {
            return Target.GetHashCode() ^ Property.GetHashCode();
        }
    }
}