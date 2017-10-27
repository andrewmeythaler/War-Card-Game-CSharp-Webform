using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MegaChallengeWar.Domain;

namespace MegaChallengeWar
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}
		public string UniCodeConverter(string cardName, Dictionary<string, string> _unicodes)
		{
			string codepoint;
			int code = 0;
			if (_unicodes.TryGetValue(cardName, out codepoint))
				code = int.Parse(codepoint, System.Globalization.NumberStyles.HexNumber);
			else
			{
				codepoint = "1F0A0";
				code = int.Parse(codepoint, System.Globalization.NumberStyles.HexNumber);
			}
			return char.ConvertFromUtf32(code);
		}
		public Stack<Card> Shuffle(List<Card> _unshuffled)
		{
			Random _shuffler = new Random();
			Stack<Card> temp = new Stack<Card>();
			int card = _shuffler.Next(0, _unshuffled.Count - 1);
			do
			{
				card = _shuffler.Next(0, _unshuffled.Count - 1);
				temp.Push(_unshuffled.ElementAt(card));
				_unshuffled.RemoveAt(card);
			} while (_unshuffled.Count > 0);
			return temp;
		}

		protected void play_Click(object sender, EventArgs e)
		{
			Stack<Card> ShuffledDeck = new Stack<Card>();
			Queue<Card> P1Hand = new Queue<Card>();
			Queue<Card> P2Hand = new Queue<Card>();
			List<Card> unshuffledDeck = new List<Card>();
			Card temp;
			string tString;
			Dictionary<string, string> _unicodes;
			_unicodes = new Dictionary<string, string>() { { "Blank", "1F0A1" } };
			for (int y = 0; y < 4; y++)
			{
				for (int x = 1; x < 14; x++)
				{
					string _sName;
					string _vName;
					string _skName;
					string _vkName;
					if (y == 0) { _sName = "of Spades"; _skName = "1F0A"; }
					else if (y == 1) { _sName = "of Hearts"; _skName = "1F0B"; }
					else if (y == 2) { _sName = "of Diamonds"; _skName = "1F0C"; }
					else { _sName = "of Clubs"; _skName = "1F0D"; }
					if (x == 1) { _vName = "Ace"; _vkName = x.ToString(); }
					else if (x == 2) { _vName = "Two"; _vkName = x.ToString(); }
					else if (x == 3) { _vName = "Three"; _vkName = x.ToString(); }
					else if (x == 4) { _vName = "Four"; _vkName = x.ToString(); }
					else if (x == 5) { _vName = "Five"; _vkName = x.ToString(); }
					else if (x == 6) { _vName = "Six"; _vkName = x.ToString(); }
					else if (x == 7) { _vName = "Seven"; _vkName = x.ToString(); }
					else if (x == 8) { _vName = "Eight"; _vkName = x.ToString(); }
					else if (x == 9) { _vName = "Nine"; _vkName = x.ToString(); }
					else if (x == 10) { _vName = "Ten"; _vkName = "A"; }
					else if (x == 11) { _vName = "Jack"; _vkName = "B"; }
					else if (x == 12) { _vName = "Queen"; _vkName = "C"; }
					else { _vName = "King"; _vkName = "E"; }
					_unicodes.Add(String.Format("{0} {1}", _vName, _sName), String.Format("{0}{1}", _skName, _vkName));
					tString = String.Format("{0} {1}", _vName, _sName);
					temp = new Card(tString, x, UniCodeConverter(String.Format("{0}{1}", _skName, _vkName), _unicodes));
					unshuffledDeck.Add(temp);
				}
			}
			foreach (Card t in unshuffledDeck)
			{
				t.CardIcon = UniCodeConverter(t.CardName, _unicodes);
			}
			ShuffledDeck = Shuffle(unshuffledDeck);
			int player = 1;
			do
			{
				if (player == 1) { P1Hand.Enqueue(ShuffledDeck.Pop()); player++; }
				else if (player == 2) { P2Hand.Enqueue(ShuffledDeck.Pop()); player--; }
			} while (ShuffledDeck.Count > 0);
			Overseer controller = new Overseer(P1Hand, P2Hand, UniCodeConverter("lank", _unicodes));
			do
			{
				Label1.Text += controller.Battle();
			} while (controller.WinCount1 < 7 && controller.WinCount2 < 7 && controller.WinCount1 > -1 && controller.WinCount2 > -1);
			if (controller.WinCount1 > controller.WinCount2)
			{
				Label1.Text += String.Format("<br>Player 1 is victorious! Having won {0} wars.", controller.WinCount1);
			}
			else if (controller.WinCount1 < controller.WinCount2)
			{
				Label1.Text += String.Format("<br>Player 2 is victorious! Having won {0} wars.", controller.WinCount2);
			}
		}
	}
}