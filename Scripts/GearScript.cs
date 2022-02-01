using UnityEngine;
/// <summary>
/// Script for animate settings gear
/// </summary>
public class GearScript : MonoBehaviour
{
    /// <summary>
    /// Switch gear
    /// </summary>
    public void TurnGear()
    {
        if (gameObject.GetComponent<Animator>().GetBool("BTNisPressed"))
        {
            gameObject.GetComponent<Animator>().SetBool("BTNisPressed", false);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("BTNisPressed", true);
        }
    }
}
