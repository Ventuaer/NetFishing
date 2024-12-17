using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatLogManager : MonoBehaviour
{
    public GameObject chatMessageTemplate; // Drag the ChatMessageTemplate here
    public Transform contentTransform;    // Drag the Content GameObject here
    public ScrollRect scrollRect;         // Drag the Scroll View's ScrollRect component here

    private Queue<GameObject> chatMessages = new Queue<GameObject>();
    private int maxMessages = 10;
    void Start()
{
    //ScrollToBottom();
}
    public void AddMessage(string message)
{
    // Create a new chat message instance
    GameObject newMessage = Instantiate(chatMessageTemplate, contentTransform);
    newMessage.SetActive(true);
    newMessage.GetComponent<Text>().text = message;

    // Position the new message
    RectTransform newMessageRect = newMessage.GetComponent<RectTransform>();
    if (chatMessages.Count > 0)
    {
        // Get the last message's RectTransform
        RectTransform lastMessageRect = chatMessages.Peek().GetComponent<RectTransform>();

        // Calculate the new position
        float spacing = 5f; // Space between messages
        newMessageRect.anchoredPosition = new Vector2(
            lastMessageRect.anchoredPosition.x,
            lastMessageRect.anchoredPosition.y - lastMessageRect.sizeDelta.y - spacing
        );
    }
    else
    {
        // First message, position at the top of the container
        newMessageRect.anchoredPosition = new Vector2(110, 0);
    }

    // Add the message to the queue
    chatMessages.Enqueue(newMessage);

    // Remove old messages if necessary
    if (chatMessages.Count > maxMessages)
    {
        GameObject oldMessage = chatMessages.Dequeue();
        Destroy(oldMessage);
    }

    // Update the content size to fit all messages
    UpdateContentSize();

    // Scroll to the bottom
    //ScrollToBottom();
}
private void UpdateContentSize()
{
    float totalHeight = 0f;
    foreach (var message in chatMessages)
    {
        RectTransform rect = message.GetComponent<RectTransform>();
        totalHeight += rect.sizeDelta.y + 5f; // Add spacing between messages
    }

    RectTransform contentRect = contentTransform.GetComponent<RectTransform>();
    contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, totalHeight);
}
private void ScrollToBottom()
{
    Canvas.ForceUpdateCanvases(); // Ensure UI is fully updated
    scrollRect.verticalNormalizedPosition = 0f; // 0 = bottom, 1 = top
}

}
