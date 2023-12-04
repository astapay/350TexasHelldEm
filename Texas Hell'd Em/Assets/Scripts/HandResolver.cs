/******************************************************************************
// File Name:       HandResolver.cs
// Author:          Andrew Stapay
// Creation Date:   October 25, 2023
//
// Description:     A class that determines the winner between two poker hands.
                    Includes all of the necessary logic to do so.
******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class HandResolver
{
    // <summary>
    // Determines the winner of multiple inputted hands
    // </summary>
    // <param name="hands"> The array of hands to evaluate </param>
    // <returns> A tuple of types int and int.
    // The first value represents the player that won the poker game
    // The second value represents the type of hand that player won with
    // </returns>
    public Tuple<int,int> ResolveHands(CardData[][] hands)
    {
        // We will create a copy of the hands to evaluate
        // So that we don't mess up what appears on screen
        CardData[][] handsCopy = new CardData[hands.Length][];

        for(int i = 0;i < hands.GetLength(0);i++)
        {
            handsCopy[i] = new CardData[hands[i].Length];
        }

        for(int i = 0;i < hands.GetLength(0);i++)
        {
            for(int j = 0;j < hands[i].Length;j++)
            {
                handsCopy[i][j] = hands[i][j];
            }
        }

        // Sort each hand
        handsCopy = SortHands(handsCopy);

        // Find the values of each hand
        int[] handResults = new int[handsCopy.Length];

        for(int i = 0;i < handsCopy.Length;i++)
        {
            handResults[i] = FindHand(handsCopy[i]);
        }

        // Find the strongest hand in the game
        int bestHandIndex = 0;
        
        for(int i = 1;i < handsCopy.Length;i++)
        {
            if (handResults[i] > handResults[bestHandIndex])
            {
                bestHandIndex = i;
            }
        }

        // We also need to check for ties
        CardData[][] tiedHands = new CardData[0][];

        for(int i = 0;i < handsCopy.Length;i++)
        {
            if (handResults[i] == handResults[bestHandIndex])
            {
                CardData[][] temp = new CardData[tiedHands.Length + 1][];

                for(int j = 0;j < tiedHands.Length;j++)
                {
                    temp[j] = tiedHands[j];
                }

                temp[tiedHands.Length] = handsCopy[i];

                tiedHands = temp;
            }
        }

        // If we only have 1 hand with the best value, then that is our winner
        // Otherwise, we need to go to the tiebreaker
        if(tiedHands.Length == 1)
        {
            return new Tuple<int, int>(bestHandIndex, 
                handResults[bestHandIndex]);
        }
        else
        {
            // Find which hand won the tiebreaker
            int handInTied = Tiebreaker(tiedHands, handResults[bestHandIndex]);

            // Find which hand won in the original array
            for(int i = 0;i < hands.Length;i++)
            {
                bool found = true;

                // If any card in this hand is different, we haven't found
                // the correct hand
                for(int j = 0; j < handsCopy[i].Length; j++)
                {
                    if (handsCopy[i][j].getRank() != tiedHands[handInTied][j].getRank())
                    {
                        found = false;
                        break;
                    }

                    if (handsCopy[i][j].getSuit() !=
                        tiedHands[handInTied][j].getSuit())
                    {
                        found = false;
                        break;
                    }
                }

                // If we found it, we may return the winner
                if(found)
                {
                    return new Tuple<int, int>(i, handResults[bestHandIndex]);
                }
            }
        }

        // Technically speaking, we should never reach this line of code
        // Still, if all else fails, we'll just send a default value
        return new Tuple<int, int>(0, 0);
    }

    // <summary>
    // Finds the type of poker hand that the inputted hand is
    // </summary>
    // <param name="hand"> The hand to evaluate </param>
    // <returns> An int representing the general strength of hand </returns>
    private int FindHand(CardData[] hand)
    {
        // We need to find a few values of our particular hand
        // See descriptions of the methods below
        bool flush = FindFlush(hand);
        bool straight = FindStraight(hand);
        int ofAKind = FindOfAKind(hand);
        int pairs = FindPairs(hand);

        // Now, we determine what type of hand we have
        if (straight && flush && ConfirmStraightFlush(hand))
        {
            return 8; //Straight Flush
        }
        else if (ofAKind == 4)
        {
            return 7; //4 of a Kind
        }
        else if (ofAKind == 3 && pairs >= 1)
        {
            return 6; //Full House
        }
        else if (flush)
        {
            return 5; //Flush
        }
        else if (straight)
        {
            return 4; //Straight
        }
        else if (ofAKind == 3)
        {
            return 3; //3 of a Kind
        }
        else if (pairs == 2)
        {
            return 2; //Two Pair
        }
        else if (pairs == 1)
        {
            return 1; //One Pair
        }

        return 0; //High Card
    }

    // <summary>
    // Sorts an inputted array of Cards from greatest rank to least rank
    // Uses a Bubble Sort algorithm
    // </summary>
    // <param name="hand"> The array to be sorted </param>
    // <returns> The original array, now sorted </returns>
    private CardData[][] SortHands(CardData[][] hands)
    {
        // Loop to sort each row of our hands array
        for(int i = 0;i < hands.Length;i++)
        {
            // Control variable to keep track of how long we need to sort
            bool changesMade = true;

            // Loop until we no longer make new changes
            while (changesMade)
            {
                // Set control variable to false
                changesMade = false;

                // Loop through array
                for (int j = 1; j < hands[i].Length; j++)
                {
                    // If we find a rank out of place, swap places
                    // We also made a change, so set the control variable to true
                    if (hands[i][j - 1].getRank() < hands[i][j].getRank())
                    {
                        CardData temp = hands[i][j];
                        hands[i][j] = hands[i][j - 1];
                        hands[i][j - 1] = temp;

                        changesMade = true;
                    }
                }
            }
        }
        

        // Return the array, which is now sorted
        return hands;
    }

    // <summary>
    // Confirms that a given hand contains a Straight Flush
    // </summary>
    // <param name="hand"> The hand to evaluate <\param>
    // <returns> true if the hand does contain a Straight Flush,
    // false otherwise </returns>
    private bool ConfirmStraightFlush(CardData[] hand)
    {
        // If our hand length is 5, then we absolutely have a Straight Flush
        if(hand.Length == 5)
        {
            return true;
        }

        // We will find the indices of the first cards that appear
        // in the straight and the flush
        int straightIndex = FindStraightIndex(hand);
        int flushIndex = FindFlushIndex(hand);

        // If the indices are the same, then we have a Straight Flush
        return straightIndex == flushIndex;
    }

    // <summary>
    // Determines whether an inputted hand contains a Flush
    // </summary>
    // <param name="hand"> The hand to evaluate </param>
    // <returns> true if the hand contains a Flush,
    // false otherwise </returns>
    private bool FindFlush(CardData[] hand)
    {
        // We will use an array of ints to keep track of
        // the number of each suit
        int[] ct = new int[4];

        // For each card in the hand, we will count the number of
        // each suit that appears
        for (int i = 0; i < hand.Length; i++)
        {
            ct[hand[i].getSuit()]++;
        }

        // Now, we loop through ct and see if any suit appears
        // 5 or more times.
        for(int i = 0;i < 4;i++)
        {
            if (ct[i] >= 5)
            {
                return true;
            }
        }

        // Otherwise, return false
        return false;
    }

    // <summary>
    // Determines the first index of a card contributing to a Flush
    // in a given hand
    // </summary>
    // <param name="hand"> The hand to be evaluated </param>
    // <returns> The index of the first card contributing to a Flush </returns>
    private int FindFlushIndex(CardData[] hand)
    {
        // We need to count the suits again
        int[] ct = new int[4];

        // This time, we will store the indices of the first time a suit
        // appears in the hand
        int[] firstSuit = new int[4];

        // Initializing firstSuit
        for (int i = 0; i < firstSuit.Length;i++)
        {
            firstSuit[i] = -1;
        }

        // Loop through the hand
        // This time, we will also store the index if one hasn't been
        // stored already
        for (int i = 0; i < hand.Length; i++)
        {
            ct[hand[i].getSuit()]++;

            if (firstSuit[hand[i].getSuit()] == -1)
            {
                firstSuit[hand[i].getSuit()] = i;
            }
        }

        // Loop through ct
        // If we find a suit with 5 or more instances, return the first
        // index where that suit appears
        for (int i = 0; i < 4; i++)
        {
            if (ct[i] >= 5)
            {
                return firstSuit[i];
            }
        }
        return -1;
    }

    // <summary>
    // Determines whether an inputted hand contains a Straight
    // </summary>
    // <param name="hand"> The hand to evaluate </param>
    // <returns> true if the hand contains a Straight,
    // false otherwise </returns>
    private bool FindStraight(CardData[] hand)
    {
        // Counter to keep track of how many ranks we have in a row
        int ct = 1;

        // Loop through hand
        for(int i = 1;i < hand.Length; i++)
        {
            // If the previous Card's rank - 1 is the same as this Card's rank,
            // then we have a potential straight going on
            if (hand[i-1].getRank() - 1 == hand[i].getRank())
            {
                ct++;

                // If we make it to 5, then we have a straight
                if(ct == 5)
                {
                    break;
                }
            }
            // Otherwise, reset the counter
            else
            {
                ct = 1;
            }
        }

        return ct >= 5;
    }

    // <summary>
    // Determines the first index of a card contributing to a Straight
    // in a given hand
    // </summary>
    // <param name="hand"> The hand to be evaluated </param>
    // <returns> The index of the first card contributing to a Straight
    // </returns>
    private int FindStraightIndex(CardData[] hand)
    {
        // We're going to keep count of rank again,
        // and we are also going to save our index
        int ct = 1;
        int i;

        // Loop through hand, doing the same as in FindStraight
        for (i = 1; i < hand.Length; i++)
        {
            if (hand[i - 1].getRank() - 1 == hand[i].getRank())
            {
                ct++;

                if (ct == 5)
                {
                    break;
                }
            }
            else
            {
                ct = 1;
            }
        }

        // After we've found the straight, we return the index we ended at - 4
        // to get the first index of the straight
        return i - 4;
    }

    // <summary>
    // Determines the number of cards of a kind in a given hand
    // </summary>
    // <param name="hand"> The hand to evaluate </param>
    // <returns> The number of cards of a kind in hand </returns>
    private int FindOfAKind(CardData[] hand)
    {
        // Variables to keep track of how many of a kind we have
        int ct = 1;
        int max = 0;

        // Looping through
        for (int i = 1; i < hand.Length; i++)
        {
            // If the previous Card's rank is the same as this Card's rank,
            // increase the counter
            if (hand[i - 1].getRank() == hand[i].getRank())
            {
                ct++;
            }
            // Otherwise, we will check to see if we will save our counter
            else
            {
                // We want ct to be at least 3
                // Since we don't want to count pairs
                if(ct >= 3)
                {
                    max = ct;
                }

                // Reset the counter too
                ct = 1;
            }
        }

        // Return the maximum number of a kind we found
        return max;
    }

    // <summary>
    // Determines the first index of the best number of a kind in a hand
    // </summary>
    // <param name="hand"> The hand to evaluate </param>
    // <returns> The index of the first card of the best number of a kind
    // in hand </returns>
    private int FindOfAKindIndex(CardData[] hand)
    {
        // We will keep track of the ranks again,
        // and we will save the index too
        int ct = 1;
        int i;

        // Loop through hand, doing the same as in FindOfAKind
        for (i = 1; i < hand.Length; i++)
        {
            if (hand[i - 1].getRank() == hand[i].getRank())
            {
                ct++;
            }
            else
            {
                if (ct >= 3)
                {
                    break;
                }

                ct = 1;
            }
        }

        // Return the correct index
        // We know this based on the index we left on and the number of a kind
        return i - ct + 1;
    }

    // <summary>
    // Determines the number of pairs in a given hand
    // </summary>
    // <param name="hand"> The hand to be evaluated </param>
    // <returns> The number of pairs in hand </returns>
    private int FindPairs(CardData[] hand)
    {
        // Keep a counter of the pairs
        int ct = 0;

        // Loop through hand
        for (int i = 1; i < hand.Length; i++)
        {
            // If the previous Card's rank is the same as this Card's rank,
            // increment the counter
            if (hand[i - 1].getRank() == hand[i].getRank())
            {
                ct++;

                // We are also going to increment i again
                // This is because we don't want to account for 3 of a kinds
                // for the sake of finding Full Houses correctly
                // We don't need to worry as much about 4 of a kinds
                i++;
            }
        }

        // Return the number of pairs
        return ct;
    }

    // <summary>
    // Determines the starting indices of all pairs in a hand
    // </summary>
    // <param name="hand"> The hand to evaluate </param>
    // <returns> An array of ints representing the indices of the pairs in hand
    // </returns>
    private int[] FindPairIndex(CardData[] hand)
    {
        // Start with an empty array
        int[] back = new int[0];

        // Loop through hand the same way we did in FindPairs
        for (int i = 1; i < hand.Length; i++)
        {
            if (hand[i - 1].getRank() == hand[i].getRank())
            {
                // This time, we will save the index in our array
                // Create a temp array with one more slot
                int[] temp = new int[back.Length + 1];

                // Copy back into temp
                for(int j = 0;j < back.Length;j++)
                {
                    temp[j] = back[j];
                }

                // Add this index - 1 to get the starting index for this pair
                temp[back.Length] = i - 1;

                // Set back to temp;
                back = temp;

                // Increment i again
                i++;
            }
        }

        // Return the array
        return back;
    }

    // <summary>
    // Decides on which hand wins when they have the same hand type
    // </summary>
    // <param name="hands"> The array of hands to evaluate </param>
    // <param name="handType"> The type of hand that the two hands share </param>
    // <returns> the index of the winning hand in the inputted array </returns>
    private int Tiebreaker(CardData[][] hands, int handType)
    {
        // We will sort the hands again just in case
        hands = SortHands(hands);

        // We will break the tie based on handType
        switch(handType)
        {
            case 0: //High Card
                {
                    // We will evaluate each hand one-on-one and keep track of
                    // who is winning
                    int winningHand = 0;

                    for(int i = 1;i < hands.Length;i++)
                    {
                        bool temp = TiebreakerHigh(hands[winningHand], hands[i]);

                        // If the new hand beat winningHand, set winningHand to that
                        if(!temp)
                        {
                            winningHand = i;
                        }
                    }

                    // Return the winning hand
                    return winningHand;
                }
                break;
            case 1: //One Pair
            case 2: //Two Pair
                {
                    // The strategy for the rest of these cases will generally
                    // be the same as what we did for high card
                    // The only changes that we're making is the sub-tiebreaker
                    // method we will call
                    // As such, we are essentially doing the same thing for
                    // the rest of this method
                    int winningHand = 0;

                    for (int i = 1; i < hands.Length; i++)
                    {
                        bool temp = TiebreakerPair(hands[winningHand], hands[i]);

                        if (!temp)
                        {
                            winningHand = i;
                        }
                    }

                    return winningHand;
                }
                break;
            case 3: //3 of a Kind
                {
                    int winningHand = 0;

                    for (int i = 1; i < hands.Length; i++)
                    {
                        bool temp = TiebreakerThree(hands[winningHand], hands[i]);

                        if (!temp)
                        {
                            winningHand = i;
                        }
                    }

                    return winningHand;
                }
                break;
            case 4: //Straight
            case 8: //Straight Flush
                {
                    // According to the rules of poker, these are compared
                    // the same way
                    int winningHand = 0;

                    for (int i = 1; i < hands.Length; i++)
                    {
                        bool temp = TiebreakerStraight(hands[winningHand], hands[i]);

                        if (!temp)
                        {
                            winningHand = i;
                        }
                    }

                    return winningHand;
                }
                break;
            case 5: //Flush
                {
                    int winningHand = 0;

                    for (int i = 1; i < hands.Length; i++)
                    {
                        bool temp = TiebreakerFlush(hands[winningHand], hands[i]);

                        if (!temp)
                        {
                            winningHand = i;
                        }
                    }

                    return winningHand;
                }
                break;
            case 6: //Full House
                {
                    int winningHand = 0;

                    for (int i = 1; i < hands.Length; i++)
                    {
                        bool temp = TiebreakerFull(hands[winningHand], hands[i]);

                        if (!temp)
                        {
                            winningHand = i;
                        }
                    }

                    return winningHand;
                }
                break;
            case 7: //4 of a Kind
                {
                    int winningHand = 0;

                    for (int i = 1; i < hands.Length; i++)
                    {
                        bool temp = TiebreakerFour(hands[winningHand], hands[i]);

                        if (!temp)
                        {
                            winningHand = i;
                        }
                    }

                    return winningHand;
                }
                break;
            default:
                break;
        }

        // If something terrible happened, return 0
        return 0;
    }

    /// <summary>
    /// Handles the logic of a tiebreaker under High Card rules between
    /// two hands
    /// </summary>
    /// <param name="hand1"> The first hand to evaluate </param>
    /// <param name="hand2"> The seconde hand to evaluate </param>
    /// <returns> true if hand1 defeats hand2, false otherwise </returns>
    private bool TiebreakerHigh(CardData[] hand1, CardData[] hand2)
    {
        // We simply loop through each hand
        // Whichever finds the higher card first wins
        for (int i = 0; i < hand1.Length; i++)
        {
            if (hand1[i].getRank() > hand2[i].getRank())
            {
                return true;
            }
            else if (hand1[i].getRank() < hand2[i].getRank())
            {
                return false;
            }
        }

        // If that failed, we tied
        return true;
    }

    private bool TiebreakerPair(CardData[] hand1, CardData[] hand2)
    {
        // We can do these with the same code due to the nature
        // of FindPairIndex
        int[] pairIndex1 = FindPairIndex(hand1);
        int[] pairIndex2 = FindPairIndex(hand2);

        // The higher pair wins the tie
        if (hand1[pairIndex1[0]].getRank() > hand2[pairIndex2[0]].getRank())
        {
            return true;
        }
        else if (hand1[pairIndex1[0]].getRank() < hand2[pairIndex2[0]].getRank())
        {
            return false;
        }
        // If that didn't break it, we will check to see
        // if we had two pair
        else if (pairIndex1.Length >= 2)
        {
            // The next pairs will attempt to break the tie now
            if (hand1[pairIndex1[1]].getRank() > hand2[pairIndex2[1]].getRank())
            {
                return true;
            }
            else if (hand1[pairIndex1[1]].getRank() < hand2[pairIndex2[1]].getRank())
            {
                return false;
            }
            else
            {
                // All of that failed, so now we need to find the
                // "kicker", the next highest card that is not
                // included in the pairs
                // Looping variables
                int i = 0;
                int j = 0;
                int nextIndex1 = 0;
                int nextIndex2 = 0;

                // Loop through hands 1 and 2
                do
                {
                    // When we are at an index that corresponds
                    // to a pair, we will skip it
                    while (nextIndex1 < pairIndex1.Length && i == pairIndex1[nextIndex1])
                    {
                        i += 2;
                        nextIndex1++;
                    }

                    // Do the same for hand2
                    while (nextIndex2 < pairIndex2.Length && j == pairIndex2[nextIndex2])
                    {
                        j += 2;
                        nextIndex2++;
                    }

                    // Check the ranks
                    if (i < hand1.Length && j < hand2.Length)
                    {
                        if (hand1[i].getRank() > hand2[j].getRank())
                        {
                            return true;
                        }
                        else if (hand1[i].getRank() < hand2[j].getRank())
                        {
                            return false;
                        }
                    }

                    i++;
                    j++;
                } while (i < hand1.Length && j < hand1.Length);

                // If that failed, we tied
                return true;
            }
        }
        else
        {
            // All of that failed, so now we need to find the
            // "kicker", the next highest card that is not
            // included in the pairs
            // Looping variables
            int i = 0;
            int j = 0;
            int nextIndex1 = 0;
            int nextIndex2 = 0;

            // Loop through hands 1 and 2
            do
            {
                // When we are at an index that corresponds
                // to a pair, we will skip it
                while (nextIndex1 < pairIndex1.Length && i == pairIndex1[nextIndex1])
                {
                    i += 2;
                    nextIndex1++;
                }

                // Do the same for hand2
                while (nextIndex2 < pairIndex2.Length && j == pairIndex2[nextIndex2])
                {
                    j += 2;
                    nextIndex2++;
                }

                // Check the ranks
                if (i < hand1.Length && j < hand2.Length)
                {
                    if (hand1[i].getRank() > hand2[j].getRank())
                    {
                        return true;
                    }
                    else if (hand1[i].getRank() < hand2[j].getRank())
                    {
                        return false;
                    }
                }

                i++;
                j++;
            } while (i < hand1.Length && j < hand2.Length);

            // If that failed, we tied
            return true;
        }
    }

    private bool TiebreakerThree(CardData[] hand1, CardData[] hand2)
    {
        // We'll find the indices of the 3 of a Kinds
        int kindIndex1 = FindOfAKindIndex(hand1);
        int kindIndex2 = FindOfAKindIndex(hand2);

        // Now, we check the ranks of the 3 of a Kinds
        // Higher rank wins
        if (hand1[kindIndex1].getRank() > hand2[kindIndex2].getRank())
        {
            return true;
        }
        else if (hand1[kindIndex1].getRank() < hand2[kindIndex2].getRank())
        {
            return false;
        }
        else
        {
            // That failed, so now we find the kicker again
            // We can do this in a simplified way of the way we
            // did it for pairs
            int i = 0;
            int j = 0;

            do
            {
                if (i == kindIndex1)
                {
                    i += 3;
                }

                if (j == kindIndex2)
                {
                    j += 3;
                }

                if (i < hand1.Length && j < hand2.Length)
                {
                    if (hand1[i].getRank() > hand2[j].getRank())
                    {
                        return true;
                    }
                    else if (hand1[i].getRank() < hand2[j].getRank())
                    {
                        return false;
                    }
                }

                i++;
                j++;
            } while (i < hand1.Length && j < hand2.Length);

            // If that failed, we tied
            return true;
        }
    }

    private bool TiebreakerStraight(CardData[] hand1, CardData[] hand2)
    {
        // Find the indices of the Straights
        int straightIndex1 = FindStraightIndex(hand1);
        int straightIndex2 = FindStraightIndex(hand2);

        // Compare the ranks of the highest Cards of the Straights
        if (hand1[straightIndex1].getRank() > hand2[straightIndex2].getRank())
        {
            return true;
        }
        else if (hand1[straightIndex1].getRank() < hand2[straightIndex2].getRank())
        {
            return false;
        }

        // If that failed, we tied
        return true;
    }

    private bool TiebreakerFlush(CardData[] hand1, CardData[] hand2)
    {
        // Find the indices of the Flushes
        int flushIndex1 = FindFlushIndex(hand1);
        int flushIndex2 = FindFlushIndex(hand2);

        // We also need to know what suit the Flushes are
        int suit1 = hand1[flushIndex1].getSuit();
        int suit2 = hand2[flushIndex2].getSuit();

        // Loop through the hands
        do
        {
            // We need to make sure we are selecting the right
            // Card to test
            // So, we skip cards until we find the correct suit
            while (flushIndex1 < hand1.Length &&
                hand1[flushIndex1].getSuit() != suit1)
            {
                flushIndex1++;
            }

            // Do the same for hand2
            while (flushIndex2 < hand2.Length &&
                hand2[flushIndex2].getSuit() != suit2)
            {
                flushIndex2++;
            }

            // Compare ranks
            if (flushIndex1 < hand1.Length && flushIndex2 < hand2.Length)
            {
                if (hand1[flushIndex1].getRank() > hand2[flushIndex2].getRank())
                {
                    return true;
                }
                else if (hand1[flushIndex1].getRank() < hand2[flushIndex2].getRank())
                {
                    return false;
                }
            }

            flushIndex1++;
            flushIndex2++;
        } while (flushIndex1 < hand1.Length && flushIndex2 < hand2.Length);

        // If that failed, we tied
        return true;
    }

    private bool TiebreakerFull(CardData[] hand1, CardData[] hand2)
    {
        // First, we compare the 3 of a Kind that makes up the
        // Full House
        // Get the indices
        int kindIndex1 = FindOfAKindIndex(hand1);
        int kindIndex2 = FindOfAKindIndex(hand2);

        // Compare the ranks
        if (hand1[kindIndex1].getRank() > hand2[kindIndex2].getRank())
        {
            return true;
        }
        else if (hand1[kindIndex1].getRank() < hand2[kindIndex2].getRank())
        {
            return false;
        }
        else
        {
            // If that failed, we now compare the Pair
            int[] pairIndex1 = FindPairIndex(hand1);
            int[] pairIndex2 = FindPairIndex(hand2);

            if (hand1[pairIndex1[0]].getRank() > hand2[pairIndex2[0]].getRank())
            {
                return true;
            }
            else if (hand1[pairIndex1[0]].getRank() < hand2[pairIndex2[0]].getRank())
            {
                return false;
            }
        }

        // If that failed, we tied
        return true;
    }

    private bool TiebreakerFour(CardData[] hand1, CardData[] hand2)
    {
        // Find the indices
        int kindIndex1 = FindOfAKindIndex(hand1);
        int kindIndex2 = FindOfAKindIndex(hand2);

        // Compare the ranks
        if (hand1[kindIndex1].getRank() > hand2[kindIndex2].getRank())
        {
            return true;
        }
        else if (hand1[kindIndex1].getRank() < hand2[kindIndex2].getRank())
        {
            return false;
        }

        // We really really should not reach this line of code
        // since there cannot be two of the same 4 of a Kinds with
        // only one deck
        // Still, just in case, otherwise we tied
        return true;
    }
}
