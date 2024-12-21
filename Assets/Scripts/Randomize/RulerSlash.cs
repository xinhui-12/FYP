
using System.Collections;
using UnityEngine;
using TMPro;

public class RulerSlash : MonoBehaviour
{
    // The word animation after being slash
    public Vector3 targetPosition;
    public float moveSpeed = 1f;
    public float dissolveDuration = 1f;
    private bool isOtherInTrigger = false;
    [HideInInspector]
    public int slashTime = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (isOtherInTrigger) return;
        if (other.CompareTag("Word"))
        {
            isOtherInTrigger = true;
            other.gameObject.GetComponentInParent<WordJitter>().isTrigger = true;
            StartCoroutine(SlashWord(other.gameObject));
        }
    }

    IEnumerator SlashWord(GameObject word)
    {
        word.GetComponentInParent<TMP_Text>().color = Color.red;
        yield return StartCoroutine(MoveUpCoroutine(word.GetComponentInParent<RectTransform>()));
        
        AudioSource wordAudio = word.GetComponent<AudioSource>();
        yield return StartCoroutine(PlayAudio(wordAudio));
        
        yield return StartCoroutine(Dissolve(word.GetComponentInParent<TMP_Text>(), Color.red));

        isOtherInTrigger = false;
        slashTime++;
    }

    IEnumerator MoveUpCoroutine(RectTransform rect)
    {
        rect.localRotation = Quaternion.Euler(0, 0, 0);
        while (Vector3.Distance(rect.anchoredPosition, targetPosition) > 0.1f)
        {
            rect.anchoredPosition = Vector3.MoveTowards(rect.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        rect.anchoredPosition = targetPosition;
        
    }

    IEnumerator PlayAudio(AudioSource wordAudio)
    {
        if (wordAudio != null && wordAudio.clip != null)
        {
            wordAudio.Play();
            yield return new WaitForSeconds(wordAudio.clip.length);
        }
    }

    IEnumerator Dissolve(TMP_Text word, Color color)
    {
        float elapsed = 0f;
        while (elapsed < dissolveDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / dissolveDuration);
            word.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }
        word.color = new Color(color.r, color.g, color.b, 0);
        word.gameObject.SetActive(false);
    }
}
