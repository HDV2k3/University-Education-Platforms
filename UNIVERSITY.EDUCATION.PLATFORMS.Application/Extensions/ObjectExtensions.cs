namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions
{
    public static class ObjectExtensions
    {
        public static Guid ToGuid(this object? value, Guid defaultValue = default)
        {
            if (value == null) return defaultValue;

            if (value is Guid g) return g;

            if (Guid.TryParse(value.ToString(), out Guid parsed))
                return parsed;

            return defaultValue;
        }

        public static Guid? ToNullableGuid(this object? value)
        {
            if (value == null) return null;

            if (value is Guid g) return g;

            if (Guid.TryParse(value.ToString(), out Guid parsed))
                return parsed;

            return null;
        }
    }
}
