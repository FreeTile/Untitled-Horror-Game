using UnityEngine;
using System.Collections;
//This script plays footstep sounds.
//It will play a footstep sound after a set amount of distance travelled.
//When playing a footstep sound, this script will cast a ray downwards.
//If that ray hits an object on the Ground layer, it will check the object's tag to determine the surface type.
//If the tag is "Grass", then it plays the Grass footstep sound; if the tag is "House", then it plays the House footstep sound.
//If the ray does not hit an object on the Ground layer, we assume the player is not stepping on the ground and play the House footstep sound.
public class Footsteps : MonoBehaviour
{
    public FMODUnity.EventReference m_EventPath;

    // Surface variables
    // Range: 0.0f - 1.0f
    // These values represent the amount of each type of surface found when raycasting to the ground.
    public float m_House;
    public float m_Grass;
    // Can add more if necessary, but for now only two of them...

    // Step variables
    // These variables are used to control when the player executes a footstep.
    // This is very basic, and simply executes a footstep based on distance travelled.
    // Ideally, in this case, footsteps would be triggered based on the headbob script.
    // Or if there was an animated player model, it could be triggered from the animation system.
    // You could also add variation based on speed travelled, and whether the player is running or walking.
    public float m_StepDistance = 2.0f;
    float m_StepRand;
    Vector3 m_PrevPos;
    float m_DistanceTravelled;

    // Debug:
    // If m_Debug is true, this script will:
    // - Draw a debug line to represent the ray that was cast into the ground.
    // - Draw the triangle of the mesh that was hit by the ray that was cast into the ground.
    // - Log the surface values to the console.
    // - Log to the console when an expected game parameter is not found in the FMOD Studio event.
    public bool m_Debug;
    Vector3 m_LinePos;
    Vector3 m_TrianglePoint0; // Not used in current implementation
    Vector3 m_TrianglePoint1; // Not used in current implementation
    Vector3 m_TrianglePoint2; // Not used in current implementation

    void Start()
    {
        // Initialise random seed
        Random.InitState(System.DateTime.Now.Second);

        // Initialise member variables
        m_StepRand = Random.Range(0.0f, 0.5f);
        m_PrevPos = transform.position;
        m_LinePos = transform.position;
    }

    void Update()
    {
        m_DistanceTravelled += (transform.position - m_PrevPos).magnitude;
        if (m_DistanceTravelled >= m_StepDistance + m_StepRand) // TODO: Play footstep sound based on position from headbob script
        {
            PlayFootstepSound();
            m_StepRand = Random.Range(0.0f, 0.5f); // Adding subtle random variation to the distance required before a step is taken - re-randomise after each step.
            m_DistanceTravelled = 0.0f;
        }

        m_PrevPos = transform.position;

        if (m_Debug)
        {
            Debug.DrawLine(m_LinePos, m_LinePos + Vector3.down * 1000.0f);
            Debug.DrawLine(m_TrianglePoint0, m_TrianglePoint1);
            Debug.DrawLine(m_TrianglePoint1, m_TrianglePoint2);
            Debug.DrawLine(m_TrianglePoint2, m_TrianglePoint0);
        }
    }

    void PlayFootstepSound()
    {
        // Defaults: if the character is stepping on the ground, by default the Grass sound is played.
        m_House = 0.0f;
        m_Grass = 1.0f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000.0f))
        {
            if (m_Debug)
                m_LinePos = transform.position;

            // Check if the object under the character is on the "Ground" layer
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // Check the object's tag to determine the type of surface
                if (hit.collider.CompareTag("Grass"))
                {
                    m_Grass = 1.0f;
                    m_House = 0.0f;
                }
                else if (hit.collider.CompareTag("House"))
                {
                    m_Grass = 0.0f;
                    m_House = 1.0f;
                }
                else
                {
                    // If the tag does not match the expected ones, default to playing the Grass sound.
                    m_Grass = 1.0f;
                    m_House = 0.0f;
                }
            }
            else
            {
                // If the ray hit an object not on the "Ground" layer, assume the character is not stepping on the ground – play the House sound.
                m_Grass = 0.0f;
                m_House = 1.0f;
            }
        }
        else
        {
            // If the ray did not hit anything, by default assume the character is not stepping on the ground.
            m_Grass = 0.0f;
            m_House = 1.0f;
        }

        if (m_Debug)
            Debug.Log("House: " + m_House + " Grass: " + m_Grass);

        // Create and start the FMOD event instance with the set parameters
        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(m_EventPath);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

        e.setParameterByName("House", m_House);
        e.setParameterByName("Grass", m_Grass);

        e.start();
        e.release(); // Release each event instance immediately; they are fire-and-forget one-shot instances.
    }

    // The GetMaterialIndex method is no longer used since we do not check for materials.
    // int GetMaterialIndex(RaycastHit hit)
    // {
    //     Mesh m = hit.collider.gameObject.GetComponent<MeshFilter>().mesh;
    //     int[] triangle = new int[]
    //     {
    //         m.triangles[hit.triangleIndex * 3 + 0],
    //         m.triangles[hit.triangleIndex * 3 + 1],
    //         m.triangles[hit.triangleIndex * 3 + 2]
    //     };
    //     for(int i = 0; i < m.subMeshCount; ++i)
    //     {
    //         int[] triangles = m.GetTriangles(i);
    //         for(int j = 0; j < triangles.Length; j += 3)
    //         {
    //             if(triangles[j + 0] == triangle[0] &&
    //                 triangles[j + 1] == triangle[1] &&
    //                 triangles[j + 2] == triangle[2])
    //                 return i;
    //         }
    //     }
    //     return -1;
    // }
}
