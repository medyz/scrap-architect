using UnityEngine;

namespace ScrapArchitect.Garage
{
    public class GarageZone : MonoBehaviour
    {
        [Header("Zone Settings")]
        public string zoneName = "Zone";
        public string interactionText = "Press E to interact";
        public ZoneType zoneType;
        public float interactionRange = 3f;
        
        [Header("Visual Feedback")]
        public Material normalMaterial;
        public Material highlightMaterial;
        public Renderer zoneRenderer;
        
        [Header("Audio")]
        public AudioClip interactionSound;
        public AudioSource audioSource;
        
        private bool isHighlighted = false;
        private Material originalMaterial;
        
        public void Initialize(string name, string text, ZoneType type)
        {
            zoneName = name;
            interactionText = text;
            zoneType = type;
            
            // Setup renderer
            if (zoneRenderer == null)
            {
                zoneRenderer = GetComponent<Renderer>();
            }
            
            if (zoneRenderer != null)
            {
                originalMaterial = zoneRenderer.sharedMaterial;
            }
            
            // Setup audio
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
        
        public bool IsInRange()
        {
            // This will be called by GarageManager to check if player is in range
            return true; // GarageManager handles the actual distance check
        }
        
        public string GetInteractionText()
        {
            return interactionText;
        }
        
        public void Highlight()
        {
            if (!isHighlighted && zoneRenderer != null && highlightMaterial != null)
            {
                zoneRenderer.sharedMaterial = highlightMaterial;
                isHighlighted = true;
            }
        }
        
        public void Unhighlight()
        {
            if (isHighlighted && zoneRenderer != null && originalMaterial != null)
            {
                zoneRenderer.sharedMaterial = originalMaterial;
                isHighlighted = false;
            }
        }
        
        public void PlayInteractionSound()
        {
            if (audioSource != null && interactionSound != null)
            {
                audioSource.PlayOneShot(interactionSound);
            }
        }
        
        void OnDrawGizmosSelected()
        {
            // Draw interaction range in editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
