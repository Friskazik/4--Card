namespace UnityEngine
{
    public static class SpriteExtensions
    {
        public static Color SetAlpha(this SpriteRenderer spriteRenderer, float value) 
        {
            var color = spriteRenderer.color;
            color.a = value; 
            spriteRenderer.color = color;

            return color;
        }
    }
}