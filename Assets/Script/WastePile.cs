using System.Threading.Tasks;
using UnityEngine;

public class WastePile : MonoBehaviour
{
    public Transform WastePilePosition;
    private CardScript topCard;


    public async Task AddCardToWastePile(CardScript card)
    {
        card.transform.SetParent(WastePilePosition);
        await card.MoveToDestination(WastePilePosition);
        if (topCard != null)
        {
            Destroy(topCard.gameObject);
        }
        topCard = card;
        topCard.onCardClicked.RemoveAllListeners();
    }


    public CardScript GetTopCard()
    {
        return topCard;
    }

      public async Task ReceiveCardFromDeck(CardScript card)
    {
        card.FlipWithAnimation();
        await AddCardToWastePile(card);
    }

}
