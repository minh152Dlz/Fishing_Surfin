using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDistance = 2f;
    [SerializeField] private float jumpDuration = 4f;
    [SerializeField] private float cooldownTime = 5f;
    private bool isUsingAbility = false;
    private bool isCooldown = false;
    private Animator myAnim;
    private float skillCooldownTimer = 0f;
    public Image frogBar;
    void Start()
    {
        if (characterTransform == null)
        {
            characterTransform = GetComponent<Transform>();
            myAnim = GetComponent<Animator>();
        }
    }
    void Update()
    {
        UpdateSkillBar();
    }
    public void UseAbility()
    {
        if (!isUsingAbility && !isCooldown)
        {
            StartCoroutine(FrogJump());
        }
    }

    private IEnumerator FrogJump()
    {
        isUsingAbility = true;
        frogBar.fillAmount = 0;
        Vector3 startPosition = characterTransform.position;
        Vector3 endPosition = startPosition + new Vector3(jumpDistance, 0, 0);
        float timeElapsed = 0f;

        while (timeElapsed < jumpDuration)
        {
            myAnim.SetTrigger("Jump");
            float t = timeElapsed / jumpDuration;
            float parabolicY = -4 * jumpHeight * (t * t - t);
            characterTransform.position = Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0, parabolicY, 0);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterTransform.position = endPosition;
        isUsingAbility = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
    private void UpdateSkillBar()
    {
        if (isCooldown)
        {
            skillCooldownTimer -= Time.deltaTime;
            frogBar.fillAmount = Mathf.Clamp(skillCooldownTimer / cooldownTime, 0, 1);
        }
        else
        {
            frogBar.fillAmount = 0;
        }
    }
}
