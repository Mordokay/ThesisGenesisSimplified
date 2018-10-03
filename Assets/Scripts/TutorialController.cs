using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    public GameObject SwitchLanguageButton;
    bool isPTLanguage;

    public GameObject prophetBaloon;

    public int tutorialStage;
    public GameObject[] tutorials;

    public Text introQuestions;
    public Text optionsQuestions;
    public GameObject[] questions;

    bool usedW;
    bool usedA;
    bool usedS;
    bool usedD;

    public Text TextW;
    public Text TextA;
    public Text TextS;
    public Text TextD;

    public Text prisonWarning;

    public Text nextPersonalQuestionsButton;

    public Text question1_Personal;

    public Text question2_Personal;
    public Text question2_Personal_OptionA;
    public Text question2_Personal_OptionB;
    public Text question2_Personal_OptionC;

    public Text question3_Personal;
    public Text question3_Personal_OptionA;
    public Text question3_Personal_OptionB;
    public Text question3_Personal_OptionC;

    public Text question4_Personal;
    public Text question4_Personal_OptionA;
    public Text question4_Personal_OptionB;
    public Text question4_Personal_OptionC;
    public Text question4_Personal_OptionD;
    public Text nextExtraQuestionsButton;

    public Text question1_Extra;
    public Text question2_Extra;
    public Text quitExtraQuestionsButton;


    public Text answerQuestionsText_winPanel;
    public Text quitText_winPanel;
    public Text youWinText_winPanel;

    public List<GameObject> tutorialColliders;

    void Start () {
        tutorialStage = 0;
        usedW = false;
        usedA = false;
        usedS = false;
        usedD = false;

        isPTLanguage = false;
    }
	
    public void NextTutorial()
    {
        tutorialStage += 1;
        if (tutorialStage < tutorials.Length)
        {
            RefreshTutorialTexts();
        }
        else if (tutorialStage >= tutorials.Length)
        {
            prophetBaloon.SetActive(false);
            DisableTutorialColliders();

            StartCoroutine(this.GetComponent<MySQLManager>().LogEventAtTime("Finished Tutorial"));
        }
    }

    public void SwitchLanguage()
    {
        if (isPTLanguage)
        {
            isPTLanguage = !isPTLanguage;
            SwitchLanguageButton.GetComponentInChildren<Text>().text = "EN";
            prisonWarning.text = "Next Time" + System.Environment.NewLine + "be more" + System.Environment.NewLine + "carefull!";

            //puts in english
            tutorials[0].GetComponentInChildren<Text>().text = "Oh hi! ... didn't see you there! Where did you come from?" + System.Environment.NewLine;
            tutorials[0].transform.GetChild(1).GetComponentInChildren<Text>().text = "Dont Know";

            tutorials[1].GetComponentInChildren<Text>().text = " ... well  it doesn't matter. People call me the prophet and right now I am in urgent need of help!!!";
            tutorials[1].transform.GetChild(1).GetComponentInChildren<Text>().text = "I can Help!";

            tutorials[2].GetComponentInChildren<Text>().text = "What you can see behind me is the temple of souls and unless you get the golden relics to the altar this world is going to be destroyed.";
            tutorials[2].transform.GetChild(1).GetComponentInChildren<Text>().text = "Oh No!";

            tutorials[3].GetComponentInChildren<Text>().text = " ... yeah its very sad. I used to be part of this world until the guardians got corrupted by the darkness and started mind controlling the villagers ...";
            tutorials[3].transform.GetChild(1).GetComponentInChildren<Text>().text = "So sad ...";

            tutorials[4].GetComponentInChildren<Text>().text = "anyways ... we are both outsiders on this world and the guardians are after us. I am hurt so I can't get all the relics myself. Bring me 12 relics without being caught";
            tutorials[4].transform.GetChild(1).GetComponentInChildren<Text>().text = "Sure!";

            tutorials[5].GetComponentInChildren<Text>().text = "Relics can come in diferent shapes:";
            tutorials[5].transform.GetChild(1).GetComponentInChildren<Text>().text = "I see!";

            tutorials[6].transform.GetChild(0).GetComponentInChildren<Text>().text = "To move around the map you Have to use they keys" + System.Environment.NewLine + "Try using them now!";

            tutorials[7].GetComponentInChildren<Text>().text = "Awesome! Now try to use your axe by pressing the left mouse button ...";

            tutorials[8].GetComponentInChildren<Text>().text = "You are doing great! Now that you know the basics lets try and grab that wood relic close to you ...";

            tutorials[9].GetComponentInChildren<Text>().text = "You can see that a villager was watching you gathering that relic!" + System.Environment.NewLine + "Vilagers cannot attack you but they can tell the guardians what you are doing, so be careful!";
            tutorials[9].transform.GetChild(1).GetComponentInChildren<Text>().text = "Okey";

            tutorials[10].GetComponentInChildren<Text>().text = "anyways ... As you can see on the top right of the screen, the gold relic you just grabbed is now inside your stash.";
            tutorials[10].transform.GetChild(1).GetComponentInChildren<Text>().text = "I see";

            tutorials[11].GetComponentInChildren<Text>().text = "Now drop the relic at the altar on the center of the temple ...";

            tutorials[12].GetComponentInChildren<Text>().text = "You did it! We are 1 relic closer to the ultimate sacrifice!" + System.Environment.NewLine + "PS: You can also check your progress on the white bar on the right.";
            tutorials[12].transform.GetChild(1).GetComponentInChildren<Text>().text = "Nice!";

            tutorials[13].GetComponentInChildren<Text>().text = "When NPCs talk about your actions you will see an axe symbol on their baloon box.";
            tutorials[13].transform.GetChild(1).GetComponentInChildren<Text>().text = "Ah!";

            tutorials[14].GetComponentInChildren<Text>().text = "If you are being chased by a guardian you can always escape with a quick dash. ";
            tutorials[14].transform.GetChild(1).GetComponentInChildren<Text>().text = "Cool!";

            tutorials[15].GetComponentInChildren<Text>().text = "To use the \"dash\" simply press space or the right mouse button while pointing with the mouse at the direction you want to go!  Try it ...";

            tutorials[16].GetComponentInChildren<Text>().text = "You are all set to go! Remember that your stash can't carry more than 2 relics. Drop the relics at the altar to free some space!";
            tutorials[16].transform.GetChild(1).GetComponentInChildren<Text>().text = "Got it!";

            tutorials[17].GetComponentInChildren<Text>().text = "Farewell traveler. I am counting on you! Dont let me down!";
            tutorials[17].transform.GetChild(1).GetComponentInChildren<Text>().text = "Farewell!";

            introQuestions.text = "Please indicate how you felt while playing the game for each of the items, on the following scale:";
            optionsQuestions.text = "    not at all   slightly	moderately   fairly	  extremely";
            optionsQuestions.fontSize = 11;

            questions[0].GetComponent<Text>().text = "I felt content";
            questions[1].GetComponent<Text>().text = "I felt skilful";
            questions[2].GetComponent<Text>().text = "I was interested in the game's story";
            questions[3].GetComponent<Text>().text = "I thought it was fun";
            questions[4].GetComponent<Text>().text = "I was fully occupied with the game";
            questions[5].GetComponent<Text>().text = "I felt happy";
            questions[6].GetComponent<Text>().text = "It gave me a bad mood";
            questions[7].GetComponent<Text>().text = "I thought about other things";
            questions[8].GetComponent<Text>().text = "I found it tiresome";
            questions[9].GetComponent<Text>().text = "I felt competent";
            questions[10].GetComponent<Text>().text = "I thought it was hard";
            questions[11].GetComponent<Text>().text = "It was aesthetically pleasing";
            questions[12].GetComponent<Text>().text = "I forgot everything around me";
            questions[13].GetComponent<Text>().text = "I felt good";
            questions[14].GetComponent<Text>().text = "I was good at it";
            questions[15].GetComponent<Text>().text = "I felt bored";
            questions[16].GetComponent<Text>().text = "I felt successful";
            questions[17].GetComponent<Text>().text = "I felt imaginative";
            questions[18].GetComponent<Text>().text = "I felt that I could explore things";
            questions[19].GetComponent<Text>().text = "I enjoyed it";
            questions[20].GetComponent<Text>().text = "I was fast at reaching the game's targets";
            questions[21].GetComponent<Text>().text = "I felt annoyed";
            questions[22].GetComponent<Text>().text = "I felt pressured";
            questions[23].GetComponent<Text>().text = "I felt irritable";
            questions[24].GetComponent<Text>().text = "I lost track of time";
            questions[25].GetComponent<Text>().text = "I felt challenged";
            questions[26].GetComponent<Text>().text = "I found it impressive";
            questions[27].GetComponent<Text>().text = "I was deeply concentrated in the game";
            questions[28].GetComponent<Text>().text = "I felt frustrated";
            questions[29].GetComponent<Text>().text = "It felt like a rich experience";
            questions[30].GetComponent<Text>().text = "I lost connection with the outside world";
            questions[31].GetComponent<Text>().text = "I felt time pressure";
            questions[32].GetComponent<Text>().text = "I had to put a lot of effort into it";

            youWinText_winPanel.text = "YOU WIN!!!";
            answerQuestionsText_winPanel.text = "Answer" + System.Environment.NewLine + "Questions";
            quitText_winPanel.text = "QUIT";
            
            nextPersonalQuestionsButton.text = "NEXT";
            question1_Personal.text = "1. Age";
            question2_Personal.text = "2. Sex";
            question2_Personal_OptionA.text = "Male";
            question2_Personal_OptionB.text = "Female";
            question2_Personal_OptionC.text = "Rather not say";
            question3_Personal.text = "3. How often do you play video games?";
            question3_Personal_OptionA.text = "I don't play video games.";
            question3_Personal_OptionB.text = "I play video games occasionally when the opportunity presents itself.";
            question3_Personal_OptionC.text = "I make some time in my schedule to play video games.";
            question4_Personal.text = "4. Are you familiar with the game genre (top-down action game)?";
            question4_Personal_OptionA.text = "I don’t play video games.";
            question4_Personal_OptionB.text = "I play video games but not of this genre.";
            question4_Personal_OptionC.text = "I am familiar with the genre and played at least one game of the genre.";
            question4_Personal_OptionD.text = "This genre is one of my favorites, and I played several games of this genre.";
            nextExtraQuestionsButton.text = "NEXT";
            question1_Extra.text = "1. How would you describe the behavior of the guardians?";
            question2_Extra.text = "2. Could you tell me an interesting situation that happened during the game?";
            quitExtraQuestionsButton.text = "QUIT";
        }
        else
        {
            isPTLanguage = !isPTLanguage;
            SwitchLanguageButton.GetComponentInChildren<Text>().text = "PT";
            prisonWarning.text = "Mais cuidado" + System.Environment.NewLine + "para a" + System.Environment.NewLine + "próxima!";
            //puts in portuguese

            tutorials[0].GetComponentInChildren<Text>().text = "Oh olá! Não te vi ai! De onde viestes?";
            tutorials[0].transform.GetChild(1).GetComponentInChildren<Text>().text = "Não Sei";

            tutorials[1].GetComponentInChildren<Text>().text = " ... agora não importa. As pessoas chamam-me profeta e neste momento eu preciso da tua ajuda!!!";
            tutorials[1].transform.GetChild(1).GetComponentInChildren<Text>().text = "Claro!";

            tutorials[2].GetComponentInChildren<Text>().text = "O que vês atrás de mim e o Templo das Almas e se não conseguires trazer todas as relíquias douradas para o altar, este mundo vai ser destruído";
            tutorials[2].transform.GetChild(1).GetComponentInChildren<Text>().text = "Oh Não :(";

            tutorials[3].GetComponentInChildren<Text>().text = "... sim é muito triste. Eu fazia parte deste mundo até ao momento em que os guardiões ficaram corrompidos pela escuridão e começaram a controlar os aldeões ...";
            tutorials[3].transform.GetChild(1).GetComponentInChildren<Text>().text = ":(";

            tutorials[4].GetComponentInChildren<Text>().text = "... somos os únicos sobreviventes deste mundo e os guardiões estão atrás de nós. Eu estou magoado por isso preciso que me apanhes 12 relíquias!";
            tutorials[4].transform.GetChild(1).GetComponentInChildren<Text>().text = "Compreendo";

            tutorials[5].GetComponentInChildren<Text>().text = "As relíquias têm formas diferentes:";
            tutorials[5].transform.GetChild(1).GetComponentInChildren<Text>().text = "OK!";

            tutorials[6].transform.GetChild(0).GetComponentInChildren<Text>().text = "Para andar pelo mapa usa as teclas do teu teclado" + System.Environment.NewLine;

            tutorials[7].GetComponentInChildren<Text>().text = "Perfeito! Agora tenta usar o machado pressionando o botão esquerdo do rato...";

            tutorials[8].GetComponentInChildren<Text>().text = "Boa! Agora que conheces os controlos básicos tenta apanhar a relíquia de madeira que está perto de ti...";

            tutorials[9].GetComponentInChildren<Text>().text = "Consegues ver que um aldeão viu-te a apanhar a relíquia!" + System.Environment.NewLine + "Os aldeões não te atacam mas podem ir contar ao guardião mais próximo o que estavas a fazer, portanto tem cuidado!";
            tutorials[9].transform.GetChild(1).GetComponentInChildren<Text>().text = "OK";

            tutorials[10].GetComponentInChildren<Text>().text = "bem ... como podes ver no canto superior direito, a relíquia que foi agarrada já se encontra dentro do teu \"Saco\"";
            tutorials[10].transform.GetChild(1).GetComponentInChildren<Text>().text = "Eu vejo";

            tutorials[11].GetComponentInChildren<Text>().text = "Agora retorna a relíquia ao altar que se encontra no centro do templo...";

            tutorials[12].GetComponentInChildren<Text>().text = "Conseguiste! Estamos 1 relíquia mais perto do sacrifício final!" + System.Environment.NewLine + "PS: Podes ver o progresso total na barra branca à direita do ecrã.";
            tutorials[12].transform.GetChild(1).GetComponentInChildren<Text>().text = "Boa!";

            tutorials[13].GetComponentInChildren<Text>().text = "Quando os NPCs falam sobre as tuas acções aparece um machado na sua caixa de diálogo.";
            tutorials[13].transform.GetChild(1).GetComponentInChildren<Text>().text = "Ah!";

            tutorials[14].GetComponentInChildren<Text>().text = "Se estiveres a ser perseguido por um guardião podes tentar escapar usando o \" Correr\".";
            tutorials[14].transform.GetChild(1).GetComponentInChildren<Text>().text = "Como?";

            tutorials[15].GetComponentInChildren<Text>().text = "Para usar o \"correr\" pressiona na tecla SPACE ou no botão direito do rato enquanto apontas com o rato na direcção que queres ir! Tenta agora...";

            tutorials[16].GetComponentInChildren<Text>().text = "Estas pronto para começar! Lembra-te que o \"Stash\" não consegue transportar mais do que 2 relíquias. Deixa as relíquias no altar para libertar espaço!";
            tutorials[16].transform.GetChild(1).GetComponentInChildren<Text>().text = "Percebido!";

            tutorials[17].GetComponentInChildren<Text>().text = "Adeus viajante. Estou a contar contigo! Boa Sorte!";
            tutorials[17].transform.GetChild(1).GetComponentInChildren<Text>().text = "Adeus!";

            introQuestions.text = "Por favor, indique como se sentiu ao jogar o jogo para cada um dos seguintes pontos, na seguinte escala:";
            optionsQuestions.text = "         discordo            discordo 	   Indiferente        concordo          concordo" + System.Environment.NewLine +
                                            "      totalmente    parcialmente                            parcialmente   totalmente";
            optionsQuestions.fontSize = 9;

            questions[0].GetComponent<Text>().text = "Senti satisfação";
            questions[1].GetComponent<Text>().text = "Senti-me hábil.";
            questions[2].GetComponent<Text>().text = "Estava interessado na história do jogo";
            questions[3].GetComponent<Text>().text = "Achei divertido";
            questions[4].GetComponent<Text>().text = "Estava totalmente ocupado com o jogo";
            questions[5].GetComponent<Text>().text = "Senti-me feliz";
            questions[6].GetComponent<Text>().text = "O jogo deu-me mau humor";
            questions[7].GetComponent<Text>().text = "Pensei noutras coisas";
            questions[8].GetComponent<Text>().text = "Achei cansativo";
            questions[9].GetComponent<Text>().text = "Senti-me competente";
            questions[10].GetComponent<Text>().text = "Era difícil";
            questions[11].GetComponent<Text>().text = "Era esteticamente agradável";
            questions[12].GetComponent<Text>().text = "Esqueci-me de tudo em meu redor";
            questions[13].GetComponent<Text>().text = "Senti-me bem";
            questions[14].GetComponent<Text>().text = "Joguei bem";
            questions[15].GetComponent<Text>().text = "Senti-me aborrecido";
            questions[16].GetComponent<Text>().text = "Senti-me bem sucedido";
            questions[17].GetComponent<Text>().text = "Senti-me imaginativo";
            questions[18].GetComponent<Text>().text = "Senti que poderia explorar coisas";
            questions[19].GetComponent<Text>().text = "Eu gostei do jogo";
            questions[20].GetComponent<Text>().text = "Eu fui rápido a alcançar os objectivos do jogo";
            questions[21].GetComponent<Text>().text = "Senti-me chateado";
            questions[22].GetComponent<Text>().text = "Senti-me pressionado";
            questions[23].GetComponent<Text>().text = "Senti-me irritável";
            questions[24].GetComponent<Text>().text = "Perdi a noção do tempo";
            questions[25].GetComponent<Text>().text = "Senti-me desafiado";
            questions[26].GetComponent<Text>().text = "Achei impressionante";
            questions[27].GetComponent<Text>().text = "Estava profundamente concentrado no jogo";
            questions[28].GetComponent<Text>().text = "Senti-me frustrado";
            questions[29].GetComponent<Text>().text = "Pareceu-me uma experiência rica";
            questions[30].GetComponent<Text>().text = "Perdi a conexão com o mundo exterior";
            questions[31].GetComponent<Text>().text = "Senti a pressão do tempo";
            questions[32].GetComponent<Text>().text = "Tive de me esforçar muito";

            youWinText_winPanel.text = "GANHASTE!!!";
            answerQuestionsText_winPanel.text = "Responder" + System.Environment.NewLine + "Questionário";
            quitText_winPanel.text = "SAIR";

            nextPersonalQuestionsButton.text = "PRÓXIMO";
            question1_Personal.text = "1. Idade";
            question2_Personal.text = "2. Sexo";
            question2_Personal_OptionA.text = "Masculino";
            question2_Personal_OptionB.text = "Feminino";
            question2_Personal_OptionC.text = "Prefiro não dizer";
            question3_Personal.text = "3. Qual a frequência com que joga jogos?";
            question3_Personal_OptionA.text = "Eu não jogo jogos.";
            question3_Personal_OptionB.text = "Jogo ocasionalmente quando tenho oportunidade.";
            question3_Personal_OptionC.text = "Tento arranjar espaço no meu horário para jogar jogos.";
            question4_Personal.text = "4. Está familiarizado com este tipo de jogo (top-down action game)?";
            question4_Personal_OptionA.text = "Eu não jogo jogos.";
            question4_Personal_OptionB.text = "Eu jogo jogos mas não deste género.";
            question4_Personal_OptionC.text = "Estou familiarizado com o género e joguei pelo menos um jogo deste género.";
            question4_Personal_OptionD.text = "Este género de jogo é um dos meus preferidos.";
            nextExtraQuestionsButton.text = "PRÓXIMO";
            question1_Extra.text = "1. Como descreveria o comportamento dos guardiões?";
            question2_Extra.text = "2. Consegue descrever uma situação interessante que tenha acontecido durante o jogo?";
            quitExtraQuestionsButton.text = "SAIR";

        }
    }

    void RefreshTutorialTexts()
    {
        foreach(GameObject tutorial in tutorials)
        {
            tutorial.SetActive(false);
        }
        tutorials[tutorialStage].SetActive(true);
    }

    void DisableTutorialColliders()
    {
        foreach(GameObject col in tutorialColliders)
        {
            col.SetActive(false);
        }
    }

	void Update () {
        //Check if the tutorial has ended

        if (tutorialStage == 6)
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                this.GetComponent<Beacon>().SpreadInitialGoldenMessages();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                usedW = true;
                TextW.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                usedA = true;
                TextA.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                usedS = true;
                TextS.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                usedD = true;
                TextD.color = Color.red;
            }

            if (usedW && usedA && usedS && usedD)
            {
                NextTutorial();
            }
        }
        else if (tutorialStage == 7 && Input.GetMouseButtonDown(0))
        {
            NextTutorial();
        }
        else if (tutorialStage == 15 && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1)))
        {
            NextTutorial();
        }
    }
}
