using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   [SerializeField] Animator animator;

   public void BeginGame()
   {
       animator.SetTrigger("Fade");
   }
}
