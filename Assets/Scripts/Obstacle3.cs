using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle3 : MonoBehaviour
{
    public Vector3 posA, posB;
    public float travelDuration;
    public float waitDuration;
    private IEnumerator Start()
    {
        // Loops each cycles
        while (Application.isPlaying)
        {
            // First step, travel from A to B
            float counter1 = 0f;

            while (counter1 < travelDuration)
            {
                transform.position = Vector3.Lerp(posA, posB, counter1 / travelDuration);
                counter1 += Time.deltaTime;
                yield return null;
            }

            // Make sure you're exactly at B, in case the counter 
            // wasn't precisely equal to travelDuration at the end
            transform.position = posB;

            // Second step, wait
            yield return new WaitForSeconds(waitDuration);

            // Third step, travel back from B to A
            float counter2 = 0f;

            while (counter2 < travelDuration)
            {
                transform.position = Vector3.Lerp(posB, posA, counter2 / travelDuration);
                counter2 += Time.deltaTime;
                yield return null;
            }

            transform.position = posA;

            // Finally, wait
            yield return new WaitForSeconds(waitDuration);
        }
    }
}
