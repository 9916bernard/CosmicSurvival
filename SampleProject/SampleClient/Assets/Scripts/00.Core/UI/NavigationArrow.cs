using UnityEngine;
using UnityEngine.UI;

public class NavigationArrow : MonoBehaviour
{
    public Base baseStation; // The target (AllyCarrier) to point to
    public RectTransform arrowRectTransform; // The RectTransform of the arrow image
    public Camera mainCamera; // The main camera for screen clamping
    public Canvas canvas; // The canvas
    public float screenPadding = 100f; // Padding from the left and right screen edges
    public float verticalPadding = 200f; // Padding from the top and bottom screen edges
    public float visibilityRadius = 5f; // Radius around the station where it becomes invisible
    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer
   


    public void Init(Base baseStation)
    {
        this.baseStation = baseStation;
        mainCamera = baseStation.MainCamera;
    }
    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(baseStation.transform.position);

        if (IsTargetVisible()) // Ensure the target is visible based on player's position
        {
            SetSpriteAlpha(0f); // Hide the arrow if the target is visible
        }
        else
        {
            SetSpriteAlpha(1f); // Show the arrow if the target is not visible

            // Convert screen position to canvas position
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out canvasPosition);

            // Clamp the position to the edge of the screen with padding
            Vector2 clampedPosition = ClampToScreenEdges(canvasPosition);

            // Update arrow position
            arrowRectTransform.anchoredPosition = clampedPosition;

            // Rotate the arrow to point towards the target
            Vector2 direction = (Vector2)baseStation.transform.position - (Vector2)baseStation.field._Player.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrowRectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private bool IsTargetVisible()
    {
        // Check if the target is within the visibility radius of the player
        float distance = Vector3.Distance(baseStation.transform.position, baseStation.field._Player.transform.position);
        return distance <= visibilityRadius;
    }

    Vector2 ClampToScreenEdges(Vector2 position)
    {
        float screenWidth = canvas.GetComponent<RectTransform>().rect.width;
        float screenHeight = canvas.GetComponent<RectTransform>().rect.height;

        float x = Mathf.Clamp(position.x, -screenWidth / 2 + arrowRectTransform.rect.width / 2 + screenPadding, screenWidth / 2 - arrowRectTransform.rect.width / 2 - screenPadding);
        float y = Mathf.Clamp(position.y, -screenHeight / 2 + arrowRectTransform.rect.height / 2 + verticalPadding, screenHeight / 2 - arrowRectTransform.rect.height / 2 - verticalPadding);

        return new Vector2(x, y);
    }

    void SetSpriteAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
