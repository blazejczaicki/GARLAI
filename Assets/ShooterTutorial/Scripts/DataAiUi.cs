using System.Collections;
using System.Collections.Generic;
using TMPro;
using TopShooter;
using UnityEngine;

public class DataAiUi : MonoBehaviour
{
	private Canvas canvas;
	private List<DataPanel> dataAIpanels=new List<DataPanel>();
	[SerializeField] private GameObject panel;

	[SerializeField] private TextMeshProUGUI currentGeneration;
	[SerializeField] private TextMeshProUGUI generationLimit;
	[SerializeField] private TextMeshProUGUI currentIteration;
	[SerializeField] private TextMeshProUGUI iterationLimit;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	public void InitDataUI(PlayerAI player)
	{
		float offset = 40;
		var panRect = panel.GetComponent<RectTransform>();
		float tmpRect = panRect.anchoredPosition.y;
		for (int i = 0; i < player.DataAI.Count; i++)
		{
			var newPanel = Instantiate(panel);
			newPanel.SetActive(true);
			newPanel.transform.SetParent(panel.transform.parent);
			var rect = newPanel.GetComponent<RectTransform>();

			rect.anchorMin = panRect.anchorMin;
			rect.anchorMax = panRect.anchorMax;
			rect.anchoredPosition = new Vector2(panRect.anchoredPosition.x, tmpRect - offset);
			rect.sizeDelta = panRect.sizeDelta;
			rect.localScale = panRect.localScale;
			//rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, tmpRect- offset);
			tmpRect = rect.anchoredPosition.y;

			newPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text= player.DataAI[i].nameVal.ToString();
			var txtPro = newPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
			dataAIpanels.Add(new DataPanel(player.DataAI[i].nameVal, txtPro));
		}
	}

	public void ShowSimData(int genLimit, int genCurrent, int itLimit, int itCurrent)
	{
		currentGeneration.text = genCurrent.ToString();
		generationLimit.text = genLimit.ToString();
		currentIteration.text = itCurrent.ToString();
		iterationLimit.text = itLimit.ToString();
	}

	public void SetPlayersRef(List<PlayerAI> players)
	{
		foreach (var p in players)
		{
			p.OnPlayerClick += ShowData;
		}
	}


    public void ShowData(List<DataAI> data)
	{
		foreach (var p in dataAIpanels)
		{
			p.text.text = data[(int)p.name].currentVal.ToString();
		}
	}
}

public struct DataPanel
{
	public VariableName name;
	public TextMeshProUGUI text;

	public DataPanel(VariableName name, TextMeshProUGUI text)
	{
		this.name = name;
		this.text = text;
	}
}