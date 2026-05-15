using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Settings")]
    [SerializeField] private string acceptedTag; 
    [SerializeField] private Image fillImage;
    [SerializeField] private float totalItemsNeeded = 3f;
    [SerializeField] private GameObject questCanvas;

    public bool IsQuestComplete { get; set; } 

    private float currentCount = 0f;
    private float questStartTime; 
    private List<GameObject> collectedItems = new List<GameObject>(); 
    
    private static QuestManager[] allBins;
    
    private static int mistakeCount = 0;
    private static float lastMistakeTime = 0f;

    private void Awake()
    {
        if (allBins == null) 
        {
            allBins = FindObjectsByType<QuestManager>(FindObjectsSortMode.None);
        }
        
        if (fillImage != null) fillImage.fillAmount = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < questStartTime + 0.2f) return; 

        if (other.CompareTag(acceptedTag) && !IsQuestComplete)
        {
            HandleValidItem(other.gameObject);
        }
        else if (!other.CompareTag(acceptedTag) && !other.CompareTag("Player"))
        {
            if (Time.time > lastMistakeTime + 0.5f)
            {
                lastMistakeTime = Time.time;
                mistakeCount++;
                Debug.Log($"Wrong Bin! Mistake {mistakeCount}/3");

                if (mistakeCount >= 3)
                {
                    ApplyPenalty();
                }
            }
        }
    }

    private void ApplyPenalty()
    {
        PlayerCurrency playerBank = FindFirstObjectByType<PlayerCurrency>();
        if (playerBank != null)
        {
            playerBank.DeductGold(10);
            Debug.Log("Deducted 10 gold for 3 mistakes!");
        }
        
        mistakeCount = 0; 
    }

    private void HandleValidItem(GameObject item)
    {
        currentCount++;
        if (fillImage != null) fillImage.fillAmount = currentCount / totalItemsNeeded;

        collectedItems.Add(item);
        item.SetActive(false);

        CheckAllBinsFinished();
    }

    public void ResetQuest()
    {
        questStartTime = Time.time; 
        currentCount = 0f;
        mistakeCount = 0; 
        IsQuestComplete = false;
        
        if (fillImage != null) fillImage.fillAmount = 0f;
        if (questCanvas != null) questCanvas.SetActive(true);

        foreach (GameObject item in collectedItems)
        {
            if (item != null)
            {
                TrashItem trashComponent = item.GetComponent<TrashItem>();
                if (trashComponent != null)
                {
                    trashComponent.ResetTrash();
                }
            }
        }

        collectedItems.Clear();
    }

    private void CheckAllBinsFinished()
    {
        foreach (QuestManager bin in allBins)
        {
            if (bin.currentCount < bin.totalItemsNeeded) return; 
        }

        foreach (QuestManager bin in allBins)
        {
            if (bin.IsQuestComplete) return; 
            bin.IsQuestComplete = true; 
        }

        if (questCanvas != null) questCanvas.SetActive(false);
        Debug.Log("ALL QUESTS COMPLETE!");

        PlayerCurrency playerBank = FindFirstObjectByType<PlayerCurrency>();
        if (playerBank != null)
        {
            int goldReward = Random.Range(100, 150); 
            playerBank.AddGold(goldReward);
        }
    }
}