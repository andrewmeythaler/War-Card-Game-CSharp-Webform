using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MegaChallengeWar.Domain
{
	public class Card
	{
		public string CardName { get; set; }
		public int CardValue { get; set; }
		public string CardIcon { get; set; }
		public Card(string cardName, int cardValue, string _cardIcon)
		{
			CardName = cardName;
			CardValue = cardValue;
			CardIcon = _cardIcon;
		}
	}
	public class Overseer
	{
		private Queue<Card> p1Hand { get; set; }
		private Queue<Card> p2Hand { get; set; }
		private Stack<Card> warSpoils { get; set; }
		public int WinCount1 { get; set; }
		public int WinCount2 { get; set; }
		public Card p1Active { get; set; }
		public Card p2Active { get; set; }
		private string blankIcon { get; set; }
		public Overseer(Queue<Card> _p1Hand, Queue<Card> _p2Hand, string _blank)
		{
			p1Hand = _p1Hand;
			p2Hand = _p2Hand;
			WinCount1 = 0;
			WinCount2 = 0;
			blankIcon = _blank;
			warSpoils = new Stack<Card>();
		}
		public String Battle()
		{
			string report = "";
			if (p1Hand.Count <= 0)
			{
				report += "<br> Player 1 has lost!";
				return report;
			}
			else if (p2Hand.Count <= 0)
			{
				report += "<br> Player 2 has lost!";
				return report;
			}
			p1Active = p1Hand.Dequeue();
			p2Active = p2Hand.Dequeue();
			if (p1Active.CardValue > p2Active.CardValue)
			{
				report = String.Format("<br>Player 1's {0} has defeated Player 2's {1}.		{2} vs {3}",
				p1Active.CardName, p2Active.CardName, p1Active.CardIcon, p2Active.CardIcon);
				p1Hand.Enqueue(p1Active);
				p1Hand.Enqueue(p2Active);
			}
			else if (p2Active.CardValue > p1Active.CardValue)
			{
				report = (p1Hand.Count > 0)
				? String.Format("<br>Player 2's {0} has defeated Player 1's {1}.		{2} vs {3}",
				p2Active.CardName, p1Active.CardName, p1Active.CardIcon, p2Active.CardIcon)
				: String.Format("<br>Player 2's {0} has defeated Player 1's {1}.		{2} vs {3} <br> Player 1 has lost!",
				p2Active.CardName, p1Active.CardName, p1Active.CardIcon, p2Active.CardIcon);
				p2Hand.Enqueue(p1Active);
				p2Hand.Enqueue(p2Active);
			}
			else
			{
				report = String.Format("<br>Player 1's {0} has tied with Player 2's {1}.		{2} vs {3}" +
				"<br>***This means war!***",
				p1Active.CardName, p2Active.CardName, p1Active.CardIcon, p2Active.CardIcon);
				warSpoils.Push(p1Active);
				warSpoils.Push(p2Active);
				report += War();
			}
			return report;
		}
		public string War()
		{
			string tempBattleOut = "";
			string warReport = "";
			int tCount = 0;
			if (p1Hand.Count <= 0 || p2Hand.Count <= 0)
			{
				if (p1Hand.Count > p2Hand.Count)
				{
					warReport += "<br> Player 1 has defeated player 2!";
				}
				else if (p2Hand.Count > p1Hand.Count)
				{
					warReport += "<br> Player 2 has defeated player 1!";
				}
				else
				{
					warReport += "<br> The war is a draw, everyone loses!";
				}
				WinCount1 = -1;
				WinCount2 = -1;
				return warReport;
			}
			else
			{
				int mill1 = (p1Hand.Count >= 4) ? mill1 = 4 : mill1 = p1Hand.Count;
				int mill2 = (p2Hand.Count >= 4) ? mill2 = 4 : mill2 = p2Hand.Count;
				warReport += String.Format("<br> Player 1 plays {0} cards face down", mill1 - 1);
				for (int x = 1; x < mill1; x++)
				{
					warSpoils.Push(p1Hand.Dequeue());
					warReport += String.Format("{0}", blankIcon);
				}
				warReport += String.Format("<br> Player 2 plays {0} cards face down", mill2 - 1);
				for (int x = 1; x < mill2; x++)
				{
					warSpoils.Push(p2Hand.Dequeue());
					warReport += String.Format("{0}", blankIcon);
				}
				tCount = p1Hand.Count();
				tempBattleOut = Battle();
				if (p1Hand.Count >= tCount)
				{
					warReport += tempBattleOut;
					WinCount1++;
					foreach (Card t in warSpoils)
					{
						p1Hand.Enqueue(t);
					}
					warSpoils.Clear();
				}
				else
				{
					warReport += tempBattleOut;
					WinCount2++;
					foreach (Card t in warSpoils)
					{
						p2Hand.Enqueue(t);
					}
					warSpoils.Clear();
				}
				return warReport;
			}
		}
	}
}