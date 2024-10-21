using System;

namespace ClientPlugin
{
    class CheckboxAttribute: Attribute
    {
        public readonly string Description;

        public CheckboxAttribute(string description = null)
        {
            this.Description = description;
        }
    }
}